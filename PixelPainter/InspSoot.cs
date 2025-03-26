using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PixelPainter
{
    public partial class InspSoot : Form
    {
        private readonly string normalImagePath = @"C:\devSJ\C#_MarchProject\PixelPainter\bin\Debug\Image.bmp"; //정상 이미지 경로
        private Mat src = new Mat();
        private Mat src2 = new Mat();
        private Mat diffImage = new Mat();

        public InspSoot()
        {
            InitializeComponent();

            // 정상이미지를 경로로 고정
            src2 = Cv2.ImRead(normalImagePath);
            if (src2.Empty()) return;

            pictureBox2.Image = BitmapConverter.ToBitmap(src2);
        }

        private void openBTN1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src);
            }
        }

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

            detectSoot();
        }

        private void detectSoot()
        {
            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            Mat gray1 = new Mat();
            Mat gray2 = new Mat();
            Cv2.CvtColor(src, gray1, ColorConversionCodes.BGR2GRAY);
            Cv2.CvtColor(src2, gray2, ColorConversionCodes.BGR2GRAY);

            // 1. 히스토그램 균등화: 이미지를 균등화하여 대비를 개선
            Cv2.EqualizeHist(gray1, gray1);
            Cv2.EqualizeHist(gray2, gray2);

            // 2. 블러 적용하여 노이즈 제거
            Cv2.GaussianBlur(gray1, gray1, new OpenCvSharp.Size(5, 5), 0);
            Cv2.GaussianBlur(gray2, gray2, new OpenCvSharp.Size(5, 5), 0);

            // 3. 밝기 증가 부분 강조 (Soot 검출)
            Mat diffImage = new Mat();
            Cv2.Subtract(gray2, gray1, diffImage); // 밝아진 부분 강조

            // 4. 연한 Soot도 잡을 수 있게 임계값을 낮추어 적용
            Mat sootMask = new Mat();
            Cv2.Threshold(diffImage, sootMask, 10, 255, ThresholdTypes.Binary); // 10으로 낮춰서 연한 soot도 잡기

            // 5. Soot와 배경의 경계를 분명히 하기 위해 Canny 엣지 검출 적용
            Mat edges = new Mat();
            Cv2.Canny(sootMask, edges, 100, 200);

            Cv2.ImShow("Canny Edges", edges);

            Cv2.WaitKey(5);

            // 6. 배경(어두운 부분) 제거 (PCB의 검정색 배경 제외)
            Mat bgMask = new Mat();
            Cv2.Threshold(gray1, bgMask, 37, 255, ThresholdTypes.BinaryInv);

            // 7. 중심 부분만 남기기 (외곽 영역 제거)
            int roiWidth = diffImage.Cols * 80 / 100;   // 중심 너비 (80%)
            int roiHeight = diffImage.Rows * 79 / 100;  // 중심 높이 (79%)
            int x = (diffImage.Cols - roiWidth) / 2;
            int y = (diffImage.Rows - roiHeight) / 2;
            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);

            // 외곽을 제거하기 위해 mask 생성
            Mat mask = Mat.Zeros(sootMask.Size(), MatType.CV_8UC1); // sootMask 크기로 마스크 생성
            Cv2.Rectangle(mask, innerROI, new Scalar(255), -1); // 중심 영역을 흰색으로 유지

            // 8. 중심 부분만 남기고 외곽은 제거
            Cv2.BitwiseAnd(sootMask, mask, sootMask); // 최종 Soot 영역에서 외곽을 제거

            // 9. 최종 Soot 검출 결과
            Mat finalMask = new Mat();
            Cv2.BitwiseAnd(sootMask, bgMask, finalMask);

            // 10. 윤곽선 검출
            OpenCvSharp.Point[][] contours;
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
                            // 두 박스를 병합
                            current = UnionRect(current, mergedBoxes[i]);
                            mergedBoxes.RemoveAt(i);
                            merged = true;
                            i--; // 리스트 크기가 줄어들므로 인덱스 조정
                        }
                    }

                    // 최소 크기 기준을 만족하는 박스만 추가
                    if (current.Width * current.Height >= minArea)
                    {
                        newBoxes.Add(current);
                    }
                }

                mergedBoxes = newBoxes;
            } while (merged); // 더 이상 병합이 발생하지 않을 때까지 반복

            return mergedBoxes;
        }

        // 두 바운딩 박스가 가까운지 판단하는 함수 (중심점 거리 기준)
        private bool IsClose(Rect box1, Rect box2, int threshold)
        {
            int centerX1 = box1.X + box1.Width / 2;
            int centerY1 = box1.Y + box1.Height / 2;
            int centerX2 = box2.X + box2.Width / 2;
            int centerY2 = box2.Y + box2.Height / 2;

            double distance = Math.Sqrt(Math.Pow(centerX1 - centerX2, 2) + Math.Pow(centerY1 - centerY2, 2));

            return distance < threshold;
        }

        // 두 박스를 병합하는 함수
        private Rect UnionRect(Rect box1, Rect box2)
        {
            int x = Math.Min(box1.X, box2.X);
            int y = Math.Min(box1.Y, box2.Y);
            int width = Math.Max(box1.X + box1.Width, box2.X + box2.Width) - x;
            int height = Math.Max(box1.Y + box1.Height, box2.Y + box2.Height) - y;

            return new Rect(x, y, width, height);
        }
    }
}
