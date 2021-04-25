using Bezetting2.Data;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static Bezetting2.DatumVijfPloegUtils;

namespace Bezetting2.Invoer
{
    public partial class AfwijkingInvoerReeksForm : Form
    {
        public AfwijkingInvoerReeksForm()
        {
            InitializeComponent();
            labelAftellen.Visible = false;
            comboBox1.SelectedIndex = 0;
        }

        private void ButtonVoerUit_Click(object sender, EventArgs e)
        {
            bool okido = true;
            labelAftellen.Visible = true;
            if (string.IsNullOrEmpty(textBoxAfwijking.Text))
            {
                okido = false;
                string boodschap = "Afwijking leeg, wissen ?";
                DialogResult iRet = MessageBox.Show(boodschap, "Afwijking", MessageBoxButtons.YesNo);
                if (iRet == DialogResult.Yes)
                    okido = true;
            }

            if (okido)
            {
                textBoxAfwijking.Text = textBoxAfwijking.Text.ToUpper();
                string eerste_2 = textBoxAfwijking.Text.Length >= 2 ? textBoxAfwijking.Text.Substring(0, 2) : textBoxAfwijking.Text;
                int maand = ProgData.igekozenmaand;
                int jaar = ProgData.igekozenjaar;
                DateTime start = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, int.Parse(labelDatum.Text));
                // afhankelijk van keuze 
                if (comboBox1.SelectedIndex == 0)
                {
                    // volgens rooster
                    // eerste datum is labelDatum
                    // vul afwijking op op normale rooster dagen.
                    int aantal = (int)AantalDagen.Value;
                    while (aantal > 0)
                    {
                        labelAftellen.Text = aantal.ToString();
                        labelAftellen.Refresh();
                        
                        // welke dienst heeft deze kleur ?
                        var ploeg = ProgData.Get_Gebruiker_Kleur(labelPersoneelnr.Text, start);
                        var dienst = GetDienst(ProgData.GekozenRooster(), start, ploeg);

                        if (!string.IsNullOrEmpty(dienst)) // dus werkdag
                        {
                            ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelPersoneelnr.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, ProgData.Huidige_Gebruiker_Naam());
                            // DEBUG
                            //Thread.Sleep(300);
                            if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD" || eerste_2 == "DD")
                            {
                                ProgData.VulInLooptExtraDienst(textBoxAfwijking.Text, start, labelNaam.Text);
                                // DEBUG
                                //Thread.Sleep(300);
                            }
                            aantal--;
                        }
                        start = start.AddDays(1);
                        ProgData.igekozenmaand = start.Month;
                        ProgData.igekozenjaar = start.Year;
                    }
                }
                // Aantal copyeren volgens kalender dagen
                if (comboBox1.SelectedIndex == 1)
                {
                    ProgData.igekozenmaand = start.Month;
                    ProgData.igekozenjaar = start.Year;

                    //ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);
                    //ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur, 15);
                    // volgens kalender
                    for (int i = 0; i < AantalDagen.Value; i++)
                    {
                        labelAftellen.Text = i.ToString();
                        labelAftellen.Refresh();
                        ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelPersoneelnr.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, ProgData.Huidige_Gebruiker_Naam());
                        // DEBUG
                        // Thread.Sleep(300);
                        if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD" || eerste_2 == "DD")
                        {
                            ProgData.VulInLooptExtraDienst(textBoxAfwijking.Text, start, labelNaam.Text);
                            // DEBUG
                            //Thread.Sleep(300);
                        }
                        start = start.AddDays(1);
                    }
                }

