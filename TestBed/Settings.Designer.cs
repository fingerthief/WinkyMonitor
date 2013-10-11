namespace TestBed
{
    partial class Settings
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
            this.txtZip = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboDisk = new System.Windows.Forms.ComboBox();
            this.comboNic = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbStartHour = new System.Windows.Forms.ComboBox();
            this.cmbDayEndHour = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbStartMinute = new System.Windows.Forms.ComboBox();
            this.cmbDayEndHourMins = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMoney = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtZip
            // 
            this.txtZip.Location = new System.Drawing.Point(62, 12);
            this.txtZip.Name = "txtZip";
            this.txtZip.Size = new System.Drawing.Size(104, 20);
            this.txtZip.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(169, 92);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ZIP:";
            // 
            // comboDisk
            // 
            this.comboDisk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDisk.FormattingEnabled = true;
            this.comboDisk.Location = new System.Drawing.Point(62, 38);
            this.comboDisk.Name = "comboDisk";
            this.comboDisk.Size = new System.Drawing.Size(104, 21);
            this.comboDisk.TabIndex = 3;
            // 
            // comboNic
            // 
            this.comboNic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboNic.FormattingEnabled = true;
            this.comboNic.Location = new System.Drawing.Point(250, 38);
            this.comboNic.Name = "comboNic";
            this.comboNic.Size = new System.Drawing.Size(110, 21);
            this.comboNic.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Drive:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(216, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "NIC:";
            // 
            // cmbStartHour
            // 
            this.cmbStartHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartHour.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbStartHour.FormattingEnabled = true;
            this.cmbStartHour.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cmbStartHour.Location = new System.Drawing.Point(62, 65);
            this.cmbStartHour.Name = "cmbStartHour";
            this.cmbStartHour.Size = new System.Drawing.Size(49, 21);
            this.cmbStartHour.TabIndex = 7;
            // 
            // cmbDayEndHour
            // 
            this.cmbDayEndHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDayEndHour.FormattingEnabled = true;
            this.cmbDayEndHour.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cmbDayEndHour.Location = new System.Drawing.Point(250, 65);
            this.cmbDayEndHour.Name = "cmbDayEndHour";
            this.cmbDayEndHour.Size = new System.Drawing.Size(55, 21);
            this.cmbDayEndHour.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Day Start:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(193, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Day End:";
            // 
            // cmbStartMinute
            // 
            this.cmbStartMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartMinute.FormattingEnabled = true;
            this.cmbStartMinute.Items.AddRange(new object[] {
            "0",
            "10",
            "20",
            "30",
            "40",
            "50"});
            this.cmbStartMinute.Location = new System.Drawing.Point(117, 65);
            this.cmbStartMinute.Name = "cmbStartMinute";
            this.cmbStartMinute.Size = new System.Drawing.Size(49, 21);
            this.cmbStartMinute.TabIndex = 8;
            // 
            // cmbDayEndHourMins
            // 
            this.cmbDayEndHourMins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDayEndHourMins.FormattingEnabled = true;
            this.cmbDayEndHourMins.Items.AddRange(new object[] {
            "0",
            "10",
            "20",
            "30",
            "40",
            "50"});
            this.cmbDayEndHourMins.Location = new System.Drawing.Point(311, 65);
            this.cmbDayEndHourMins.Name = "cmbDayEndHourMins";
            this.cmbDayEndHourMins.Size = new System.Drawing.Size(49, 21);
            this.cmbDayEndHourMins.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(172, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Hourly Wage:";
            // 
            // txtMoney
            // 
            this.txtMoney.Location = new System.Drawing.Point(250, 12);
            this.txtMoney.Name = "txtMoney";
            this.txtMoney.Size = new System.Drawing.Size(110, 20);
            this.txtMoney.TabIndex = 17;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 121);
            this.Controls.Add(this.txtMoney);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbDayEndHourMins);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbDayEndHour);
            this.Controls.Add(this.cmbStartMinute);
            this.Controls.Add(this.cmbStartHour);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboNic);
            this.Controls.Add(this.comboDisk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtZip);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtZip;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboDisk;
        private System.Windows.Forms.ComboBox comboNic;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbStartHour;
        private System.Windows.Forms.ComboBox cmbDayEndHour;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbStartMinute;
        private System.Windows.Forms.ComboBox cmbDayEndHourMins;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMoney;
    }
}