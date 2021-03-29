namespace Bezetting2.Invoer
{
    partial class InstellingenProgrammaForm
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
            this.checkBoxGebruikRuilExtra = new System.Windows.Forms.CheckBox();
            this.checkBoxGebruikSnipper = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxMinAantalPersonen = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxRooster = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxTelVakAlsVK = new System.Windows.Forms.CheckBox();
            this.checkBoxWachtoverzichtAls2Dagen = new System.Windows.Forms.CheckBox();
            this.textBoxLocatieKalender = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBoxGebruikRuilExtra
            // 
            this.checkBoxGebruikRuilExtra.AutoSize = true;
            this.checkBoxGebruikRuilExtra.Location = new System.Drawing.Point(12, 12);
            this.checkBoxGebruikRuilExtra.Name = "checkBoxGebruikRuilExtra";
            this.checkBoxGebruikRuilExtra.Size = new System.Drawing.Size(153, 17);
            this.checkBoxGebruikRuilExtra.TabIndex = 0;
            this.checkBoxGebruikRuilExtra.Text = "Gebruik RuilExtra Diensten";
            this.checkBoxGebruikRuilExtra.UseVisualStyleBackColor = true;
            this.checkBoxGebruikRuilExtra.CheckedChanged += new System.EventHandler(this.CheckBoxGebruikRuilExtra_CheckedChanged);
            // 
            // checkBoxGebruikSnipper
            // 
            this.checkBoxGebruikSnipper.AutoSize = true;
            this.checkBoxGebruikSnipper.Location = new System.Drawing.Point(10, 46);
            this.checkBoxGebruikSnipper.Name = "checkBoxGebruikSnipper";
            this.checkBoxGebruikSnipper.Size = new System.Drawing.Size(186, 17);
            this.checkBoxGebruikSnipper.TabIndex = 1;
            this.checkBoxGebruikSnipper.Text = "Gebruik Snipper Dagen Aanvraag";
            this.checkBoxGebruikSnipper.UseVisualStyleBackColor = true;
            this.checkBoxGebruikSnipper.CheckedChanged += new System.EventHandler(this.CheckBoxGebruikSnipper_CheckedChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonSave.Location = new System.Drawing.Point(604, 352);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // textBoxMinAantalPersonen
            // 
            this.textBoxMinAantalPersonen.Location = new System.Drawing.Point(10, 79);
            this.textBoxMinAantalPersonen.Name = "textBoxMinAantalPersonen";
            this.textBoxMinAantalPersonen.Size = new System.Drawing.Size(41, 20);
            this.textBoxMinAantalPersonen.TabIndex = 3;
            this.textBoxMinAantalPersonen.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Minimaal aaantal Personen";
            // 
            // comboBoxRooster
            // 
            this.comboBoxRooster.FormattingEnabled = true;
            this.comboBoxRooster.Items.AddRange(new object[] {
            "5pl",
            "dd"});
            this.comboBoxRooster.Location = new System.Drawing.Point(12, 114);
            this.comboBoxRooster.Name = "comboBoxRooster";
            this.comboBoxRooster.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRooster.TabIndex = 5;
            this.comboBoxRooster.SelectedIndexChanged += new System.EventHandler(this.ComboBoxRooster_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Rooster";
            // 
            // checkBoxTelVakAlsVK
            // 
            this.checkBoxTelVakAlsVK.AutoSize = true;
            this.checkBoxTelVakAlsVK.Location = new System.Drawing.Point(12, 152);
            this.checkBoxTelVakAlsVK.Name = "checkBoxTelVakAlsVK";
            this.checkBoxTelVakAlsVK.Size = new System.Drawing.Size(215, 17);
            this.checkBoxTelVakAlsVK.TabIndex = 7;
            this.checkBoxTelVakAlsVK.Text = "Tel \"VAK\" (vakantie) als VK in overzicht";
            this.checkBoxTelVakAlsVK.UseVisualStyleBackColor = true;
            this.checkBoxTelVakAlsVK.CheckedChanged += new System.EventHandler(this.CheckBoxTelVakAlsVK_CheckedChanged);
            // 
            // checkBoxWachtoverzichtAls2Dagen
            // 
            this.checkBoxWachtoverzichtAls2Dagen.AutoSize = true;
            this.checkBoxWachtoverzichtAls2Dagen.Location = new System.Drawing.Point(12, 175);
            this.checkBoxWachtoverzichtAls2Dagen.Name = "checkBoxWachtoverzichtAls2Dagen";
            this.checkBoxWachtoverzichtAls2Dagen.Size = new System.Drawing.Size(184, 17);
            this.checkBoxWachtoverzichtAls2Dagen.TabIndex = 8;
            this.checkBoxWachtoverzichtAls2Dagen.Text = "Wacht Overzicht 2 dagen tegelijk";
            this.checkBoxWachtoverzichtAls2Dagen.UseVisualStyleBackColor = true;
            this.checkBoxWachtoverzichtAls2Dagen.CheckedChanged += new System.EventHandler(this.checkBoxWachtoverzichtAls2Dagen_CheckedChanged);
            // 
            // textBoxLocatieKalender
            // 
            this.textBoxLocatieKalender.Location = new System.Drawing.Point(13, 199);
            this.textBoxLocatieKalender.Name = "textBoxLocatieKalender";
            this.textBoxLocatieKalender.Size = new System.Drawing.Size(483, 20);
            this.textBoxLocatieKalender.TabIndex = 9;
            this.textBoxLocatieKalender.TextChanged += new System.EventHandler(this.textBoxLocatieKalender_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(503, 205);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Locatie kalender snelkoppeling";
            // 
            // InstellingenProgrammaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 387);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxLocatieKalender);
            this.Controls.Add(this.checkBoxWachtoverzichtAls2Dagen);
            this.Controls.Add(this.checkBoxTelVakAlsVK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxRooster);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMinAantalPersonen);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.checkBoxGebruikSnipper);
            this.Controls.Add(this.checkBoxGebruikRuilExtra);
            this.Name = "InstellingenProgrammaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Instellingen Bezetting 2.0";
            this.Shown += new System.EventHandler(this.InstellingenProgrammaForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxGebruikRuilExtra;
        private System.Windows.Forms.CheckBox checkBoxGebruikSnipper;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxMinAantalPersonen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxRooster;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxTelVakAlsVK;
        private System.Windows.Forms.CheckBox checkBoxWachtoverzichtAls2Dagen;
        private System.Windows.Forms.TextBox textBoxLocatieKalender;
        private System.Windows.Forms.Label label3;
    }
}