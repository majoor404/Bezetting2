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
            this.checkBoxGebruikRuilExtra.CheckedChanged += new System.EventHandler(this.checkBoxGebruikRuilExtra_CheckedChanged);
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
            this.checkBoxGebruikSnipper.CheckedChanged += new System.EventHandler(this.checkBoxGebruikSnipper_CheckedChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonSave.Location = new System.Drawing.Point(266, 352);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
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
            // InstellingenProgrammaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 387);
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
    }
}