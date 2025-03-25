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
            // 이미지 크기 계산
            int roiWidth = diffImage.Cols * 80 / 100;   // 전체 너비의 80%
            int roiHeight = diffImage.Rows * 80 / 100;  // 전체 높이의 80%

            // 중심을 기준으로 ROI 좌표 설정
            int x = (diffImage.Cols - roiWidth) / 2;  // 중앙 정렬 X 좌표
            int y = (diffImage.Rows - roiHeight) / 2; // 중앙 정렬 Y 좌표

            // ROI 생성
            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);

            // 안쪽 영역만 자르기 (ROI에 맞게 이미지를 잘라냄)
            Mat roiImage = new Mat(diffImage, innerROI);

            // 그레이스케일 변환
            Mat grayDiff = new Mat();
            Cv2.CvtColor(roiImage, grayDiff, ColorConversionCodes.BGR2GRAY);

            // 이진화 (Threshold 적용)
            Mat binaryDiff = new Mat();
            Cv2.Threshold(grayDiff, binaryDiff, 50, 255, ThresholdTypes.Binary);

            // 윤곽선 검출 (ROI 영역 내에서만)
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
                    float minAspectRatio = 3;  // 최소 비율 (예: 길이가 너비보다 3배 이상)
                    float maxAspectRatio = 10;  // 최대 비율 (예: 길이가 너비보다 10배 이하)

                    if (aspectRatio >= minAspectRatio && aspectRatio <= maxAspectRatio)  // 길이가 최소 n배 이상, 최대 n배 이하일 때 직선 형태로 인식
                    {
                        // ROI 영역 내에서 좌표를 계산하여 실제 이미지에 표시할 때 조정
                        Rect boundingBox = Cv2.BoundingRect(contour);
                        boundingBox.X += x;  // ROI의 X 좌표를 더하여 실제 이미지의 좌표로 변환
                        boundingBox.Y += y;  // ROI의 Y 좌표를 더하여 실제 이미지의 좌표로 변환

                        // 실제 이미지에 파란색 박스 그리기
                        Cv2.Rectangle(resultImage, boundingBox, new Scalar(255, 0, 0), 2);
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
