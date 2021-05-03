
namespace Bezetting2.Invoer
{
    partial class ImportOpKleurForm
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
            this.labelOudeKleur = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxNieuweKleur = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonImport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.labelNaam = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data was opgeslagen van kleur :";
            // 
            // labelOudeKleur
            // 
            this.labelOudeKleur.AutoSize = true;
            this.labelOudeKleur.Location = new System.Drawing.Point(250, 33);
            this.labelOudeKleur.Name = "labelOudeKleur";
            this.labelOudeKleur.Size = new System.Drawing.Size(31, 13);
            this.labelOudeKleur.TabIndex = 0;
            this.labelOudeKleur.Text = "Kleur";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Op welke kleur moet ik deze importeren ?";
            // 
            // comboBoxNieuweKleur
            // 
            this.comboBoxNieuweKleur.FormattingEnabled = true;
            this.comboBoxNieuweKleur.Items.AddRange(new object[] {
            "Blauw",
            "Geel",
            "Groen",
            "Wit",
            "Rood",
            "DD"});
            this.comboBoxNieuweKleur.Location = new System.Drawing.Point(253, 54);
            this.comboBoxNieuweKleur.Name = "comboBoxNieuweKleur";
            this.comboBoxNieuweKleur.Size = new System.Drawing.Size(121, 21);
            this.comboBoxNieuweKleur.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(12, 120);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(119, 40);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonImport
            // 
            this.buttonImport.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonImport.Location = new System.Drawing.Point(255, 120);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(119, 40);
            this.buttonImport.TabIndex = 2;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Data was opgeslagen van persoon :";
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(250, 9);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(35, 13);
            this.labelNaam.TabIndex = 0;
            this.labelNaam.Text = "Naam";
            // 
            // ImportOpKleurForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 181);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.comboBoxNieuweKleur);
            this.Controls.Add(this.labelNaam);
            this.Controls.Add(this.labelOudeKleur);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "ImportOpKleurForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Op Kleur ?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label labelOudeKleur;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox comboBoxNieuweKleur;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label labelNaam;
    }
}