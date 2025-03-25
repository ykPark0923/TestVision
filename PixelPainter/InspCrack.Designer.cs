namespace PixelPainter
{
    partial class InspCrack
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openBTN1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.diffBTN = new System.Windows.Forms.Button();
            this.alignBTN = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(11, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(571, 550);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // openBTN1
            // 
            this.openBTN1.Location = new System.Drawing.Point(49, 592);
            this.openBTN1.Name = "openBTN1";
            this.openBTN1.Size = new System.Drawing.Size(74, 32);
            this.openBTN1.TabIndex = 2;
            this.openBTN1.Text = "열기";
            this.openBTN1.UseVisualStyleBackColor = true;
            this.openBTN1.Click += new System.EventHandler(this.openBTN_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(451, 588);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 28);
            this.textBox1.TabIndex = 7;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(589, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(571, 550);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // diffBTN
            // 
            this.diffBTN.Location = new System.Drawing.Point(1087, 584);
            this.diffBTN.Name = "diffBTN";
            this.diffBTN.Size = new System.Drawing.Size(74, 32);
            this.diffBTN.TabIndex = 10;
            this.diffBTN.Text = "검사";
            this.diffBTN.UseVisualStyleBackColor = true;
            this.diffBTN.Click += new System.EventHandler(this.diffBTN_Click);
            // 
            // alignBTN
            // 
            this.alignBTN.Location = new System.Drawing.Point(349, 584);
            this.alignBTN.Name = "alignBTN";
            this.alignBTN.Size = new System.Drawing.Size(74, 32);
            this.alignBTN.TabIndex = 11;
            this.alignBTN.Text = "align";
            this.alignBTN.UseVisualStyleBackColor = true;
            this.alignBTN.Click += new System.EventHandler(this.alignBTN_Click);
            // 
            // InspAffine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1369, 752);
            this.Controls.Add(this.alignBTN);
            this.Controls.Add(this.diffBTN);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.openBTN1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "InspAffine";
            this.Text = "editIMG";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button openBTN1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button diffBTN;
        private System.Windows.Forms.Button alignBTN;
    }
}