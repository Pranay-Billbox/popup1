namespace test_new_popup
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.boxName = new System.Windows.Forms.TextBox();
            this.boxMobile = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.Label();
            this.txtMobile = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panelGender = new System.Windows.Forms.Panel();
            this.radioFemale = new System.Windows.Forms.RadioButton();
            this.radioMale = new System.Windows.Forms.RadioButton();
            this.radioUnder18 = new System.Windows.Forms.CheckBox();
            this.panelGender.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(230, 346);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 63);
            this.button1.TabIndex = 4;
            this.button1.Text = "E-Bill";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // boxName
            // 
            this.boxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxName.Location = new System.Drawing.Point(230, 138);
            this.boxName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.boxName.Name = "boxName";
            this.boxName.Size = new System.Drawing.Size(296, 43);
            this.boxName.TabIndex = 1;
            this.boxName.TextChanged += new System.EventHandler(this.boxName_TextChanged_1);
            // 
            // boxMobile
            // 
            this.boxMobile.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxMobile.Location = new System.Drawing.Point(230, 59);
            this.boxMobile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.boxMobile.MaxLength = 10;
            this.boxMobile.Name = "boxMobile";
            this.boxMobile.Size = new System.Drawing.Size(296, 43);
            this.boxMobile.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.AutoSize = true;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(85, 138);
            this.txtName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(127, 37);
            this.txtName.TabIndex = 9;
            this.txtName.Text = "Name :";
            // 
            // txtMobile
            // 
            this.txtMobile.AutoSize = true;
            this.txtMobile.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMobile.Location = new System.Drawing.Point(22, 59);
            this.txtMobile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtMobile.Name = "txtMobile";
            this.txtMobile.Size = new System.Drawing.Size(190, 37);
            this.txtMobile.TabIndex = 8;
            this.txtMobile.Text = "Mobile No.:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(51, 240);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(160, 36);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Paper-Bill";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panelGender
            // 
            this.panelGender.Controls.Add(this.radioFemale);
            this.panelGender.Controls.Add(this.radioMale);
            this.panelGender.Location = new System.Drawing.Point(230, 240);
            this.panelGender.Name = "panelGender";
            this.panelGender.Size = new System.Drawing.Size(156, 35);
            this.panelGender.TabIndex = 10;
            // 
            // radioFemale
            // 
            this.radioFemale.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioFemale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioFemale.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioFemale.Location = new System.Drawing.Point(69, 0);
            this.radioFemale.Name = "radioFemale";
            this.radioFemale.Size = new System.Drawing.Size(87, 35);
            this.radioFemale.TabIndex = 1;
            this.radioFemale.TabStop = true;
            this.radioFemale.Text = "Female";
            this.radioFemale.UseVisualStyleBackColor = true;
            // 
            // radioMale
            // 
            this.radioMale.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioMale.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioMale.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioMale.Location = new System.Drawing.Point(0, 0);
            this.radioMale.Name = "radioMale";
            this.radioMale.Size = new System.Drawing.Size(69, 35);
            this.radioMale.TabIndex = 0;
            this.radioMale.TabStop = true;
            this.radioMale.Text = "Male";
            this.radioMale.UseVisualStyleBackColor = true;
            // 
            // radioUnder18
            // 
            this.radioUnder18.AutoSize = true;
            this.radioUnder18.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioUnder18.Location = new System.Drawing.Point(413, 240);
            this.radioUnder18.Name = "radioUnder18";
            this.radioUnder18.Size = new System.Drawing.Size(148, 36);
            this.radioUnder18.TabIndex = 11;
            this.radioUnder18.Text = "Under18";
            this.radioUnder18.UseVisualStyleBackColor = true;
            this.radioUnder18.CheckedChanged += new System.EventHandler(this.checkbox2_CheckedChanged_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 472);
            this.Controls.Add(this.radioUnder18);
            this.Controls.Add(this.panelGender);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtMobile);
            this.Controls.Add(this.boxMobile);
            this.Controls.Add(this.boxName);
            this.Controls.Add(this.button1);
            this.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BillBox";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelGender.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox boxName;
        private System.Windows.Forms.TextBox boxMobile;
        private System.Windows.Forms.Label txtName;
        private System.Windows.Forms.Label txtMobile;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panelGender;
        private System.Windows.Forms.RadioButton radioFemale;
        private System.Windows.Forms.RadioButton radioMale;
        private System.Windows.Forms.CheckBox radioUnder18;
    }
}

