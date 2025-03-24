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
        private bool _IsCracked;
        public InspCrack()
        {
            InitializeComponent();
        }

        private Mat src = new Mat();
        private Mat src2 = new Mat();
        private Mat aligned1 = new Mat();
        private Mat aligned2 = new Mat();
        private Mat diffImage = new Mat();

        private void openBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src = Cv2.ImRead(openFileDialog1.FileName);
                src = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src);
            }
        }
        private void openBTN2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                src2 = Cv2.ImRead(openFileDialog2.FileName);
                pictureBox2.Image = BitmapConverter.ToBitmap(src2);
            }
        }
        private void contourBTN_Click(object sender, EventArgs e)
        {
            if (src.Empty())
            {
                Console.WriteLine("이미지를 로드하세요");
                return;
            }

            // 크랙 검사 HoughLine
            //if (IsPCBCracked(src))

            if (IsPCBCracked(src))
            {
                textBox1.Text = "NG: Crack"; // 크랙이 감지되면 NG 출력
            }
            else
            {
                textBox1.Text = "OK"; // 정상이면 OK 출력
            }
        }
        private void diffBTN_Click(object sender, EventArgs e)
        {
            AlignAndCompare();
        }
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

                if (perimeter <= 2150 && perimeter >= 2090)
                {
                    _IsCracked = false;  //크랙없음
                    // 4️윤곽선을 이미지에 그림
                    Cv2.DrawContours(dst, new Point[][] { approx }, -1, new Scalar(255, 0, 0), 2, LineTypes.AntiAlias);
                }
                else
                {
                    _IsCracked = true;  //크랙있음
                    // 4️윤곽선을 이미지에 그림
                    Cv2.DrawContours(dst, new Point[][] { approx }, -1, new Scalar(0, 0, 255), 2, LineTypes.AntiAlias);
                }

                pictureBox1.Image = BitmapConverter.ToBitmap(dst);
            }
            return _IsCracked;
        }
        private void AlignAndCompare()
        {
            if (src.Empty() || src2.Empty())
            {
                MessageBox.Show("이미지를 먼저 로드하세요.");
                return;
            }

            // PCB 정렬
            aligned1 = AlignPCB(src);
            aligned2 = AlignPCB(src2);

            // 정렬된 이미지 표시
            Cv2.ImShow("Aligned Image 1", aligned1);
            Cv2.ImShow("Aligned Image 2", aligned2);

            // 크기 맞추기 (정렬 후 이미지 크기가 다를 경우 대비)
            //if (aligned1.Size() != aligned2.Size())
            //{
            //    Cv2.Resize(aligned2, aligned2, aligned1.Size());
            //}

            // 이미지 비교 (Bitwise XOR 활용)
            diffImage = new Mat();
            Cv2.Absdiff(aligned1, aligned2, diffImage);

            // 비교 결과 표시
            Cv2.ImShow("Difference", diffImage);
            //pictureBox2.Image = BitmapConverter.ToBitmap(diffImage);

            Cv2.WaitKey(0);
        }
        private Point2f[] OrderPoints(Point[] points)
        {
            Point2f[] ordered = new Point2f[4];

            // X 좌표 기준으로 정렬
            var sortedX = points.OrderBy(p => p.X).ToArray();
            var left = sortedX.Take(2).OrderBy(p => p.Y).ToArray();  // 좌측 상/하
            var right = sortedX.Skip(2).OrderBy(p => p.Y).ToArray(); // 우측 상/하

            ordered[0] = left[0];  // 좌상단
            ordered[1] = right[0]; // 우상단
            ordered[2] = right[1]; // 우하단
            ordered[3] = left[1];  // 좌하단

            return ordered;
        }
        private Mat AlignPCB(Mat src)
        {
            Mat gray = new Mat();
            Mat binary = new Mat();
            Mat result = new Mat();

            // 1. 그레이스케일 변환
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            // 2. 적응형 이진화 OR Otsu 이진화 (조명 상태에 따라 선택)
            Cv2.AdaptiveThreshold(gray, binary, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 11, 2);
            // Cv2.Threshold(gray, binary, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);  // Otsu 이진화 사용 가능

            // 3. 컨투어 검출
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 4. 가장 큰 컨투어 찾기 (면적 기준)
            Point[] largestContour = contours.OrderByDescending(c => Cv2.ContourArea(c)).FirstOrDefault();
            if (largestContour == null || Cv2.ContourArea(largestContour) < 100) return src;

            // 5. 컨투어 근사화하여 4개 점 찾기
            Point[] approx = Cv2.ApproxPolyDP(largestContour, 0.02 * Cv2.ArcLength(largestContour, true), true);

            // 6. Convex Hull 보정 후 다시 시도
            if (approx.Length != 4)
            {
                approx = Cv2.ConvexHull(approx);
            }

            // 7. 여전히 4개가 아니라면 바운딩 박스 사용
            if (approx.Length != 4)
            {
                Console.WriteLine("Crack");
                Rect boundingBox = Cv2.BoundingRect(largestContour);
                approx = new Point[]
                {
            new Point(boundingBox.Left, boundingBox.Top),
            new Point(boundingBox.Right, boundingBox.Top),
            new Point(boundingBox.Right, boundingBox.Bottom),
            new Point(boundingBox.Left, boundingBox.Bottom)
                };
            }

            // 8. 투시 변환 적용
            if (approx.Length == 4)
            {
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
                MessageBox.Show("정확한 꼭지점을 찾을 수 없어 원본 이미지를 반환합니다.");
                return src;
            }
        }        
    }
}