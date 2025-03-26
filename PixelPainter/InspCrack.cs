using System;
using System.Collections.Generic;
using System.Linq;
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
        private Mat aligned1 = new Mat();
        private Mat aligned2 = new Mat();
        private Mat diffImage = new Mat();
        private Mat inversePerspectiveMatrix = new Mat();

        private float min = 100;
        private float max = 5000;

        private void openBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src);
            }
        }
        // 크랙 이미지 윤곽선 그리기
        private void diffBTN_Click(object sender, EventArgs e)
        {
            DiffCompare();
        }
        private void alignBTN_Click(object sender, EventArgs e)
        {
            Mat temp1 = src.Clone();
            Mat temp2 = src2.Clone();
            Mat aligned1 = null;
            Mat aligned2 = null;

            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            // PCB 정렬
            aligned1 = PCBAlign(src, src2);
            aligned2 = PCBAlign(src2, src2);

            if (aligned1 != null && aligned2 != null)
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
        private void DiffCompare()
        {
            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            // 이미지 윤곽선 그리기 전 임시 저장
            Mat temp1 = src.Clone();
            Mat temp2 = src2.Clone();

            // PCB 정렬 (각 이미지에 대해 DetectCirclesUsingBlobs 호출)
            aligned1 = PCBAlign(src, src2);
            aligned2 = PCBAlign(src2, src2);

            if (aligned1 != null && aligned2 != null)
            {
                src = temp1.Clone();
                src2 = temp2.Clone();
            }
            else
            {
                Console.WriteLine("align 미검.");
            }

            diffImage = new Mat();
            Cv2.Absdiff(aligned1, aligned2, diffImage);
            //Cv2.ImShow("diffImage", diffImage);
            detectCrack(diffImage);
        }
        private Mat PCBAlign(Mat src, Mat src2)
        {
            Mat gray = new Mat();
            Mat result = new Mat();

            // 1) 그레이스케일 변환
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            #region 좌우상하 원검출을 위해 안쪽영역 지움
            int roiWidth = src.Cols * 75 / 100;
            int roiHeight = src.Rows * 100 / 100;
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

            // 2) 이진화 (Threshold)
            Cv2.Threshold(gray, gray, 30, 255, ThresholdTypes.Binary);

            // 3) 윤곽선(contour) 탐색
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(gray, out contours, out hierarchy,
                             RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            // 4) 면적 기준으로 컨투어 정렬 (내림차순: 가장 큰 면적부터)
            var sortedContours = contours
                .OrderByDescending(c => Cv2.ContourArea(c))
                .ToList();

            // 5) "가장 큰 컨투어 4개"를 건너뛰고, "5번째~8번째" 컨투어(총 4개)를 선택
            var targetContours = sortedContours.Skip(4).Take(4);

            // 6) 선택된 컨투어에서 중심을 구해서 리스트에 추가
            List<Point2f> centerPoints = new List<Point2f>();
            foreach (var contour in targetContours)
            {
                Point2f center;
                float radius;
                Cv2.MinEnclosingCircle(contour, out center, out radius);
                centerPoints.Add(center);
            }

            // 7) 만약 검출된 중심이 4개가 아니라면 오류 처리
            if (centerPoints.Count != 4)
            {
                Console.WriteLine("Error: 4개의 중심을 검출하지 못함. 검출된 중심 개수: " + centerPoints.Count);
                return null;
            }

            // 8) 중심점들을 좌상단, 우상단, 우하단, 좌하단 순으로 정렬
            Point2f[] sortedCenters = SortCenters(centerPoints);

            // 9) 패딩 적용 및 투시 변환
            int paddingX = 30;
            int paddingY = 30;
            Point2f[] dstPoints =
            {
                new Point2f(paddingX, paddingY),
                new Point2f(src2.Cols - 1 - paddingX, paddingY),
                new Point2f(src2.Cols - 1 - paddingX, src2.Rows - 1 - paddingY),
                new Point2f(paddingX, src2.Rows - 1 - paddingY)
            };

            Mat perspectiveMatrix = Cv2.GetPerspectiveTransform(sortedCenters, dstPoints);
            inversePerspectiveMatrix = perspectiveMatrix.Inv();
            Cv2.WarpPerspective(src, result, perspectiveMatrix, new Size(src2.Cols, src2.Rows));

            return result;
        }
        private void detectCrack(Mat sourceImage)
        {
            Mat diffImage = sourceImage;
            //crack은 외곽에만 생김, 내부영역 지워버림
            // 이미지 크기 계산
            int roiWidth = diffImage.Cols * 90 / 100;   // 전체 너비의 80%
            int roiHeight = diffImage.Rows * 90 / 100;  // 전체 높이의 80%

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
            //Cv2.ImShow("binaryDiff", binaryDiff);

            // 윤곽선 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binaryDiff, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            bool crackDetected = false;
            Mat resultImage = src.Clone();

            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area >= min && area<=max)  // 일정 크기 이상만 감지
                {
                    Rect boundingBox = Cv2.BoundingRect(contour);

                    // boundingBox의 좌상단 좌표 (시작 좌표)
                    Point2f topLeft = new Point2f(boundingBox.X, boundingBox.Y);

                    // 역변환하여 원본 좌표를 계산
                    Point2f originalTopLeft = perspectiveInverseTransform(topLeft, inversePerspectiveMatrix);

                    // boundingBox의 좌상단 좌표를 계산된 원본 좌표로 보정
                    Rect boundingBoxWithOffset = new Rect(
                        (int)(boundingBox.X + (originalTopLeft.X - topLeft.X)),
                        (int)(boundingBox.Y + (originalTopLeft.Y - topLeft.Y)),
                        boundingBox.Width,
                        boundingBox.Height
                    );

                    // 원본 이미지에 사각형 그리기
                    Cv2.Rectangle(resultImage, boundingBoxWithOffset, new Scalar(0, 100, 255), 2);  // 주황 박스 그리기
                    crackDetected = true;
                }
            }

            // 결과 표시
            pictureBox1.Image = BitmapConverter.ToBitmap(resultImage);

            // 크랙이 감지되었다면 NG 처리
            textBox1.Text = crackDetected ? "NG: Crack" : "OK";
        }
        // 전달된 4개의 중심점을 좌상단, 우상단, 우하단, 좌하단 순으로 정렬
        private Point2f[] SortCenters(List<Point2f> centers)
        {
            if (centers.Count != 4)
                throw new ArgumentException("정렬을 위해서는 정확히 4개의 중심점이 필요합니다.");

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
        private Point2f perspectiveInverseTransform(Point2f point, Mat inverseMatrix)
        {
            // Homogeneous 좌표로 변환 (3x1 크기의 행렬로 설정)
            Mat homogenousPoint = new Mat(3, 1, MatType.CV_32F);
            homogenousPoint.Set<float>(0, 0, point.X);
            homogenousPoint.Set<float>(1, 0, point.Y);
            homogenousPoint.Set<float>(2, 0, 1); // 동차 좌표로 변환

            // inverseMatrix와 homogenousPoint의 데이터 타입을 맞추기 위해 변환 (CV_32F)
            if (inverseMatrix.Type() != MatType.CV_32F)
            {
                inverseMatrix.ConvertTo(inverseMatrix, MatType.CV_32F);
            }

            // 행렬 곱셈: 역변환 행렬을 적용
            Mat transformedPoint = inverseMatrix * homogenousPoint; // 행렬 곱셈

            // 역변환 후 좌표
            float x = transformedPoint.Get<float>(0, 0) / transformedPoint.Get<float>(2, 0);
            float y = transformedPoint.Get<float>(1, 0) / transformedPoint.Get<float>(2, 0);

            return new Point2f(x, y);
        }


    }
}
