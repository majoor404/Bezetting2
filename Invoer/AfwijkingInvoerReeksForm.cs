using Bezetting2.Data;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Bezetting2.Invoer
{
    public partial class AfwijkingInvoerReeksForm : Form
    {
        public AfwijkingInvoerReeksForm()
        {
            InitializeComponent();
        }

        private void buttonVoerUit_Click(object sender, EventArgs e)
        {
            // maak 2 maanden extra alvast
            int maand = ProgData.igekozenmaand;
            int jaar = ProgData.igekozenjaar;
            ProgData.igekozenmaand++;
            if (ProgData.igekozenmaand > 12)
            {
                ProgData.igekozenmaand = 1;
                ProgData.igekozenjaar++;
            }
            if (!Directory.Exists(ProgData.GetDir()))
                Directory.CreateDirectory(ProgData.GetDir());
            if (!File.Exists(ProgData._Ploeg_Namen_Locatie))
            {
                ProgData.Lees_Namen_lijst();            // lees alle mensen in sectie , personeel_lijst
                ProgData.MaakPloegNamenLijst(ProgData.GekozenKleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
                ProgData.SavePloegNamenLijst();     // save ploegbezetting (de mensen)
                ProgData.MaakLegeBezetting(ProgData.sgekozenjaar(), ProgData.igekozenmaand.ToString(), ProgData.GekozenKleur); // in deze roetine wordt het ook opgeslagen
            }
            ProgData.igekozenmaand++;
            if (ProgData.igekozenmaand > 12)
            {
                ProgData.igekozenmaand = 1;
                ProgData.igekozenjaar++;
            }
            if (!Directory.Exists(ProgData.GetDir()))
                Directory.CreateDirectory(ProgData.GetDir());
            if (!File.Exists(ProgData._Ploeg_Namen_Locatie))
            {
                ProgData.Lees_Namen_lijst();            // lees alle mensen in sectie , personeel_lijst
                ProgData.MaakPloegNamenLijst(ProgData.GekozenKleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
                ProgData.SavePloegNamenLijst();     // save ploegbezetting (de mensen)
                ProgData.MaakLegeBezetting(ProgData.sgekozenjaar(), ProgData.igekozenmaand.ToString(), ProgData.GekozenKleur); // in deze roetine wordt het ook opgeslagen
            }
            ProgData.igekozenmaand = maand;
            ProgData.igekozenjaar = jaar;

            DateTime start = new DateTime(jaar, maand, int.Parse(labelDatum.Text));
            // afhankelijk van keuze 
            if (comboBox1.SelectedIndex == 0)
            {
                // volgens rooster
                // eerste datum is labelDatum
                // vul afwijking op op normale rooster dagen.
                int aantal = (int)AantalDagen.Value;
                while (aantal > 0)
                {
                    ProgData.LoadPloegBezettingLijst();
                    werkdag ver = ProgData.Bezetting_Ploeg_Lijst.First(a => (a._naam == labelNaam.Text) && (a._dagnummer.ToString() == start.Day.ToString()));
                    if (ver._standaarddienst != "") // dus werkdag
                    {
                        ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelNaam.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, this.Text);
                        aantal--;
                    }
                    start = start.AddDays(1);
                    ProgData.igekozenmaand = start.Month;
                    ProgData.igekozenjaar = start.Year;
                }
                //ProgData.Main.VulViewScherm();
                //ProgData.Main.KleurMaandButton();
            }
            // Aantal copyeren volgens kalender dagen
            if (comboBox1.SelectedIndex == 1)
            {
                // zet datum goed en kleur goed
                //string bewaar_kleur = ProgData.GekozenKleur;
                //int bewaar_maand = ProgData.igekozenmaand;
                //int bewaar_jaar = ProgData.igekozenjaar;

                // zet nieuwe kleur en datum
                //ProgData.GekozenKleur = kleur;
                ProgData.igekozenmaand = start.Month;
                ProgData.igekozenjaar = start.Year;

                ProgData.LoadPloegBezettingLijst();
                ProgData.LoadVeranderingenPloegLijst();
                // volgens kalender
                for (int i = 0; i < AantalDagen.Value; i++)
                {
                    ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelNaam.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, this.Text);
                    start = start.AddDays(1);
                }

                // datum terug en kleur goed
                //ProgData.GekozenKleur = bewaar_kleur;
               

            }

            // ploeg invullen
            if (comboBox1.SelectedIndex == 2)
            {
                foreach (personeel per in ProgData.kleur_personeel_lijst)
                {
                    ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, per._achternaam, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, this.Text);
                }
            }
            // GP 2 op 2 af
            if (comboBox1.SelectedIndex == 3)
            {
                // eerste datum is labelDatum
                // vul afwijking op op normale rooster dagen.
                int aantal = (int)AantalDagen.Value;
                bool Schrijf_GP = true;

                ProgData.LoadPloegBezettingLijst();
                werkdag ver = ProgData.Bezetting_Ploeg_Lijst.First(a => (a._naam == labelNaam.Text) && (a._dagnummer.ToString() == start.Day.ToString()));

                while (aantal > 0)
                {
                    while (ver._standaarddienst == "")
                    {
                        start = start.AddDays(1);
                        
                        ProgData.igekozenmaand = start.Month;
                        ProgData.igekozenjaar = start.Year;
                        if (start.Month != maand)           // als nieuwe maand deze laden, anders gaat het fout bij "ver = ProgData.Bezetting_Ploeg_Lijst.First"
                            ProgData.LoadPloegBezettingLijst();

                        ver = ProgData.Bezetting_Ploeg_Lijst.First(a => (a._naam == labelNaam.Text) && (a._dagnummer.ToString() == start.Day.ToString()));
                    }

                    if (Schrijf_GP) ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelNaam.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, this.Text);
                    aantal--;
                    start = start.AddDays(1);
                    ProgData.igekozenmaand = start.Month;
                    ProgData.igekozenjaar = start.Year;
                    if (start.Month != maand)
                        ProgData.LoadPloegBezettingLijst();

                    ver = ProgData.Bezetting_Ploeg_Lijst.First(a => (a._naam == labelNaam.Text) && (a._dagnummer.ToString() == start.Day.ToString()));

                    if (Schrijf_GP) ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelNaam.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, this.Text);
                    aantal--;
                    start = start.AddDays(1);
                    ProgData.igekozenmaand = start.Month;
                    ProgData.igekozenjaar = start.Year;
                    if (start.Month != maand)
                        ProgData.LoadPloegBezettingLijst();
                    ver = ProgData.Bezetting_Ploeg_Lijst.First(a => (a._naam == labelNaam.Text) && (a._dagnummer.ToString() == start.Day.ToString()));

                    while (ver._standaarddienst == "")
                    {
                        start = start.AddDays(1);
                        ProgData.igekozenmaand = start.Month;
                        ProgData.igekozenjaar = start.Year;
                        if (start.Month != maand)
                            ProgData.LoadPloegBezettingLijst();
                        ver = ProgData.Bezetting_Ploeg_Lijst.First(a => (a._naam == labelNaam.Text) && (a._dagnummer.ToString() == start.Day.ToString()));
                    }

                    ProgData.igekozenmaand = start.Month;
                    ProgData.igekozenjaar = start.Year;
                    if (start.Month != maand)
                        ProgData.LoadPloegBezettingLijst();
                    ver = ProgData.Bezetting_Ploeg_Lijst.First(a => (a._naam == labelNaam.Text) && (a._dagnummer.ToString() == start.Day.ToString()));
                    Schrijf_GP = !Schrijf_GP;
                }
                AantalDagen.Enabled = true;
            }
            ProgData.igekozenmaand = maand;
            ProgData.igekozenjaar = jaar;

            ProgData.Main.VulViewScherm();
            ProgData.Main.KleurMaandButton();
        }

        private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAfwijking.Text = listBoxItems.SelectedItem.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 2)
            {
                AantalDagen.Enabled = true;
            }

            if (comboBox1.SelectedIndex == 2)
            {
                AantalDagen.Enabled = false;
                AantalDagen.Value = 1;
            }
            if (comboBox1.SelectedIndex == 3)
            {
                textBoxAfwijking.Text = "GP";
                textBoxRede.Text = "Generatie Pack";
                AantalDagen.Value = 24;
            }
        }
    }
}
