﻿namespace PixelPainter
{
    partial class InspScratch
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.openBTN = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.diffBTN = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.txtareaMin = new System.Windows.Forms.TextBox();
            this.txtareaMax = new System.Windows.Forms.TextBox();
            this.txtminAspectRatio = new System.Windows.Forms.TextBox();
            this.txtmaxAspectRatio = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(571, 550);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(589, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(571, 550);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // openBTN
            // 
            this.openBTN.Location = new System.Drawing.Point(12, 568);
            this.openBTN.Name = "openBTN";
            this.openBTN.Size = new System.Drawing.Size(74, 32);
            this.openBTN.TabIndex = 3;
            this.openBTN.Text = "열기";
            this.openBTN.UseVisualStyleBackColor = true;
            this.openBTN.Click += new System.EventHandler(this.openBTN_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(537, 572);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 28);
            this.textBox1.TabIndex = 8;
            // 
            // diffBTN
            // 
            this.diffBTN.Location = new System.Drawing.Point(1086, 572);
            this.diffBTN.Name = "diffBTN";
            this.diffBTN.Size = new System.Drawing.Size(74, 32);
            this.diffBTN.TabIndex = 11;
            this.diffBTN.Text = "검사";
            this.diffBTN.UseVisualStyleBackColor = true;
            this.diffBTN.Click += new System.EventHandler(this.diffBTN_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(1166, 12);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(571, 550);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 12;
            this.pictureBox3.TabStop = false;
            // 
            // txtareaMin
            // 
            this.txtareaMin.Location = new System.Drawing.Point(131, 620);
            this.txtareaMin.Name = "txtareaMin";
            this.txtareaMin.Size = new System.Drawing.Size(100, 28);
            this.txtareaMin.TabIndex = 13;
            // 
            // txtareaMax
            // 
            this.txtareaMax.Location = new System.Drawing.Point(287, 620);
            this.txtareaMax.Name = "txtareaMax";
            this.txtareaMax.Size = new System.Drawing.Size(100, 28);
            this.txtareaMax.TabIndex = 14;
            // 
            // txtminAspectRatio
            // 
            this.txtminAspectRatio.Location = new System.Drawing.Point(131, 675);
            this.txtminAspectRatio.Name = "txtminAspectRatio";
            this.txtminAspectRatio.Size = new System.Drawing.Size(100, 28);
            this.txtminAspectRatio.TabIndex = 15;
            // 
            // txtmaxAspectRatio
            // 
            this.txtmaxAspectRatio.Location = new System.Drawing.Point(287, 675);
            this.txtmaxAspectRatio.Name = "txtmaxAspectRatio";
            this.txtmaxAspectRatio.Size = new System.Drawing.Size(100, 28);
            this.txtmaxAspectRatio.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 630);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 17;
            this.label1.Text = "면적";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 685);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 18;
            this.label2.Text = "비율";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(89, 630);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 18);
            this.label3.TabIndex = 19;
            this.label3.Text = "min";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 685);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 18);
            this.label4.TabIndex = 20;
            this.label4.Text = "min";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(238, 630);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 18);
            this.label5.TabIndex = 21;
            this.label5.Text = "max";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(238, 685);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 18);
            this.label6.TabIndex = 22;
            this.label6.Text = "max";
            // 
            // InspScratch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1800, 752);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtmaxAspectRatio);
            this.Controls.Add(this.txtminAspectRatio);
            this.Controls.Add(this.txtareaMax);
            this.Controls.Add(this.txtareaMin);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.diffBTN);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.openBTN);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "InspScratch";
            this.Text = "InspScratch";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button openBTN;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button diffBTN;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TextBox txtareaMin;
        private System.Windows.Forms.TextBox txtareaMax;
        private System.Windows.Forms.TextBox txtminAspectRatio;
        private System.Windows.Forms.TextBox txtmaxAspectRatio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}