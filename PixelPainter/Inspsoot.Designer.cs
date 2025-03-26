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
            ((System.ComponentModel.ISupportInitialize)(this.pbNG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOK)).BeginInit();
            this.SuspendLayout();
            // 
            // pbNG
            // 
            this.pbNG.Location = new System.Drawing.Point(12, 23);
            this.pbNG.Name = "pbNG";
            this.pbNG.Size = new System.Drawing.Size(254, 261);
            this.pbNG.TabIndex = 0;
            this.pbNG.TabStop = false;
            // 
            // pbOK
            // 
            this.pbOK.Location = new System.Drawing.Point(314, 23);
            this.pbOK.Name = "pbOK";
            this.pbOK.Size = new System.Drawing.Size(254, 261);
            this.pbOK.TabIndex = 1;
            this.pbOK.TabStop = false;
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(558, 331);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 2;
            this.btnOpen.Text = "button1";
            this.btnOpen.UseVisualStyleBackColor = true;
            // 
            // btnDifference
            // 
            this.btnDifference.Location = new System.Drawing.Point(558, 380);
            this.btnDifference.Name = "btnDifference";
            this.btnDifference.Size = new System.Drawing.Size(75, 23);
            this.btnDifference.TabIndex = 3;
            this.btnDifference.Text = "button1";
            this.btnDifference.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // Inspsoot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDifference);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.pbOK);
            this.Controls.Add(this.pbNG);
            this.Name = "Inspsoot";
            this.Text = "InspSoot";
            ((System.ComponentModel.ISupportInitialize)(this.pbNG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbOK)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbNG;
        private System.Windows.Forms.PictureBox pbOK;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnDifference;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}