using System;
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

            if(src.Empty() || src2.Empty())
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

            Cv2.ImShow("align1", aligned1);
            Cv2.ImShow("align2", aligned2);
            src = temp1.Clone();
            src2 = temp2.Clone();

        }

        private Mat AlignPCB(Mat src, Mat src2)
        {
            Mat gray = new Mat();
            Mat binary = new Mat();
            Mat result = new Mat();

            // src 이미지를 회색조로 변환
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // Adaptive Thresholding 적용
            Cv2.AdaptiveThreshold(gray, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 11, 2);

            // 외곽선 찾기
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 가장 큰 외곽선 찾기
            Point[] largestContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).FirstOrDefault();
            if (largestContour == null || Cv2.ContourArea(largestContour) < 100)
                return src;

            // 외곽선을 근사화하여 4개의 점을 추출
            Point[] approx = Cv2.ApproxPolyDP(largestContour, 0.02 * Cv2.ArcLength(largestContour, true), true);

            // 4개의 점을 얻으면
            if (approx.Length == 4)
            {
                // 4개의 점을 시계방향으로 정렬
                Point2f[] srcPoints = OrderPoints(approx);


                // 4개의 점을 이미지에 그리기
                for (int i = 0; i < 4; i++)
                {
                    // 각 점을 빨간색 원으로 표시
                    Cv2.Circle(src, new Point((int)srcPoints[i].X, (int)srcPoints[i].Y), 10, new Scalar(0, 0, 255), -1);  // -1은 채우기
                }

                // 기준 이미지는 src2로 설정
                Point2f[] dstPoints =
                {
                    new Point2f(0, 0),  // 왼쪽 위
                    new Point2f(src2.Cols - 1, 0),  // 오른쪽 위
                    new Point2f(src2.Cols - 1, src2.Rows - 1),  // 오른쪽 아래
                    new Point2f(0, src2.Rows - 1)  // 왼쪽 아래
                };



                // 아핀 변환 행렬 계산
                //Mat affineMatrix = Cv2.GetAffineTransform(srcPoints, dstPoints);
                // src 이미지를 src2에 맞춰 아핀 변환 적용
                //Cv2.WarpAffine(src, result, affineMatrix, src2.Size());


                // Perspective 변환 행렬 계산
                Mat perspectiveMatrix = Cv2.GetPerspectiveTransform(srcPoints, dstPoints);
                Cv2.WarpPerspective(src, result, perspectiveMatrix, src2.Size());


                return result;
            }
            else
            {
                // 4개 점이 아닌 경우 null 반환
                return null;
            }
        }

        private Point2f[] OrderPoints(Point[] points)
        {
            Point2f[] ordered = new Point2f[4];
            var sortedX = points.OrderBy(p => p.X).ToArray();
            var left = sortedX.Take(2).OrderBy(p => p.Y).ToArray();
            var right = sortedX.Skip(2).OrderBy(p => p.Y).ToArray();

            ordered[0] = left[0];  // 왼쪽 위
            ordered[1] = right[0];  // 오른쪽 위
            ordered[2] = right[1];  // 오른쪽 아래
            ordered[3] = left[1];  // 왼쪽 아래

            return ordered;
        }
    }
}