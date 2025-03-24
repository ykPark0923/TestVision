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
        private bool  _IsCracked;

        public InspCrack()
        {
            InitializeComponent();
        }

        private Mat src = new Mat();

        private void openBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlignAndCompare();
        }

        private void AlignAndCompare()
        {
            if (src.Empty())
            {
                Console.WriteLine("이미지를 로드하세요");
                return;
            }

            // 크랙 검사
            if (IsPCBCracked(src))
            {
                textBox1.Text = "NG: Crack"; // 크랙이 감지되면 NG 출력
            }
            else
            {
                textBox1.Text = "OK"; // 정상이면 OK 출력
            }
        }

        // 크랙 감지 함수
        private bool IsPCBCracked(Mat src)
        {
            Mat gray = new Mat();
            Mat binary = new Mat();
            Mat morp = new Mat();
            Mat image = new Mat();
            Mat dst = src.Clone(); // 원본 이미지 복사하여 출력용으로 사용

            Mat Kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(2, 2));
            Point[][] contours;

            // 1. 그레이스케일 변환
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            //2. 이진화 (PCB의 밝은 영역 강조)
            //Cv2.AdaptiveThreshold(gray, binary, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 3, 1);
            Cv2.Threshold(gray, binary, 50, 255, ThresholdTypes.Binary);

            // 3. 모폴로지 연산 (잡음 제거 및 선명한 윤곽 생성)
            Cv2.MorphologyEx(binary, morp, MorphTypes.Close, Kernel, new Point(-1, -1), 2);


            // 5. 윤곽선 검출 (외곽선만 추출)
            Cv2.FindContours(morp, out contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length > 0)
            {
                // 1️면적이 가장 큰 윤곽선 찾기
                Point[] largestContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).FirstOrDefault();

                // 2️윤곽선의 둘레 길이(ArcLength) 계산
                double perimeter = Cv2.ArcLength(largestContour, true);
                double epsilon = perimeter * 0.001; // 윤곽선을 단순화하는 정도

                //Console.WriteLine("Largest Contour Perimeter : " + perimeter);

                // 3️윤곽선 단순화
                Point[] approx = Cv2.ApproxPolyDP(largestContour, epsilon, true);

                // 4️윤곽선을 이미지에 그림
                Cv2.DrawContours(dst, new Point[][] { approx }, -1, new Scalar(255, 0, 0), 2, LineTypes.AntiAlias);


                pictureBox1.Image = BitmapConverter.ToBitmap(dst);

                if (perimeter > 2148 || perimeter < 2091)
                {
                    _IsCracked = true;  //크랙있음
                }
                else
                {
                    _IsCracked = false;  //크랙없음
                }
            }
            return _IsCracked;
        }

    }
}
