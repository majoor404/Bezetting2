namespace Bezetting2.Invoer
{
    partial class InvoerLoopExtraDienst
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
            this.label3 = new System.Windows.Forms.Label();
            this.labelDatum = new System.Windows.Forms.Label();
            this.labelDienst = new System.Windows.Forms.Label();
            this.textBoxLoopt = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonInvoer = new System.Windows.Forms.Button();
            this.labelKleur = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Wie Loopt Extra/Ruil Voor :";
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(12, 34);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(35, 13);
            this.labelNaam.TabIndex = 1;
            this.labelNaam.Text = "Naam";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(101, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Op";
            // 
            // labelDatum
            // 
            this.labelDatum.AutoSize = true;
            this.labelDatum.Location = new System.Drawing.Point(210, 34);
            this.labelDatum.Name = "labelDatum";
            this.labelDatum.Size = new System.Drawing.Size(55, 13);
            this.labelDatum.TabIndex = 3;
            this.labelDatum.Text = "25-4-1965";
            // 
            // labelDienst
            // 
            this.labelDienst.AutoSize = true;
            this.labelDienst.Location = new System.Drawing.Point(280, 35);
            this.labelDienst.Name = "labelDienst";
            this.labelDienst.Size = new System.Drawing.Size(74, 13);
            this.labelDienst.TabIndex = 4;
            this.labelDienst.Text = "eerste Middag";
            // 
            // textBoxLoopt
            // 
            this.textBoxLoopt.Location = new System.Drawing.Point(62, 85);
            this.textBoxLoopt.Name = "textBoxLoopt";
            this.textBoxLoopt.Size = new System.Drawing.Size(250, 20);
            this.textBoxLoopt.TabIndex = 5;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(24, 145);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(125, 26);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonInvoer
            // 
            this.buttonInvoer.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonInvoer.Location = new System.Drawing.Point(229, 145);
            this.buttonInvoer.Name = "buttonInvoer";
            this.buttonInvoer.Size = new System.Drawing.Size(125, 26);
            this.buttonInvoer.TabIndex = 7;
            this.buttonInvoer.Text = "Invoer";
            this.buttonInvoer.UseVisualStyleBackColor = true;
            // 
            // labelKleur
            // 
            this.labelKleur.AutoSize = true;
            this.labelKleur.Location = new System.Drawing.Point(128, 35);
            this.labelKleur.Name = "labelKleur";
            this.labelKleur.Size = new System.Drawing.Size(36, 13);
            this.labelKleur.TabIndex = 8;
            this.labelKleur.Text = "Blauw";
            // 
            // InvoerLoopExtraDienst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 199);
            this.Controls.Add(this.labelKleur);
            this.Controls.Add(this.buttonInvoer);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxLoopt);
            this.Controls.Add(this.labelDienst);
            this.Controls.Add(this.labelDatum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelNaam);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InvoerLoopExtraDienst";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Loop Extra/Ruil Dienst";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label labelNaam;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label labelDatum;
        public System.Windows.Forms.Label labelDienst;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonInvoer;
        public System.Windows.Forms.Label labelKleur;
        public System.Windows.Forms.TextBox textBoxLoopt;
    }
}