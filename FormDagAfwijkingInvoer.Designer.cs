namespace Bezetting2
{
    partial class FormDagAfwijkingInvoer
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
            this.textBoxAfwijking = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxRede = new System.Windows.Forms.TextBox();
            this.buttonHistory = new System.Windows.Forms.Button();
            this.buttonVoerIn = new System.Windows.Forms.Button();
            this.listBoxItems = new System.Windows.Forms.ListBox();
            this.labelNaam = new System.Windows.Forms.Label();
            this.labelDatum = new System.Windows.Forms.Label();
            this.labelPersoneelnr = new System.Windows.Forms.Label();
            this.labelMaand = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Afwijking tov Rooster";
            // 
            // textBoxAfwijking
            // 
            this.textBoxAfwijking.Location = new System.Drawing.Point(13, 206);
            this.textBoxAfwijking.Name = "textBoxAfwijking";
            this.textBoxAfwijking.Size = new System.Drawing.Size(249, 20);
            this.textBoxAfwijking.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rede Afwijking tov Rooster";
            // 
            // textBoxRede
            // 
            this.textBoxRede.Location = new System.Drawing.Point(12, 262);
            this.textBoxRede.Name = "textBoxRede";
            this.textBoxRede.Size = new System.Drawing.Size(250, 20);
            this.textBoxRede.TabIndex = 4;
            // 
            // buttonHistory
            // 
            this.buttonHistory.Location = new System.Drawing.Point(14, 317);
            this.buttonHistory.Name = "buttonHistory";
            this.buttonHistory.Size = new System.Drawing.Size(118, 22);
            this.buttonHistory.TabIndex = 5;
            this.buttonHistory.Text = "History";
            this.buttonHistory.UseVisualStyleBackColor = true;
            this.buttonHistory.Click += new System.EventHandler(this.buttonHistory_Click);
            // 
            // buttonVoerIn
            // 
            this.buttonVoerIn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonVoerIn.Location = new System.Drawing.Point(144, 319);
            this.buttonVoerIn.Name = "buttonVoerIn";
            this.buttonVoerIn.Size = new System.Drawing.Size(118, 22);
            this.buttonVoerIn.TabIndex = 6;
            this.buttonVoerIn.Text = "Voer in";
            this.buttonVoerIn.UseVisualStyleBackColor = true;
            this.buttonVoerIn.Click += new System.EventHandler(this.buttonVoerIn_Click);
            // 
            // listBoxItems
            // 
            this.listBoxItems.FormattingEnabled = true;
            this.listBoxItems.Items.AddRange(new object[] {
            "Wis",
            "VK",
            "8OI",
            "A",
            "ADV",
            "VF",
            "1 OI Eerste",
            "2 OI Eerste",
            "1 OI Laatste",
            "2 OI Laatste"});
            this.listBoxItems.Location = new System.Drawing.Point(13, 61);
            this.listBoxItems.Name = "listBoxItems";
            this.listBoxItems.Size = new System.Drawing.Size(250, 134);
            this.listBoxItems.TabIndex = 7;
            this.listBoxItems.SelectedIndexChanged += new System.EventHandler(this.listBoxItems_SelectedIndexChanged);
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(10, 10);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(39, 13);
            this.labelNaam.TabIndex = 8;
            this.labelNaam.Text = "Majoor";
            // 
            // labelDatum
            // 
            this.labelDatum.AutoSize = true;
            this.labelDatum.Location = new System.Drawing.Point(197, 10);
            this.labelDatum.Name = "labelDatum";
            this.labelDatum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelDatum.Size = new System.Drawing.Size(19, 13);
            this.labelDatum.TabIndex = 9;
            this.labelDatum.Text = "25";
            // 
            // labelPersoneelnr
            // 
            this.labelPersoneelnr.AutoSize = true;
            this.labelPersoneelnr.Location = new System.Drawing.Point(126, 10);
            this.labelPersoneelnr.Name = "labelPersoneelnr";
            this.labelPersoneelnr.Size = new System.Drawing.Size(43, 13);
            this.labelPersoneelnr.TabIndex = 10;
            this.labelPersoneelnr.Text = "590588";
            // 
            // labelMaand
            // 
            this.labelMaand.AutoSize = true;
            this.labelMaand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMaand.Location = new System.Drawing.Point(236, 10);
            this.labelMaand.Name = "labelMaand";
            this.labelMaand.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelMaand.Size = new System.Drawing.Size(27, 13);
            this.labelMaand.TabIndex = 11;
            this.labelMaand.Text = "April";
            // 
            // FormDagAfwijkingInvoer
            // 
            this.AcceptButton = this.buttonVoerIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 353);
            this.Controls.Add(this.labelMaand);
            this.Controls.Add(this.labelPersoneelnr);
            this.Controls.Add(this.labelDatum);
            this.Controls.Add(this.labelNaam);
            this.Controls.Add(this.listBoxItems);
            this.Controls.Add(this.buttonVoerIn);
            this.Controls.Add(this.buttonHistory);
            this.Controls.Add(this.textBoxRede);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxAfwijking);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDagAfwijkingInvoer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Shown += new System.EventHandler(this.FormDagAfwijkingInvoer_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxAfwijking;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxRede;
        private System.Windows.Forms.Button buttonHistory;
        private System.Windows.Forms.Button buttonVoerIn;
        private System.Windows.Forms.ListBox listBoxItems;
        public System.Windows.Forms.Label labelNaam;
        public System.Windows.Forms.Label labelDatum;
        public System.Windows.Forms.Label labelPersoneelnr;
        public System.Windows.Forms.Label labelMaand;
    }
}