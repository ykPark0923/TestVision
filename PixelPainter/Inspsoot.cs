using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Size = OpenCvSharp.Size;
using Point = OpenCvSharp.Point;

namespace PixelPainter
{
    public partial class Inspsoot : Form
    {
        private Mat normalImage = new Mat(); // 양품 이미지
        private Mat src = new Mat(); // 검사 이미지
        private Mat binaryImage = new Mat(); // 이진화 이미지

        public Inspsoot()
        {
            InitializeComponent();
        }

        // 🔹 [양품 이미지 불러오기]
        private void btnLoadNormalImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                normalImage = Cv2.ImRead(openFileDialog1.FileName);
                pbOK.Image = ResizeImage(BitmapConverter.ToBitmap(normalImage), pbOK.Size);
            }
        }

        // 🔹 [검사 이미지 불러오기]
        private void btnLoadTestImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                pbNG.Image = ResizeImage(BitmapConverter.ToBitmap(src), pbNG.Size);
            }
        }

        // 🔹 [이진화 체크박스 - 이미지 변경]
        private void checkBoxThreshold_CheckedChanged(object sender, EventArgs e)
        {
            UpdateImageDisplay();
        }

        // 🔹 [그을음 검출 버튼]
        private void btnDetectBurn_Click(object sender, EventArgs e)
        {
            DetectSoot();
        }

        // 🔹 [이미지 업데이트]
        private void UpdateImageDisplay()
        {
            if (!src.Empty())
            {
                Mat displayImage = checkBoxThreshold.Checked ? binaryImage : src;
                pbNG.Image = ResizeImage(BitmapConverter.ToBitmap(displayImage), pbNG.Size);
            }
        }

        // 🔹 [이미지 리사이즈 함수]
        private Bitmap ResizeImage(Bitmap img, System.Drawing.Size newSize)
        {
            return new Bitmap(img, newSize);
        }

        // 🔹 [그을음 검출 함수]
        private void DetectSoot()
        {
            if (src.Empty() || normalImage.Empty())
            {
                MessageBox.Show("양품 및 검사 이미지를 먼저 로드하세요.");
                return;
            }

            // 1. 그레이스케일 변환
            Mat gray1 = new Mat(), gray2 = new Mat();
            Cv2.CvtColor(src, gray1, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(normalImage, gray2, ColorConversionCodes.BGR2GRAY);

            // 2. 블러 적용 (노이즈 제거)
            Cv2.GaussianBlur(gray1, gray1, new Size(5, 5), 0);
            Cv2.GaussianBlur(gray2, gray2, new Size(5, 5), 0);

            // 3. 차이 계산
            Mat diffImage = new Mat();
            Cv2.Absdiff(gray1, gray2, diffImage);

            // 4. 이진화 (Threshold 적용)
            binaryImage = new Mat();
            Cv2.Threshold(diffImage, binaryImage, 30, 255, ThresholdTypes.Binary);

            // 5. 윤곽선 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binaryImage, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            List<Rect> boundingBoxes = new List<Rect>();

            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > 20)
                {
                    boundingBoxes.Add(Cv2.BoundingRect(contour));
                }
            }

            // 6. 가까운 영역 병합
            List<Rect> mergedBoxes = MergeBoundingBoxes(boundingBoxes, 20);

            // 7. 원본 이미지에 ROI 표시 (보라색 박스)
            Mat resultImage = src.Clone();
            foreach (var box in mergedBoxes)
            {
                Cv2.Rectangle(resultImage, box, new Scalar(255, 0, 255), 2);
            }

            // 결과 표시
            pbNG.Image = ResizeImage(BitmapConverter.ToBitmap(resultImage), pbNG.Size);
        }

        // 🔹 [ROI 병합 함수]
        private List<Rect> MergeBoundingBoxes(List<Rect> boxes, int mergeThreshold)
        {
            List<Rect> merged = new List<Rect>();

            while (boxes.Count > 0)
            {
                Rect current = boxes[0];
                boxes.RemoveAt(0);

                for (int i = boxes.Count - 1; i >= 0; i--)
                {
                    Rect other = boxes[i];

                    if ((current & other).Width * (current & other).Height > 0 || DistanceBetweenRects(current, other) < mergeThreshold)
                    {
                        current = Cv2.BoundingRect(new Point[] { current.TopLeft, current.BottomRight, other.TopLeft, other.BottomRight });
                        boxes.RemoveAt(i);
                    }
                }
                merged.Add(current);
            }
            return merged;
        }

        // 🔹 [ROI 거리 계산 함수]
        private double DistanceBetweenRects(Rect a, Rect b)
        {
            int dx = Math.Max(0, Math.Max(a.Left, b.Left) - Math.Min(a.Right, b.Right));
            int dy = Math.Max(0, Math.Max(a.Top, b.Top) - Math.Min(a.Bottom, b.Bottom));
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