                // voor hele ploeg invullen
                if (comboBox1.SelectedIndex == 2)
                {
                    foreach (personeel per in ProgData.AlleMensen.LijstPersoonKleur)
                    {
                        ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, per._persnummer.ToString(), start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, ProgData.Huidige_Gebruiker_Naam());
                        // DEBUG
                        // Thread.Sleep(300);
                        if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD" || eerste_2 == "DD")
                        {
                            ProgData.VulInLooptExtraDienst(textBoxAfwijking.Text, start, labelNaam.Text);
                            // DEBUG
                            //Thread.Sleep(300);
                        }
                    }
                }
                // GP 2 op 2 af
                if (comboBox1.SelectedIndex == 3)
                {
                    // eerste datum is labelDatum
                    // vul afwijking op op normale rooster dagen.
                    int aantal = (int)AantalDagen.Value;
                    bool Schrijf_GP = true;

                    // welke dienst heeft deze kleur ?
                    var ploeg = ProgData.Get_Gebruiker_Kleur(labelPersoneelnr.Text);
                    var dienst = GetDienst(ProgData.GekozenRooster(), start, ploeg);

                    while (aantal > 0)
                    {
                        labelAftellen.Text = aantal.ToString();
                        labelAftellen.Refresh();
                        while (string.IsNullOrEmpty(dienst))
                        {
                            start = start.AddDays(1);

                            ProgData.igekozenmaand = start.Month;
                            ProgData.igekozenjaar = start.Year;
                            
                            dienst = GetDienst(ProgData.GekozenRooster(), start, ploeg);
                        }

                        if (Schrijf_GP) ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelPersoneelnr.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, ProgData.Huidige_Gebruiker_Naam());
                        //DEBUG
                        //Thread.Sleep(300);
                        aantal--;
                        start = start.AddDays(1);
                        ProgData.igekozenmaand = start.Month;
                        ProgData.igekozenjaar = start.Year;
                        
                        dienst = GetDienst(ProgData.GekozenRooster(), start, ploeg);

                        if (Schrijf_GP) ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelPersoneelnr.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, ProgData.Huidige_Gebruiker_Naam());
                        //DEBUG
                        //Thread.Sleep(300);
                        aantal--;
                        start = start.AddDays(1);
                        ProgData.igekozenmaand = start.Month;
                        ProgData.igekozenjaar = start.Year;
                        dienst = GetDienst(ProgData.GekozenRooster(), start, ploeg);
                        
                        while (string.IsNullOrEmpty(dienst))
                        {
                            start = start.AddDays(1);
                            ProgData.igekozenmaand = start.Month;
                            ProgData.igekozenjaar = start.Year;
                            dienst = GetDienst(ProgData.GekozenRooster(), start, ploeg);
                        }

                        ProgData.igekozenmaand = start.Month;
                        ProgData.igekozenjaar = start.Year;
                        dienst = GetDienst(ProgData.GekozenRooster(), start, ploeg);
                        Schrijf_GP = !Schrijf_GP;
                    }
                    AantalDagen.Enabled = true;
                }
                
                // aantal X om de Y dagen
                if (comboBox1.SelectedIndex == 4)
                {
                    // eerste datum is labelDatum
                    int X = (int)AantalDagen.Value;
                    int Y = (int)numericUpDownOmDeAantalDagen.Value;

                    while (X > 0)
                    {
                        labelAftellen.Text = X.ToString();
                        labelAftellen.Refresh();

                        // gaat fout als ploegbezetting niet bestaat!!
                        //ProgData.CheckFiles(ProgData.GekozenKleur);
                        // nu check in loadploegbezetting gedaan
                        
                        ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelPersoneelnr.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, ProgData.Huidige_Gebruiker_Naam());
                        //DEBUG
                        //Thread.Sleep(300);
                        if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD" || eerste_2 == "DD")
                        {
                            ProgData.VulInLooptExtraDienst(textBoxAfwijking.Text, start, labelNaam.Text);
                            // DEBUG
                            //Thread.Sleep(300);
                        }
                        X--;
                        start = start.AddDays(Y);
                        ProgData.igekozenmaand = start.Month;
                        ProgData.igekozenjaar = start.Year;
                    }
                }
                if (comboBox1.SelectedIndex == 5)
                {
                    int AantalAfwijkingX = (int) numericUpDownAfwHerX.Value;
                    int HerhallingOmYDagen = (int)numericUpDownY.Value;
                    int Aantal = (int)AantalDagen.Value;

                    DateTime EersteStartDag = start;

                    while (Aantal > 0)
                    {
                        for (int i = 0; i < AantalAfwijkingX; i++)
                        {
                            labelAftellen.Text = i.ToString();
                            labelAftellen.Refresh();
                            ProgData.RegelAfwijkingOpDatumEnKleur(start, ProgData.GekozenKleur, labelPersoneelnr.Text, start.Day.ToString(), textBoxAfwijking.Text, textBoxRede.Text, ProgData.Huidige_Gebruiker_Naam());
                            //DEBUG
                            //Thread.Sleep(300);
                            if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD" || eerste_2 == "DD")
                            {
                                ProgData.VulInLooptExtraDienst(textBoxAfwijking.Text, start, labelNaam.Text);
                                // DEBUG
                                //Thread.Sleep(300);
                            }
                            start = start.AddDays(1);
                        }
                        Aantal--;
                        start = EersteStartDag;
                        start = start.AddDays(HerhallingOmYDagen);
                        EersteStartDag = start;
                    }
                }

                ProgData.igekozenmaand = maand;
                ProgData.igekozenjaar = jaar;

                ProgData.Main.VulViewScherm();
                ProgData.Main.KleurMaandButton();
            }
        }

        private void ListBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAfwijking.Text = listBoxItems.SelectedItem.ToString();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelSpeciaal.Visible = false;
            panelSpeciaal2.Visible = false;
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
            if (comboBox1.SelectedIndex == 4)
            {
                panelSpeciaal.Visible = true;
                numericUpDownOmDeAantalDagen.Value = 1;
            }
            if (comboBox1.SelectedIndex == 5)
            {
                panelSpeciaal2.Visible = true;
                numericUpDownY.Value = 1;
                numericUpDownAfwHerX.Value = 1;
            }
        }

    }
}
