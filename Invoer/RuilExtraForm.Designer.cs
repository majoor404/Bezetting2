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
            this.checkBoxVerbergOudeVragen = new System.Windows.Forms.CheckBox();
            this.buttonAanvraagIntrekken = new System.Windows.Forms.Button();
            this.groupBoxFilter = new System.Windows.Forms.GroupBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.checkBoxVerbergIngevulde = new System.Windows.Forms.CheckBox();
            this.checkBoxVerbergCancel = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBoxFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Op Ploeg";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monthCalendar1.Location = new System.Drawing.Point(96, 180);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 1;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.MonthCalendar1_DateChanged);
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
            this.comboBoxPloeg.Location = new System.Drawing.Point(96, 144);
            this.comboBoxPloeg.Name = "comboBoxPloeg";
            this.comboBoxPloeg.Size = new System.Drawing.Size(171, 24);
            this.comboBoxPloeg.TabIndex = 2;
            this.comboBoxPloeg.SelectedIndexChanged += new System.EventHandler(this.ComboBoxPloeg_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Op Datum";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 366);
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
            this.comboBoxDienst.Location = new System.Drawing.Point(96, 363);
            this.comboBoxDienst.Name = "comboBoxDienst";
            this.comboBoxDienst.Size = new System.Drawing.Size(171, 24);
            this.comboBoxDienst.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 403);
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
            this.comboBoxWerkplek.Location = new System.Drawing.Point(96, 400);
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
            this.groupBox1.Size = new System.Drawing.Size(295, 630);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Extra/Ruil Dienst Gevraagd";
            // 
            // checkBoxRO
            // 
            this.checkBoxRO.AutoSize = true;
            this.checkBoxRO.Location = new System.Drawing.Point(96, 548);
            this.checkBoxRO.Name = "checkBoxRO";
            this.checkBoxRO.Size = new System.Drawing.Size(61, 20);
            this.checkBoxRO.TabIndex = 20;
            this.checkBoxRO.Text = "Rood";
            this.checkBoxRO.UseVisualStyleBackColor = true;
            // 
            // checkBoxGR
            // 
            this.checkBoxGR.AutoSize = true;
            this.checkBoxGR.Location = new System.Drawing.Point(96, 522);
            this.checkBoxGR.Name = "checkBoxGR";
            this.checkBoxGR.Size = new System.Drawing.Size(64, 20);
            this.checkBoxGR.TabIndex = 19;
            this.checkBoxGR.Text = "Groen";
            this.checkBoxGR.UseVisualStyleBackColor = true;
            // 
            // checkBoxGE
            // 
            this.checkBoxGE.AutoSize = true;
            this.checkBoxGE.Location = new System.Drawing.Point(96, 496);
            this.checkBoxGE.Name = "checkBoxGE";
            this.checkBoxGE.Size = new System.Drawing.Size(56, 20);
            this.checkBoxGE.TabIndex = 18;
            this.checkBoxGE.Text = "Geel";
            this.checkBoxGE.UseVisualStyleBackColor = true;
            // 
            // checkBoxWI
            // 
            this.checkBoxWI.AutoSize = true;
            this.checkBoxWI.Location = new System.Drawing.Point(96, 470);
            this.checkBoxWI.Name = "checkBoxWI";
            this.checkBoxWI.Size = new System.Drawing.Size(46, 20);
            this.checkBoxWI.TabIndex = 17;
            this.checkBoxWI.Text = "Wit";
            this.checkBoxWI.UseVisualStyleBackColor = true;
            // 
            // checkBoxBL
            // 
            this.checkBoxBL.AutoSize = true;
            this.checkBoxBL.Location = new System.Drawing.Point(96, 444);
            this.checkBoxBL.Name = "checkBoxBL";
            this.checkBoxBL.Size = new System.Drawing.Size(63, 20);
            this.checkBoxBL.TabIndex = 16;
            this.checkBoxBL.Text = "Blauw";
            this.checkBoxBL.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 446);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Door Kleur";
            // 
            // textBoxAanvraagVoor
            // 
            this.textBoxAanvraagVoor.Location = new System.Drawing.Point(128, 105);
            this.textBoxAanvraagVoor.Name = "textBoxAanvraagVoor";
            this.textBoxAanvraagVoor.Size = new System.Drawing.Size(139, 22);
            this.textBoxAanvraagVoor.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Aanvraag Voor :";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(177, 64);
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
            this.radioButton1.Location = new System.Drawing.Point(20, 64);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(97, 20);
            this.radioButton1.TabIndex = 11;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Extra Dienst";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // buttonVraagAan
            // 
            this.buttonVraagAan.Enabled = false;
            this.buttonVraagAan.Location = new System.Drawing.Point(96, 584);
            this.buttonVraagAan.Name = "buttonVraagAan";
            this.buttonVraagAan.Size = new System.Drawing.Size(171, 29);
            this.buttonVraagAan.TabIndex = 10;
            this.buttonVraagAan.Text = "Vraag Aan";
            this.buttonVraagAan.UseVisualStyleBackColor = true;
            this.buttonVraagAan.Click += new System.EventHandler(this.Button1_Click);
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
            this.listViewExtra.Size = new System.Drawing.Size(1061, 569);
            this.listViewExtra.TabIndex = 3;
            this.listViewExtra.UseCompatibleStateImageBehavior = false;
            this.listViewExtra.View = System.Windows.Forms.View.Details;
            this.listViewExtra.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewExtra_ColumnClick);
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
            this.columnHeader3.Width = 80;
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
            this.columnHeader6.Width = 150;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Werkplek";
            this.columnHeader7.Width = 80;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Door Ploeg";
            this.columnHeader8.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Loopt door";
            this.columnHeader9.Width = 400;
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
            this.buttonVulDienst.Location = new System.Drawing.Point(1223, 615);
            this.buttonVulDienst.Name = "buttonVulDienst";
            this.buttonVulDienst.Size = new System.Drawing.Size(171, 29);
            this.buttonVulDienst.TabIndex = 11;
            this.buttonVulDienst.Text = "Dienst invullen";
            this.buttonVulDienst.UseVisualStyleBackColor = true;
            this.buttonVulDienst.Click += new System.EventHandler(this.Button2_Click);
            // 
            // checkBoxVerbergOudeVragen
            // 
            this.checkBoxVerbergOudeVragen.AutoSize = true;
            this.checkBoxVerbergOudeVragen.Checked = true;
            this.checkBoxVerbergOudeVragen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVerbergOudeVragen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxVerbergOudeVragen.Location = new System.Drawing.Point(333, 608);
            this.checkBoxVerbergOudeVragen.Name = "checkBoxVerbergOudeVragen";
            this.checkBoxVerbergOudeVragen.Size = new System.Drawing.Size(213, 20);
            this.checkBoxVerbergOudeVragen.TabIndex = 12;
            this.checkBoxVerbergOudeVragen.Text = "Verberg aanvragen in verleden";
            this.checkBoxVerbergOudeVragen.UseVisualStyleBackColor = true;
            this.checkBoxVerbergOudeVragen.CheckedChanged += new System.EventHandler(this.checkBoxVerbergOudeVragen_CheckedChanged);
            // 
            // buttonAanvraagIntrekken
            // 
            this.buttonAanvraagIntrekken.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAanvraagIntrekken.Location = new System.Drawing.Point(1027, 615);
            this.buttonAanvraagIntrekken.Name = "buttonAanvraagIntrekken";
            this.buttonAanvraagIntrekken.Size = new System.Drawing.Size(171, 29);
            this.buttonAanvraagIntrekken.TabIndex = 11;
            this.buttonAanvraagIntrekken.Text = "Aanvraag Intrekken";
            this.buttonAanvraagIntrekken.UseVisualStyleBackColor = true;
            this.buttonAanvraagIntrekken.Click += new System.EventHandler(this.buttonAanvraagIntrekken_Click);
            // 
            // groupBoxFilter
            // 
            this.groupBoxFilter.Controls.Add(this.radioButton5);
            this.groupBoxFilter.Controls.Add(this.radioButton4);
            this.groupBoxFilter.Controls.Add(this.radioButton3);
            this.groupBoxFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFilter.Location = new System.Drawing.Point(756, 600);
            this.groupBoxFilter.Name = "groupBoxFilter";
            this.groupBoxFilter.Size = new System.Drawing.Size(250, 54);
            this.groupBoxFilter.TabIndex = 13;
            this.groupBoxFilter.TabStop = false;
            this.groupBoxFilter.Text = " Filter Soort Diensten ";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton5.Location = new System.Drawing.Point(185, 28);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(56, 20);
            this.radioButton5.TabIndex = 0;
            this.radioButton5.Text = "Extra";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton4.Location = new System.Drawing.Point(130, 28);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(49, 20);
            this.radioButton4.TabIndex = 0;
            this.radioButton4.Text = "Ruil";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton3.Location = new System.Drawing.Point(6, 28);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(101, 20);
            this.radioButton3.TabIndex = 0;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Ruil En Extra";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // checkBoxVerbergIngevulde
            // 
            this.checkBoxVerbergIngevulde.AutoSize = true;
            this.checkBoxVerbergIngevulde.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxVerbergIngevulde.Location = new System.Drawing.Point(333, 634);
            this.checkBoxVerbergIngevulde.Name = "checkBoxVerbergIngevulde";
            this.checkBoxVerbergIngevulde.Size = new System.Drawing.Size(138, 20);
            this.checkBoxVerbergIngevulde.TabIndex = 14;
            this.checkBoxVerbergIngevulde.Text = "Verberg ingevulde";
            this.checkBoxVerbergIngevulde.UseVisualStyleBackColor = true;
            this.checkBoxVerbergIngevulde.CheckedChanged += new System.EventHandler(this.checkBoxVerbergIngevulde_CheckedChanged);
            // 
            // checkBoxVerbergCancel
            // 
            this.checkBoxVerbergCancel.AutoSize = true;
            this.checkBoxVerbergCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxVerbergCancel.Location = new System.Drawing.Point(477, 634);
            this.checkBoxVerbergCancel.Name = "checkBoxVerbergCancel";
            this.checkBoxVerbergCancel.Size = new System.Drawing.Size(174, 20);
            this.checkBoxVerbergCancel.TabIndex = 15;
            this.checkBoxVerbergCancel.Text = "Verberg Zelf Ingetrokken";
            this.checkBoxVerbergCancel.UseVisualStyleBackColor = true;
            this.checkBoxVerbergCancel.CheckedChanged += new System.EventHandler(this.checkBoxVerbergCancel_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(704, 424);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(344, 129);
            this.button1.TabIndex = 16;
            this.button1.Text = "test invoer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // RuilExtraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1421, 666);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxVerbergCancel);
            this.Controls.Add(this.checkBoxVerbergIngevulde);
            this.Controls.Add(this.groupBoxFilter);
            this.Controls.Add(this.checkBoxVerbergOudeVragen);
            this.Controls.Add(this.buttonAanvraagIntrekken);
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
            this.groupBoxFilter.ResumeLayout(false);
            this.groupBoxFilter.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBoxVerbergOudeVragen;
        private System.Windows.Forms.Button buttonAanvraagIntrekken;
        private System.Windows.Forms.GroupBox groupBoxFilter;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.CheckBox checkBoxVerbergIngevulde;
        private System.Windows.Forms.CheckBox checkBoxVerbergCancel;
        private System.Windows.Forms.Button button1;
    }
}