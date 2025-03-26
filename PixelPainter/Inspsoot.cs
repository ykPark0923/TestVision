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
        private void openBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                normalImage = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox2.Image = ResizeImage(BitmapConverter.ToBitmap(normalImage), pictureBox2.Size);
            }
        }

        // 🔹 [검사 이미지 불러오기]
        private void openBTN2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = ResizeImage(BitmapConverter.ToBitmap(src), pictureBox1.Size);
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
                pictureBox1.Image = ResizeImage(BitmapConverter.ToBitmap(displayImage), pictureBox1.Size);
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

            // 1. 그레이스케일 변환 및 대비 개선
            Mat gray1 = new Mat(), gray2 = new Mat();
            Cv2.CvtColor(src, gray1, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(normalImage, gray2, ColorConversionCodes.BGR2GRAY);

            Cv2.EqualizeHist(gray1, gray1);
            Cv2.EqualizeHist(gray2, gray2);

            // 2. 블러 적용 (노이즈 제거)
            Cv2.GaussianBlur(gray1, gray1, new Size(5, 5), 0);
            Cv2.GaussianBlur(gray2, gray2, new Size(5, 5), 0);

            // 3. 밝기 증가 부분 강조 (Soot 검출)
            Mat diffImage = new Mat();
            Cv2.Subtract(gray2, gray1, diffImage); // 밝아진 부분 강조

            // 4. 이진화 적용 (연한 Soot도 감지)
            Mat sootMask = new Mat();
            Cv2.Threshold(diffImage, sootMask, 10, 255, ThresholdTypes.Binary);

            // 5. Canny 엣지 검출 적용
            Mat edges = new Mat();
            Cv2.Canny(sootMask, edges, 100, 200);

            // 6. 배경(어두운 부분) 제거 (PCB의 검정색 배경 제외)
            Mat bgMask = new Mat();
            Cv2.Threshold(gray1, bgMask, 37, 255, ThresholdTypes.BinaryInv);

            // 7. 중심 부분만 남기기 (외곽 영역 제거)
            int roiWidth = diffImage.Cols * 80 / 100;
            int roiHeight = diffImage.Rows * 79 / 100;
            int x = (diffImage.Cols - roiWidth) / 2;
            int y = (diffImage.Rows - roiHeight) / 2;
            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);

            // 외곽을 제거하기 위해 mask 생성
            Mat mask = Mat.Zeros(sootMask.Size(), MatType.CV_8UC1);
            Cv2.Rectangle(mask, innerROI, new Scalar(255), -1);

            // 8. 중심 부분만 남기고 외곽은 제거
            Cv2.BitwiseAnd(sootMask, mask, sootMask);

            // 9. 최종 Soot 검출 결과
            Mat finalMask = new Mat();
            Cv2.BitwiseAnd(sootMask, bgMask, finalMask);

            // 10. 윤곽선 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(finalMask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            List<Rect> boundingBoxes = new List<Rect>();
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > 4) // 작은 노이즈 무시
                {
                    boundingBoxes.Add(Cv2.BoundingRect(contour));
                }
            }

            // 11. 가까운 영역 병합
            List<Rect> mergedBoxes = MergeBoundingBoxes(boundingBoxes, 100);

            // 12. 결과 표시
            Mat resultImage = src.Clone();
            foreach (var box in mergedBoxes)
            {
                Cv2.Rectangle(resultImage, box, new Scalar(0, 0, 255), 2); // 빨간색 박스 표시
            }

            pictureBox1.Image = BitmapConverter.ToBitmap(resultImage);
        }

        // 🔹 [ROI 병합 함수]
        public List<Rect> MergeBoundingBoxes(List<Rect> boxes, int threshold, int minArea = 50)
        {
            if (boxes.Count == 0)
                return new List<Rect>();

            List<Rect> mergedBoxes = new List<Rect>(boxes);
            bool merged;

            do
            {
                merged = false;
                List<Rect> newBoxes = new List<Rect>();

                while (mergedBoxes.Count > 0)
                {
                    Rect current = mergedBoxes[0];
                    mergedBoxes.RemoveAt(0);

                    for (int i = 0; i < mergedBoxes.Count; i++)
                    {
                        if (IsClose(current, mergedBoxes[i], threshold))
                        {
                            current = UnionRect(current, mergedBoxes[i]);
                            mergedBoxes.RemoveAt(i);
                            merged = true;
                            i--;
                        }
                    }

                    if (current.Width * current.Height >= minArea)
                    {
                        newBoxes.Add(current);
                    }
                }

                mergedBoxes = newBoxes;
            } while (merged);

            return mergedBoxes;
        }

        // 🔹 [두 바운딩 박스가 가까운지 판단하는 함수]
        private bool IsClose(Rect box1, Rect box2, int threshold)
        {
            int centerX1 = box1.X + box1.Width / 2;
            int centerY1 = box1.Y + box1.Height / 2;
            int centerX2 = box2.X + box2.Width / 2;
            int centerY2 = box2.Y + box2.Height / 2;

            double distance = Math.Sqrt(Math.Pow(centerX1 - centerX2, 2) + Math.Pow(centerY1 - centerY2, 2));

            return distance < threshold;
        }

        // 🔹 [두 박스를 병합하는 함수]
        private Rect UnionRect(Rect box1, Rect box2)
        {
            int x = Math.Min(box1.X, box2.X);
            int y = Math.Min(box1.Y, box2.Y);
            int width = Math.Max(box1.Right, box2.Right) - x;
            int height = Math.Max(box1.Bottom, box2.Bottom) - y;

            return new Rect(x, y, width, height);
        }
    }
}
