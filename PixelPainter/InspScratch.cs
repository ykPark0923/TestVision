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
        // 정상 이미지 경로 설정 (스크래치가 없는 원본 이미지)
        private readonly string normalImagePath = @"Image.bmp";

        public InspScratch()
        {
            InitializeComponent();

            // TextBox에 초기값 설정
            txtareaMin.Text = "10";       // 최소 면적 초기값
            txtareaMax.Text = "600";      // 최대 면적 초기값
            txtminAspectRatio.Text = "2"; // 최소 비율 초기값
            txtmaxAspectRatio.Text = "25"; // 최대 비율 초기값

            // 정상 이미지를 로드하여 src2 변수에 저장
            src2 = Cv2.ImRead(normalImagePath);
            // 이미지를 제대로 로드하지 못했다면 종료
            if (src2.Empty()) return;

            // 정상 이미지를 pictureBox2에 표시
            pictureBox2.Image = BitmapConverter.ToBitmap(src2);
        }

        // 이미지들을 저장할 변수들 선언
        private Mat src = new Mat();   // 현재 불러온 이미지
        private Mat src2 = new Mat();  // 정상 이미지
        private Mat diffImage = new Mat();  // 이미지 차이 저장 변수

        // 열기 버튼 클릭 시 실행되는 이벤트 핸들러
        private void openBTN_Click(object sender, EventArgs e)
        {
            // 파일 열기 다이얼로그에서 이미지를 선택하면 실행
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 선택된 파일을 src 변수에 로드
                src = Cv2.ImRead(openFileDialog1.FileName);
                // 불러온 이미지를 pictureBox1에 표시
                pictureBox1.Image = BitmapConverter.ToBitmap(src);
            }
        }

        // 차이 비교 버튼 클릭 시 실행되는 이벤트 핸들러
        private void diffBTN_Click(object sender, EventArgs e)
        {
            // 이미지를 비교하는 메서드 호출
            DiffCompare();
        }
        private void DiffCompare()
        {
            // 두 이미지의 차이를 계산하여 diffImage에 저장
            diffImage = new Mat();
            Cv2.Absdiff(src, src2, diffImage);

            // 차이 이미지를 기반으로 스크래치를 탐지하는 함수 호출
            detectScratch();
        }

        private void detectScratch()
        {
            // TextBox에서 면적 및 비율 값을 읽어옵니다.
            int minArea = int.Parse(txtareaMin.Text);   // 최소 면적
            int maxArea = int.Parse(txtareaMax.Text);   // 최대 면적
            float minAspectRatio = float.Parse(txtminAspectRatio.Text); // 최소 비율
            float maxAspectRatio = float.Parse(txtmaxAspectRatio.Text); // 최대 비율

            int roiWidth = diffImage.Cols * 80 / 100;
            int roiHeight = diffImage.Rows * 80 / 100;

            int x = (diffImage.Cols - roiWidth) / 2;
            int y = (diffImage.Rows - roiHeight) / 2;

            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);

            Mat roiImage = new Mat(diffImage, innerROI);

            Mat grayDiff = new Mat();
            Cv2.CvtColor(roiImage, grayDiff, ColorConversionCodes.BGR2GRAY);

            Mat binaryDiff = new Mat();
            Cv2.Threshold(grayDiff, binaryDiff, 50, 255, ThresholdTypes.Binary);

            pictureBox2.Image = BitmapConverter.ToBitmap(binaryDiff);

            Mat kernel = new Mat(5, 5, MatType.CV_8U, Scalar.All(30));

            Mat closedDiff = new Mat();
            Cv2.MorphologyEx(binaryDiff, closedDiff, MorphTypes.Close, kernel);

            pictureBox3.Image = BitmapConverter.ToBitmap(closedDiff);

            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(closedDiff, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            bool scratchDetected = false;
            Mat resultImage = src.Clone();

            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area >= minArea && area <= maxArea)  // 면적 조건 수정
                {
                    RotatedRect box = Cv2.MinAreaRect(contour);
                    float aspectRatio = Math.Max(box.Size.Width, box.Size.Height) / Math.Min(box.Size.Width, box.Size.Height);

                    if (aspectRatio >= minAspectRatio && aspectRatio <= maxAspectRatio)  // 비율 조건 수정
                    {
                        Rect boundingBox = Cv2.BoundingRect(contour);
                        boundingBox.X += x;
                        boundingBox.Y += y;

                        Cv2.Rectangle(resultImage, boundingBox, new Scalar(0, 0, 255), 2);
                        scratchDetected = true;
                    }
                }
            }

            pictureBox1.Image = BitmapConverter.ToBitmap(resultImage);

            textBox1.Text = scratchDetected ? "NG: Scratch" : "OK";
        }
    }
}
