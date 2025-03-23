using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
 



namespace PixelPainter
{   
    
    enum ImageOperation
    {
        OpAdd = 0,
        OpSubtract,
        OpMultiply,
        OpDivide,
        OpMax,
        OpMin,
        OpAbs,
        OpAbDiff,
        and,
        or,
        xor,
        not,
        compare,
    }

    enum ImageFilter
    {
        FilterBlur = 0,
        FilterBoxFilter,
        FilterMedianBlur,
        FilterGaussianBlur,
        FilterBilateral,
        FilterSobel,
        FilterScharr,
        FilterLaplacian,
        FilterCanny,

    }

    enum ImageColor
    {
        Color = 0,
        Mono,
    }


    public partial class editIMG : Form
    {        
        public editIMG()
        {
            InitializeComponent();
        }

        private bool isImageLoaded = false; //불러온이미지없이 연산시 예외처리
        private bool isImageOp = false;     //저장할 연산이미지없을시 예외처리
        public Mat blur = new Mat();
        public Mat blur2 = new Mat();
        public Mat dst = new Mat();
        public Mat dst2 = new Mat();
        public Mat src1 = new Mat();
        public Mat src2 = new Mat();

        public Mat src3 = new Mat();
        public Mat src4 = new Mat();

        private void openBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {  //DialogResult는 windows forms 라이브러리에서 제공하는 열거형.
                //대화상자(Dialog)에서 반환되는 값 표현
                pictureBox1.Load(openFileDialog1.FileName);

                src1 = Cv2.ImRead(openFileDialog1.FileName);
                src3 = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src1);

