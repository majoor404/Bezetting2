namespace Bezetting2
{
    partial class DagAfwijkingInvoerForm
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
            this.buttonReeks = new System.Windows.Forms.Button();
            this.buttonCancelInvoer = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Afwijking tov Rooster";
            // 
            // textBoxAfwijking
            // 
            this.textBoxAfwijking.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxAfwijking.Location = new System.Drawing.Point(15, 409);
            this.textBoxAfwijking.Name = "textBoxAfwijking";
            this.textBoxAfwijking.Size = new System.Drawing.Size(250, 20);
            this.textBoxAfwijking.TabIndex = 2;
            this.textBoxAfwijking.TextChanged += new System.EventHandler(this.TextBoxAfwijking_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 442);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rede Afwijking tov Rooster";
            // 
            // textBoxRede
            // 
            this.textBoxRede.Location = new System.Drawing.Point(15, 458);
            this.textBoxRede.Name = "textBoxRede";
            this.textBoxRede.Size = new System.Drawing.Size(250, 20);
            this.textBoxRede.TabIndex = 4;
            // 
            // buttonHistory
            // 
            this.buttonHistory.Location = new System.Drawing.Point(15, 518);
            this.buttonHistory.Name = "buttonHistory";
            this.buttonHistory.Size = new System.Drawing.Size(115, 25);
            this.buttonHistory.TabIndex = 5;
            this.buttonHistory.Text = "History";
            this.buttonHistory.UseVisualStyleBackColor = true;
            this.buttonHistory.Click += new System.EventHandler(this.ButtonHistory_Click);
            // 
            // buttonVoerIn
            // 
            this.buttonVoerIn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonVoerIn.Enabled = false;
            this.buttonVoerIn.Location = new System.Drawing.Point(150, 518);
            this.buttonVoerIn.Name = "buttonVoerIn";
            this.buttonVoerIn.Size = new System.Drawing.Size(115, 25);
            this.buttonVoerIn.TabIndex = 6;
            this.buttonVoerIn.Text = "Voer in";
            this.buttonVoerIn.UseVisualStyleBackColor = true;
            this.buttonVoerIn.Click += new System.EventHandler(this.ButtonVoerIn_Click);
            // 
            // listBoxItems
            // 
            this.listBoxItems.FormattingEnabled = true;
            this.listBoxItems.Items.AddRange(new object[] {
            "VK",
            "8OI",
            "A",
            "VRIJ",
            "VAK",
            "OPLO",
            "VF",
            "Z",
            "1/2 VK",
            "OI 1 eerste",
            "OI 2 eerste",
            "OI 1 laatste",
            "OI 2 laatste",
            "BV",
            "ED-O",
            "ED-M",
            "ED-N",
            "RD-O",
            "RD-M",
            "RD-N",
            "VD-O",
            "VD-M",
            "VD-N",
            "DD"});
            this.listBoxItems.Location = new System.Drawing.Point(15, 87);
            this.listBoxItems.Name = "listBoxItems";
            this.listBoxItems.Size = new System.Drawing.Size(250, 316);
            this.listBoxItems.TabIndex = 7;
            this.listBoxItems.SelectedIndexChanged += new System.EventHandler(this.ListBoxItems_SelectedIndexChanged);
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(112, 9);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(39, 13);
            this.labelNaam.TabIndex = 8;
            this.labelNaam.Text = "Majoor";
            // 
            // labelDatum
            // 
            this.labelDatum.AutoSize = true;
            this.labelDatum.Location = new System.Drawing.Point(112, 33);
            this.labelDatum.Name = "labelDatum";
            this.labelDatum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelDatum.Size = new System.Drawing.Size(19, 13);
            this.labelDatum.TabIndex = 9;
            this.labelDatum.Text = "25";
            // 
            // labelPersoneelnr
            // 
            this.labelPersoneelnr.AutoSize = true;
            this.labelPersoneelnr.Location = new System.Drawing.Point(222, 10);
            this.labelPersoneelnr.Name = "labelPersoneelnr";
            this.labelPersoneelnr.Size = new System.Drawing.Size(43, 13);
            this.labelPersoneelnr.TabIndex = 10;
            this.labelPersoneelnr.Text = "590588";
            // 
            // labelMaand
            // 
            this.labelMaand.AutoSize = true;
            this.labelMaand.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMaand.Location = new System.Drawing.Point(185, 33);
            this.labelMaand.Name = "labelMaand";
            this.labelMaand.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelMaand.Size = new System.Drawing.Size(27, 13);
            this.labelMaand.TabIndex = 11;
            this.labelMaand.Text = "April";
            // 
            // buttonReeks
            // 
            this.buttonReeks.Location = new System.Drawing.Point(15, 556);
            this.buttonReeks.Name = "buttonReeks";
            this.buttonReeks.Size = new System.Drawing.Size(250, 25);
            this.buttonReeks.TabIndex = 12;
            this.buttonReeks.Text = "Voer Reeks in";
            this.buttonReeks.UseVisualStyleBackColor = true;
            this.buttonReeks.Click += new System.EventHandler(this.ButtonReeks_Click);
            // 
            // buttonCancelInvoer
            // 
            this.buttonCancelInvoer.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCancelInvoer.Location = new System.Drawing.Point(15, 484);
            this.buttonCancelInvoer.Name = "buttonCancelInvoer";
            this.buttonCancelInvoer.Size = new System.Drawing.Size(250, 25);
            this.buttonCancelInvoer.TabIndex = 13;
            this.buttonCancelInvoer.Text = "Cancel Invoer (verwijder)";
            this.buttonCancelInvoer.UseVisualStyleBackColor = true;
            this.buttonCancelInvoer.Click += new System.EventHandler(this.ButtonCancelInvoer_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Naam : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Wijzig Datum :";
            // 
            // DagAfwijkingInvoerForm
            // 
            this.AcceptButton = this.buttonVoerIn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 592);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonCancelInvoer);
            this.Controls.Add(this.buttonReeks);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DagAfwijkingInvoerForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
        private System.Windows.Forms.Button buttonReeks;
        private System.Windows.Forms.Button buttonCancelInvoer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}