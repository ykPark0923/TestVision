namespace PixelPainter
{
    partial class editIMG
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ColorcomboBox = new System.Windows.Forms.ComboBox();
            this.FiltercomboBox = new System.Windows.Forms.ComboBox();
            this.OpcomboBox = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openBTN2 = new System.Windows.Forms.Button();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(363, 367);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(369, 8);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(369, 367);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // openBTN
            // 
            this.openBTN.Location = new System.Drawing.Point(127, 395);
            this.openBTN.Margin = new System.Windows.Forms.Padding(2);
            this.openBTN.Name = "openBTN";
            this.openBTN.Size = new System.Drawing.Size(52, 21);
            this.openBTN.TabIndex = 2;
            this.openBTN.Text = "열기";
            this.openBTN.UseVisualStyleBackColor = true;
            this.openBTN.Click += new System.EventHandler(this.openBTN_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ColorcomboBox);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.FiltercomboBox);
            this.groupBox1.Controls.Add(this.OpcomboBox);
            this.groupBox1.Location = new System.Drawing.Point(742, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(164, 196);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "group";
            // 
            // ColorcomboBox
            // 
            this.ColorcomboBox.FormattingEnabled = true;
            this.ColorcomboBox.Items.AddRange(new object[] {
            "Color",
            "Mono"});
            this.ColorcomboBox.Location = new System.Drawing.Point(8, 109);
            this.ColorcomboBox.Margin = new System.Windows.Forms.Padding(2);
            this.ColorcomboBox.Name = "ColorcomboBox";
            this.ColorcomboBox.Size = new System.Drawing.Size(134, 20);
            this.ColorcomboBox.TabIndex = 4;
            this.ColorcomboBox.SelectedIndexChanged += new System.EventHandler(this.ColorcomboBox_SelectedIndexChanged);
            // 
            // FiltercomboBox
            // 
            this.FiltercomboBox.FormattingEnabled = true;
            this.FiltercomboBox.Items.AddRange(new object[] {
            "FilterBlur",
            "FilterBoxFilter",
            "FilterMedianBlur",
            "FilterGaussianBlur",
            "FilterBilateral",
            "FilterSobel",
            "FilterScharr",
            "FilterLaplacian",
            "FilterCanny"});
            this.FiltercomboBox.Location = new System.Drawing.Point(8, 72);
            this.FiltercomboBox.Margin = new System.Windows.Forms.Padding(2);
            this.FiltercomboBox.Name = "FiltercomboBox";
            this.FiltercomboBox.Size = new System.Drawing.Size(134, 20);
            this.FiltercomboBox.TabIndex = 1;
            this.FiltercomboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // OpcomboBox
            // 
            this.OpcomboBox.FormattingEnabled = true;
            this.OpcomboBox.Items.AddRange(new object[] {
            "OpAdd",
            "OpSubtract",
            "OpMultiply",
            "OpDivide",
            "OpMax",
            "OpMin",
            "OpAbs",
            "OpAbDiff",
            "and",
            "or",
            "xor",
            "not",
            "compare"});
            this.OpcomboBox.Location = new System.Drawing.Point(8, 33);
            this.OpcomboBox.Margin = new System.Windows.Forms.Padding(2);
            this.OpcomboBox.Name = "OpcomboBox";
            this.OpcomboBox.Size = new System.Drawing.Size(134, 20);
            this.OpcomboBox.TabIndex = 0;
            this.OpcomboBox.SelectedIndexChanged += new System.EventHandler(this.OpcomboBox_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openBTN2
            // 
            this.openBTN2.Location = new System.Drawing.Point(534, 404);
            this.openBTN2.Margin = new System.Windows.Forms.Padding(2);
            this.openBTN2.Name = "openBTN2";
            this.openBTN2.Size = new System.Drawing.Size(52, 21);
            this.openBTN2.TabIndex = 5;
            this.openBTN2.Text = "열기";
            this.openBTN2.UseVisualStyleBackColor = true;
            this.openBTN2.Click += new System.EventHandler(this.openBTN2_Click);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.FileName = "openFileDialog2";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(51, 152);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(176, 21);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 159);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "연산값";
            // 
            // editIMG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 501);
            this.Controls.Add(this.openBTN2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.openBTN);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "editIMG";
            this.Text = "editIMG";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button openBTN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox FiltercomboBox;
        private System.Windows.Forms.ComboBox OpcomboBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ComboBox ColorcomboBox;
        private System.Windows.Forms.Button openBTN2;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}