using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp.Extensions;
using OpenCvSharp;

namespace PixelPainter
{
    public partial class InspDent : Form
    {
        private readonly string normalImagePath = @"C:\devSJ\C#_MarchProject\PixelPainter\bin\Debug\Image.bmp"; //정상 이미지 경로
        private Mat src = new Mat();
        private Mat src2 = new Mat();
        private Mat diffImage = new Mat();

        public InspDent()
        {
            InitializeComponent();

            // 정상이미지를 경로로 고정
            src2 = Cv2.ImRead(normalImagePath);
            if (src2.Empty()) return;

            pbOK.Image = BitmapConverter.ToBitmap(src2);
        }


        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                pbNG.Image = BitmapConverter.ToBitmap(src);
            }
        }

        private void btnDifference_Click(object sender, EventArgs e)
        {
            DetectDent();
        }

        private void DetectDent()
        {
            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            //// 1️⃣ 정상 이미지와 검사 이미지의 차이 계산
            //diffImage = new Mat();
            //Cv2.Absdiff(src, src2, diffImage);

            //// 2️⃣ 그레이스케일 변환
            //Mat grayDiff = new Mat();
            //Cv2.CvtColor(diffImage, grayDiff, ColorConversionCodes.BGR2GRAY);

            //// 3️⃣ 이진화 (Dent 부분만 강조)
            //Mat binaryDiff = new Mat();
            //Cv2.Threshold(grayDiff, binaryDiff, 30, 255, ThresholdTypes.Binary); // Dent가 흰색으로 남음

            //// 4️⃣ 외곽 영역 제거 (중심부만 남김)
            //int roiWidth = diffImage.Cols * 86 / 100;   // 중심 너비 (86%)
            //int roiHeight = diffImage.Rows * 79 / 100;  // 중심 높이 (79%)
            //int x = (diffImage.Cols - roiWidth) / 2;
            //int y = (diffImage.Rows - roiHeight) / 2;
            //Rect innerROI = new Rect(x, y, roiWidth, roiHeight);

            //// 외곽을 검정색으로 덮기 (중심만 남김)
            //Mat mask = Mat.Zeros(binaryDiff.Size(), MatType.CV_8UC1);
            //Cv2.Rectangle(mask, innerROI, new Scalar(255), -1); // 중심을 흰색으로 유지
            //Cv2.BitwiseAnd(binaryDiff, mask, binaryDiff); // 중심 부분만 유지

            //// 5️⃣ 윤곽선 검출 → Dent 영역 찾기
            //OpenCvSharp.Point[][] dentContours;
            //HierarchyIndex[] hierarchy;
            //Cv2.FindContours(binaryDiff, out dentContours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            //bool dentDetected = false;
            //Mat resultImage = src.Clone();

            //foreach (var contour in dentContours)
            //{
            //    double area = Cv2.ContourArea(contour);
            //    if (area > 25)  // 일정 크기 이상의 Dent만 감지
            //    {
            //        Rect boundingBox = Cv2.BoundingRect(contour);
            //        Cv2.Rectangle(resultImage, boundingBox, new Scalar(0, 255, 255), 2); // 노란색 박스 표시
            //        dentDetected = true;
            //    }
            //}

            //// 6️⃣ 결과 표시
            //pbNG.Image = BitmapConverter.ToBitmap(resultImage);

            //// Dent 검출 여부 출력
            //txtResult.Text = dentDetected ? "NG: Dent Detected" : "OK";

            //soot**************************
            Mat gray1 = new Mat();
            Mat gray2 = new Mat();
            Cv2.CvtColor(src, gray1, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(src2, gray2, ColorConversionCodes.BGR2GRAY);

            // 2. 블러(Blur) 적용하여 노이즈 제거
            Cv2.GaussianBlur(gray1, gray1, new Size(5, 5), 0);
            Cv2.GaussianBlur(gray2, gray2, new Size(5, 5), 0);

            // 3. 밝기 차이 계산 (절대값)
            Mat diffImage = new Mat();
            Cv2.Absdiff(gray1, gray2, diffImage); // 절대 차이 연산 (음수 값 보정)

            // 4. 밝아진 부분(Soot)만 강조 (Threshold 적용)
            Mat sootMask = new Mat();
            Cv2.Threshold(diffImage, sootMask, 30, 255, ThresholdTypes.Binary); // 밝아진 부분만 남김

            // 5. 배경(어두운 부분) 제거 (Inverse Threshold)
            Mat bgMask = new Mat();
            Cv2.Threshold(gray1, bgMask, 50, 255, ThresholdTypes.BinaryInv); // 배경 제거

            // 6. 그을음(Soot)만 남기기
            Mat finalMask = new Mat();
            Cv2.BitwiseAnd(sootMask, bgMask, finalMask);

            // 7. 윤곽선 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(finalMask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            List<Rect> boundingBoxes = new List<Rect>();

            // 윤곽선들을 사각형 영역(Rect)으로 변환하여 저장
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > 20) // 작은 노이즈 무시
                {
                    boundingBoxes.Add(Cv2.BoundingRect(contour));
                }
            }

            // 8. 가까운 영역 병합
            List<Rect> mergedBoxes = MergeBoundingBoxes(boundingBoxes, 20); // 20px 이하 간격이면 병합



            // 9. 병합된 ROI를 결과 이미지에 표시
            Mat resultImage = src.Clone();
            foreach (var box in mergedBoxes)
            {
                Cv2.Rectangle(resultImage, box, new Scalar(0, 0, 255), 2); // 빨간색 박스 (병합된 Soot 영역)
            }

            // 결과 표시
            pbNG.Image = BitmapConverter.ToBitmap(resultImage);

            // --- 윤곽선 병합 함수 ---
            // 가까운 두 사각형이 있을 경우 병합
            List<Rect> MergeBoundingBoxes(List<Rect> boxes, int mergeThreshold)
            {
                List<Rect> merged = new List<Rect>();

                while (boxes.Count > 0)
                {
                    Rect current = boxes[0];
                    boxes.RemoveAt(0);

                    for (int i = boxes.Count - 1; i >= 0; i--)
                    {
                        Rect other = boxes[i];

                        // 두 사각형이 가까우면 병합
                        if ((current & other).Width * (current & other).Height > 0 || DistanceBetweenRects(current, other) < mergeThreshold)
                        {
                            // 병합된 Rect 계산
                            current = Cv2.BoundingRect(new Point[] { current.TopLeft, current.BottomRight, other.TopLeft, other.BottomRight });
                            boxes.RemoveAt(i);
                        }
                    }
                    merged.Add(current);
                }
                return merged;
            }

            // --- 두 개의 ROI 거리 계산 ---
            // 두 사각형의 중심 간 거리 계산
            double DistanceBetweenRects(Rect a, Rect b)
            {
                int dx = Math.Max(0, Math.Max(a.Left, b.Left) - Math.Min(a.Right, b.Right));
                int dy = Math.Max(0, Math.Max(a.Top, b.Top) - Math.Min(a.Bottom, b.Bottom));
                return Math.Sqrt(dx * dx + dy * dy);
            }


        }
    }
}
