using System;
using System.Collections.Generic;
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
            DiffCompare();
        }
        private void DiffCompare()
        {
            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }


            //이미지 윤곽선 그리기전 이미지 임시저장
            Mat temp1 = src.Clone();
            Mat temp2 = src2.Clone();

            // PCB 정렬
            Mat aligned1 = DetectCirclesUsingBlobs(src, src2);
            Mat aligned2 = DetectCirclesUsingBlobs(src2, src2);

            if (aligned1 != null && aligned2 != null)
            {
                src = temp1.Clone();
                src2 = temp2.Clone();
            }
            else
            {
                Console.WriteLine("align 미검.");
            }

            diffImage = new Mat();
            Cv2.Absdiff(aligned1, aligned2, diffImage);

            Cv2.ImShow("diffImage", diffImage);

            //detectCrack();
        }
        private void alignBTN_Click(object sender, EventArgs e)
        {
            Mat temp1 = new Mat();  //윤곽선 그리기 전 이미지를 임시저장
            Mat temp2 = new Mat();  //윤곽선 그리기 전 이미지를 임시저장
            Mat aligned1 = null;
            Mat aligned2 = null;

            if (src.Empty() || src2.Empty())
            {
                Console.WriteLine("이미지를 먼저 로드하세요.");
                return;
            }

            //이미지 윤곽선 그리기전 이미지 임시저장
            temp1 = src.Clone();
            temp2 = src2.Clone();

            // PCB 정렬
            aligned1 = DetectCirclesUsingBlobs(src, src2);
            aligned2 = DetectCirclesUsingBlobs(src2, src2);

            if (aligned1 != null && aligned2 != null)
            {
                //Cv2.ImShow("align1", aligned1);
                //Cv2.ImShow("align2", aligned2);
                src = temp1.Clone();
                src2 = temp2.Clone();
            }
            else
            {
                Console.WriteLine("align 미검.");
            }

        }
        private Mat DetectCirclesUsingBlobs(Mat src, Mat src2)
        {
            Mat gray = new Mat();
            Mat result = new Mat();

            // 1) 그레이스케일 변환
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            #region 좌우상하 원검출을 위해 안쪽영역 지움
            int roiWidth = src.Cols * 75 / 100;
            int roiHeight = src.Rows * 100 / 100;
            int x = (src.Cols - roiWidth) / 2;
            int y = (src.Rows - roiHeight) / 2;
            Rect innerROI = new Rect(x, y, roiWidth, roiHeight);
            Cv2.Rectangle(gray, innerROI, new Scalar(0), -1);

            roiWidth = src.Cols * 100 / 100;
            roiHeight = src.Rows * 70 / 100;
            x = (src.Cols - roiWidth) / 2;
            y = (src.Rows - roiHeight) / 2;
            innerROI = new Rect(x, y, roiWidth, roiHeight);
            Cv2.Rectangle(gray, innerROI, new Scalar(0), -1);
            #endregion

            // 2) 이진화 (Threshold)
            Cv2.Threshold(gray, gray, 30, 255, ThresholdTypes.Binary);
            Cv2.ImShow("GaussianBlur", gray);

            // 3) 윤곽선(contour) 탐색
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(gray, out contours, out hierarchy,RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            // 4) 검은색 영역(컨투어)들의 중심을 저장할 리스트
            List<Point2f> centerPoints = new List<Point2f>();

            for (int i = 0; i < contours.Length; i++)
            {
                double area = Cv2.ContourArea(contours[i]);
                if (area < 5)  // 노이즈 제거
                    continue;

                Moments m = Cv2.Moments(contours[i]);
                if (Math.Abs(m.M00) < 1e-5)
                    continue;

                double cx = m.M10 / m.M00;
                double cy = m.M01 / m.M00;
                centerPoints.Add(new Point2f((float)cx, (float)cy));

                // 원본에 빨간 점으로 표시
                Cv2.Circle(src, new OpenCvSharp.Point((int)cx, (int)cy),
                           5, new Scalar(0, 0, 255), -1);
            }

            // 5) 표시된 결과 이미지 보기
            Cv2.ImShow("Center", src);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            // 최소 4개의 원이 검출되었는지 확인
            if (centerPoints.Count < 4)
            {
                Console.WriteLine("Error: 원이 4개 검출되지 않음!");
                return null;
            }

            // 6) 중심점들 정렬 (좌상단, 우상단, 우하단, 좌하단 순)
            Point2f[] sortedCenters = SortCenters(centerPoints);

            // 7) 패딩 적용 및 투시 변환
            int paddingX = 30;
            int paddingY = 30;

            Point2f[] dstPoints =
            {
                new Point2f(paddingX, paddingY),
                new Point2f(src2.Cols - 1 - paddingX, paddingY),
                new Point2f(src2.Cols - 1 - paddingX, src2.Rows - 1 - paddingY),
                new Point2f(paddingX, src2.Rows - 1 - paddingY)
            };

            Mat perspectiveMatrix = Cv2.GetPerspectiveTransform(sortedCenters, dstPoints);
            Cv2.WarpPerspective(src, result, perspectiveMatrix, new Size(src2.Cols, src2.Rows));

            return result;
        }


        // 원 중심점을 좌상단, 우상단, 우하단, 좌하단 순으로 정렬
        private Point2f[] SortCenters(List<Point2f> centers)
        {
            var sortedX = centers.OrderBy(p => p.X).ToArray();
            var left = sortedX.Take(2).OrderBy(p => p.Y).ToArray();
            var right = sortedX.Skip(2).OrderBy(p => p.Y).ToArray();

            return new Point2f[]
            {
                left[0],   // 좌상단
                right[0],  // 우상단
                right[1],  // 우하단
                left[1]    // 좌하단
            };
        }
    }
}