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
            this.listViewHis = new System.Windows.Forms.ListView();
            this.Naam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Afwijking = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Op = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IngevoerdDoor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OpDatum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Rede = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBoxDag = new System.Windows.Forms.ComboBox();
            this.comboBoxIngevoerdDoor = new System.Windows.Forms.ComboBox();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.listViewHis.Location = new System.Drawing.Point(13, 41);
            this.listViewHis.Name = "listViewHis";
            this.listViewHis.Size = new System.Drawing.Size(782, 487);
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
            // comboBoxDag
            // 
            this.comboBoxDag.FormattingEnabled = true;
            this.comboBoxDag.Location = new System.Drawing.Point(270, 14);
            this.comboBoxDag.Name = "comboBoxDag";
            this.comboBoxDag.Size = new System.Drawing.Size(78, 21);
            this.comboBoxDag.TabIndex = 3;
            // 
            // comboBoxIngevoerdDoor
            // 
            this.comboBoxIngevoerdDoor.FormattingEnabled = true;
            this.comboBoxIngevoerdDoor.Location = new System.Drawing.Point(354, 14);
            this.comboBoxIngevoerdDoor.Name = "comboBoxIngevoerdDoor";
            this.comboBoxIngevoerdDoor.Size = new System.Drawing.Size(143, 21);
            this.comboBoxIngevoerdDoor.TabIndex = 4;
            // 
            // buttonFilter
            // 
            this.buttonFilter.Location = new System.Drawing.Point(503, 12);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(103, 23);
            this.buttonFilter.TabIndex = 6;
            this.buttonFilter.Text = "Zet Filter";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // HistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 539);
            this.Controls.Add(this.buttonFilter);
            this.Controls.Add(this.comboBoxIngevoerdDoor);
            this.Controls.Add(this.comboBoxDag);
            this.Controls.Add(this.listViewHis);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "History";
            this.Shown += new System.EventHandler(this.History_Shown);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView listViewHis;
        private System.Windows.Forms.ColumnHeader Naam;
        private System.Windows.Forms.ColumnHeader Afwijking;
        private System.Windows.Forms.ColumnHeader Op;
        private System.Windows.Forms.ColumnHeader IngevoerdDoor;
        private System.Windows.Forms.ColumnHeader OpDatum;
        private System.Windows.Forms.ColumnHeader Rede;
        private System.Windows.Forms.ComboBox comboBoxIngevoerdDoor;
        public System.Windows.Forms.ComboBox comboBoxDag;
        private System.Windows.Forms.Button buttonFilter;
    }
}