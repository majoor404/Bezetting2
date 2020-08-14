﻿namespace Bezetting2
{
    partial class EditPersoneel
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
            this.comboBoxFilter = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPersNum = new System.Windows.Forms.TextBox();
            this.ViewNamen = new System.Windows.Forms.ListView();
            this.PersoneelNummer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Naam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Voornaam = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Kleur = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Rechten = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxAchterNaam = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxVoorNaam = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAdres = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxPostcode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxWoonplaats = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxEmailWerk = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxEmailThuis = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxWerkplek = new System.Windows.Forms.TextBox();
            this.labelWerkplek = new System.Windows.Forms.Label();
            this.textBoxFuntie = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxTelWerk = new System.Windows.Forms.TextBox();
            this.labelTelWerk = new System.Windows.Forms.Label();
            this.textBoxAdresCodeWerk = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxTelMobWerk = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxTelMobPrive = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxTelThuis = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxKleur = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonVoegToe = new System.Windows.Forms.Button();
            this.LabelRoosterNieuw = new System.Windows.Forms.Label();
            this.groupBoxNieuwRooster = new System.Windows.Forms.GroupBox();
            this.labelNieuwRoosterDatum = new System.Windows.Forms.Label();
            this.buttonCancelVerhuis = new System.Windows.Forms.Button();
            this.buttonRechten = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonNieuw = new System.Windows.Forms.Button();
            this.groupBoxNieuwRooster.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(691, 317);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Filter";
            // 
            // comboBoxFilter
            // 
            this.comboBoxFilter.FormattingEnabled = true;
            this.comboBoxFilter.Location = new System.Drawing.Point(742, 313);
            this.comboBoxFilter.Name = "comboBoxFilter";
            this.comboBoxFilter.Size = new System.Drawing.Size(208, 21);
            this.comboBoxFilter.TabIndex = 2;
            this.comboBoxFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Personeel Nummer";
            // 
            // textBoxPersNum
            // 
            this.textBoxPersNum.Location = new System.Drawing.Point(123, 314);
            this.textBoxPersNum.Name = "textBoxPersNum";
            this.textBoxPersNum.Size = new System.Drawing.Size(201, 20);
            this.textBoxPersNum.TabIndex = 4;
            // 
            // ViewNamen
            // 
            this.ViewNamen.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.PersoneelNummer,
            this.Naam,
            this.Voornaam,
            this.Kleur,
            this.Rechten});
            this.ViewNamen.FullRowSelect = true;
            this.ViewNamen.HideSelection = false;
            this.ViewNamen.Location = new System.Drawing.Point(11, 11);
            this.ViewNamen.Name = "ViewNamen";
            this.ViewNamen.Size = new System.Drawing.Size(939, 290);
            this.ViewNamen.TabIndex = 5;
            this.ViewNamen.UseCompatibleStateImageBehavior = false;
            this.ViewNamen.View = System.Windows.Forms.View.Details;
            this.ViewNamen.SelectedIndexChanged += new System.EventHandler(this.ViewNamen_SelectedIndexChanged);
            // 
            // PersoneelNummer
            // 
            this.PersoneelNummer.Text = "Personeel Num.";
            this.PersoneelNummer.Width = 176;
            // 
            // Naam
            // 
            this.Naam.Text = "Naam";
            this.Naam.Width = 257;
            // 
            // Voornaam
            // 
            this.Voornaam.Text = "Voornaam";
            this.Voornaam.Width = 245;
            // 
            // Kleur
            // 
            this.Kleur.Text = "Kleur";
            this.Kleur.Width = 157;
            // 
            // Rechten
            // 
            this.Rechten.Text = "Rechten";
            // 
            // textBoxAchterNaam
            // 
            this.textBoxAchterNaam.Location = new System.Drawing.Point(123, 346);
            this.textBoxAchterNaam.Name = "textBoxAchterNaam";
            this.textBoxAchterNaam.Size = new System.Drawing.Size(201, 20);
            this.textBoxAchterNaam.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 349);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Achter Naam";
            // 
            // textBoxVoorNaam
            // 
            this.textBoxVoorNaam.Location = new System.Drawing.Point(123, 378);
            this.textBoxVoorNaam.Name = "textBoxVoorNaam";
            this.textBoxVoorNaam.Size = new System.Drawing.Size(201, 20);
            this.textBoxVoorNaam.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 381);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Voornaam";
            // 
            // textBoxAdres
            // 
            this.textBoxAdres.Location = new System.Drawing.Point(123, 410);
            this.textBoxAdres.Name = "textBoxAdres";
            this.textBoxAdres.Size = new System.Drawing.Size(201, 20);
            this.textBoxAdres.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 413);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Adres";
            // 
            // textBoxPostcode
            // 
            this.textBoxPostcode.Location = new System.Drawing.Point(123, 442);
            this.textBoxPostcode.Name = "textBoxPostcode";
            this.textBoxPostcode.Size = new System.Drawing.Size(201, 20);
            this.textBoxPostcode.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 445);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Postcode";
            // 
            // textBoxWoonplaats
            // 
            this.textBoxWoonplaats.Location = new System.Drawing.Point(123, 474);
            this.textBoxWoonplaats.Name = "textBoxWoonplaats";
            this.textBoxWoonplaats.Size = new System.Drawing.Size(201, 20);
            this.textBoxWoonplaats.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 477);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Woonplaats";
            // 
            // textBoxEmailWerk
            // 
            this.textBoxEmailWerk.Location = new System.Drawing.Point(123, 506);
            this.textBoxEmailWerk.Name = "textBoxEmailWerk";
            this.textBoxEmailWerk.Size = new System.Drawing.Size(201, 20);
            this.textBoxEmailWerk.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 509);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "E-Mail Werk";
            // 
            // textBoxEmailThuis
            // 
            this.textBoxEmailThuis.Location = new System.Drawing.Point(123, 538);
            this.textBoxEmailThuis.Name = "textBoxEmailThuis";
            this.textBoxEmailThuis.Size = new System.Drawing.Size(201, 20);
            this.textBoxEmailThuis.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 541);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "E-Mail Thuis";
            // 
            // textBoxWerkplek
            // 
            this.textBoxWerkplek.Location = new System.Drawing.Point(456, 538);
            this.textBoxWerkplek.Name = "textBoxWerkplek";
            this.textBoxWerkplek.Size = new System.Drawing.Size(201, 20);
            this.textBoxWerkplek.TabIndex = 35;
            // 
            // labelWerkplek
            // 
            this.labelWerkplek.AutoSize = true;
            this.labelWerkplek.Location = new System.Drawing.Point(345, 541);
            this.labelWerkplek.Name = "labelWerkplek";
            this.labelWerkplek.Size = new System.Drawing.Size(53, 13);
            this.labelWerkplek.TabIndex = 34;
            this.labelWerkplek.Text = "Werkplek";
            // 
            // textBoxFuntie
            // 
            this.textBoxFuntie.Location = new System.Drawing.Point(456, 506);
            this.textBoxFuntie.Name = "textBoxFuntie";
            this.textBoxFuntie.Size = new System.Drawing.Size(201, 20);
            this.textBoxFuntie.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(345, 509);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Funtie";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(345, 477);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 30;
            this.label12.Text = "Rooster";
            // 
            // textBoxTelWerk
            // 
            this.textBoxTelWerk.Location = new System.Drawing.Point(456, 442);
            this.textBoxTelWerk.Name = "textBoxTelWerk";
            this.textBoxTelWerk.Size = new System.Drawing.Size(201, 20);
            this.textBoxTelWerk.TabIndex = 29;
            // 
            // labelTelWerk
            // 
            this.labelTelWerk.AutoSize = true;
            this.labelTelWerk.Location = new System.Drawing.Point(345, 445);
            this.labelTelWerk.Name = "labelTelWerk";
            this.labelTelWerk.Size = new System.Drawing.Size(51, 13);
            this.labelTelWerk.TabIndex = 28;
            this.labelTelWerk.Text = "Tel Werk";
            // 
            // textBoxAdresCodeWerk
            // 
            this.textBoxAdresCodeWerk.Location = new System.Drawing.Point(456, 410);
            this.textBoxAdresCodeWerk.Name = "textBoxAdresCodeWerk";
            this.textBoxAdresCodeWerk.Size = new System.Drawing.Size(201, 20);
            this.textBoxAdresCodeWerk.TabIndex = 27;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(345, 413);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(91, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "Adres Code Werk";
            // 
            // textBoxTelMobWerk
            // 
            this.textBoxTelMobWerk.Location = new System.Drawing.Point(456, 378);
            this.textBoxTelMobWerk.Name = "textBoxTelMobWerk";
            this.textBoxTelMobWerk.Size = new System.Drawing.Size(201, 20);
            this.textBoxTelMobWerk.TabIndex = 25;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(345, 381);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(75, 13);
            this.label15.TabIndex = 24;
            this.label15.Text = "Tel Mob Werk";
            // 
            // textBoxTelMobPrive
            // 
            this.textBoxTelMobPrive.Location = new System.Drawing.Point(456, 346);
            this.textBoxTelMobPrive.Name = "textBoxTelMobPrive";
            this.textBoxTelMobPrive.Size = new System.Drawing.Size(201, 20);
            this.textBoxTelMobPrive.TabIndex = 23;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(345, 349);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(73, 13);
            this.label16.TabIndex = 22;
            this.label16.Text = "Tel Mob Prive";
            // 
            // textBoxTelThuis
            // 
            this.textBoxTelThuis.Location = new System.Drawing.Point(456, 314);
            this.textBoxTelThuis.Name = "textBoxTelThuis";
            this.textBoxTelThuis.Size = new System.Drawing.Size(201, 20);
            this.textBoxTelThuis.TabIndex = 21;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(345, 317);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(51, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Tel Thuis";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(694, 342);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(120, 26);
            this.buttonSave.TabIndex = 36;
            this.buttonSave.Text = "Save Persoon";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(693, 534);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(261, 26);
            this.buttonClose.TabIndex = 38;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxKleur
            // 
            this.textBoxKleur.Location = new System.Drawing.Point(456, 473);
            this.textBoxKleur.Name = "textBoxKleur";
            this.textBoxKleur.ReadOnly = true;
            this.textBoxKleur.Size = new System.Drawing.Size(201, 20);
            this.textBoxKleur.TabIndex = 39;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(694, 449);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 26);
            this.button1.TabIndex = 40;
            this.button1.Text = "Verhuis van Rooster";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonVoegToe
            // 
            this.buttonVoegToe.Location = new System.Drawing.Point(830, 412);
            this.buttonVoegToe.Name = "buttonVoegToe";
            this.buttonVoegToe.Size = new System.Drawing.Size(120, 26);
            this.buttonVoegToe.TabIndex = 41;
            this.buttonVoegToe.Text = "Voeg Toe Nieuw Lid";
            this.buttonVoegToe.UseVisualStyleBackColor = true;
            this.buttonVoegToe.Click += new System.EventHandler(this.buttonVoegToe_Click);
            // 
            // LabelRoosterNieuw
            // 
            this.LabelRoosterNieuw.AutoSize = true;
            this.LabelRoosterNieuw.Location = new System.Drawing.Point(6, 16);
            this.LabelRoosterNieuw.Name = "LabelRoosterNieuw";
            this.LabelRoosterNieuw.Size = new System.Drawing.Size(44, 13);
            this.LabelRoosterNieuw.TabIndex = 43;
            this.LabelRoosterNieuw.Text = "Rooster";
            // 
            // groupBoxNieuwRooster
            // 
            this.groupBoxNieuwRooster.Controls.Add(this.labelNieuwRoosterDatum);
            this.groupBoxNieuwRooster.Controls.Add(this.LabelRoosterNieuw);
            this.groupBoxNieuwRooster.Location = new System.Drawing.Point(693, 484);
            this.groupBoxNieuwRooster.Name = "groupBoxNieuwRooster";
            this.groupBoxNieuwRooster.Size = new System.Drawing.Size(261, 41);
            this.groupBoxNieuwRooster.TabIndex = 44;
            this.groupBoxNieuwRooster.TabStop = false;
            this.groupBoxNieuwRooster.Text = "Nieuw Rooster";
            // 
            // labelNieuwRoosterDatum
            // 
            this.labelNieuwRoosterDatum.AutoSize = true;
            this.labelNieuwRoosterDatum.Location = new System.Drawing.Point(116, 16);
            this.labelNieuwRoosterDatum.Name = "labelNieuwRoosterDatum";
            this.labelNieuwRoosterDatum.Size = new System.Drawing.Size(55, 13);
            this.labelNieuwRoosterDatum.TabIndex = 44;
            this.labelNieuwRoosterDatum.Text = "25-4-1965";
            // 
            // buttonCancelVerhuis
            // 
            this.buttonCancelVerhuis.Location = new System.Drawing.Point(830, 448);
            this.buttonCancelVerhuis.Name = "buttonCancelVerhuis";
            this.buttonCancelVerhuis.Size = new System.Drawing.Size(120, 26);
            this.buttonCancelVerhuis.TabIndex = 45;
            this.buttonCancelVerhuis.Text = "Cancel Verhuis";
            this.buttonCancelVerhuis.UseVisualStyleBackColor = true;
            this.buttonCancelVerhuis.Click += new System.EventHandler(this.buttonCancelVerhuis_Click);
            // 
            // buttonRechten
            // 
            this.buttonRechten.Location = new System.Drawing.Point(694, 378);
            this.buttonRechten.Name = "buttonRechten";
            this.buttonRechten.Size = new System.Drawing.Size(256, 26);
            this.buttonRechten.TabIndex = 37;
            this.buttonRechten.Text = "Rechten van persoon";
            this.buttonRechten.UseVisualStyleBackColor = true;
            this.buttonRechten.Click += new System.EventHandler(this.buttonRechten_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(830, 342);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(120, 26);
            this.buttonDelete.TabIndex = 46;
            this.buttonDelete.Text = "Delete Persoon";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonNieuw
            // 
            this.buttonNieuw.Location = new System.Drawing.Point(694, 413);
            this.buttonNieuw.Name = "buttonNieuw";
            this.buttonNieuw.Size = new System.Drawing.Size(120, 26);
            this.buttonNieuw.TabIndex = 47;
            this.buttonNieuw.Text = "Nieuw Persoon";
            this.buttonNieuw.UseVisualStyleBackColor = true;
            this.buttonNieuw.Click += new System.EventHandler(this.buttonNieuw_Click);
            // 
            // EditPersoneel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 576);
            this.Controls.Add(this.buttonNieuw);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCancelVerhuis);
            this.Controls.Add(this.groupBoxNieuwRooster);
            this.Controls.Add(this.buttonVoegToe);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxKleur);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRechten);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxWerkplek);
            this.Controls.Add(this.labelWerkplek);
            this.Controls.Add(this.textBoxFuntie);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxTelWerk);
            this.Controls.Add(this.labelTelWerk);
            this.Controls.Add(this.textBoxAdresCodeWerk);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBoxTelMobWerk);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBoxTelMobPrive);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.textBoxTelThuis);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.textBoxEmailThuis);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxEmailWerk);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxWoonplaats);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPostcode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxAdres);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxVoorNaam);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxAchterNaam);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ViewNamen);
            this.Controls.Add(this.textBoxPersNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxFilter);
            this.Controls.Add(this.label1);
            this.Name = "EditPersoneel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EditPersoneel";
            this.Shown += new System.EventHandler(this.EditPersoneel_Shown);
            this.groupBoxNieuwRooster.ResumeLayout(false);
            this.groupBoxNieuwRooster.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPersNum;
        public System.Windows.Forms.ListView ViewNamen;
        private System.Windows.Forms.ColumnHeader PersoneelNummer;
        private System.Windows.Forms.ColumnHeader Naam;
        private System.Windows.Forms.ColumnHeader Voornaam;
        private System.Windows.Forms.ColumnHeader Kleur;
        private System.Windows.Forms.TextBox textBoxAchterNaam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxVoorNaam;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAdres;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxPostcode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxWoonplaats;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxEmailWerk;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxEmailThuis;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxWerkplek;
        private System.Windows.Forms.Label labelWerkplek;
        private System.Windows.Forms.TextBox textBoxFuntie;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxTelWerk;
        private System.Windows.Forms.Label labelTelWerk;
        private System.Windows.Forms.TextBox textBoxAdresCodeWerk;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxTelMobWerk;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxTelMobPrive;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxTelThuis;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxKleur;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonVoegToe;
        public System.Windows.Forms.Label LabelRoosterNieuw;
        private System.Windows.Forms.GroupBox groupBoxNieuwRooster;
        public System.Windows.Forms.Label labelNieuwRoosterDatum;
        private System.Windows.Forms.Button buttonCancelVerhuis;
        private System.Windows.Forms.Button buttonRechten;
        private System.Windows.Forms.ColumnHeader Rechten;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonNieuw;
    }
}