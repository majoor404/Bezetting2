namespace Bezetting2
{
    partial class HistoryForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.listViewHis = new System.Windows.Forms.ListView();
            this.Naam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Afwijking = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Op = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IngevoerdDoor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OpDatum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Rede = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxFilter = new System.Windows.Forms.CheckBox();
            this.comboBoxDag = new System.Windows.Forms.ComboBox();
            this.comboBoxIngevoerdDoor = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(664, 544);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listViewHis
            // 
            this.listViewHis.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Naam,
            this.Afwijking,
            this.Op,
            this.IngevoerdDoor,
            this.OpDatum,
            this.Rede});
            this.listViewHis.FullRowSelect = true;
            this.listViewHis.HideSelection = false;
            this.listViewHis.Location = new System.Drawing.Point(13, 13);
            this.listViewHis.Name = "listViewHis";
            this.listViewHis.Size = new System.Drawing.Size(782, 515);
            this.listViewHis.TabIndex = 1;
            this.listViewHis.UseCompatibleStateImageBehavior = false;
            this.listViewHis.View = System.Windows.Forms.View.Details;
            // 
            // Naam
            // 
            this.Naam.Text = "Naam";
            this.Naam.Width = 100;
            // 
            // Afwijking
            // 
            this.Afwijking.Text = "Afwijking tov Rooster";
            this.Afwijking.Width = 156;
            // 
            // Op
            // 
            this.Op.Text = "Dag";
            this.Op.Width = 83;
            // 
            // IngevoerdDoor
            // 
            this.IngevoerdDoor.Text = "Ingevoerd Door";
            this.IngevoerdDoor.Width = 138;
            // 
            // OpDatum
            // 
            this.OpDatum.Text = "Op Datum";
            this.OpDatum.Width = 83;
            // 
            // Rede
            // 
            this.Rede.Text = "Rede";
            this.Rede.Width = 213;
            // 
            // checkBoxFilter
            // 
            this.checkBoxFilter.AutoSize = true;
            this.checkBoxFilter.Location = new System.Drawing.Point(15, 546);
            this.checkBoxFilter.Name = "checkBoxFilter";
            this.checkBoxFilter.Size = new System.Drawing.Size(48, 17);
            this.checkBoxFilter.TabIndex = 2;
            this.checkBoxFilter.Text = "Filter";
            this.checkBoxFilter.UseVisualStyleBackColor = true;
            this.checkBoxFilter.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // comboBoxDag
            // 
            this.comboBoxDag.FormattingEnabled = true;
            this.comboBoxDag.Location = new System.Drawing.Point(256, 542);
            this.comboBoxDag.Name = "comboBoxDag";
            this.comboBoxDag.Size = new System.Drawing.Size(78, 21);
            this.comboBoxDag.TabIndex = 3;
            // 
            // comboBoxIngevoerdDoor
            // 
            this.comboBoxIngevoerdDoor.FormattingEnabled = true;
            this.comboBoxIngevoerdDoor.Location = new System.Drawing.Point(351, 542);
            this.comboBoxIngevoerdDoor.Name = "comboBoxIngevoerdDoor";
            this.comboBoxIngevoerdDoor.Size = new System.Drawing.Size(121, 21);
            this.comboBoxIngevoerdDoor.TabIndex = 4;
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 581);
            this.Controls.Add(this.comboBoxIngevoerdDoor);
            this.Controls.Add(this.comboBoxDag);
            this.Controls.Add(this.checkBoxFilter);
            this.Controls.Add(this.listViewHis);
            this.Controls.Add(this.button1);
            this.Name = "HistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "History";
            this.Shown += new System.EventHandler(this.History_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listViewHis;
        private System.Windows.Forms.ColumnHeader Naam;
        private System.Windows.Forms.ColumnHeader Afwijking;
        private System.Windows.Forms.ColumnHeader Op;
        private System.Windows.Forms.ColumnHeader IngevoerdDoor;
        private System.Windows.Forms.ColumnHeader OpDatum;
        private System.Windows.Forms.ColumnHeader Rede;
        private System.Windows.Forms.ComboBox comboBoxIngevoerdDoor;
        public System.Windows.Forms.CheckBox checkBoxFilter;
        public System.Windows.Forms.ComboBox comboBoxDag;
    }
}