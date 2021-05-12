namespace Bezetting2.InlogGebeuren
{
    partial class RechtenInstellenForm
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
            this.checkBoxAllePloegen = new System.Windows.Forms.CheckBox();
            this.labelRechtenNivo = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.radioButton50 = new System.Windows.Forms.RadioButton();
            this.radioButton25 = new System.Windows.Forms.RadioButton();
            this.radioButton0 = new System.Windows.Forms.RadioButton();
            this.checkBoxAlleenZelf = new System.Windows.Forms.CheckBox();
            this.checkBoxAlleenAndere = new System.Windows.Forms.CheckBox();
            this.panel25 = new System.Windows.Forms.Panel();
            this.panel25.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rechten van gebruiker  (rechten nummer) : ";
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(12, 47);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(39, 13);
            this.labelNaam.TabIndex = 1;
            this.labelNaam.Text = "Majoor";
            // 
            // labelPersoneelNummer
            // 
            this.labelPersoneelNummer.AutoSize = true;
            this.labelPersoneelNummer.Location = new System.Drawing.Point(73, 47);
            this.labelPersoneelNummer.Name = "labelPersoneelNummer";
            this.labelPersoneelNummer.Size = new System.Drawing.Size(43, 13);
            this.labelPersoneelNummer.TabIndex = 2;
            this.labelPersoneelNummer.Text = "590588";
            // 
            // checkBoxAllePloegen
            // 
            this.checkBoxAllePloegen.AutoSize = true;
            this.checkBoxAllePloegen.Location = new System.Drawing.Point(15, 225);
            this.checkBoxAllePloegen.Name = "checkBoxAllePloegen";
            this.checkBoxAllePloegen.Size = new System.Drawing.Size(234, 17);
            this.checkBoxAllePloegen.TabIndex = 5;
            this.checkBoxAllePloegen.Text = "Deze rechten op alle ploegen (rechten + 50)";
            this.checkBoxAllePloegen.UseVisualStyleBackColor = true;
            this.checkBoxAllePloegen.CheckedChanged += new System.EventHandler(this.CheckBoxAllePloegen_CheckedChanged);
            // 
            // labelRechtenNivo
            // 
            this.labelRechtenNivo.AutoSize = true;
            this.labelRechtenNivo.Location = new System.Drawing.Point(236, 17);
            this.labelRechtenNivo.Name = "labelRechtenNivo";
            this.labelRechtenNivo.Size = new System.Drawing.Size(13, 13);
            this.labelRechtenNivo.TabIndex = 8;
            this.labelRechtenNivo.Text = "0";
            // 
            // buttonSave
            // 
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSave.Location = new System.Drawing.Point(15, 309);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(268, 23);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Cancel";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(15, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(268, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 259);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(268, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Reset Wachtwoord";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // radioButton50
            // 
            this.radioButton50.AutoSize = true;
            this.radioButton50.Location = new System.Drawing.Point(15, 99);
            this.radioButton50.Name = "radioButton50";
            this.radioButton50.Size = new System.Drawing.Size(268, 17);
            this.radioButton50.TabIndex = 12;
            this.radioButton50.TabStop = true;
            this.radioButton50.Text = "Personeel toevoegen, verwijderen eigen wacht (50)";
            this.radioButton50.UseVisualStyleBackColor = true;
            this.radioButton50.CheckedChanged += new System.EventHandler(this.RadioButton0_CheckedChanged);
            // 
            // radioButton25
            // 
            this.radioButton25.AutoSize = true;
            this.radioButton25.Location = new System.Drawing.Point(15, 143);
            this.radioButton25.Name = "radioButton25";
            this.radioButton25.Size = new System.Drawing.Size(180, 17);
            this.radioButton25.TabIndex = 13;
            this.radioButton25.TabStop = true;
            this.radioButton25.Text = "Maak bezetting eigen wacht (25)";
            this.radioButton25.UseVisualStyleBackColor = true;
            this.radioButton25.CheckedChanged += new System.EventHandler(this.RadioButton0_CheckedChanged);
            // 
            // radioButton0
            // 
            this.radioButton0.AutoSize = true;
            this.radioButton0.Location = new System.Drawing.Point(15, 187);
            this.radioButton0.Name = "radioButton0";
            this.radioButton0.Size = new System.Drawing.Size(170, 17);
            this.radioButton0.TabIndex = 14;
            this.radioButton0.TabStop = true;
            this.radioButton0.Text = "Geen rechten, alleen kijken (0)";
            this.radioButton0.UseVisualStyleBackColor = true;
            this.radioButton0.CheckedChanged += new System.EventHandler(this.RadioButton0_CheckedChanged);
            // 
            // checkBoxAlleenZelf
            // 
            this.checkBoxAlleenZelf.AutoSize = true;
            this.checkBoxAlleenZelf.Location = new System.Drawing.Point(13, 15);
            this.checkBoxAlleenZelf.Name = "checkBoxAlleenZelf";
            this.checkBoxAlleenZelf.Size = new System.Drawing.Size(124, 17);
            this.checkBoxAlleenZelf.TabIndex = 15;
            this.checkBoxAlleenZelf.Text = "Alleen je zelf invullen";
            this.checkBoxAlleenZelf.UseVisualStyleBackColor = true;
            this.checkBoxAlleenZelf.CheckedChanged += new System.EventHandler(this.checkBoxAlleenZelf_CheckedChanged);
            // 
            // checkBoxAlleenAndere
            // 
            this.checkBoxAlleenAndere.AutoSize = true;
            this.checkBoxAlleenAndere.Location = new System.Drawing.Point(13, 39);
            this.checkBoxAlleenAndere.Name = "checkBoxAlleenAndere";
            this.checkBoxAlleenAndere.Size = new System.Drawing.Size(179, 17);
            this.checkBoxAlleenAndere.TabIndex = 16;
            this.checkBoxAlleenAndere.Text = "Alleen wacht invullen, niet je zelf";
            this.checkBoxAlleenAndere.UseVisualStyleBackColor = true;
            this.checkBoxAlleenAndere.CheckedChanged += new System.EventHandler(this.checkBoxAlleenAndere_CheckedChanged);
            // 
            // panel25
            // 
            this.panel25.Controls.Add(this.checkBoxAlleenZelf);
            this.panel25.Controls.Add(this.checkBoxAlleenAndere);
            this.panel25.Location = new System.Drawing.Point(298, 114);
            this.panel25.Name = "panel25";
            this.panel25.Size = new System.Drawing.Size(214, 68);
            this.panel25.TabIndex = 17;
            // 
            // RechtenInstellenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 400);
            this.Controls.Add(this.panel25);
            this.Controls.Add(this.radioButton0);
            this.Controls.Add(this.radioButton25);
            this.Controls.Add(this.radioButton50);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelRechtenNivo);
            this.Controls.Add(this.checkBoxAllePloegen);
            this.Controls.Add(this.labelPersoneelNummer);
            this.Controls.Add(this.labelNaam);
            this.Controls.Add(this.label1);
            this.Name = "RechtenInstellenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rechten Instellen";
            this.Shown += new System.EventHandler(this.RechtenInstellenForm_Shown);
            this.panel25.ResumeLayout(false);
            this.panel25.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label labelNaam;
        public System.Windows.Forms.Label labelPersoneelNummer;
        public System.Windows.Forms.CheckBox checkBoxAllePloegen;
        public System.Windows.Forms.Label labelRechtenNivo;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.RadioButton radioButton50;
        public System.Windows.Forms.RadioButton radioButton25;
        public System.Windows.Forms.RadioButton radioButton0;
        private System.Windows.Forms.CheckBox checkBoxAlleenZelf;
        private System.Windows.Forms.CheckBox checkBoxAlleenAndere;
        private System.Windows.Forms.Panel panel25;
    }
}