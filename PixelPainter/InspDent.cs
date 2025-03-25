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
        private readonly string normalImagePath = @"C:\devSJ\C#_MarchProject\PixelPainter\bin\Debug\Image.bmp";
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

            // 1️⃣ 정상 이미지와 검사 이미지의 차이 계산
            diffImage = new Mat();
            Cv2.Absdiff(src, src2, diffImage);

            // 2️⃣ 그레이스케일 변환
            Mat grayDiff = new Mat();
            Cv2.CvtColor(diffImage, grayDiff, ColorConversionCodes.BGR2GRAY);

            // 3️⃣ 이진화 (Dent 부분만 강조)
            Mat binaryDiff = new Mat();
            Cv2.Threshold(grayDiff, binaryDiff, 30, 255, ThresholdTypes.Binary); // Dent가 흰색으로 남음

            // 4️⃣ 외곽 영역 제거 (중심부만 남김)
            int roiWidth = diffImage.Cols * 86 / 100;   // 중심 너비 (50%)
            int roiHeight = diffImage.Rows * 79 / 100;  // 중심 높이 (50%)
            int x = (diffImage.Cols - roiWidth) / 2;
            int y = (diffImage.Rows - roiHeight) / 2;
            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);

            // 외곽을 검정색으로 덮기 (중심만 남김)
            Mat mask = Mat.Zeros(binaryDiff.Size(), MatType.CV_8UC1);
            Cv2.Rectangle(mask, innerROI, new Scalar(255), -1); // 중심을 흰색으로 유지
            Cv2.BitwiseAnd(binaryDiff, mask, binaryDiff); // 중심 부분만 유지

            // 5️⃣ 윤곽선 검출 → Dent 영역 찾기
            OpenCvSharp.Point[][] dentContours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binaryDiff, out dentContours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            bool dentDetected = false;
            Mat resultImage = src.Clone();

            foreach (var contour in dentContours)
            {
                double area = Cv2.ContourArea(contour);
                if (area > 3)  // 일정 크기 이상의 Dent만 감지
                {
                    Rect boundingBox = Cv2.BoundingRect(contour);
                    Cv2.Rectangle(resultImage, boundingBox, new Scalar(0, 255, 255), 2); // 빨간색 박스 표시
                    dentDetected = true;
                }
            }

            // 6️⃣ 결과 표시
            pbNG.Image = BitmapConverter.ToBitmap(resultImage);

            // Dent 검출 여부 출력
            txtResult.Text = dentDetected ? "NG: Dent Detected" : "OK";
        }
    }
}
