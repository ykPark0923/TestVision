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

        // 두 이미지를 비교하는 메서드
        private void DiffCompare()
        {
            // 이미지가 제대로 로드되지 않으면 오류 메시지 출력 후 종료
            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            // 두 이미지의 차이를 계산하여 diffImage에 저장
            diffImage = new Mat();
            Cv2.Absdiff(src, src2, diffImage);

            // 차이 이미지를 기반으로 스크래치를 탐지하는 함수 호출
            detectScratch();
        }

        // 스크래치를 탐지하는 메서드
        private void detectScratch()
        {
            // 이미지 크기 계산
            int roiWidth = diffImage.Cols * 80 / 100;   // 전체 너비의 80%
            int roiHeight = diffImage.Rows * 80 / 100;  // 전체 높이의 80%

            // 중심을 기준으로 ROI (Region of Interest, 관심 영역) 좌표 설정
            int x = (diffImage.Cols - roiWidth) / 2;  // 중앙 정렬 X 좌표
            int y = (diffImage.Rows - roiHeight) / 2; // 중앙 정렬 Y 좌표

            // ROI 객체 생성 (스크래치를 탐지할 영역)
            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);

            // 안쪽 영역만 잘라낸 이미지를 생성
            Mat roiImage = new Mat(diffImage, innerROI);

            // 그레이스케일 변환 (이진화 및 윤곽선 검출을 위한 전처리)
            Mat grayDiff = new Mat();
            Cv2.CvtColor(roiImage, grayDiff, ColorConversionCodes.BGR2GRAY);

            // 이진화 (Threshold 적용하여 차이 이미지를 흑백으로 변환)
            Mat binaryDiff = new Mat();
            Cv2.Threshold(grayDiff, binaryDiff, 50, 255, ThresholdTypes.Binary);

            // 윤곽선 검출 (차이 이미지에서 물체의 경계를 찾음)
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binaryDiff, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            bool scratchDetected = false;  // 스크래치 여부를 추적할 변수
            Mat resultImage = src.Clone();  // 결과를 저장할 이미지 (원본 이미지의 복사본)

            // 검출된 모든 윤곽선에 대해 반복
            foreach (var contour in contours)
            {
                // 각 윤곽선의 면적 계산
                double area = Cv2.ContourArea(contour);
                // 면적이 너무 작거나 너무 큰 윤곽선은 제외
                if (area > 10 && area < 500)
                {
                    // 윤곽선의 경계를 직선 형태로 만드는 최소 영역을 계산 (스크래치 형태가 직선일 것이라 가정)
                    RotatedRect box = Cv2.MinAreaRect(contour);
                    // 직사각형의 가로 세로 비율 계산
                    float aspectRatio = Math.Max(box.Size.Width, box.Size.Height) / Math.Min(box.Size.Width, box.Size.Height);

                    // 직선 같은 형태만 스크래치로 판단하기 위해 비율 범위 설정
                    float minAspectRatio = 3;  // 최소 비율 (예: 길이가 너비보다 3배 이상)
                    float maxAspectRatio = 10;  // 최대 비율 (예: 길이가 너비보다 10배 이하)

                    // 비율이 지정 범위 내에 있을 경우에만 스크래치로 판단
                    if (aspectRatio >= minAspectRatio && aspectRatio <= maxAspectRatio)
                    {
                        // ROI 영역 내에서 좌표를 실제 이미지 좌표로 변환
                        Rect boundingBox = Cv2.BoundingRect(contour);
                        boundingBox.X += x;  // ROI의 X 좌표를 더하여 실제 이미지 좌표로 변환
                        boundingBox.Y += y;  // ROI의 Y 좌표를 더하여 실제 이미지 좌표로 변환

                        // 결과 이미지에 스크래치 부분에 파란색 박스 그리기
                        Cv2.Rectangle(resultImage, boundingBox, new Scalar(255, 0, 0), 2);
                        scratchDetected = true;  // 스크래치가 감지되었음을 표시
                    }
                }
            }

            // 결과 이미지를 pictureBox1에 표시
            pictureBox1.Image = BitmapConverter.ToBitmap(resultImage);

            // 스크래치가 감지되었으면 "NG: Scratch" 표시, 아니면 "OK"
            textBox1.Text = scratchDetected ? "NG: Scratch" : "OK";
        }
    }
}
