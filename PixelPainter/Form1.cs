using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelPainter
{
    //enum을 사용시 의미를 명확히 하고 코드의 가독성과 유지보수성을 높일 수 있음.
    // 열거형 이름(DrawLine, DrawRectangle, 등)을 사용하여 숫자 값을 사용하는 것보다 더 직관적
    //그리기 타입 
    enum DrawType
    {
        DrawNone = 0,
        DrawLine,
        DrawRectangle,
        DrawCircle
    }

    //라인 칼라
    enum DrawColor : int
    {
        ColorBlack = 0,  //0으로 이렇게 할당해주면 자동으로 아래는 순서대로 할당됨. //만약 4로 할당하면 밑에는 5부터 자동할당
        ColorRed,         //=1
        ColorOrange,      //=2 이런식으로 자동으로 할당. 
        ColorYellow,
        ColorGreen,
        ColorBlue,
        ColorPurple,
        ColorWhite
    }
    public partial class Form1 : Form
    {

        private System.Drawing.Point _startPos; //시작점
        private System.Drawing.Point _currentPos; //현재위치
        private bool _isDrawing = false; //그리기 모드
        private DrawType _drawType; //도형 형태
        private int _lineThickness = 2; //라인 두께

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InspCrack editIMG = new InspCrack();

            // 모달로 띄움 (ShowDialog 호출)
            DialogResult result = editIMG.ShowDialog();
        }
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        private void 새로만들기toolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;  //있는거 삭제시킴
        }



        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {  //DialogResult는 windows forms 라이브러리에서 제공하는 열거형.
                //대화상자(Dialog)에서 반환되는 값 표현
                pictureBox1.Load(openFileDialog1.FileName);

                Mat src = Cv2.ImRead(openFileDialog1.FileName);
                pictureBox1.Image = BitmapConverter.ToBitmap(src);
                   
            }
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        
        //마우스 왼쪽 버튼 선택
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_drawType != DrawType.DrawNone) // 그릴 도형이 설정되어 있을 때만 그리기 동작을 수행.->drawNone인경우 아무 버튼이 선택되어 있지 않은 상태임.(기본상태)
                {
                    _startPos = e.Location; //현재좌표를 시작좌표에 저장. 
                    _isDrawing = true; //그리기상태 활성화

                }
            }
        }
        //마우스 이동시 이벤트
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing) //그리기상태 활성화 되었을 때만 
            {
                _currentPos=e.Location;//현재 마우스 좌표 저장
                pictureBox1.Invalidate();//화면을 다시 그리도록 요청. // 컨트롤의 전체 화면을 무효화하고 컨트롤을 다시 그립니다.
                //마우스를 움직일때 마다 그렸다 지웠다 반복함.
                //
            }
        }

        //마우스 버튼 UP 이벤트 (떼어졌을 때)
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _isDrawing=false; // 그리기 상태를 비활성화 (그리기를 멈춤)//도형하나의 그리기 끝
                //pictureBox1.Invalidate(); //마우스 업(마우스 놓으면 바로 사라짐 )
                //최종 선을 그리기 위해 화면 갱신 
            }
        }

        //직선 그리기 버튼 이벤트
        private void btnDrawLine_Click(object sender, EventArgs e)
        {
            _drawType = DrawType.DrawLine; // 그리기 타입을 '직선'으로 설정
        }

        private void btnDrawRec_Click(object sender, EventArgs e)
        {
            _drawType = DrawType.DrawRectangle; // 그리기 타입을 '사각형'으로 설정
        }

        private void btnDrawCir_Click(object sender, EventArgs e)
        {
            _drawType = DrawType.DrawCircle; // 그리기 타입을 '원'으로 설정
        }


        // 두 점을 받아서 사각형의 Rectangle 객체를 반환하는 유틸리티 함수

        private Rectangle GetRectangle(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            // 순서대로 x, y, width, height 값으로 Rectangle을 생성
            return new Rectangle(
                Math.Min(p1.X, p2.X),  // 최소 x 값을 좌측 상단 x 좌표로 설정
                Math.Min(p1.Y, p2.Y),   // 최소 y 값을 좌측 상단 y 좌표로 설정
                Math.Abs(p2.X - p1.X),  // 두 점 간의 x 차이만큼 너비(width)를 계산
                Math.Abs(p2.Y - p1.Y)    // 두 점 간의 y 차이만큼 높이(height)를 계산
                );
        }

        //선 두께 선택시 두께값 지정
        private void cbLineThickness_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 선택된 두께 값을 _lineThickness에 저장
            _lineThickness = Convert.ToInt32(cbLineThickness.SelectedItem);
        }

        //색깔 할당
        private Color GetSelColor()
        {
            // 기본 색상 변수 선언
            Color cirLine = new Color();
            // 콤보박스에서 선택된 색상을 DrawColor 타입으로 변환
            //*****예로들어 index값이 1 이면. DrawColor에서 1로 정한 ColorRed로 변환됨. 
            DrawColor drawColor = (DrawColor)cbLineColor.SelectedIndex; //선 색상에서 선택한 index값(int형) drawcolor에 저장. 

            // 선택된 색상에 따라 실제 색상을 cirLine에 할당
            switch (drawColor)
            {
                case DrawColor.ColorBlack:
                    cirLine = Color.Black;
                    break;
                case DrawColor.ColorRed:
                    cirLine = Color.Red;
                    break;
                case DrawColor.ColorOrange:
                    cirLine = Color.Orange;
                    break;

                case DrawColor.ColorYellow:
                    cirLine = Color.Yellow;
                    break;
                case DrawColor.ColorGreen:
                    cirLine = Color.Green;
                    break;
                case DrawColor.ColorBlue:
                    cirLine = Color.Blue;
                    break;
                case DrawColor.ColorPurple:
                    cirLine = Color.Purple;
                    break;
                case DrawColor.ColorWhite:
                    cirLine = Color.White;
                    break;
                default:
                    cirLine = Color.Black;
                    break;

            }
            return cirLine;// 최종 색상 반환
        }

        // PictureBox 그래픽 업데이트 이벤트
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (_isDrawing) // 그리기 모드일 때만 실행
            {
                Graphics grp = e.Graphics; // 그리기 위한 Graphics 객체 얻기
                Color cirLine = GetSelColor(); // 선택된 색상 가져오기
                Pen pen = new Pen(cirLine, _lineThickness); // Pen 객체 생성 (선 색상 및 두께 설정)

                // 그리기 모드에 따라 그리기 작업 수행
                switch (_drawType)
                {
                    case DrawType.DrawLine: // 직선 그리기
                        grp.DrawLine(pen, _startPos.X, _startPos.Y, _currentPos.X, _currentPos.Y); // 직선 그리기
                        break;
                    case DrawType.DrawRectangle: // 사각형 그리기
                        var rect = GetRectangle(_startPos, _currentPos); // 시작점과 끝점을 이용해 사각형 영역 계산
                        grp.DrawRectangle(pen, rect); // 사각형 그리기
                        break;
                    case DrawType.DrawCircle: // 원 그리기
                        var ellipse = GetRectangle(_startPos, _currentPos); // 원을 그리기 위한 사각형 영역 계산
                        grp.DrawEllipse(pen, ellipse); // 원 그리기
                        break;
                }

                // Pen 객체 메모리 해제 (성능 최적화 및 리소스 관리)
                pen.Dispose();
            }
        }
    }
}
