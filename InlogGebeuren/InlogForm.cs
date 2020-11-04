using System;
using System.Linq;
using System.Text;
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

            if (textBoxNum.Text.Length == 7 && textBoxNum.Text[0] == 'A')
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
                else if (textBoxNum.Text == "590588" && textBoxPass.Text == "kompas59")
                {
                    ProgData.Huidige_Gebruiker_Personeel_nummer = "590588";
                    ProgData.RechtenHuidigeGebruiker = 100;
                }
                else
                {
                    bool juist = false;
                    personeel persoon = ProgData.ListPersoneel.First(b => b._persnummer.ToString() == textBoxNum.Text);
                    if (textBoxNum.Text == textBoxPass.Text)
                    {
                        {
                            // juiste inlog
                            ProgData.Huidige_Gebruiker_Personeel_nummer = persoon._persnummer.ToString();
                            ProgData.RechtenHuidigeGebruiker = 1; // ingelogd maar kan verder niks
                            ProgData.GekozenKleur = persoon._kleur;
                            ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = persoon._kleur;
                            juist = true;
                        }
                    }
                    if (ProgData.Unscramble(persoon._passwoord) == textBoxPass.Text)
                    {
                        {
                            // juiste inlog
                            ProgData.Huidige_Gebruiker_Personeel_nummer = persoon._persnummer.ToString();
                            ProgData.RechtenHuidigeGebruiker = persoon._rechten;
                            ProgData.GekozenKleur = persoon._kleur;
                            ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = persoon._kleur;
                            juist = true;
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

        private void textBoxPass_TextChanged(object sender, EventArgs e)
        {
            buttonVerander.Enabled = false;
            //check passwoord
            try
            {
                personeel persoon = ProgData.ListPersoneel.First(b => b._persnummer.ToString() == textBoxNum.Text);
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
            personeel persoon = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxNum.Text);
            // encrypt pass
            if (textBoxChangePasswoord.Text == textBoxNum.Text)
            {
                MessageBox.Show("personeel nummer mag niet passwoord zijn");
            }
            else
            {
                persoon._passwoord = ProgData.Scramble(textBoxChangePasswoord.Text);
                ProgData.Save_Namen_lijst();
                MessageBox.Show("Wachtwoord is aangepast, log nu nogmaals in met dit wachtwoord.");

                textBoxNum.Enabled = true;
                textBoxPass.Enabled = true;
                buttonOke.Enabled = true;
            }
        }

        private void textBoxNum_TextChanged(object sender, EventArgs e)
        {
            // na reset passwoord is het "verander_nu", pas meteen aan.
            try
            {
                personeel persoon = ProgData.ListPersoneel.First(b => b._persnummer.ToString() == textBoxNum.Text);
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

        private void label3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBoxNum.Text = "590588";
            textBoxPass.Text = "kompas59";
        }
    }
}