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
            this.trackBarRechten = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxAllePloegen = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelRechtenNivo = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRechten)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Rechten van gebruiker : ";
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(169, 17);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(39, 13);
            this.labelNaam.TabIndex = 1;
            this.labelNaam.Text = "Majoor";
            // 
            // labelPersoneelNummer
            // 
            this.labelPersoneelNummer.AutoSize = true;
            this.labelPersoneelNummer.Location = new System.Drawing.Point(274, 18);
            this.labelPersoneelNummer.Name = "labelPersoneelNummer";
            this.labelPersoneelNummer.Size = new System.Drawing.Size(43, 13);
            this.labelPersoneelNummer.TabIndex = 2;
            this.labelPersoneelNummer.Text = "590588";
            // 
            // trackBarRechten
            // 
            this.trackBarRechten.Location = new System.Drawing.Point(15, 54);
            this.trackBarRechten.Maximum = 50;
            this.trackBarRechten.Name = "trackBarRechten";
            this.trackBarRechten.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBarRechten.Size = new System.Drawing.Size(45, 326);
            this.trackBarRechten.TabIndex = 3;
            this.trackBarRechten.TickFrequency = 25;
            this.trackBarRechten.ValueChanged += new System.EventHandler(this.trackBarRechten_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 358);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Geen rechten, alleen kijken (0)";
            // 
            // checkBoxAllePloegen
            // 
            this.checkBoxAllePloegen.AutoSize = true;
            this.checkBoxAllePloegen.Location = new System.Drawing.Point(15, 398);
            this.checkBoxAllePloegen.Name = "checkBoxAllePloegen";
            this.checkBoxAllePloegen.Size = new System.Drawing.Size(165, 17);
            this.checkBoxAllePloegen.TabIndex = 5;
            this.checkBoxAllePloegen.Text = "Deze rechten op alle ploegen";
            this.checkBoxAllePloegen.UseVisualStyleBackColor = true;
            this.checkBoxAllePloegen.CheckedChanged += new System.EventHandler(this.checkBoxAllePloegen_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Personeel toevoegen, verwijderen eigen wacht (50)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(66, 212);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Maak bezetting eigen wacht (25)";
            // 
            // labelRechtenNivo
            // 
            this.labelRechtenNivo.AutoSize = true;
            this.labelRechtenNivo.Location = new System.Drawing.Point(389, 18);
            this.labelRechtenNivo.Name = "labelRechtenNivo";
            this.labelRechtenNivo.Size = new System.Drawing.Size(13, 13);
            this.labelRechtenNivo.TabIndex = 8;
            this.labelRechtenNivo.Text = "0";
            // 
            // buttonSave
            // 
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonSave.Location = new System.Drawing.Point(292, 348);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(162, 23);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Cancel";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(292, 392);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(292, 302);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(162, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Reset Wachtwoord";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // RechtenInstellenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 428);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelRechtenNivo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxAllePloegen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBarRechten);
            this.Controls.Add(this.labelPersoneelNummer);
            this.Controls.Add(this.labelNaam);
            this.Controls.Add(this.label1);
            this.Name = "RechtenInstellenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rechten Instellen";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRechten)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label labelNaam;
        public System.Windows.Forms.Label labelPersoneelNummer;
        public System.Windows.Forms.TrackBar trackBarRechten;
        public System.Windows.Forms.CheckBox checkBoxAllePloegen;
        public System.Windows.Forms.Label labelRechtenNivo;
        private System.Windows.Forms.Button button2;
    }
}