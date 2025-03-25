using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PixelPainter
{
    public partial class InspScratch : Form
    {

        private readonly string normalImagePath = @"Image.bmp";

        public InspScratch()
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

            detectScratch();
        }

        private void detectScratch()
        {
            // 그레이스케일 변환
            Mat grayDiff = new Mat();
            Cv2.CvtColor(diffImage, grayDiff, ColorConversionCodes.BGR2GRAY);

            // 이진화 (Threshold 적용)
            Mat binaryDiff = new Mat();
            Cv2.Threshold(grayDiff, binaryDiff, 50, 255, ThresholdTypes.Binary);

            // 윤곽선 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binaryDiff, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            bool scratchDetected = false;
            Mat resultImage = src.Clone();

            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > 10 && area < 500)  // 일정 크기 범위 내에서 스크래치만 감지
                {
                    // 윤곽선의 경계 직선화 (스크래치 형태는 직선으로 간주)
                    RotatedRect box = Cv2.MinAreaRect(contour);
                    float aspectRatio = Math.Max(box.Size.Width, box.Size.Height) / Math.Min(box.Size.Width, box.Size.Height);

                    // 직선처럼 긴 형태만 스크래치로 감지
                    if (aspectRatio > 5)  // 길이가 넓이보다 5배 이상일 때 직선 형태로 인식
                    {
                        Rect boundingBox = Cv2.BoundingRect(contour);
                        Cv2.Rectangle(resultImage, boundingBox, new Scalar(0, 0, 255), 2); // 빨간색 박스 그리기
                        scratchDetected = true;
                    }
                }
            }

            // 결과 표시
            pictureBox1.Image = BitmapConverter.ToBitmap(resultImage);

            // 스크래치가 감지되었다면 NG 처리
            textBox1.Text = scratchDetected ? "NG: Scratch" : "OK";
        }
    }
}
