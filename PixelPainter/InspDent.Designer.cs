namespace PixelPainter
{
    partial class InspDent
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
            this.pbNG = new System.Windows.Forms.PictureBox();
            this.pbOK = new System.Windows.Forms.PictureBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnDifference = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbNG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOK)).BeginInit();
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
            // pbNG
            // 
            this.pbNG.Location = new System.Drawing.Point(12, 12);
            this.pbNG.Name = "pbNG";
            this.pbNG.Size = new System.Drawing.Size(454, 445);
            this.pbNG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbNG.TabIndex = 0;
            this.pbNG.TabStop = false;
            // 
            // pbOK
            // 
            this.pbOK.Location = new System.Drawing.Point(482, 12);
            this.pbOK.Name = "pbOK";
            this.pbOK.Size = new System.Drawing.Size(454, 445);
            this.pbOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbOK.TabIndex = 1;
            this.pbOK.TabStop = false;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(990, 423);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(104, 34);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnDifference
            // 
            this.btnDifference.Location = new System.Drawing.Point(990, 491);
            this.btnDifference.Name = "btnDifference";
            this.btnDifference.Size = new System.Drawing.Size(101, 36);
            this.btnDifference.TabIndex = 3;
            this.btnDifference.Text = "inspect";
            this.btnDifference.UseVisualStyleBackColor = true;
            this.btnDifference.Click += new System.EventHandler(this.btnDifference_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(735, 499);
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(201, 28);
            this.txtResult.TabIndex = 4;
            // 
            // InspDent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 574);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnDifference);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.pbOK);
            this.Controls.Add(this.pbNG);
            this.Name = "InspDent";
            this.Text = "DentImg";
            ((System.ComponentModel.ISupportInitialize)(this.pbNG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOK)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox pbNG;
        private System.Windows.Forms.PictureBox pbOK;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnDifference;
        private System.Windows.Forms.TextBox txtResult;
    }
}