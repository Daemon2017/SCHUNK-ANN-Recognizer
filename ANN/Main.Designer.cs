namespace ANN
{
    partial class SchunkANN
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.RecognizeSchunkBtn = new System.Windows.Forms.Button();
            this.ResultsSchunkTxtBox = new System.Windows.Forms.TextBox();
            this.ResultSchunkPicBox = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RecognizeKukaBtn = new System.Windows.Forms.Button();
            this.ResultsKukaTxtBox = new System.Windows.Forms.TextBox();
            this.ClearKukaBtn = new System.Windows.Forms.Button();
            this.InputsSchunkBtn = new System.Windows.Forms.Button();
            this.OutputsSchunkBtn = new System.Windows.Forms.Button();
            this.NamesSchunkBtn = new System.Windows.Forms.Button();
            this.NamesKukaBtn = new System.Windows.Forms.Button();
            this.OutputsKukaBtn = new System.Windows.Forms.Button();
            this.InputsKukaBtn = new System.Windows.Forms.Button();
            this.TeachSchunkBtn = new System.Windows.Forms.Button();
            this.TeachKukaBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ResultSchunkPicBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // RecognizeSchunkBtn
            // 
            this.RecognizeSchunkBtn.Enabled = false;
            this.RecognizeSchunkBtn.Location = new System.Drawing.Point(0, 236);
            this.RecognizeSchunkBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RecognizeSchunkBtn.Name = "RecognizeSchunkBtn";
            this.RecognizeSchunkBtn.Size = new System.Drawing.Size(188, 37);
            this.RecognizeSchunkBtn.TabIndex = 4;
            this.RecognizeSchunkBtn.Text = "Распознать";
            this.RecognizeSchunkBtn.UseVisualStyleBackColor = true;
            this.RecognizeSchunkBtn.Click += new System.EventHandler(this.RecognizeSchunkBtn_Click);
            // 
            // ResultsSchunkTxtBox
            // 
            this.ResultsSchunkTxtBox.Location = new System.Drawing.Point(0, 17);
            this.ResultsSchunkTxtBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ResultsSchunkTxtBox.Multiline = true;
            this.ResultsSchunkTxtBox.Name = "ResultsSchunkTxtBox";
            this.ResultsSchunkTxtBox.Size = new System.Drawing.Size(188, 215);
            this.ResultsSchunkTxtBox.TabIndex = 12;
            // 
            // ResultSchunkPicBox
            // 
            this.ResultSchunkPicBox.Location = new System.Drawing.Point(270, 0);
            this.ResultSchunkPicBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ResultSchunkPicBox.Name = "ResultSchunkPicBox";
            this.ResultSchunkPicBox.Size = new System.Drawing.Size(150, 325);
            this.ResultSchunkPicBox.TabIndex = 18;
            this.ResultSchunkPicBox.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RecognizeSchunkBtn);
            this.groupBox1.Controls.Add(this.ResultsSchunkTxtBox);
            this.groupBox1.Location = new System.Drawing.Point(9, 47);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(188, 278);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Определение формы";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RecognizeKukaBtn);
            this.groupBox2.Controls.Add(this.ResultsKukaTxtBox);
            this.groupBox2.Location = new System.Drawing.Point(424, 47);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(188, 278);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Определение веса";
            // 
            // RecognizeKukaBtn
            // 
            this.RecognizeKukaBtn.Enabled = false;
            this.RecognizeKukaBtn.Location = new System.Drawing.Point(0, 236);
            this.RecognizeKukaBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RecognizeKukaBtn.Name = "RecognizeKukaBtn";
            this.RecognizeKukaBtn.Size = new System.Drawing.Size(188, 37);
            this.RecognizeKukaBtn.TabIndex = 4;
            this.RecognizeKukaBtn.Text = "Распознать";
            this.RecognizeKukaBtn.UseVisualStyleBackColor = true;
            this.RecognizeKukaBtn.Click += new System.EventHandler(this.RecognizeKukaBtn_Click);
            // 
            // ResultsKukaTxtBox
            // 
            this.ResultsKukaTxtBox.Location = new System.Drawing.Point(0, 17);
            this.ResultsKukaTxtBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ResultsKukaTxtBox.Multiline = true;
            this.ResultsKukaTxtBox.Name = "ResultsKukaTxtBox";
            this.ResultsKukaTxtBox.Size = new System.Drawing.Size(188, 215);
            this.ResultsKukaTxtBox.TabIndex = 12;
            // 
            // ClearKukaBtn
            // 
            this.ClearKukaBtn.Location = new System.Drawing.Point(616, 47);
            this.ClearKukaBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ClearKukaBtn.Name = "ClearKukaBtn";
            this.ClearKukaBtn.Size = new System.Drawing.Size(65, 49);
            this.ClearKukaBtn.TabIndex = 28;
            this.ClearKukaBtn.Text = "Очистить ИНС";
            this.ClearKukaBtn.UseVisualStyleBackColor = true;
            this.ClearKukaBtn.Click += new System.EventHandler(this.ClearKukaBtn_Click);
            // 
            // InputsSchunkBtn
            // 
            this.InputsSchunkBtn.Location = new System.Drawing.Point(201, 101);
            this.InputsSchunkBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.InputsSchunkBtn.Name = "InputsSchunkBtn";
            this.InputsSchunkBtn.Size = new System.Drawing.Size(65, 49);
            this.InputsSchunkBtn.TabIndex = 29;
            this.InputsSchunkBtn.Text = "Входы";
            this.InputsSchunkBtn.UseVisualStyleBackColor = true;
            this.InputsSchunkBtn.Click += new System.EventHandler(this.InputsSchunkBtn_Click);
            // 
            // OutputsSchunkBtn
            // 
            this.OutputsSchunkBtn.Location = new System.Drawing.Point(201, 154);
            this.OutputsSchunkBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OutputsSchunkBtn.Name = "OutputsSchunkBtn";
            this.OutputsSchunkBtn.Size = new System.Drawing.Size(65, 49);
            this.OutputsSchunkBtn.TabIndex = 30;
            this.OutputsSchunkBtn.Text = "Выходы";
            this.OutputsSchunkBtn.UseVisualStyleBackColor = true;
            this.OutputsSchunkBtn.Click += new System.EventHandler(this.OutputsSchunkBtn_Click);
            // 
            // NamesSchunkBtn
            // 
            this.NamesSchunkBtn.Location = new System.Drawing.Point(201, 208);
            this.NamesSchunkBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.NamesSchunkBtn.Name = "NamesSchunkBtn";
            this.NamesSchunkBtn.Size = new System.Drawing.Size(65, 49);
            this.NamesSchunkBtn.TabIndex = 31;
            this.NamesSchunkBtn.Text = "Названия";
            this.NamesSchunkBtn.UseVisualStyleBackColor = true;
            this.NamesSchunkBtn.Click += new System.EventHandler(this.NamesSchunkBtn_Click);
            // 
            // NamesKukaBtn
            // 
            this.NamesKukaBtn.Location = new System.Drawing.Point(616, 208);
            this.NamesKukaBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.NamesKukaBtn.Name = "NamesKukaBtn";
            this.NamesKukaBtn.Size = new System.Drawing.Size(65, 49);
            this.NamesKukaBtn.TabIndex = 34;
            this.NamesKukaBtn.Text = "Названия";
            this.NamesKukaBtn.UseVisualStyleBackColor = true;
            this.NamesKukaBtn.Click += new System.EventHandler(this.NamesKukaBtn_Click);
            // 
            // OutputsKukaBtn
            // 
            this.OutputsKukaBtn.Location = new System.Drawing.Point(616, 154);
            this.OutputsKukaBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OutputsKukaBtn.Name = "OutputsKukaBtn";
            this.OutputsKukaBtn.Size = new System.Drawing.Size(65, 49);
            this.OutputsKukaBtn.TabIndex = 33;
            this.OutputsKukaBtn.Text = "Выходы";
            this.OutputsKukaBtn.UseVisualStyleBackColor = true;
            this.OutputsKukaBtn.Click += new System.EventHandler(this.OutputsKukaBtn_Click);
            // 
            // InputsKukaBtn
            // 
            this.InputsKukaBtn.Location = new System.Drawing.Point(616, 101);
            this.InputsKukaBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.InputsKukaBtn.Name = "InputsKukaBtn";
            this.InputsKukaBtn.Size = new System.Drawing.Size(65, 49);
            this.InputsKukaBtn.TabIndex = 35;
            this.InputsKukaBtn.Text = "Входы";
            this.InputsKukaBtn.UseVisualStyleBackColor = true;
            this.InputsKukaBtn.Click += new System.EventHandler(this.InputsKukaBtn_Click);
            // 
            // TeachSchunkBtn
            // 
            this.TeachSchunkBtn.Location = new System.Drawing.Point(9, 6);
            this.TeachSchunkBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TeachSchunkBtn.Name = "TeachSchunkBtn";
            this.TeachSchunkBtn.Size = new System.Drawing.Size(257, 37);
            this.TeachSchunkBtn.TabIndex = 18;
            this.TeachSchunkBtn.Text = "Обучить";
            this.TeachSchunkBtn.UseVisualStyleBackColor = true;
            this.TeachSchunkBtn.Click += new System.EventHandler(this.TeachSchunkBtn_Click);
            // 
            // TeachKukaBtn
            // 
            this.TeachKukaBtn.Enabled = false;
            this.TeachKukaBtn.Location = new System.Drawing.Point(424, 6);
            this.TeachKukaBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TeachKukaBtn.Name = "TeachKukaBtn";
            this.TeachKukaBtn.Size = new System.Drawing.Size(257, 37);
            this.TeachKukaBtn.TabIndex = 36;
            this.TeachKukaBtn.Text = "Обучить";
            this.TeachKukaBtn.UseVisualStyleBackColor = true;
            this.TeachKukaBtn.Click += new System.EventHandler(this.TeachKukaBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(685, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 325);
            this.pictureBox1.TabIndex = 37;
            this.pictureBox1.TabStop = false;
            // 
            // SchunkANN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 602);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TeachKukaBtn);
            this.Controls.Add(this.TeachSchunkBtn);
            this.Controls.Add(this.InputsKukaBtn);
            this.Controls.Add(this.NamesKukaBtn);
            this.Controls.Add(this.OutputsKukaBtn);
            this.Controls.Add(this.NamesSchunkBtn);
            this.Controls.Add(this.OutputsSchunkBtn);
            this.Controls.Add(this.InputsSchunkBtn);
            this.Controls.Add(this.ClearKukaBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ResultSchunkPicBox);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(854, 749);
            this.MinimumSize = new System.Drawing.Size(854, 605);
            this.Name = "SchunkANN";
            this.Text = "SCHUNK ANN Recognizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ResultSchunkPicBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button RecognizeSchunkBtn;
        private System.Windows.Forms.TextBox ResultsSchunkTxtBox;
        private System.Windows.Forms.PictureBox ResultSchunkPicBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button RecognizeKukaBtn;
        private System.Windows.Forms.TextBox ResultsKukaTxtBox;
        private System.Windows.Forms.Button ClearKukaBtn;
        private System.Windows.Forms.Button InputsSchunkBtn;
        private System.Windows.Forms.Button OutputsSchunkBtn;
        private System.Windows.Forms.Button NamesSchunkBtn;
        private System.Windows.Forms.Button NamesKukaBtn;
        private System.Windows.Forms.Button OutputsKukaBtn;
        private System.Windows.Forms.Button InputsKukaBtn;
        private System.Windows.Forms.Button TeachSchunkBtn;
        private System.Windows.Forms.Button TeachKukaBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

