using System;
using System.Linq;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace PixelPainter
{
    public partial class editIMG : Form
    {
        public editIMG()
        {
            InitializeComponent();
        }

        private bool isImageLoaded = false;
        private Mat src1 = new Mat();
        private Mat src2 = new Mat();
        private Mat aligned1 = new Mat();
        private Mat aligned2 = new Mat();
        private Mat diffImage = new Mat();

        private void openBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                src1 = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src1);
                isImageLoaded = true;
            }
        }

        private void openBTN2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                src2 = Cv2.ImRead(openFileDialog2.FileName);
                pictureBox2.Image = BitmapConverter.ToBitmap(src2);
                isImageLoaded = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlignAndCompare();
        }

        private void AlignAndCompare()
        {
            if (src1.Empty() || src2.Empty())
            {
                MessageBox.Show("이미지를 먼저 로드하세요.");
                return;
            }

            // PCB 정렬
            aligned1 = AlignPCB(src1);
            aligned2 = AlignPCB(src2);

            // 정렬된 이미지 표시
            Cv2.ImShow("Aligned Image 1", aligned1);
            Cv2.ImShow("Aligned Image 2", aligned2);

            // 크기 맞추기 (정렬 후 이미지 크기가 다를 경우 대비)
            if (aligned1.Size() != aligned2.Size())
            {
                Cv2.Resize(aligned2, aligned2, aligned1.Size());
            }

            // 이미지 비교 (Bitwise XOR 활용)
            diffImage = new Mat();
            Cv2.Absdiff(aligned1, aligned2, diffImage);

            // 비교 결과 표시
            Cv2.ImShow("Difference", diffImage);
            pictureBox2.Image = BitmapConverter.ToBitmap(diffImage);

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



#region operation

//enum ImageOperation
//{
//    OpAdd = 0,
//    OpSubtract,
//    OpMultiply,
//    OpDivide,
//    OpMax,
//    OpMin,
//    OpAbs,
//    OpAbDiff,
//    and,
//    or,
//    xor,
//    not,
//    compare,
//}

//enum ImageFilter
//{
//    FilterBlur = 0,
//    FilterBoxFilter,
//    FilterMedianBlur,
//    FilterGaussianBlur,
//    FilterBilateral,
//    FilterSobel,
//    FilterScharr,
//    FilterLaplacian,
//    FilterCanny,

//}

//enum ImageColor
//{
//    Color = 0,
//    Mono,
//}



//private void OpcomboBox_SelectedIndexChanged(object sender, EventArgs e)
//{
//    if (!isImageLoaded)
//    {
//        MessageBox.Show("먼저 이미지를 열어주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//        return;
//    }


//    ImageOperation selType = (ImageOperation)OpcomboBox.SelectedIndex;

//    switch (selType)
//    {
//        case ImageOperation.OpAdd:
//            Cv2.Add(src1, src2, dst);
//            break;
//        case ImageOperation.OpSubtract:
//            Cv2.Subtract(src1, src2, dst);
//            break;
//        case ImageOperation.OpMultiply:
//            Cv2.Multiply(src1, src2, dst);
//            break;
//        case ImageOperation.OpDivide:
//            Cv2.Divide(src1, src2, dst);
//            break;
//        case ImageOperation.OpMax:
//            Cv2.Max(src1, src2, dst);
//            break;
//        case ImageOperation.OpMin:
//            Cv2.Min(src1, src2, dst);
//            break;
//        case ImageOperation.OpAbs:
//            Cv2.Multiply(src1, src2, dst);
//            Cv2.Abs(dst);
//            break;
//        case ImageOperation.OpAbDiff:
//            Mat matMul = new Mat();
//            Cv2.Multiply(src1, src2, matMul);
//            Cv2.Absdiff(src2, matMul, dst);
//            break;
//        case ImageOperation.and:
//            Cv2.BitwiseAnd(src1, src2, dst);
//            break;
//        case ImageOperation.or:
//            Cv2.BitwiseOr(src1, src2, dst);
//            break;
//        case ImageOperation.xor:
//            Cv2.BitwiseXor(src1, src2, dst);
//            break;
//        case ImageOperation.not:
//            Cv2.BitwiseNot(src1, dst);
//            break;
//        case ImageOperation.compare:
//            Cv2.Compare(src1, src2, dst, CmpType.EQ);
//            break;
//    }

//    // OpenCvSharp의 Mat을 Bitmap으로 변환하여 PictureBox에 출력
//    pictureBox2.Image = BitmapConverter.ToBitmap(dst);
//    isImageOp = true;

//    // 연산 결과를 TextBox에 출력 (예: Mat의 픽셀 평균값)
//    Scalar mean = Cv2.Mean(dst); // Mat의 평균값 계산
//    textBox1.Text = $"Mean: {mean.Val0:F2}, {mean.Val1:F2}, {mean.Val2:F2}";

//}

//private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
//{
//    if (!isImageLoaded)
//    {
//        MessageBox.Show("먼저 이미지를 열어주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//        return;
//    }

//    Cv2.GaussianBlur(src1, blur, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);
//    Cv2.GaussianBlur(src2, blur2, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);

//    ImageFilter selType = (ImageFilter)FiltercomboBox.SelectedIndex;

//    switch (selType)
//    {
//        case ImageFilter.FilterBlur:
//            Cv2.Blur(src1, dst, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);
//            Cv2.Blur(src2, dst2, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);
//            break;
//        case ImageFilter.FilterBoxFilter:
//            Cv2.BoxFilter(src1, dst, MatType.CV_8UC3, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), true, BorderTypes.Default);
//            Cv2.BoxFilter(src2, dst2, MatType.CV_8UC3, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), true, BorderTypes.Default);
//            break;
//        case ImageFilter.FilterMedianBlur:
//            Cv2.MedianBlur(src1, dst, 9);
//            Cv2.MedianBlur(src2, dst2, 9);
//            break;
//        case ImageFilter.FilterGaussianBlur:
//            Cv2.GaussianBlur(src1, dst, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);
//            Cv2.GaussianBlur(src2, dst2, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);
//            break;
//        case ImageFilter.FilterBilateral:
//            Cv2.BilateralFilter(src1, dst, 9, 3, 3, BorderTypes.Default);
//            Cv2.BilateralFilter(src2, dst2, 9, 3, 3, BorderTypes.Default);
//            break;
//        case ImageFilter.FilterSobel:
//            Cv2.Sobel(blur, dst, MatType.CV_32F, 1, 0, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
//            Cv2.Sobel(blur2, dst2, MatType.CV_32F, 1, 0, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
//            dst.ConvertTo(dst, MatType.CV_8UC1);
//            dst2.ConvertTo(dst2, MatType.CV_8UC1);
//            break;
//        case ImageFilter.FilterScharr:
//            Cv2.Scharr(blur, dst, MatType.CV_32F, 1, 0, scale: 1, delta: 0, BorderTypes.Default);
//            Cv2.Scharr(blur2, dst2, MatType.CV_32F, 1, 0, scale: 1, delta: 0, BorderTypes.Default);
//            dst.ConvertTo(dst, MatType.CV_8UC1);
//            dst2.ConvertTo(dst2, MatType.CV_8UC1);
//            break;
//        case ImageFilter.FilterLaplacian:
//            Cv2.Laplacian(blur, dst, MatType.CV_32F, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
//            Cv2.Laplacian(blur2, dst2, MatType.CV_32F, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
//            dst.ConvertTo(dst, MatType.CV_8UC1);
//            dst2.ConvertTo(dst2, MatType.CV_8UC1);
//            break;
//        case ImageFilter.FilterCanny:
//            Cv2.Canny(blur, dst, 100, 200, 3, true);
//            Cv2.Canny(blur2, dst2, 100, 200, 3, true);
//            break;
//    }

//    // OpenCvSharp의 Mat을 Bitmap으로 변환하여 PictureBox에 출력
//    Console.Write(dst.ToString());
//    pictureBox1.Image = BitmapConverter.ToBitmap(dst);
//    pictureBox2.Image = BitmapConverter.ToBitmap(dst2);
//    isImageOp = true;

//    // 연산 결과를 TextBox에 출력 (예: Mat의 픽셀 평균값)
//    Scalar mean = Cv2.Mean(dst); // Mat의 평균값 계산
//    //textBox1.Text = $"Mean: {mean.Val0:F2}, {mean.Val1:F2}, {mean.Val2:F2}";
//}

//private void ColorcomboBox_SelectedIndexChanged(object sender, EventArgs e)
//{
//    if (!isImageLoaded)
//    {
//        MessageBox.Show("먼저 이미지를 열어주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//        return;
//    }


//    ImageColor selType = (ImageColor)ColorcomboBox.SelectedIndex;

//    switch (selType)
//    {
//        case ImageColor.Color:
//            dst = src3.Clone();
//            dst2 = src4.Clone();
//            break;
//        case ImageColor.Mono:
//            Cv2.CvtColor(src3, dst, ColorConversionCodes.BGR2GRAY);
//            Cv2.CvtColor(src4, dst2, ColorConversionCodes.BGR2GRAY);
//            break;
//    }
//    src1 = dst;
//    src2 = dst2;

//    // OpenCvSharp의 Mat을 Bitmap으로 변환하여 PictureBox에 출력
//    pictureBox1.Image = BitmapConverter.ToBitmap(dst);
//    pictureBox2.Image = BitmapConverter.ToBitmap(dst2);
//    isImageOp = true;

//    // 연산 결과를 TextBox에 출력 (예: Mat의 픽셀 평균값)
//    Scalar mean = Cv2.Mean(dst); // Mat의 평균값 계산
//    //textBox1.Text = $"Mean: {mean.Val0:F2}, {mean.Val1:F2}, {mean.Val2:F2}";
//}
#endregion