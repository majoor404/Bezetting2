using Bezetting2.Data;
using Bezetting2.InlogGebeuren;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class EditPersoneel : Form
    {
        private int selpersnummer;
        private int rechten;

        public EditPersoneel()
        {
            InitializeComponent();
        }

        private void EditPersoneel_Shown(object sender, EventArgs e)
        {
            comboBoxFilter.Enabled = true;
            ProgData.Lees_Namen_lijst();
            ViewNamen.Items.Clear();
            comboBoxFilter.Items.Clear();
            string[] arr = new string[5];
            ListViewItem itm;
            foreach (personeel a in ProgData.ListPersoneel)
            {
                arr[0] = a._persnummer.ToString();
                arr[1] = a._achternaam;
                arr[2] = a._voornaam;
                arr[3] = a._kleur;
                arr[4] = a._rechten.ToString();
                itm = new ListViewItem(arr);
                ViewNamen.Items.Add(itm);

                if (a._kleur != null && !comboBoxFilter.Items.Contains(a._kleur))
                    comboBoxFilter.Items.Add(a._kleur);
            }
            comboBoxFilter.Text = "";
            comboBoxFilter.Items.Add("");
            textBoxPersNum.Text = "";
            textBoxAchterNaam.Text = "";
            textBoxVoorNaam.Text = "";
            textBoxAdres.Text = "";
            textBoxPostcode.Text = "";
            textBoxWoonplaats.Text = "";
            textBoxEmailThuis.Text = "";
            textBoxEmailWerk.Text = "";
            textBoxTelThuis.Text = "";
            textBoxTelMobPrive.Text = "";
            textBoxTelMobWerk.Text = "";
            textBoxAdresCodeWerk.Text = "";
            textBoxTelWerk.Text = "";
            textBoxKleur.Text = "";
            textBoxFuntie.Text = "";
            textBoxWerkplek.Text = "";
            LabelRoosterNieuw.Text = "";
            labelNieuwRoosterDatum.Text = "";
            vuilwerk.Checked = false;

            // als rechten 50, dan alleen eigen kleur
            if (ProgData.RechtenHuidigeGebruiker < 51)
            {
                // haal kleur van gebruiker
                comboBoxFilter.Text = ProgData.Huidige_Gebruiker_Werkt_Op_Kleur;
                comboBoxFilter.Enabled = false;
            }
            if (ProgData.RechtenHuidigeGebruiker > 100)
            {
                // direct edit ploeg rooster als admin
                MessageBox.Show("als Admin nu direct rooster wissel mogelijk");
                textBoxKleur.ReadOnly = false;
            }
        }

        // filter aangepast
        private void ComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewNamen.Items.Clear();
            string[] arr = new string[5];
            ListViewItem itm;
            foreach (personeel a in ProgData.ListPersoneel)
            {
                if (a._kleur == comboBoxFilter.Text || string.IsNullOrEmpty(comboBoxFilter.Text))
                {
                    arr[0] = a._persnummer.ToString();
                    arr[1] = a._achternaam;
                    arr[2] = a._voornaam;
                    arr[3] = a._kleur;
                    arr[4] = a._rechten.ToString();
                    itm = new ListViewItem(arr);
                    ViewNamen.Items.Add(itm);
                }
            }
            textBoxPersNum.Text = "";
            textBoxAchterNaam.Text = "";
            textBoxVoorNaam.Text = "";
            textBoxAdres.Text = "";
            textBoxPostcode.Text = "";
            textBoxWoonplaats.Text = "";
            textBoxEmailThuis.Text = "";
            textBoxEmailWerk.Text = "";
            textBoxTelThuis.Text = "";
            textBoxTelMobPrive.Text = "";
            textBoxTelMobWerk.Text = "";
            textBoxAdresCodeWerk.Text = "";
            textBoxTelWerk.Text = "";
            textBoxKleur.Text = "";
            textBoxFuntie.Text = "";
            textBoxWerkplek.Text = "";
            LabelRoosterNieuw.Text = "";
            vuilwerk.Checked = false;
        }

        // geklikt op naam in overzicht
        private void ViewNamen_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_ = comboBoxFilter.Text;
            selpersnummer = 0;
            try
            {
                // ik kom hier elke keer 2 maal bij klikken
                // als er al 1 gesellcteerd is en ik klik een andere
                // zie je eerst geen item geklikt, dan de volgende.
                // dus niks in catch invullen
                var test = ViewNamen.SelectedItems[0].SubItems[0].Text;
                selpersnummer = int.Parse(ViewNamen.SelectedItems[0].SubItems[0].Text);
            }
            catch
            {
            }
            foreach (personeel a in ProgData.ListPersoneel)
            {
                if (a._persnummer == selpersnummer)
                {
                    textBoxPersNum.Text = a._persnummer.ToString();
                    textBoxAchterNaam.Text = a._achternaam;
                    textBoxVoorNaam.Text = a._voornaam;
                    textBoxAdres.Text = a._adres;
                    textBoxPostcode.Text = a._postcode;
                    textBoxWoonplaats.Text = a._woonplaats;
                    textBoxEmailThuis.Text = a._emailthuis;
                    textBoxEmailWerk.Text = a._emailwerk;
                    textBoxTelThuis.Text = a._telthuis;
                    textBoxTelMobPrive.Text = a._tel06prive;
                    textBoxTelMobWerk.Text = a._tel06werk;
                    textBoxAdresCodeWerk.Text = a._adrescodewerk;
                    textBoxTelWerk.Text = a._telwerk;
                    textBoxKleur.Text = a._kleur;
                    textBoxFuntie.Text = a._funtie;
                    textBoxWerkplek.Text = a._werkgroep;
                    vuilwerk.Checked = bool.Parse(a._vuilwerk);

                    LabelRoosterNieuw.Text = a._nieuwkleur;
                    if (!string.IsNullOrEmpty(LabelRoosterNieuw.Text))
                    {
                        labelNieuwRoosterDatum.Text = a._verhuisdatum.ToString("d");
                        button1.Enabled = false;
                    }
                    else
                    {
                        labelNieuwRoosterDatum.Text = "";
                        button1.Enabled = true;
                    }
                    rechten = a._rechten;
                }
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                personeel persoon_gekozen = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
                ProgData.ListPersoneel.Remove(persoon_gekozen);
                ButtonVoegToe_Click(this, null);
                EditPersoneel_Shown(this, null);
            }
            catch
            {
                MessageBox.Show("Kan personeel nummer niet vinden om record te veranderen.");
            }
        }

        // verhuis bericht naar andere kleur
        private void VerhuisKnop_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Ingevulde dagen worden niet overgenomen naar nieuw rooster, toekomst ;-), doorgaan ?", "Vraagje", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {


                MessageBox.Show("Gebruik dit alleen als gebruiker langer als een maand verhuisd, dus eind datum ligt maand verder dan nu!");
                VerhuisForm verhuis = new VerhuisForm();
                verhuis.labelNaam.Text = textBoxAchterNaam.Text;
                verhuis.labelPersoneelNummer.Text = textBoxPersNum.Text;
                verhuis.labelHuidigRooster.Text = textBoxKleur.Text;
                verhuis.ShowDialog();

                try
                {
                    personeel persoon_gekozen = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);

                    // zat op deze kleur, maar gaat naar nieuwe
                    // dus zet kruisjes dat hij niet meer aanwezig is.
                    persoon_gekozen._verhuisdatum = verhuis.dateTimeVerhuisDatum.Value;
                    persoon_gekozen._nieuwkleur = verhuis.comboBoxNieuwRooster.Text;
                    ProgData.Save_Namen_lijst();

                    int eerste_dag_weg = persoon_gekozen._verhuisdatum.Day;

                    LabelRoosterNieuw.Text = persoon_gekozen._nieuwkleur;

                    string GekozenKleurInBeeld = ProgData.GekozenKleur;

                    // zet path goed van kleur ploeg
                    ProgData.GekozenKleur = persoon_gekozen._kleur;
                    // zet maand en jaar goed van verhuis datum
                    ProgData.igekozenmaand = persoon_gekozen._verhuisdatum.Month;
                    ProgData.Igekozenjaar = persoon_gekozen._verhuisdatum.Year;

                    int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);

                    if (ProgData.GekozenKleur != "Nieuw") // als nieuw persoon, dan hoef je niet X te zetten bij weg gaan ploeg
                    {
                        for (int i = eerste_dag_weg; i < aantal_dagen_deze_maand + 1; i++)
                        {
                            ProgData.RegelAfwijking(persoon_gekozen._achternaam, i.ToString(), "X", "Rooster Wissel", "Verhuizing", ProgData.GekozenKleur);
                        }
                    }

                    // tevens 12 maanden verder de X
                    for (int maandenverder = 1; maandenverder < 12; maandenverder++)
                    {

                        DateTime dummy = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, 1);
                        dummy = dummy.AddMonths(1);
                        aantal_dagen_deze_maand = DateTime.DaysInMonth(dummy.Year, dummy.Month);
                        ProgData.Igekozenjaar = dummy.Year;
                        ProgData.igekozenmaand = dummy.Month;

                        if (ProgData.GekozenKleur != "Nieuw") // als nieuw persoon, dan hoef je niet X te zetten bij weg gaan ploeg
                        {
                            for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
                            {
                                ProgData.RegelAfwijking(persoon_gekozen._achternaam, i.ToString(), "X", "Rooster Wissel", "Verhuizing", ProgData.GekozenKleur);
                            }
                        }
                    }

                    // komt van andere kleur
                    //GekozenKleurInBeeld = ProgData.GekozenKleur;

                    ProgData.GekozenKleur = persoon_gekozen._nieuwkleur;
                    // zet maand en jaar goed van verhuis datum
                    ProgData.igekozenmaand = persoon_gekozen._verhuisdatum.Month;
                    ProgData.Igekozenjaar = persoon_gekozen._verhuisdatum.Year;

                    ProgData.MaakPloegNamenLijst(ProgData.GekozenKleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
                    ProgData.SavePloegNamenLijst(30);     // save ploegbezetting (de mensen)

                    // moet nieuwe collega toevoegen aan bezetting

                    int aantal_dagen_dezemaand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);
                    ProgData.LoadPloegBezetting(ProgData.GekozenKleur, 30); // want nieuwe kleur gekozen
                                                                            // maak ruimte voor nieuwe collega in werkdag tabel
                    for (int i = 1; i < aantal_dagen_dezemaand + 1; i++)
                    {
                        DateTime dat = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, i);
                        werkdag dag = new werkdag
                        {
                            _naam = persoon_gekozen._achternaam,
                            _standaarddienst = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), dat, persoon_gekozen._nieuwkleur),
                            _werkplek = "",
                            _afwijkingdienst = "",
                            _dagnummer = i
                        };
                        ProgData.ListWerkdagPloeg.Add(dag);
                    }
                    ProgData.SavePloegBezetting(ProgData.GekozenKleur, 30);

                    for (int i = 1; i < eerste_dag_weg; i++)
                    {
                        DateTime dat = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, i);
                        ProgData.RegelAfwijkingOpDatumEnKleur(dat, persoon_gekozen._nieuwkleur, persoon_gekozen._achternaam, i.ToString(), "X", "Rooster Wissel", "Verhuizing");
                    }

                    // gevraagde afwijkingen/vakantie's op oude wacht, zodat ze kunnen verhuizen naar nieuwe
                    // meegeven eerste dag.
                    Verhuis_oude_afwijkingen(textBoxPersNum.Text, eerste_dag_weg, ProgData.igekozenmaand, ProgData.Igekozenjaar);

                    LabelRoosterNieuw.Text = persoon_gekozen._nieuwkleur;
                    if (!string.IsNullOrEmpty(LabelRoosterNieuw.Text))
                    {
                        labelNieuwRoosterDatum.Text = persoon_gekozen._verhuisdatum.ToString("d");
                        button1.Enabled = false;
                    }
                    else
                    {
                        labelNieuwRoosterDatum.Text = "";
                        button1.Enabled = true;
                    }
                    ProgData.Save_Namen_lijst();

                    ProgData.GekozenKleur = GekozenKleurInBeeld;

                }
                catch { }
            }// vraagje 
        }

        private void ButtonCancelVerhuis_Click(object sender, EventArgs e)
        {
            personeel persoon_gekozen = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
            DateTime dum = new DateTime(9999, 1, 1);
            persoon_gekozen._verhuisdatum = dum;
            persoon_gekozen._nieuwkleur = "";
            labelNieuwRoosterDatum.Text = "";
            LabelRoosterNieuw.Text = "";
            ProgData.Save_Namen_lijst();
            MessageBox.Show("Kruisjes in bezetting met de hand weghalen!");
            EditPersoneel_Shown(this, null);
        }

        private void ButtonRechten_Click(object sender, EventArgs e)
        {
            RechtenInstellenForm recht = new RechtenInstellenForm();
            recht.labelNaam.Text = textBoxAchterNaam.Text;
            recht.labelPersoneelNummer.Text = textBoxPersNum.Text;
            recht.labelRechtenNivo.Text = rechten.ToString();
            if (rechten > 51)
                recht.checkBoxAllePloegen.Checked = true;
            if (rechten == 0)
                recht.radioButton0.Checked = true;
            if (rechten > 24)
                recht.radioButton25.Checked = true;
            if(rechten> 49)
                recht.radioButton50.Checked = true;

            DialogResult dialog = recht.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                personeel persoon = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
                persoon._rechten = int.Parse(recht.labelRechtenNivo.Text);
                ProgData.Save_Namen_lijst();
            }
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Delete deze naam ?", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                personeel persoon = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
                ProgData.ListPersoneel.Remove(persoon);
                ProgData.Save_Namen_lijst();
                EditPersoneel_Shown(this, null);
            }
        }

        private void ButtonVoegToe_Click(object sender, EventArgs e)
        {
            personeel a = new personeel
            {
                _persnummer = int.Parse(textBoxPersNum.Text),
                _achternaam = textBoxAchterNaam.Text,
                _voornaam = textBoxVoorNaam.Text,
                _adres = textBoxAdres.Text,
                _postcode = textBoxPostcode.Text,
                _woonplaats = textBoxWoonplaats.Text,
                _emailthuis = textBoxEmailThuis.Text,
                _emailwerk = textBoxEmailWerk.Text,
                _telthuis = textBoxTelThuis.Text,
                _tel06prive = textBoxTelMobPrive.Text,
                _tel06werk = textBoxTelMobWerk.Text,
                _adrescodewerk = textBoxAdresCodeWerk.Text,
                _telwerk = textBoxTelWerk.Text,
                _vuilwerk = vuilwerk.Checked.ToString()
            };

            if (string.IsNullOrEmpty(textBoxKleur.Text))
            {
                a._kleur = "Nieuw";
            }
            else
            {
                a._kleur = textBoxKleur.Text;
            }

            if (ProgData.RechtenHuidigeGebruiker > 100)
            {
                // direct edit ploeg rooster als admin
                a._kleur = textBoxKleur.Text;
            }

            a._funtie = textBoxFuntie.Text;
            a._werkgroep = textBoxWerkplek.Text;
            a._rechten = 0;
            ProgData.ListPersoneel.Add(a);
            ProgData.Save_Namen_lijst();
            EditPersoneel_Shown(this, null);
        }

        private void ButtonNieuw_Click(object sender, EventArgs e)
        {
            textBoxPersNum.Text = "";
            textBoxAchterNaam.Text = "";
            textBoxVoorNaam.Text = "";
            textBoxAdres.Text = "";
            textBoxPostcode.Text = "";
            textBoxWoonplaats.Text = "";
            textBoxEmailThuis.Text = "";
            textBoxEmailWerk.Text = "";
            textBoxTelThuis.Text = "";
            textBoxTelMobPrive.Text = "";
            textBoxTelMobWerk.Text = "";
            textBoxAdresCodeWerk.Text = "";
            textBoxTelWerk.Text = "";
            textBoxKleur.Text = "";
            textBoxFuntie.Text = "";
            textBoxWerkplek.Text = "";
            LabelRoosterNieuw.Text = "";
            vuilwerk.Checked = false;
            MessageBox.Show("Na invoeren naam enz, druk op voeg toe.\nDoe daarna kleur verhuizing naar juiste kleur/plek.");
        }

        // gevraagde afwijkingen/vakantie's op oude wacht, zodat ze kunnen verhuizen naar nieuwe
        private void Verhuis_oude_afwijkingen(string personeel_nummer, int eerste_dag, int maand, int jaar)
        {
            MessageBox.Show("nog te doen, copy ingevulde dagen naar nieuwe rooster");
        }
    }
}