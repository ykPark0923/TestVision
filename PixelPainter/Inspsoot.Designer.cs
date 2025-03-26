namespace PixelPainter
{
    partial class Inspsoot
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.btnDifference = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openBTN2 = new System.Windows.Forms.Button();
            this.openBTN = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.checkBoxThreshold = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDifference
            // 
            this.btnDifference.Location = new System.Drawing.Point(624, 449);
            this.btnDifference.Name = "btnDifference";
            this.btnDifference.Size = new System.Drawing.Size(75, 23);
            this.btnDifference.TabIndex = 3;
            this.btnDifference.Text = "검사";
            this.btnDifference.UseVisualStyleBackColor = true;
            this.btnDifference.Click += new System.EventHandler(this.btnDetectBurn_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(624, 409);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(220, 21);
            this.txtResult.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // openBTN2
            // 
            this.openBTN2.Location = new System.Drawing.Point(162, 409);
            this.openBTN2.Margin = new System.Windows.Forms.Padding(2);
            this.openBTN2.Name = "openBTN2";
            this.openBTN2.Size = new System.Drawing.Size(52, 21);
            this.openBTN2.TabIndex = 9;
            this.openBTN2.Text = "열기";
            this.openBTN2.UseVisualStyleBackColor = true;
            this.openBTN2.Click += new System.EventHandler(this.openBTN2_Click);
            // 
            // openBTN
            // 
            this.openBTN.Location = new System.Drawing.Point(552, 408);
            this.openBTN.Margin = new System.Windows.Forms.Padding(2);
            this.openBTN.Name = "openBTN";
            this.openBTN.Size = new System.Drawing.Size(52, 21);
            this.openBTN.TabIndex = 8;
            this.openBTN.Text = "열기";
            this.openBTN.UseVisualStyleBackColor = true;
            this.openBTN.Click += new System.EventHandler(this.openBTN_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(30, 18);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(363, 367);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(412, 18);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(369, 367);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // checkBoxThreshold
            // 
            this.checkBoxThreshold.AutoSize = true;
            this.checkBoxThreshold.Location = new System.Drawing.Point(780, 449);
            this.checkBoxThreshold.Name = "checkBoxThreshold";
            this.checkBoxThreshold.Size = new System.Drawing.Size(67, 21);
            this.checkBoxThreshold.TabIndex = 12;
            this.checkBoxThreshold.Text = "이진화";
            this.checkBoxThreshold.UseVisualStyleBackColor = true;
            // 
            // Inspsoot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 513);
            this.Controls.Add(this.checkBoxThreshold);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.openBTN2);
            this.Controls.Add(this.openBTN);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnDifference);
            this.Name = "Inspsoot";
            this.Text = "InspFire";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDifference;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button openBTN2;
        private System.Windows.Forms.Button openBTN;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox checkBoxThreshold;
    }
}
