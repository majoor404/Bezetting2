namespace Bezetting2
{
    partial class VerhuisForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelNaam = new System.Windows.Forms.Label();
            this.labelPersoneelNummer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelHuidigRooster = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxNieuwRooster = new System.Windows.Forms.ComboBox();
            this.dateTimeVerhuisDatum = new System.Windows.Forms.DateTimePicker();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Verhuis een personeel lid naar een andere ploeg/kleur/rooster";
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(13, 42);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(39, 13);
            this.labelNaam.TabIndex = 1;
            this.labelNaam.Text = "Majoor";
            // 
            // labelPersoneelNummer
            // 
            this.labelPersoneelNummer.AutoSize = true;
            this.labelPersoneelNummer.Location = new System.Drawing.Point(88, 42);
            this.labelPersoneelNummer.Name = "labelPersoneelNummer";
            this.labelPersoneelNummer.Size = new System.Drawing.Size(43, 13);
            this.labelPersoneelNummer.TabIndex = 2;
            this.labelPersoneelNummer.Text = "590588";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Huidig Rooster";
            // 
            // labelHuidigRooster
            // 
            this.labelHuidigRooster.AutoSize = true;
            this.labelHuidigRooster.Location = new System.Drawing.Point(428, 42);
            this.labelHuidigRooster.Name = "labelHuidigRooster";
            this.labelHuidigRooster.Size = new System.Drawing.Size(36, 13);
            this.labelHuidigRooster.TabIndex = 4;
            this.labelHuidigRooster.Text = "Blauw";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Verhuisd naar rooster en op welke start datum ?";
            // 
            // comboBoxNieuwRooster
            // 
            this.comboBoxNieuwRooster.FormattingEnabled = true;
            this.comboBoxNieuwRooster.Items.AddRange(new object[] {
            "Blauw",
            "Rood",
            "Wit",
            "Groen",
            "Geel",
            "DD",
            "Weg"});
            this.comboBoxNieuwRooster.Location = new System.Drawing.Point(16, 119);
            this.comboBoxNieuwRooster.Name = "comboBoxNieuwRooster";
            this.comboBoxNieuwRooster.Size = new System.Drawing.Size(229, 21);
            this.comboBoxNieuwRooster.TabIndex = 6;
            // 
            // dateTimeVerhuisDatum
            // 
            this.dateTimeVerhuisDatum.Location = new System.Drawing.Point(317, 119);
            this.dateTimeVerhuisDatum.Name = "dateTimeVerhuisDatum";
            this.dateTimeVerhuisDatum.Size = new System.Drawing.Size(229, 20);
            this.dateTimeVerhuisDatum.TabIndex = 7;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(16, 251);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(229, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(317, 251);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(229, 23);
            this.buttonOk.TabIndex = 9;
            this.buttonOk.Text = "Voer wissel uit";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // VerhuisForm
            // 
            this.AcceptButton = this.buttonCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 297);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.dateTimeVerhuisDatum);
            this.Controls.Add(this.comboBoxNieuwRooster);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelHuidigRooster);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelPersoneelNummer);
            this.Controls.Add(this.labelNaam);
            this.Controls.Add(this.label1);
            this.Name = "VerhuisForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VerhuisForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        public System.Windows.Forms.Label labelNaam;
        public System.Windows.Forms.Label labelPersoneelNummer;
        public System.Windows.Forms.Label labelHuidigRooster;
        public System.Windows.Forms.ComboBox comboBoxNieuwRooster;
        public System.Windows.Forms.DateTimePicker dateTimeVerhuisDatum;
    }
}