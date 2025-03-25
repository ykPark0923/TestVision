using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PixelPainter
{
    public partial class InspCrack : Form
    {

        private readonly string normalImagePath = @"Image.bmp";

        public InspCrack()
        {
            InitializeComponent();

            // 정상이미지를 경로로 고정
            src2 = Cv2.ImRead(normalImagePath);
            if (src2.Empty()) return;

            pictureBox2.Image = BitmapConverter.ToBitmap(src2);
        }

        private Mat src = new Mat();
        private Mat src2 = new Mat();
        private Mat diffImage = new Mat();

        private void openBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src);
            }
        }
        //크랙이미지 윤곽선 그리기
        private void diffBTN_Click(object sender, EventArgs e)
        {
            DiffCompare();
        }
        private void DiffCompare()
        {
            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            diffImage = new Mat();
            Cv2.Absdiff(src, src2, diffImage);

            detectCrack();
        }
        private void detectCrack()
        {
            //crack은 외곽에만 생김, 내부영역 지워버림
            // 이미지 크기 계산
            int roiWidth = diffImage.Cols * 80 / 100;   // 전체 너비의 80%
            int roiHeight = diffImage.Rows * 80 / 100;  // 전체 높이의 80%

            // 중심을 기준으로 ROI 좌표 설정
            int x = (diffImage.Cols - roiWidth) / 2;  // 중앙 정렬 X 좌표
            int y = (diffImage.Rows - roiHeight) / 2; // 중앙 정렬 Y 좌표

            // ROI 생성
            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);

            // 안쪽 영역을 검정색으로 채우기
            Cv2.Rectangle(diffImage, innerROI, new Scalar(0), -1);

            // 그레이스케일 변환
            Mat grayDiff = new Mat();
            Cv2.CvtColor(diffImage, grayDiff, ColorConversionCodes.BGR2GRAY);

            // 이진화 (Threshold 적용)
            Mat binaryDiff = new Mat();
            Cv2.Threshold(grayDiff, binaryDiff, 50, 255, ThresholdTypes.Binary);

            // 윤곽선 검출
            Point[][] CrackContours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binaryDiff, out CrackContours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            bool crackDetected = false;
            Mat resultImage = src.Clone();

            foreach (var contour in CrackContours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > 5)  // 일정 크기 이상만 감지
                {
                    Rect boundingBox = Cv2.BoundingRect(contour);
                    Cv2.Rectangle(resultImage, boundingBox, new Scalar(0, 0, 255), 2); // 빨간색 박스 그리기
                    crackDetected = true;
                }
            }

            // 결과 표시
            pictureBox1.Image = BitmapConverter.ToBitmap(resultImage);

            // 크랙이 감지되었다면 NG 처리
            textBox1.Text = crackDetected ? "NG: Crack" : "OK";
        }



        private void alignBTN_Click(object sender, EventArgs e)
        {
            Mat temp1 = new Mat();  //윤곽선 그리기 전 이미지를 임시저장
            Mat temp2 = new Mat();  //윤곽선 그리기 전 이미지를 임시저장
            Mat aligned1 = null;
            Mat aligned2 = null;

            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            //이미지 윤곽선 그리기전 이미지 임시저장
            temp1 = src.Clone();
            temp2 = src2.Clone();

            // PCB 정렬
            aligned1 = AlignPCB(src, src2);
            aligned2 = AlignPCB(src2, src2);

            if(aligned1 != null && aligned2 != null)
            {
                Cv2.ImShow("align1", aligned1);
                Cv2.ImShow("align2", aligned2);
                src = temp1.Clone();
                src2 = temp2.Clone();
            }
            else
            {
                Console.WriteLine("align 미검.");
            }

        }

        private Mat AlignPCB(Mat src, Mat src2)
        {
            Mat gray = new Mat(); // 원본 손상 방지를 위해 새로운 Mat 사용
            Mat result = new Mat();

            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY); // 원본을 먼저 Grayscale로 변환

            #region 좌우상하 원검출위해 안쪽영역 지움
            //crack은 외곽에만 생김, 내부영역 지워버림
            int roiWidth = src.Cols * 75 / 100;   // 전체 너비의 75%
            int roiHeight = src.Rows * 100 / 100; // 전체 높이의 100%

            int x = (src.Cols - roiWidth) / 2;
            int y = (src.Rows - roiHeight) / 2;

            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);
            Cv2.Rectangle(gray, innerROI, new Scalar(0), -1);

            roiWidth = src.Cols * 100 / 100;
            roiHeight = src.Rows * 70 / 100;
            x = (src.Cols - roiWidth) / 2;
            y = (src.Rows - roiHeight) / 2;
            innerROI = new Rect(x, y, roiWidth, roiHeight);
            Cv2.Rectangle(gray, innerROI, new Scalar(0), -1);
            #endregion

            // 블러링 (노이즈 감소)
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 2);

            // 원 검출
            CircleSegment[] circles = Cv2.HoughCircles(gray, HoughModes.Gradient, 1, gray.Rows / 8, 50, 20, 5, 50);



            // 원 검출 실패 시 종료
            if (circles.Length < 4)
            {
                Console.WriteLine("Error: 원이 4개 검출안됨!");
                return null;
            }

            // 원 중심점을 저장하고 정렬
            List<Point2f> centers = new List<Point2f>();
            foreach (var circle in circles)
            {
                centers.Add(new Point2f((float)circle.Center.X, (float)circle.Center.Y));
            }
            Point2f[] sortedCenters = SortCenters(centers);

            // 패딩 값 설정 (픽셀 단위, 필요에 따라 조정 가능)
            int paddingX = 30; // 좌우 여유 공간
            int paddingY = 30; // 상하 여유 공간

            // 패딩을 적용한 목적지 좌표 계산 (네 방향 모두 여유 공간 추가)
            Point2f[] dstPoints =
            {
                new Point2f(0 + paddingX, 0 + paddingY),  // 좌상단 (더 오른쪽, 아래쪽)
                new Point2f(src2.Cols - 1 - paddingX, 0 + paddingY),  // 우상단 (더 왼쪽, 아래쪽)
                new Point2f(src2.Cols - 1 - paddingX, src2.Rows - 1 - paddingY),  // 우하단 (더 왼쪽, 위쪽)
                new Point2f(0 + paddingX, src2.Rows - 1 - paddingY)   // 좌하단 (더 오른쪽, 위쪽)
            };

            // Perspective 변환 적용
            Mat perspectiveMatrix = Cv2.GetPerspectiveTransform(sortedCenters, dstPoints);
            Cv2.WarpPerspective(src, result, perspectiveMatrix, new Size(src2.Cols, src2.Rows));

            return result;
        }


        // 원 중심점을 좌상단, 우상단, 우하단, 좌하단 순으로 정렬
        private Point2f[] SortCenters(List<Point2f> centers)
        {
            var sortedX = centers.OrderBy(p => p.X).ToArray();
            var left = sortedX.Take(2).OrderBy(p => p.Y).ToArray();
            var right = sortedX.Skip(2).OrderBy(p => p.Y).ToArray();

            return new Point2f[]
            {
                left[0],   // 좌상단
                right[0],  // 우상단
                right[1],  // 우하단
                left[1]    // 좌하단
            };
        }
    }
}