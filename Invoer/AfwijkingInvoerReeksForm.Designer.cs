namespace Bezetting2.Invoer
{
    partial class AfwijkingInvoerReeksForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelMaand = new System.Windows.Forms.Label();
            this.labelPersoneelnr = new System.Windows.Forms.Label();
            this.labelDatum = new System.Windows.Forms.Label();
            this.labelNaam = new System.Windows.Forms.Label();
            this.listBoxItems = new System.Windows.Forms.ListBox();
            this.textBoxRede = new System.Windows.Forms.TextBox();
            this.textBoxAfwijking = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AantalDagen = new System.Windows.Forms.NumericUpDown();
            this.buttonVoerUit = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.AantalDagen)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 21;
            this.label4.Text = "Wijzeging Eerste Datum :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Naam : ";
            // 
            // labelMaand
            // 
            this.labelMaand.AutoSize = true;
            this.labelMaand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMaand.Location = new System.Drawing.Point(235, 32);
            this.labelMaand.Name = "labelMaand";
            this.labelMaand.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelMaand.Size = new System.Drawing.Size(27, 13);
            this.labelMaand.TabIndex = 19;
            this.labelMaand.Text = "April";
            // 
            // labelPersoneelnr
            // 
            this.labelPersoneelnr.AutoSize = true;
            this.labelPersoneelnr.Location = new System.Drawing.Point(222, 9);
            this.labelPersoneelnr.Name = "labelPersoneelnr";
            this.labelPersoneelnr.Size = new System.Drawing.Size(43, 13);
            this.labelPersoneelnr.TabIndex = 18;
            this.labelPersoneelnr.Text = "590588";
            // 
            // labelDatum
            // 
            this.labelDatum.AutoSize = true;
            this.labelDatum.Location = new System.Drawing.Point(196, 32);
            this.labelDatum.Name = "labelDatum";
            this.labelDatum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelDatum.Size = new System.Drawing.Size(19, 13);
            this.labelDatum.TabIndex = 17;
            this.labelDatum.Text = "25";
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(99, 9);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(39, 13);
            this.labelNaam.TabIndex = 16;
            this.labelNaam.Text = "Majoor";
            // 
            // listBoxItems
            // 
            this.listBoxItems.FormattingEnabled = true;
            this.listBoxItems.Items.AddRange(new object[] {
            "VK",
            "8OI",
            "A",
            "VAK",
            "Z",
            "VF",
            "GP",
            "OPLO",
            "OPL",
            "K",
            "ADV",
            "BV"});
            this.listBoxItems.Location = new System.Drawing.Point(12, 61);
            this.listBoxItems.Name = "listBoxItems";
            this.listBoxItems.Size = new System.Drawing.Size(250, 186);
            this.listBoxItems.TabIndex = 24;
            this.listBoxItems.SelectedIndexChanged += new System.EventHandler(this.listBoxItems_SelectedIndexChanged);
            // 
            // textBoxRede
            // 
            this.textBoxRede.Location = new System.Drawing.Point(12, 311);
            this.textBoxRede.Name = "textBoxRede";
            this.textBoxRede.Size = new System.Drawing.Size(250, 20);
            this.textBoxRede.TabIndex = 23;
            // 
            // textBoxAfwijking
            // 
            this.textBoxAfwijking.Location = new System.Drawing.Point(12, 260);
            this.textBoxAfwijking.Name = "textBoxAfwijking";
            this.textBoxAfwijking.Size = new System.Drawing.Size(250, 20);
            this.textBoxAfwijking.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 295);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Rede Afwijking tov Rooster";
            // 
            // AantalDagen
            // 
            this.AantalDagen.Location = new System.Drawing.Point(282, 88);
            this.AantalDagen.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.AantalDagen.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AantalDagen.Name = "AantalDagen";
            this.AantalDagen.Size = new System.Drawing.Size(267, 20);
            this.AantalDagen.TabIndex = 27;
            this.AantalDagen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AantalDagen.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonVoerUit
            // 
            this.buttonVoerUit.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonVoerUit.Location = new System.Drawing.Point(282, 337);
            this.buttonVoerUit.Name = "buttonVoerUit";
            this.buttonVoerUit.Size = new System.Drawing.Size(267, 23);
            this.buttonVoerUit.TabIndex = 30;
            this.buttonVoerUit.Text = "Voer Uit";
            this.buttonVoerUit.UseVisualStyleBackColor = true;
            this.buttonVoerUit.Click += new System.EventHandler(this.buttonVoerUit_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(12, 337);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(250, 23);
            this.buttonCancel.TabIndex = 31;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Aantal copyeren volgens rooster dagen",
            "Aantal copyeren volgens kalender dagen",
            "Voor in voor hele ploeg",
            "GP invoer (2 op 2 af)"});
            this.comboBox1.Location = new System.Drawing.Point(282, 61);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(267, 21);
            this.comboBox1.TabIndex = 32;
            this.comboBox1.Text = "Aantal copyeren volgens rooster dagen";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // AfwijkingInvoerReeksForm
            // 
            this.AcceptButton = this.buttonCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 372);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonVoerUit);
            this.Controls.Add(this.AantalDagen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxItems);
            this.Controls.Add(this.textBoxRede);
            this.Controls.Add(this.textBoxAfwijking);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelMaand);
            this.Controls.Add(this.labelPersoneelnr);
            this.Controls.Add(this.labelDatum);
            this.Controls.Add(this.labelNaam);
            this.Name = "AfwijkingInvoerReeksForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Afwijking Invoer Reeks";
            ((System.ComponentModel.ISupportInitialize)(this.AantalDagen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label labelMaand;
        public System.Windows.Forms.Label labelPersoneelnr;
        public System.Windows.Forms.Label labelDatum;
        public System.Windows.Forms.Label labelNaam;
        private System.Windows.Forms.ListBox listBoxItems;
        private System.Windows.Forms.TextBox textBoxRede;
        private System.Windows.Forms.TextBox textBoxAfwijking;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown AantalDagen;
        private System.Windows.Forms.Button buttonVoerUit;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}