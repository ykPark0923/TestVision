namespace PixelPainter
{
    partial class Inspsoot
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
            this.pbNG = new System.Windows.Forms.PictureBox();
            this.pbOK = new System.Windows.Forms.PictureBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnDifference = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.checkBoxThreshold = new System.Windows.Forms.CheckBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnResetImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbNG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOK)).BeginInit();
            this.SuspendLayout();
            // 
            // pbNG
            // 
            this.pbNG.Location = new System.Drawing.Point(29, 23);
            this.pbNG.Name = "pbNG";
            this.pbNG.Size = new System.Drawing.Size(299, 261);
            this.pbNG.TabIndex = 0;
            this.pbNG.TabStop = false;
            // 
            // pbOK
            // 
            this.pbOK.Location = new System.Drawing.Point(391, 23);
            this.pbOK.Name = "pbOK";
            this.pbOK.Size = new System.Drawing.Size(312, 261);
            this.pbOK.TabIndex = 1;
            this.pbOK.TabStop = false;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(612, 351);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "불량";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnLoadTestImage_Click);
            // 
            // btnDifference
            // 
            this.btnDifference.Location = new System.Drawing.Point(612, 399);
            this.btnDifference.Name = "btnDifference";
            this.btnDifference.Size = new System.Drawing.Size(75, 23);
            this.btnDifference.TabIndex = 3;
            this.btnDifference.Text = "검사";
            this.btnDifference.UseVisualStyleBackColor = true;
            this.btnDifference.Click += new System.EventHandler(this.btnDetectBurn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // checkBoxThreshold
            // 
            this.checkBoxThreshold.AutoSize = true;
            this.checkBoxThreshold.Location = new System.Drawing.Point(420, 311);
            this.checkBoxThreshold.Name = "checkBoxThreshold";
            this.checkBoxThreshold.Size = new System.Drawing.Size(93, 21);
            this.checkBoxThreshold.TabIndex = 4;
            this.checkBoxThreshold.Text = "checkBox1";
            this.checkBoxThreshold.UseVisualStyleBackColor = true;
            this.checkBoxThreshold.CheckedChanged += new System.EventHandler(this.checkBoxThreshold_CheckedChanged);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(420, 382);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(100, 21);
            this.txtResult.TabIndex = 5;
            // 
            // btnResetImage
            // 
            this.btnResetImage.Location = new System.Drawing.Point(612, 311);
            this.btnResetImage.Name = "btnResetImage";
            this.btnResetImage.Size = new System.Drawing.Size(75, 23);
            this.btnResetImage.TabIndex = 6;
            this.btnResetImage.Text = "양품";
            this.btnResetImage.UseVisualStyleBackColor = true;
            this.btnResetImage.Click += new System.EventHandler(this.btnLoadNormalImage_Click);
            // 
            // Inspsoot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnResetImage);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.checkBoxThreshold);
            this.Controls.Add(this.btnDifference);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.pbOK);
            this.Controls.Add(this.pbNG);
            this.Name = "Inspsoot";
            this.Text = "InspSoot";
            ((System.ComponentModel.ISupportInitialize)(this.pbNG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOK)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbNG;
        private System.Windows.Forms.PictureBox pbOK;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnDifference;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox checkBoxThreshold;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnResetImage;
    }
}