﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class InlogForm : Form
    {
        public InlogForm()
        {
            InitializeComponent();
        }

        private void InlogForm_Shown(object sender, EventArgs e)
        {
            buttonVerander.Enabled = false;
            ProgData.AlleMensen.Load();
            textBoxNum.Text = Environment.UserName;

            // maak van a590588 -> 590588

            if (textBoxNum.Text.Length == 7 && textBoxNum.Text[0] == 'a')
                textBoxNum.Text = textBoxNum.Text.Substring(1);

            if (textBoxNum.Text.Length == 7 && textBoxNum.Text[0] == 'A')
                textBoxNum.Text = textBoxNum.Text.Substring(1);

            textBoxPass.Text = "";
            textBoxChangePasswoord.Text = "";

            // auto inlog alleen als Environment.UserName 6 char met alleen nummers
            if (textBoxNum.Text.Length == 6 && int.TryParse(textBoxNum.Text, out _))
                groupBoxAutoInlog.Visible = true;

            // voor debug op deze pc even aanzetten ;-)
            if (Environment.UserName == "ronal")
                groupBoxAutoInlog.Visible = true;

            // auto inlog
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var autoinlogfile = $"{directory}\\bezetting2.log";
            if (File.Exists(autoinlogfile))
            {
                checkBoxAutoInlog.Checked = true;
                //_ = new List<string>();
                try
                {
                    List<string> inlognaam = File.ReadAllLines(autoinlogfile).ToList();
                    labelAutoInlogNaam.Text = inlognaam[0];
                }
                catch { }
            }
            else
            {
                labelAutoInlogNaam.Text = "";
            }
            textBoxPass.Focus();
        }

        private void ButtonOke_Click(object sender, EventArgs e)
        {
            //check passwoord
            try
            {
                if (textBoxNum.Text == "Admin" && textBoxPass.Text == DateTime.Now.ToString("ddMM"))
                {
                    ProgData.Huidige_Gebruiker_Personeel_nummer = "Admin";
                    ProgData.RechtenHuidigeGebruiker = 101;
                }
                else if (textBoxNum.Text == "000000" && textBoxPass.Text == DateTime.Now.ToString("ddMM"))
                {
                    ProgData.Huidige_Gebruiker_Personeel_nummer = "000000";
                    ProgData.RechtenHuidigeGebruiker = 100;
                }
                else
                {
                    bool juist = false;
                    personeel persoon = ProgData.AlleMensen.LijstPersonen.First(b => b._persnummer.ToString() == textBoxNum.Text);
                    if (textBoxNum.Text == textBoxPass.Text)
                    {
                        {
                            // juiste inlog
                            ProgData.Huidige_Gebruiker_Personeel_nummer = persoon._persnummer.ToString();
                            ProgData.RechtenHuidigeGebruiker = 1; // ingelogd maar kan verder niks

                            string kleur = ProgData.Get_Gebruiker_Kleur(persoon._persnummer.ToString());
                            ProgData.GekozenKleur = kleur;
                            //ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = kleur;

                            //ProgData.GekozenKleur = persoon._kleur;
                            //ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = persoon._kleur;
                            
                            juist = true;
                            MessageBox.Show("Ingelogd met passwoord wat uw personeel nummer is." +
                                "\nRechten dus 1, alleen lezen en aanvragen snipper/ruildiensten!");
                        }
                    }
                    if (!string.IsNullOrEmpty(textBoxPass.Text) && ProgData.Unscramble(persoon._passwoord) == textBoxPass.Text)
                    {
                        {
                            // juiste inlog
                            ProgData.Huidige_Gebruiker_Personeel_nummer = persoon._persnummer.ToString();
                            ProgData.RechtenHuidigeGebruiker = persoon._rechten;

                            string kleur = ProgData.Get_Gebruiker_Kleur(persoon._persnummer.ToString());
                            ProgData.GekozenKleur = kleur;
                            //ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = kleur;

                            //ProgData.GekozenKleur = persoon._kleur;
                            //ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = persoon._kleur;

                            juist = true;

                            if (textBoxNum.Text == textBoxPass.Text)
                                checkBoxAutoInlog.Checked = false;

                            if ((textBoxNum.Text != labelAutoInlogNaam.Text) && checkBoxAutoInlog.Checked
                                && (!string.IsNullOrEmpty(labelAutoInlogNaam.Text)))
                            {
                                checkBoxAutoInlog.Checked = false;
                                MessageBox.Show("Auto inlog is al op andere naam, zet deze onder die naam uit!");
                            }
                            else
                            {

                                // auto inlog
                                var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                                var autoinlogfile = $"{directory}\\bezetting2.log";

                                if (checkBoxAutoInlog.Checked)
                                {
                                    // maak document
                                    List<string> inlognaam = new List<string>
                                    {
                                        ProgData.Huidige_Gebruiker_Personeel_nummer,
                                        ProgData.RechtenHuidigeGebruiker.ToString()
                                    };
                                    try
                                    {
                                        File.WriteAllLines(autoinlogfile, inlognaam);
                                    }
                                    catch (IOException)
                                    {
                                        MessageBox.Show("autoinlog file save Error()");
                                    }
                                }
                                else
                                {
                                    if (File.Exists(autoinlogfile))
                                    {
                                        File.Delete(autoinlogfile);
                                    }
                                }
                            }
                        }
                    }
                    if (!juist)
                    {
                        MessageBox.Show("Wachtwoord fout, niet herkend!");
                    }

                }
            }
            catch
            {
                MessageBox.Show("Gebruiker niet in bezetting lijst!");
            }
        }

        private void TextBoxPass_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNum.Text == textBoxPass.Text)
                checkBoxAutoInlog.Checked = false;

            buttonVerander.Enabled = false;
            //check passwoord
            try
            {
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(b => b._persnummer.ToString(CultureInfo.CurrentCulture) == textBoxNum.Text);
                if (ProgData.Unscramble(persoon._passwoord) == textBoxPass.Text)
                {
                    // juiste inlog
                    ProgData.Huidige_Gebruiker_Personeel_nummer = persoon._persnummer.ToString(CultureInfo.CurrentCulture);
                    ProgData.RechtenHuidigeGebruiker = persoon._rechten;
                    buttonVerander.Enabled = true;
                }
            }
            catch { }
        }

        private void ButtonVerander_Click(object sender, EventArgs e)
        {
            personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == textBoxNum.Text);
            // encrypt pass
            if (textBoxChangePasswoord.Text == textBoxNum.Text)
            {
                MessageBox.Show("personeel nummer mag niet passwoord zijn");
            }
            else
            {
                persoon._passwoord = ProgData.Scramble(textBoxChangePasswoord.Text);
                //ProgData.Save_LijstNamen();
                ProgData.AlleMensen.Save();
                MessageBox.Show("Wachtwoord is aangepast, log nu nogmaals in met dit wachtwoord.");

                textBoxNum.Enabled = true;
                textBoxPass.Enabled = true;
                buttonOke.Enabled = true;
            }
        }

        private void TextBoxNum_TextChanged(object sender, EventArgs e)
        {
            if (textBoxNum.Text == "Admin" || textBoxNum.Text == "000000")
            {
                checkBoxAutoInlog.Checked = false;
            }

            // na reset passwoord is het "verander_nu", pas meteen aan.
            try
            {
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(b => b._persnummer.ToString() == textBoxNum.Text);
                if (ProgData.Unscramble(persoon._passwoord) == "verander_nu")
                {
                    MessageBox.Show("Geef nieuwe passwoord op, verboden is uw gebruikers naam/personeel nummer!");
                    textBoxNum.Enabled = false;
                    textBoxPass.Enabled = false;
                    buttonOke.Enabled = false;
                    buttonVerander.Enabled = true;
                }
            }
            catch { }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Log auto in volgende keer met deze gegevens " +
                $"\nAls ingelogd op pc met windows gebruikers naam : {Environment.UserName}");
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBoxNum.Text == textBoxPass.Text)
                checkBoxAutoInlog.Checked = false;
        }


    }
}