﻿using System;
using System.Collections.Generic;
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
            ProgData.Lees_Namen_lijst();
            textBoxNum.Text = Environment.UserName;

            // maak van a590588 -> 590588

            if (textBoxNum.Text.Length == 7 && textBoxNum.Text[0] == 'a')
                textBoxNum.Text = textBoxNum.Text.Substring(1);

            textBoxPass.Text = "";
            textBoxChangePasswoord.Text = "";

            textBoxPass.Focus();

        }

        private void buttonOke_Click(object sender, EventArgs e)
        {
            //check passwoord
            try
            {
                if (textBoxNum.Text == "Admin" && textBoxPass.Text == "konijn")
                {
                    ProgData.Huidige_Gebruiker_Personeel_nummer = "Admin";
                    ProgData.RechtenHuidigeGebruiker = 101;
                }
                else
                {
                    personeel persoon = ProgData.personeel_lijst.First(b => b._persnummer.ToString() == textBoxNum.Text);
                    if (ProgData.Unscramble(persoon._passwoord) == textBoxPass.Text)
                    {
                        {
                            // juiste inlog
                            ProgData.Huidige_Gebruiker_Personeel_nummer = persoon._persnummer.ToString();
                            ProgData.RechtenHuidigeGebruiker = persoon._rechten;
                            ProgData.GekozenKleur = persoon._kleur;
                            ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = persoon._kleur;
                        }

                    }
                    if ( textBoxNum.Text == textBoxPass.Text)
                    {
                        {
                            // juiste inlog
                            ProgData.Huidige_Gebruiker_Personeel_nummer = persoon._persnummer.ToString();
                            ProgData.RechtenHuidigeGebruiker = 1; // ingelogd maar kan verder niks
                            ProgData.GekozenKleur = persoon._kleur;
                            ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = persoon._kleur;
                        }
                    }
                }
            }
            catch { }
        }

        private void textBoxPass_TextChanged(object sender, EventArgs e)
        {
            buttonVerander.Enabled = false;
            //check passwoord
            try
            {
                personeel persoon = ProgData.personeel_lijst.First(b => b._persnummer.ToString() == textBoxNum.Text);
                if (ProgData.Unscramble(persoon._passwoord) == textBoxPass.Text)
                {
                    // juiste inlog
                    ProgData.Huidige_Gebruiker_Personeel_nummer = persoon._persnummer.ToString();
                    ProgData.RechtenHuidigeGebruiker = persoon._rechten;
                    buttonVerander.Enabled = true;
                }
            }
            catch { }

        }
        private void buttonVerander_Click(object sender, EventArgs e)
        {
            personeel persoon = ProgData.personeel_lijst.First(a => a._persnummer.ToString() == textBoxNum.Text);
            // encrypt pass
            persoon._passwoord = ProgData.Scramble(textBoxChangePasswoord.Text);
            ProgData.Save_Namen_lijst();
            MessageBox.Show("Wachtwoord is aangepast");
        }
    }
}
