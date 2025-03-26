namespace PixelPainter
{
    partial class InspSoot
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.openBTN1 = new System.Windows.Forms.Button();
            this.diffBTN = new System.Windows.Forms.Button();
            this.txtBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(461, 432);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(490, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(461, 432);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // openBTN1
            // 
            this.openBTN1.Location = new System.Drawing.Point(1024, 468);
            this.openBTN1.Name = "openBTN1";
            this.openBTN1.Size = new System.Drawing.Size(94, 37);
            this.openBTN1.TabIndex = 2;
            this.openBTN1.Text = "열기";
            this.openBTN1.UseVisualStyleBackColor = true;
            this.openBTN1.Click += new System.EventHandler(this.openBTN1_Click);
            // 
            // diffBTN
            // 
            this.diffBTN.Location = new System.Drawing.Point(1024, 525);
            this.diffBTN.Name = "diffBTN";
            this.diffBTN.Size = new System.Drawing.Size(94, 37);
            this.diffBTN.TabIndex = 3;
            this.diffBTN.Text = "검사";
            this.diffBTN.UseVisualStyleBackColor = true;
            this.diffBTN.Click += new System.EventHandler(this.diffBTN_Click);
            // 
            // txtBox1
            // 
            this.txtBox1.Location = new System.Drawing.Point(733, 531);
            this.txtBox1.Name = "txtBox1";
            this.txtBox1.Size = new System.Drawing.Size(218, 28);
            this.txtBox1.TabIndex = 4;
            // 
            // InspSoot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 615);
            this.Controls.Add(this.txtBox1);
            this.Controls.Add(this.diffBTN);
            this.Controls.Add(this.openBTN1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "InspSoot";
            this.Text = "SootIMG";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button openBTN1;
        private System.Windows.Forms.Button diffBTN;
        private System.Windows.Forms.TextBox txtBox1;
    }
}