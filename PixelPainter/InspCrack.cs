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

        private readonly string normalImagePath = @"Image.bmp";

        public InspCrack()
        {
            InitializeComponent();

            // 정상이미지를 경로로 고정
            src2 = Cv2.ImRead(normalImagePath);
            if (src2.Empty()) return;
            
            pictureBox2.Image = BitmapConverter.ToBitmap(src2);
        }

        private Mat src = new Mat();
        private Mat src2 = new Mat();
        private Mat temp = new Mat();  //윤곽선 그리기 전 이미지를 임시저장
        private Mat aligned1 = null;
        private Mat aligned2 = null;
        private Mat diffImage = new Mat();

        private void openBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src);
            }
        }        
        //크랙이미지 윤곽선 그리기
        private void diffBTN_Click(object sender, EventArgs e)
        {
            AlignAndCompare();
        }
        private void AlignAndCompare()
        {
            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            // Crack 불량 판단
            if ((IsCrackByContour(src) || IsCrackByEdge(src)) || aligned1 == null)
            {

                IsPCBCracked(src);
                textBox1.Text = "NG: Crack";
                return;
            }

            //src2 윤곽선 그리기전 이미지 임시저장
            temp = src2.Clone();

            // PCB 정렬
            aligned1 = AlignPCB(src);
            aligned2 = AlignPCB(src2);
            src2 = temp.Clone();

            pictureBox1.Image = BitmapConverter.ToBitmap(aligned1);
            pictureBox2.Image = BitmapConverter.ToBitmap(aligned2);

            // 이미지 비교 (절대차)
            diffImage = new Mat();
            Cv2.Absdiff(aligned1, aligned2, diffImage);
            Cv2.ImShow("Difference", diffImage);
            textBox1.Text = "OK";
        }
        
        // 외곽선 둘레 정상이미지 둘레와 비교
        private void IsPCBCracked(Mat src)
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

                Cv2.DrawContours(dst, new Point[][] { approx }, -1, new Scalar(0, 0, 255), 2, LineTypes.AntiAlias);
                pictureBox1.Image = BitmapConverter.ToBitmap(dst);
            }
        }

        //수평, 수직 2개 직선보다 적으면 크랙
        private bool IsCrackByEdge(Mat src)
        {
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 1.5); // 노이즈 제거

            Mat edge = new Mat();
            Cv2.Canny(gray, edge, 100, 200);
            Cv2.ImShow("edge", edge);

            LineSegmentPoint[] lines = Cv2.HoughLinesP(edge, 0.5, Math.PI / 180, 80, 100, 10);

            //Console.WriteLine("line"+lines.Length);


            // 방향별로 필터링
            int horizontal = 0;
            int vertical = 0;

            foreach (var line in lines)
            {
                double dx = line.P2.X - line.P1.X;
                double dy = line.P2.Y - line.P1.Y;
                double angle = Math.Atan2(dy, dx) * 180.0 / Math.PI;

                double length = Math.Sqrt(dx * dx + dy * dy);
                if (length < 100) continue; // 짧은 선 무시

                // 수평 또는 수직으로 간주 (허용 각도 오차 ±10도)
                if (Math.Abs(angle) < 10 || Math.Abs(angle) > 170)
                    horizontal++;
                else if (Math.Abs(angle - 90) < 10 || Math.Abs(angle + 90) < 10)
                    vertical++;
            }

            //Console.WriteLine("horizontal" + horizontal);
            //Console.WriteLine("vertical" + vertical);

            // 수평 2개 + 수직 2개 이상이어야 정상
            return (horizontal < 2 || vertical < 2); // 부족하면 Crack
        }

        //꼭짓점 4개 아니면 크랙
        private bool IsCrackByContour(Mat src)
        {
            Mat gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 1.5);

            Mat binary = new Mat();
            Cv2.Threshold(gray, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

            // 윤곽선 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            if (contours.Length == 0) return true; // 윤곽 없으면 crack

            var largest = contours.OrderByDescending(c => Cv2.ContourArea(c)).First();
            double arcLen = Cv2.ArcLength(largest, true);
            Point[] approx = Cv2.ApproxPolyDP(largest, 0.02 * arcLen, true);

            // 디버깅용 윤곽선 보기
            Mat debug = src.Clone();
            Cv2.DrawContours(debug, new[] { approx }, -1, Scalar.Red, 2);
            Cv2.ImShow("Contour", debug);

            // 핵심 판단: 꼭짓점 4개면 정상, 아니면 crack
            return approx.Length != 4;
        }
        private Mat AlignPCB(Mat src)
        {
            Mat gray = new Mat();
            Mat binary = new Mat();
            Mat result = new Mat();

            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.AdaptiveThreshold(gray, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 11, 2);

            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            Point[] largestContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).FirstOrDefault();
            if (largestContour == null || Cv2.ContourArea(largestContour) < 100) return src;

            Point[] approx = Cv2.ApproxPolyDP(largestContour, 0.02 * Cv2.ArcLength(largestContour, true), true);

            if (approx.Length != 4)
            {
                approx = Cv2.ConvexHull(approx);
            }

            if (approx.Length == 4)
            {
                Cv2.Polylines(src, new[] { approx }, true, Scalar.Red, 2);

                Point2f[] srcPoints = OrderPoints(approx);
                Point2f[] dstPoints =
                {
                    new Point2f(0, 0),
                    new Point2f(src.Cols - 1, 0),
                    new Point2f(src.Cols - 1, src.Rows - 1),
                    new Point2f(0, src.Rows - 1)
                };

                Mat perspectiveMatrix = Cv2.GetPerspectiveTransform(srcPoints, dstPoints);
                Cv2.WarpPerspective(src, result, perspectiveMatrix, src.Size());

                return result;
            }
            else
            {
                return null;
            }
        }
        private Point2f[] OrderPoints(Point[] points)
        {
            Point2f[] ordered = new Point2f[4];
            var sortedX = points.OrderBy(p => p.X).ToArray();
            var left = sortedX.Take(2).OrderBy(p => p.Y).ToArray();
            var right = sortedX.Skip(2).OrderBy(p => p.Y).ToArray();

            ordered[0] = left[0];
            ordered[1] = right[0];
            ordered[2] = right[1];
            ordered[3] = left[1];

            return ordered;
        }

    }
}