                isImageLoaded = true;  

            }
        }

        private void openBTN2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {  //DialogResult는 windows forms 라이브러리에서 제공하는 열거형.
                //대화상자(Dialog)에서 반환되는 값 표현
                pictureBox2.Load(openFileDialog2.FileName);

                src2 = Cv2.ImRead(openFileDialog2.FileName);
                src4 = Cv2.ImRead(openFileDialog2.FileName);
                pictureBox2.Image = BitmapConverter.ToBitmap(src2);

                isImageLoaded = true;

            }
        }


        private void OpcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isImageLoaded)
            {
                MessageBox.Show("먼저 이미지를 열어주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            ImageOperation selType = (ImageOperation)OpcomboBox.SelectedIndex;

            switch (selType)
            {
                case ImageOperation.OpAdd:
                    Cv2.Add(src1, src2, dst);
                    break;
                case ImageOperation.OpSubtract:
                    Cv2.Subtract(src1, src2, dst);
                    break;
                case ImageOperation.OpMultiply:
                    Cv2.Multiply(src1, src2, dst);
                    break;
                case ImageOperation.OpDivide:
                    Cv2.Divide(src1, src2, dst);
                    break;
                case ImageOperation.OpMax:
                    Cv2.Max(src1, src2, dst);
                    break;
                case ImageOperation.OpMin:
                    Cv2.Min(src1, src2, dst);
                    break;
                case ImageOperation.OpAbs:
                    Cv2.Multiply(src1, src2, dst);
                    Cv2.Abs(dst);
                    break;
                case ImageOperation.OpAbDiff:
                    Mat matMul = new Mat();
                    Cv2.Multiply(src1, src2, matMul);
                    Cv2.Absdiff(src2, matMul, dst);
                    break;
                case ImageOperation.and:
                    Cv2.BitwiseAnd(src1, src2, dst);
                    break;
                case ImageOperation.or:
                    Cv2.BitwiseOr(src1, src2, dst);
                    break;
                case ImageOperation.xor:
                    Cv2.BitwiseXor(src1, src2, dst);
                    break;
                case ImageOperation.not:
                    Cv2.BitwiseNot(src1, dst);
                    break;
                case ImageOperation.compare:
                    Cv2.Compare(src1, src2, dst, CmpType.EQ);
                    break;
            }

            // OpenCvSharp의 Mat을 Bitmap으로 변환하여 PictureBox에 출력
            pictureBox2.Image = BitmapConverter.ToBitmap(dst);
            isImageOp = true;

            // 연산 결과를 TextBox에 출력 (예: Mat의 픽셀 평균값)
            Scalar mean = Cv2.Mean(dst); // Mat의 평균값 계산
            textBox1.Text = $"Mean: {mean.Val0:F2}, {mean.Val1:F2}, {mean.Val2:F2}";

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isImageLoaded)
            {
                MessageBox.Show("먼저 이미지를 열어주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cv2.GaussianBlur(src1, blur, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);
            Cv2.GaussianBlur(src2, blur2, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);

            ImageFilter selType = (ImageFilter)FiltercomboBox.SelectedIndex;

            switch (selType)
            {
                case ImageFilter.FilterBlur:
                    Cv2.Blur(src1, dst, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);
                    Cv2.Blur(src2, dst2, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), BorderTypes.Default);
                    break;
                case ImageFilter.FilterBoxFilter:
                    Cv2.BoxFilter(src1, dst, MatType.CV_8UC3, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), true, BorderTypes.Default);
                    Cv2.BoxFilter(src2, dst2, MatType.CV_8UC3, new OpenCvSharp.Size(9, 9), new OpenCvSharp.Point(-1, -1), true, BorderTypes.Default);
                    break;
                case ImageFilter.FilterMedianBlur:
                    Cv2.MedianBlur(src1, dst, 9);
                    Cv2.MedianBlur(src2, dst2, 9);
                    break;
                case ImageFilter.FilterGaussianBlur:
                    Cv2.GaussianBlur(src1, dst, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);
                    Cv2.GaussianBlur(src2, dst2, new OpenCvSharp.Size(9, 9), 1, 1, BorderTypes.Default);
                    break;
                case ImageFilter.FilterBilateral:
                    Cv2.BilateralFilter(src1, dst, 9, 3, 3, BorderTypes.Default);
                    Cv2.BilateralFilter(src2, dst2, 9, 3, 3, BorderTypes.Default);
                    break;
                case ImageFilter.FilterSobel:
                    Cv2.Sobel(blur, dst, MatType.CV_32F, 1, 0, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
                    Cv2.Sobel(blur2, dst2, MatType.CV_32F, 1, 0, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
                    dst.ConvertTo(dst, MatType.CV_8UC1);
                    dst2.ConvertTo(dst2, MatType.CV_8UC1);
                    break;
                case ImageFilter.FilterScharr:
                    Cv2.Scharr(blur, dst, MatType.CV_32F, 1, 0, scale: 1, delta: 0, BorderTypes.Default);
                    Cv2.Scharr(blur2, dst2, MatType.CV_32F, 1, 0, scale: 1, delta: 0, BorderTypes.Default);
                    dst.ConvertTo(dst, MatType.CV_8UC1);
                    dst2.ConvertTo(dst2, MatType.CV_8UC1);
                    break;
                case ImageFilter.FilterLaplacian:
                    Cv2.Laplacian(blur, dst, MatType.CV_32F, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
                    Cv2.Laplacian(blur2, dst2, MatType.CV_32F, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
                    dst.ConvertTo(dst, MatType.CV_8UC1);
                    dst2.ConvertTo(dst2, MatType.CV_8UC1);
                    break;
                case ImageFilter.FilterCanny:
                    Cv2.Canny(blur, dst, 100, 200, 3, true);
                    Cv2.Canny(blur2, dst2, 100, 200, 3, true);
                    break;
            }

            // OpenCvSharp의 Mat을 Bitmap으로 변환하여 PictureBox에 출력
            Console.Write(dst.ToString());
            pictureBox1.Image = BitmapConverter.ToBitmap(dst);
            pictureBox2.Image = BitmapConverter.ToBitmap(dst2);
            isImageOp = true;

            // 연산 결과를 TextBox에 출력 (예: Mat의 픽셀 평균값)
            Scalar mean = Cv2.Mean(dst); // Mat의 평균값 계산
            //textBox1.Text = $"Mean: {mean.Val0:F2}, {mean.Val1:F2}, {mean.Val2:F2}";
        }

        private void ColorcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isImageLoaded)
            {
                MessageBox.Show("먼저 이미지를 열어주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            ImageColor selType = (ImageColor)ColorcomboBox.SelectedIndex;

            switch (selType)
            {
                case ImageColor.Color:
                    dst = src3.Clone();
                    dst2 = src4.Clone();
                    break;
                case ImageColor.Mono:
                    Cv2.CvtColor(src3, dst, ColorConversionCodes.BGR2GRAY);
                    Cv2.CvtColor(src4, dst2, ColorConversionCodes.BGR2GRAY);
                    break;
            }
            src1 = dst;
            src2 = dst2;

            // OpenCvSharp의 Mat을 Bitmap으로 변환하여 PictureBox에 출력
            pictureBox1.Image = BitmapConverter.ToBitmap(dst);
            pictureBox2.Image = BitmapConverter.ToBitmap(dst2);
            isImageOp = true;

            // 연산 결과를 TextBox에 출력 (예: Mat의 픽셀 평균값)
            Scalar mean = Cv2.Mean(dst); // Mat의 평균값 계산
            //textBox1.Text = $"Mean: {mean.Val0:F2}, {mean.Val1:F2}, {mean.Val2:F2}";
        }

    }
}
