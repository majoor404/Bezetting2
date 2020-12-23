namespace Bezetting2
{
    partial class InlogForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxNum = new System.Windows.Forms.TextBox();
            this.textBoxPass = new System.Windows.Forms.TextBox();
            this.buttonOke = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxChangePasswoord = new System.Windows.Forms.TextBox();
            this.buttonVerander = new System.Windows.Forms.Button();
            this.groupBoxAutoInlog = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelAutoInlogNaam = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxAutoInlog = new System.Windows.Forms.CheckBox();
            this.groupBoxAutoInlog.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Personeel Nummer";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Passwoord";
            // 
            // textBoxNum
            // 
            this.textBoxNum.Location = new System.Drawing.Point(128, 12);
            this.textBoxNum.Name = "textBoxNum";
            this.textBoxNum.Size = new System.Drawing.Size(215, 20);
            this.textBoxNum.TabIndex = 3;
            this.textBoxNum.TextChanged += new System.EventHandler(this.TextBoxNum_TextChanged);
            // 
            // textBoxPass
            // 
            this.textBoxPass.Location = new System.Drawing.Point(128, 45);
            this.textBoxPass.Name = "textBoxPass";
            this.textBoxPass.PasswordChar = '*';
            this.textBoxPass.Size = new System.Drawing.Size(215, 20);
            this.textBoxPass.TabIndex = 4;
            this.textBoxPass.TextChanged += new System.EventHandler(this.TextBoxPass_TextChanged);
            // 
            // buttonOke
            // 
            this.buttonOke.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOke.Location = new System.Drawing.Point(128, 158);
            this.buttonOke.Name = "buttonOke";
            this.buttonOke.Size = new System.Drawing.Size(215, 23);
            this.buttonOke.TabIndex = 5;
            this.buttonOke.Text = "Enter";
            this.buttonOke.UseVisualStyleBackColor = true;
            this.buttonOke.Click += new System.EventHandler(this.ButtonOke_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Passwoord Change";
            // 
            // textBoxChangePasswoord
            // 
            this.textBoxChangePasswoord.Location = new System.Drawing.Point(128, 192);
            this.textBoxChangePasswoord.Name = "textBoxChangePasswoord";
            this.textBoxChangePasswoord.PasswordChar = '*';
            this.textBoxChangePasswoord.Size = new System.Drawing.Size(215, 20);
            this.textBoxChangePasswoord.TabIndex = 7;
            // 
            // buttonVerander
            // 
            this.buttonVerander.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonVerander.Enabled = false;
            this.buttonVerander.Location = new System.Drawing.Point(128, 227);
            this.buttonVerander.Name = "buttonVerander";
            this.buttonVerander.Size = new System.Drawing.Size(215, 23);
            this.buttonVerander.TabIndex = 8;
            this.buttonVerander.Text = "Change";
            this.buttonVerander.UseVisualStyleBackColor = true;
            this.buttonVerander.Click += new System.EventHandler(this.ButtonVerander_Click);
            // 
            // groupBoxAutoInlog
            // 
            this.groupBoxAutoInlog.Controls.Add(this.label4);
            this.groupBoxAutoInlog.Controls.Add(this.labelAutoInlogNaam);
            this.groupBoxAutoInlog.Controls.Add(this.button1);
            this.groupBoxAutoInlog.Controls.Add(this.checkBoxAutoInlog);
            this.groupBoxAutoInlog.Location = new System.Drawing.Point(3, 81);
            this.groupBoxAutoInlog.Name = "groupBoxAutoInlog";
            this.groupBoxAutoInlog.Size = new System.Drawing.Size(344, 62);
            this.groupBoxAutoInlog.TabIndex = 13;
            this.groupBoxAutoInlog.TabStop = false;
            this.groupBoxAutoInlog.Text = "Auto Inlog";
            this.groupBoxAutoInlog.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Op Naam :";
            // 
            // labelAutoInlogNaam
            // 
            this.labelAutoInlogNaam.AutoSize = true;
            this.labelAutoInlogNaam.Location = new System.Drawing.Point(210, 29);
            this.labelAutoInlogNaam.Name = "labelAutoInlogNaam";
            this.labelAutoInlogNaam.Size = new System.Drawing.Size(43, 13);
            this.labelAutoInlogNaam.TabIndex = 15;
            this.labelAutoInlogNaam.Text = "000000";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(304, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 20);
            this.button1.TabIndex = 14;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBoxAutoInlog
            // 
            this.checkBoxAutoInlog.AutoSize = true;
            this.checkBoxAutoInlog.Location = new System.Drawing.Point(20, 28);
            this.checkBoxAutoInlog.Name = "checkBoxAutoInlog";
            this.checkBoxAutoInlog.Size = new System.Drawing.Size(73, 17);
            this.checkBoxAutoInlog.TabIndex = 13;
            this.checkBoxAutoInlog.Text = "Auto inlog";
            this.checkBoxAutoInlog.UseVisualStyleBackColor = true;
            this.checkBoxAutoInlog.CheckedChanged += new System.EventHandler(this.checkBoxAutoInlog_CheckedChanged);
            this.checkBoxAutoInlog.Click += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // InlogForm
            // 
            this.AcceptButton = this.buttonOke;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 264);
            this.Controls.Add(this.groupBoxAutoInlog);
            this.Controls.Add(this.buttonVerander);
            this.Controls.Add(this.textBoxChangePasswoord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonOke);
            this.Controls.Add(this.textBoxPass);
            this.Controls.Add(this.textBoxNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InlogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inlog";
            this.Shown += new System.EventHandler(this.InlogForm_Shown);
            this.groupBoxAutoInlog.ResumeLayout(false);
            this.groupBoxAutoInlog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxNum;
        private System.Windows.Forms.TextBox textBoxPass;
        private System.Windows.Forms.Button buttonOke;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxChangePasswoord;
        private System.Windows.Forms.Button buttonVerander;
        private System.Windows.Forms.GroupBox groupBoxAutoInlog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelAutoInlogNaam;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBoxAutoInlog;
    }
}