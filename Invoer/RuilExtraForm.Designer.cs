namespace Bezetting2.Invoer
{
    partial class RuilExtraForm
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
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.comboBoxPloeg = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxDienst = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxWerkplek = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxRO = new System.Windows.Forms.CheckBox();
            this.checkBoxGR = new System.Windows.Forms.CheckBox();
            this.checkBoxGE = new System.Windows.Forms.CheckBox();
            this.checkBoxWI = new System.Windows.Forms.CheckBox();
            this.checkBoxBL = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxAanvraagVoor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.buttonVraagAan = new System.Windows.Forms.Button();
            this.labelNaam = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listViewExtra = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelVorigeMaand = new System.Windows.Forms.Label();
            this.buttonVulDienst = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Op Ploeg";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(96, 142);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 1;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // comboBoxPloeg
            // 
            this.comboBoxPloeg.FormattingEnabled = true;
            this.comboBoxPloeg.Items.AddRange(new object[] {
            "Blauw",
            "Wit",
            "Geel",
            "Groen",
            "Rood"});
            this.comboBoxPloeg.Location = new System.Drawing.Point(96, 109);
            this.comboBoxPloeg.Name = "comboBoxPloeg";
            this.comboBoxPloeg.Size = new System.Drawing.Size(171, 24);
            this.comboBoxPloeg.TabIndex = 2;
            this.comboBoxPloeg.SelectedIndexChanged += new System.EventHandler(this.comboBoxPloeg_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Op Datum";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Dienst";
            // 
            // comboBoxDienst
            // 
            this.comboBoxDienst.FormattingEnabled = true;
            this.comboBoxDienst.Items.AddRange(new object[] {
            "Ochtend",
            "Middag",
            "Nacht"});
            this.comboBoxDienst.Location = new System.Drawing.Point(96, 317);
            this.comboBoxDienst.Name = "comboBoxDienst";
            this.comboBoxDienst.Size = new System.Drawing.Size(171, 24);
            this.comboBoxDienst.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 357);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 16);
            this.label4.TabIndex = 6;
            this.label4.Text = "Werkplek";
            // 
            // comboBoxWerkplek
            // 
            this.comboBoxWerkplek.FormattingEnabled = true;
            this.comboBoxWerkplek.Items.AddRange(new object[] {
            "Groen",
            "Geel",
            "Wit",
            "Blauw",
            "Geel"});
            this.comboBoxWerkplek.Location = new System.Drawing.Point(96, 354);
            this.comboBoxWerkplek.Name = "comboBoxWerkplek";
            this.comboBoxWerkplek.Size = new System.Drawing.Size(171, 24);
            this.comboBoxWerkplek.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxRO);
            this.groupBox1.Controls.Add(this.checkBoxGR);
            this.groupBox1.Controls.Add(this.checkBoxGE);
            this.groupBox1.Controls.Add(this.checkBoxWI);
            this.groupBox1.Controls.Add(this.checkBoxBL);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxAanvraagVoor);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.buttonVraagAan);
            this.groupBox1.Controls.Add(this.labelNaam);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBoxWerkplek);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxDienst);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxPloeg);
            this.groupBox1.Controls.Add(this.monthCalendar1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(24, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 570);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extra/Ruil Dienst Gevraagd";
            // 
            // checkBoxRO
            // 
            this.checkBoxRO.AutoSize = true;
            this.checkBoxRO.Location = new System.Drawing.Point(96, 499);
            this.checkBoxRO.Name = "checkBoxRO";
            this.checkBoxRO.Size = new System.Drawing.Size(61, 20);
            this.checkBoxRO.TabIndex = 20;
            this.checkBoxRO.Text = "Rood";
            this.checkBoxRO.UseVisualStyleBackColor = true;
            // 
            // checkBoxGR
            // 
            this.checkBoxGR.AutoSize = true;
            this.checkBoxGR.Location = new System.Drawing.Point(96, 473);
            this.checkBoxGR.Name = "checkBoxGR";
            this.checkBoxGR.Size = new System.Drawing.Size(64, 20);
            this.checkBoxGR.TabIndex = 19;
            this.checkBoxGR.Text = "Groen";
            this.checkBoxGR.UseVisualStyleBackColor = true;
            // 
            // checkBoxGE
            // 
            this.checkBoxGE.AutoSize = true;
            this.checkBoxGE.Location = new System.Drawing.Point(96, 447);
            this.checkBoxGE.Name = "checkBoxGE";
            this.checkBoxGE.Size = new System.Drawing.Size(56, 20);
            this.checkBoxGE.TabIndex = 18;
            this.checkBoxGE.Text = "Geel";
            this.checkBoxGE.UseVisualStyleBackColor = true;
            // 
            // checkBoxWI
            // 
            this.checkBoxWI.AutoSize = true;
            this.checkBoxWI.Location = new System.Drawing.Point(96, 421);
            this.checkBoxWI.Name = "checkBoxWI";
            this.checkBoxWI.Size = new System.Drawing.Size(46, 20);
            this.checkBoxWI.TabIndex = 17;
            this.checkBoxWI.Text = "Wit";
            this.checkBoxWI.UseVisualStyleBackColor = true;
            // 
            // checkBoxBL
            // 
            this.checkBoxBL.AutoSize = true;
            this.checkBoxBL.Location = new System.Drawing.Point(96, 395);
            this.checkBoxBL.Name = "checkBoxBL";
            this.checkBoxBL.Size = new System.Drawing.Size(63, 20);
            this.checkBoxBL.TabIndex = 16;
            this.checkBoxBL.Text = "Blauw";
            this.checkBoxBL.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 397);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Door Kleur";
            // 
            // textBoxAanvraagVoor
            // 
            this.textBoxAanvraagVoor.Location = new System.Drawing.Point(121, 51);
            this.textBoxAanvraagVoor.Name = "textBoxAanvraagVoor";
            this.textBoxAanvraagVoor.Size = new System.Drawing.Size(146, 22);
            this.textBoxAanvraagVoor.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Aanvraag Voor :";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(177, 83);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(90, 20);
            this.radioButton2.TabIndex = 12;
            this.radioButton2.Text = "Ruil Dienst";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(20, 83);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(97, 20);
            this.radioButton1.TabIndex = 11;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Extra Dienst";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // buttonVraagAan
            // 
            this.buttonVraagAan.Enabled = false;
            this.buttonVraagAan.Location = new System.Drawing.Point(96, 535);
            this.buttonVraagAan.Name = "buttonVraagAan";
            this.buttonVraagAan.Size = new System.Drawing.Size(171, 29);
            this.buttonVraagAan.TabIndex = 10;
            this.buttonVraagAan.Text = "Vraag Aan";
            this.buttonVraagAan.UseVisualStyleBackColor = true;
            this.buttonVraagAan.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelNaam
            // 
            this.labelNaam.AutoSize = true;
            this.labelNaam.Location = new System.Drawing.Point(118, 29);
            this.labelNaam.Name = "labelNaam";
            this.labelNaam.Size = new System.Drawing.Size(50, 16);
            this.labelNaam.TabIndex = 9;
            this.labelNaam.Text = "majoor";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Aanvraag Door :";
            // 
            // listViewExtra
            // 
            this.listViewExtra.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.listViewExtra.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewExtra.FullRowSelect = true;
            this.listViewExtra.GridLines = true;
            this.listViewExtra.HideSelection = false;
            this.listViewExtra.Location = new System.Drawing.Point(333, 25);
            this.listViewExtra.MultiSelect = false;
            this.listViewExtra.Name = "listViewExtra";
            this.listViewExtra.Size = new System.Drawing.Size(851, 569);
            this.listViewExtra.TabIndex = 3;
            this.listViewExtra.UseCompatibleStateImageBehavior = false;
            this.listViewExtra.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Gevraagd door";
            this.columnHeader1.Width = 110;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Gevraagd voor";
            this.columnHeader2.Width = 110;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Soort";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Op Ploeg";
            this.columnHeader4.Width = 80;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Datum";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Dienst";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Werkplek";
            this.columnHeader7.Width = 76;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Door Ploeg";
            this.columnHeader8.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Loopt door";
            this.columnHeader9.Width = 120;
            // 
            // labelVorigeMaand
            // 
            this.labelVorigeMaand.AutoSize = true;
            this.labelVorigeMaand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVorigeMaand.Location = new System.Drawing.Point(330, 9);
            this.labelVorigeMaand.Name = "labelVorigeMaand";
            this.labelVorigeMaand.Size = new System.Drawing.Size(315, 16);
            this.labelVorigeMaand.TabIndex = 6;
            this.labelVorigeMaand.Text = "Gevraagde extra/ruil diensten. (10 maanden vooruit)";
            // 
            // buttonVulDienst
            // 
            this.buttonVulDienst.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonVulDienst.Location = new System.Drawing.Point(1013, 615);
            this.buttonVulDienst.Name = "buttonVulDienst";
            this.buttonVulDienst.Size = new System.Drawing.Size(171, 29);
            this.buttonVulDienst.TabIndex = 11;
            this.buttonVulDienst.Text = "Dienst invullen";
            this.buttonVulDienst.UseVisualStyleBackColor = true;
            this.buttonVulDienst.Click += new System.EventHandler(this.button2_Click);
            // 
            // RuilExtraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 666);
            this.Controls.Add(this.buttonVulDienst);
            this.Controls.Add(this.labelVorigeMaand);
            this.Controls.Add(this.listViewExtra);
            this.Controls.Add(this.groupBox1);
            this.Name = "RuilExtraForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ruildienst Extradienst";
            this.Shown += new System.EventHandler(this.RuilExtraForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.ComboBox comboBoxPloeg;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxDienst;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxWerkplek;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label labelNaam;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBoxAanvraagVoor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListView listViewExtra;
        private System.Windows.Forms.Label labelVorigeMaand;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Button buttonVulDienst;
        public System.Windows.Forms.Button buttonVraagAan;
        private System.Windows.Forms.CheckBox checkBoxRO;
        private System.Windows.Forms.CheckBox checkBoxGR;
        private System.Windows.Forms.CheckBox checkBoxGE;
        private System.Windows.Forms.CheckBox checkBoxWI;
        private System.Windows.Forms.CheckBox checkBoxBL;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
    }
}