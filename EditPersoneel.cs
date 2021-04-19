using Bezetting2.Data;
using Bezetting2.InlogGebeuren;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using static Bezetting2.Data.MaandDataClass;

namespace Bezetting2
{
    public partial class EditPersoneel : Form
    {
        private int selpersnummer;
        private int rechten;

        private int bewaar_rechten = 0;
        private string bewaar_passwoord = ProgData.Scramble("verander_nu");
        private string bewaar_kleur;
        private string bewaar_nieuwkleur;
        private DateTime bewaar_verhuisdatum;

        // sla de veranderingen op van persoon in tijdelijke lijst
        readonly List<VeranderingenVerhuis> VeranderingenLijstTemp = new List<VeranderingenVerhuis>();

        public EditPersoneel()
        {
            InitializeComponent();
        }

        private void EditPersoneel_Shown(object sender, EventArgs e)
        {
            comboBoxFilter.Enabled = true;
            ProgData.AlleMensen.Load();
            ViewNamen.Items.Clear();
            comboBoxFilter.Items.Clear();
            string[] arr = new string[5];
            ListViewItem itm;
            foreach (personeel a in ProgData.AlleMensen.LijstPersonen)
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
            comboBoxKleur.Text = "";
            textBoxFuntie.Text = "";
            textBoxWerkplek.Text = "";
            LabelRoosterNieuw.Text = "";
            labelNieuwRoosterDatum.Text = "";
            vuilwerk.Checked = false;
            textBoxPersNum.Enabled = false;
            textBoxAchterNaam.Enabled = true;

            // als rechten 50, dan alleen eigen kleur
            if (ProgData.RechtenHuidigeGebruiker < 51)
            {
                // haal kleur van gebruiker
                comboBoxFilter.Text = ProgData.Huidige_Gebruiker_Werkt_Op_Kleur();
                comboBoxFilter.Enabled = false;
            }
            if (ProgData.RechtenHuidigeGebruiker > 100)
            {
                // direct edit ploeg rooster als admin
                MessageBox.Show("Als Admin nu direct rooster wissel mogelijk\n");
                comboBoxKleur.Enabled = true;
            }

            button1.Enabled = buttonCancelVerhuis.Enabled =
                buttonExport.Enabled = buttonImport.Enabled = ProgData.RechtenHuidigeGebruiker > 99;

            buttonExtraAdmin.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
        }

        // filter aangepast
        private void ComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewNamen.Items.Clear();
            string[] arr = new string[5];
            ListViewItem itm;
            foreach (personeel a in ProgData.AlleMensen.LijstPersonen)
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
            comboBoxKleur.Text = "";
            textBoxFuntie.Text = "";
            textBoxWerkplek.Text = "";
            LabelRoosterNieuw.Text = "";
            vuilwerk.Checked = false;
        }

        // geklikt op naam in overzicht
        private void ViewNamen_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_ = comboBoxFilter.Text;
            comboBoxKleur.Enabled = false;
            textBoxPersNum.Enabled = false;
            selpersnummer = 0;
            try
            {
                // ik kom hier elke keer 2 maal bij klikken
                // als er al 1 gesellcteerd is en ik klik een andere
                // zie je eerst geen item geklikt, dan de volgende.
                // dus niks in catch invullen
                string test = ViewNamen.SelectedItems[0].SubItems[0].Text;
                selpersnummer = int.Parse(ViewNamen.SelectedItems[0].SubItems[0].Text);
            }
            catch
            {
            }
            foreach (personeel a in ProgData.AlleMensen.LijstPersonen)
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
                    comboBoxKleur.Text = a._kleur;
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

                    if (ProgData.RechtenHuidigeGebruiker > 100)
                    {
                        comboBoxKleur.Enabled = true;
                    }
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
                string gekozen_personeel_nummer = textBoxPersNum.Text;
                personeel persoon_gekozen = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == textBoxPersNum.Text);

                BewaarRechtenEnPasswoord(persoon_gekozen);
                ProgData.AlleMensen.LijstPersonen.Remove(persoon_gekozen);
                ButtonVoegToe_Click(this, null);

                persoon_gekozen = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == gekozen_personeel_nummer);
                ReturnRechtenEnPasswoord(persoon_gekozen);
                ProgData.AlleMensen.Save();
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
            if (ProgData.RechtenHuidigeGebruiker > 99)
            {
                MessageBox.Show("Gebruik dit alleen als gebruiker langdurig of voor altijd verhuisd,\nLanger dan 6 weken!");

                if (KillAlleAndereGebruikers())
                {
                    VerhuisForm verhuis = new VerhuisForm();
                    verhuis.labelNaam.Text = textBoxAchterNaam.Text;
                    verhuis.labelPersoneelNummer.Text = textBoxPersNum.Text;
                    verhuis.labelHuidigRooster.Text = comboBoxKleur.Text;
                    DialogResult dialogResult1 = verhuis.ShowDialog();

                    int teller = 0;

                    if (dialogResult1 != DialogResult.Cancel)
                    {
                        MessageBox.Show("Geduld, copyeren van dagen, u krijgt melding als het klaar is.\nKopie 6 maanden van deze persoon naar andere kleur.");

                        try
                        {
                            personeel persoon_gekozen = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == textBoxPersNum.Text);

                            // zat op deze kleur, maar gaat naar nieuwe
                            // dus zet kruisjes dat hij niet meer aanwezig is.
                            persoon_gekozen._verhuisdatum = verhuis.dateTimeVerhuisDatum.Value;
                            persoon_gekozen._nieuwkleur = verhuis.comboBoxNieuwRooster.Text;
                            //ProgData.Save_LijstNamen();
                            ProgData.AlleMensen.Save();

                            int eerste_dag_weg = persoon_gekozen._verhuisdatum.Day;

                            LabelRoosterNieuw.Text = persoon_gekozen._nieuwkleur;

                            string GekozenKleurInBeeld = ProgData.GekozenKleur;

                            // zet path goed van kleur ploeg
                            ProgData.GekozenKleur = persoon_gekozen._kleur;
                            // zet maand en jaar goed van verhuis datum
                            ProgData.igekozenmaand = persoon_gekozen._verhuisdatum.Month;
                            ProgData.igekozenjaar = persoon_gekozen._verhuisdatum.Year;

                            // bewaar afwijkingen/vakantie's op oude wacht/kleur , zodat ze kunnen verhuizen naar nieuwe
                            // meegeven eerste dag.
                            if (persoon_gekozen._kleur != "Nieuw")
                                Bewaar_oude_afwijkingen(persoon_gekozen._persnummer, persoon_gekozen._verhuisdatum.Day, persoon_gekozen._verhuisdatum.Month, persoon_gekozen._verhuisdatum.Year);

                            int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);

                            // zet kruisjes
                            if (ProgData.GekozenKleur != "Nieuw") // als nieuw persoon, dan hoef je niet X te zetten bij weg gaan ploeg
                            {
                                for (int i = eerste_dag_weg; i < aantal_dagen_deze_maand + 1; i++)
                                {
                                    labelNieuwRoosterDatum.Text = $"{teller++}      ";
                                    labelNieuwRoosterDatum.Refresh();
                                    ProgData.RegelAfwijking(persoon_gekozen._persnummer.ToString(), i.ToString(), "X", "Rooster Wissel", "Verhuizing", ProgData.GekozenKleur);
                                }
                            }

                            ProgData.GekozenKleur = persoon_gekozen._nieuwkleur;

                            // zet maand en jaar goed van verhuis datum
                            ProgData.igekozenmaand = persoon_gekozen._verhuisdatum.Month;
                            ProgData.igekozenjaar = persoon_gekozen._verhuisdatum.Year;

                            // voeg nieuwe collega toevoegen aan bezetting

                            string naam_ = ProgData.Get_Gebruiker_Naam(textBoxPersNum.Text);
                            ProgData.MaakNieuweCollegaInBezettingAan(naam_, ProgData.GekozenKleur, ProgData.igekozenjaar, ProgData.igekozenmaand, 5);

                            // kruisjes op nieuwe ploeg
                            for (int i = 1; i < eerste_dag_weg; i++)
                            {
                                DateTime dat = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, i);
                                ProgData.RegelAfwijkingOpDatumEnKleur(dat, persoon_gekozen._nieuwkleur, persoon_gekozen._persnummer.ToString(), i.ToString(), "X", "Rooster Wissel", "Verhuizing", false);
                            }

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
                            //ProgData.Save_LijstNamen();
                            ProgData.AlleMensen.Save();

                            // gevraagde afwijkingen/vakantie's op oude wacht, zodat ze kunnen verhuizen naar nieuwe
                            // meegeven eerste dag.
                            Restore_oude_afwijkingen(persoon_gekozen._nieuwkleur, true);

                            MessageBox.Show("Klaar met Verhuis");

                            ProgData.GekozenKleur = GekozenKleurInBeeld;
                        }
                        catch { }
                    }
                }// vraagje 
            }
            else
            {
                MessageBox.Show("Alleen met rechten > 99 !");
            }
            //}
        }

        private void ButtonCancelVerhuis_Click(object sender, EventArgs e)
        {
            if (ProgData.RechtenHuidigeGebruiker > 99)
            {
                if (!string.IsNullOrEmpty(LabelRoosterNieuw.Text))
                {
                    if (KillAlleAndereGebruikers())
                    {
                        int maand = ProgData.igekozenmaand;
                        int jaar = ProgData.igekozenjaar;
                        string kleur = ProgData.GekozenKleur;
                        int teller = 0;

                        personeel persoon_gekozen = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == textBoxPersNum.Text);

                        DateTime verhuisdatum_was = persoon_gekozen._verhuisdatum;
                        string kleur_was = persoon_gekozen._nieuwkleur;
                        string kleur_terug = persoon_gekozen._kleur;

                        DateTime dum = new DateTime(9999, 1, 1);
                        persoon_gekozen._verhuisdatum = dum;
                        persoon_gekozen._nieuwkleur = "";
                        labelNieuwRoosterDatum.Text = "";
                        LabelRoosterNieuw.Text = "";
                        //ProgData.Save_LijstNamen();
                        ProgData.AlleMensen.Save();

                        // get data van nieuwkleur
                        ProgData.GekozenKleur = kleur_was;

                        Bewaar_oude_afwijkingen(persoon_gekozen._persnummer, verhuisdatum_was.Day,
                            verhuisdatum_was.Month, verhuisdatum_was.Year);

                        int eerste_dag_weg = verhuisdatum_was.Day;

                        // zet path goed van kleur ploeg
                        ProgData.GekozenKleur = kleur_terug;
                        ProgData.igekozenjaar = verhuisdatum_was.Year;
                        ProgData.igekozenmaand = verhuisdatum_was.Month;

                        int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);

                        for (int i = eerste_dag_weg; i < aantal_dagen_deze_maand + 1; i++)
                        {
                            labelNieuwRoosterDatum.Text = $"{teller++}      ";
                            labelNieuwRoosterDatum.Refresh();
                            ProgData.RegelAfwijking(ProgData.Get_Gebruiker_Nummer(persoon_gekozen._achternaam), i.ToString(), "", "Rooster Wissel Cancel", "Verhuizing", kleur_terug);
                        }

                        Restore_oude_afwijkingen(kleur_terug, false);

                        ProgData.igekozenjaar = jaar;
                        ProgData.igekozenmaand = maand;
                        ProgData.GekozenKleur = kleur;
                        EditPersoneel_Shown(this, null);

                        labelNieuwRoosterDatum.Text = "";
                        labelNieuwRoosterDatum.Refresh();

                        MessageBox.Show("Klaar met cancel verhuis");
                    }
                }
            }
            else
            {
                _ = MessageBox.Show("Allen met rechten > 99");
            }
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
            if (rechten > 49)
                recht.radioButton50.Checked = true;

            if (ProgData.RechtenHuidigeGebruiker == 25)
            {
                recht.checkBoxAllePloegen.Enabled = false;
                recht.radioButton0.Enabled = true;
                recht.radioButton25.Enabled = true;
                recht.radioButton50.Enabled = false;
            }
            if (ProgData.RechtenHuidigeGebruiker == 50)
            {
                recht.checkBoxAllePloegen.Enabled = false;
                recht.radioButton0.Enabled = true;
                recht.radioButton25.Enabled = true;
                recht.radioButton50.Enabled = true;
            }

            DialogResult dialog = recht.ShowDialog();
            if (dialog == DialogResult.OK)
            {
                try
                {
                    personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
                    persoon._rechten = int.Parse(recht.labelRechtenNivo.Text);
                    //ProgData.Save_LijstNamen();
                    ProgData.AlleMensen.Save();
                    ComboBoxFilter_SelectedIndexChanged(this, null);
                    // als rechten aangepast dan auto inlog verwijderen, zou zelfde persoon kunnen zijn.
                    // als ander auto inlog dan helaas ook even weg
                    string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string autoinlogfile = $"{directory}\\bezetting2.log";
                    File.Delete(autoinlogfile);
                }
                catch
                {
                    MessageBox.Show("Persoon niet gevonden, eerst opslaan");
                }
            }

        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Delete deze naam ?", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                // je kan je zelf niet verwijderen, dit daar als je rechten 50 hebt,
                // je na verwijderen bij andere wachten kan komen.
                if (textBoxPersNum.Text == ProgData.Huidige_Gebruiker_Personeel_nummer)
                {
                    MessageBox.Show("je kan je zelf niet verwijderen, ivm rechten");
                }
                else
                {
                    personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
                    ProgData.AlleMensen.LijstPersonen.Remove(persoon);
                    ProgData.AlleMensen.Save();
                    EditPersoneel_Shown(this, null);
                }
            }
        }

        private void ButtonVoegToe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBoxKleur.Text))
            {
                MessageBox.Show("Op welke wacht of kleur gaat persoon lopen?");
            }
            else
            {
                comboBoxKleur.Enabled = false;
                textBoxPersNum.Enabled = false;
                //textBoxAchterNaam.Enabled = false;
                buttonSave.Enabled = true;
                buttonDelete.Enabled = true;
                button1.Enabled = true;
                buttonCancelVerhuis.Enabled = true;
                buttonRechten.Enabled = true;

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

                a._kleur = comboBoxKleur.Text;

                if (ProgData.RechtenHuidigeGebruiker > 100)
                {
                    // direct edit ploeg rooster als admin
                    a._kleur = comboBoxKleur.Text;
                }

                a._funtie = textBoxFuntie.Text;
                a._werkgroep = textBoxWerkplek.Text;
                a._rechten = 0;
                ProgData.AlleMensen.LijstPersonen.Add(a);
                ProgData.AlleMensen.Save();

                ProgData.MaakNieuweCollegaInBezettingAan(textBoxAchterNaam.Text, comboBoxKleur.Text, ProgData.igekozenjaar, ProgData.igekozenmaand, 1);

                EditPersoneel_Shown(this, null);
            }
        }

        private void ButtonNieuw_Click(object sender, EventArgs e)
        {
            buttonSave.Enabled = false;
            buttonDelete.Enabled = false;
            button1.Enabled = false;
            buttonCancelVerhuis.Enabled = false;
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
            comboBoxKleur.Text = "";
            textBoxFuntie.Text = "";
            textBoxWerkplek.Text = "";
            LabelRoosterNieuw.Text = "";
            vuilwerk.Checked = false;
            comboBoxKleur.Enabled = true;
            textBoxPersNum.Enabled = true;
            textBoxAchterNaam.Enabled = true;
            buttonRechten.Enabled = false;
            MessageBox.Show("Na invoeren naam enz, druk op voeg toe.\nHierna kunt u ook rechten van deze persoon aangeven.");
            /*\nDoe daarna kleur verhuizing naar juiste kleur/plek.*/
        }

        // gevraagde afwijkingen/vakantie's op oude wacht, zodat ze kunnen verhuizen naar nieuwe
        private void Bewaar_oude_afwijkingen(int personeel_nummer, int eerste_dag, int maand, int jaar)
        {
            VeranderingenLijstTemp.Clear();

            int teller = 0;

            // sla de veranderingen op van persoon in tijdelijke lijst
            // save maand/jaar
            int backupjaar = ProgData.igekozenjaar;
            int backupmaand = ProgData.igekozenmaand;


            ProgData.igekozenjaar = jaar;
            ProgData.igekozenmaand = maand;
            int aantal_dagen_dezemaand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);

            ProgData.MaandData.Load(ProgData.GekozenKleur);

            var persnr = personeel_nummer.ToString(); // ProgData.Get_Gebruiker_Nummer(personeel_naam);

            foreach (Item a in ProgData.MaandData.MaandDataLijst)
            {
                if (a.personeel_nr_ == persnr)
                {
                    labelNieuwRoosterDatum.Text = $"{teller++}      ";
                    labelNieuwRoosterDatum.Refresh();

                    if (a.datum_.Day >= eerste_dag)
                    {
                        VeranderingenVerhuis verhuisje = new VeranderingenVerhuis
                        {
                            Maand_ = ProgData.igekozenmaand.ToString(),
                            Jaar_ = ProgData.igekozenjaar.ToString(),
                            Afwijking_ = a.afwijking_,
                            Datumafwijking_ = a.datum_,
                            Datuminvoer_ = a.ingevoerd_op_,
                            Invoerdoor_ = a.invoerdoor_,
                            Personeel_nr = a.personeel_nr_,
                            Rede_ = a.rede_,
                            Kleur_ = ProgData.GekozenKleur
                        };
                        VeranderingenLijstTemp.Add(verhuisje);
                    }
                }
            }

            // en nu volgende 5 maanden
            DateTime datum = new DateTime(jaar, maand, 1);
            for (int i = 0; i < 5; i++)
            {
                datum = datum.AddMonths(1);
                ProgData.igekozenjaar = datum.Year;
                ProgData.igekozenmaand = datum.Month;
                aantal_dagen_dezemaand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);

                var path = Path.GetFullPath($"{datum.Year}\\{maand}\\{ProgData.GekozenKleur}_Maand_Data.bin");
                if (File.Exists(path))
                {
                    ProgData.MaandData.Load(ProgData.GekozenKleur);

                    foreach (Item a in ProgData.MaandData.MaandDataLijst)
                    {
                        if (a.personeel_nr_ == persnr)
                        {

                            labelNieuwRoosterDatum.Text = $"{teller++}      ";
                            labelNieuwRoosterDatum.Refresh();

                            VeranderingenVerhuis verhuisje = new VeranderingenVerhuis
                            {
                                Maand_ = ProgData.igekozenmaand.ToString(),
                                Jaar_ = ProgData.igekozenjaar.ToString(),
                                Afwijking_ = a.afwijking_,
                                Datumafwijking_ = a.datum_,
                                Datuminvoer_ = a.ingevoerd_op_,
                                Invoerdoor_ = a.invoerdoor_,
                                Personeel_nr = a.personeel_nr_,
                                Rede_ = a.rede_,
                                Kleur_ = ProgData.GekozenKleur
                            };
                            VeranderingenLijstTemp.Add(verhuisje);
                        }
                    }
                }
            }

            // restore maand jaar
            ProgData.igekozenmaand = backupmaand;
            ProgData.igekozenjaar = backupjaar;

            // bewaar
            var naam = ProgData.Get_Gebruiker_Naam(personeel_nummer.ToString());
            string time = $"{DateTime.Now.Hour}-{DateTime.Now.Minute}";
            string filenaam = $"Bewaarde Afwijkingen-{naam}-{ eerste_dag}-{ maand}-{jaar}-{time}.bin";
            string pathloc = Path.GetFullPath($"{ProgData.igekozenjaar}\\{ProgData.igekozenmaand}\\");
            filenaam = pathloc + filenaam;
            try
            {
                using (Stream stream = File.Open(filenaam, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, VeranderingenLijstTemp);
                }
                MessageBox.Show("De oude afwijkingen opgeslagen!\n in geval van problemen kunt u deze importeren.");
            }
            catch { }
        }


        // zet bewaarde data in nieuwe ploeg
        private void Restore_oude_afwijkingen(string nieuwekleur, bool alles) // bool alles is alles terug zetten of alleen huidige maand
        {
            // save maand/jaar
            int backupjaar = ProgData.igekozenjaar;
            int backupmaand = ProgData.igekozenmaand;
            ProgData.GekozenKleur = nieuwekleur;
            string temp = labelNieuwRoosterDatum.Text;
            int teller = 0;
            DateTime volgend_maand = new DateTime(backupjaar, backupmaand, 1);
            volgend_maand = volgend_maand.AddMonths(1);
            foreach (VeranderingenVerhuis ver in VeranderingenLijstTemp)
            {
                labelNieuwRoosterDatum.Text = $"{teller++}      ";
                labelNieuwRoosterDatum.Refresh();

                if (alles || ver.Datumafwijking_ < volgend_maand)
                {

                    var naam = ProgData.Get_Gebruiker_Naam(ver.Personeel_nr);
                    // als al eerder verhuisd, dan staat tekst verhuis er al voor.
                    string invoerdoor;
                    if (ver.Invoerdoor_.Length > 8 && ver.Invoerdoor_.Substring(0, 8) != "Verhuizi")
                    {
                        invoerdoor = $"Verhuis: {ver.Invoerdoor_}";
                    }
                    else
                    {
                        invoerdoor = $"{ver.Invoerdoor_}";
                    }

                    if (ver.Afwijking_ == "X")
                        ver.Afwijking_ = "";
                    ProgData.RegelAfwijkingOpDatumEnKleur(ver.Datumafwijking_, nieuwekleur, ver.Personeel_nr, ver.Datumafwijking_.Day.ToString(), ver.Afwijking_, ver.Rede_, invoerdoor, false);
                }
            }
            labelNieuwRoosterDatum.Text = temp;
            // restore maand jaar
            ProgData.igekozenjaar = backupjaar;
            ProgData.igekozenmaand = backupmaand;
        }

        private void BewaarRechtenEnPasswoord(personeel persoon_gekozen)
        {
            // ook nieuwkleur en verhuis, als data wordt veranderd tijdens verhuis periode
            bewaar_rechten = persoon_gekozen._rechten;
            bewaar_passwoord = persoon_gekozen._passwoord;
            bewaar_kleur = persoon_gekozen._kleur;
            bewaar_nieuwkleur = persoon_gekozen._nieuwkleur;
            bewaar_verhuisdatum = persoon_gekozen._verhuisdatum;
        }

        private void ReturnRechtenEnPasswoord(personeel persoon_gekozen)
        {
            // ook nieuwkleur en verhuis, als data wordt veranderd tijdens verhuis periode
            persoon_gekozen._rechten = bewaar_rechten;
            persoon_gekozen._passwoord = bewaar_passwoord;
            persoon_gekozen._kleur = bewaar_kleur;
            persoon_gekozen._nieuwkleur = bewaar_nieuwkleur;
            persoon_gekozen._verhuisdatum = bewaar_verhuisdatum;

            // zeker zijn dat eea niet blijft hangen.
            bewaar_rechten = 0;
            bewaar_passwoord = ProgData.Scramble("verander_nu");
        }

        private void EditPersoneel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ProgData.Main.timerKill.Enabled)
            {
                try
                {
                    if (File.Exists("kill.ini"))
                        File.Delete("kill.ini");
                }
                catch (IOException)
                {
                    MessageBox.Show("Kon kill.ini niet verwijderen, verwijder deze met de hand.");
                }
                ProgData.Main.timerKill.Enabled = true;
            }
        }

        private bool KillAlleAndereGebruikers()
        {
            if (!ProgData.Main.timerKill.Enabled)
                return true;

            DialogResult dialogResult = MessageBox.Show("Doordat er veel administratie nodig is met verhuizing van personeel," +
                            "\nsluit ik alle andere gebruikers nu af!\nDit duurt 60 seconden. (Nadat u op Ja gedrukt heeft)", "Vraagje", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                // als kill.ini al bestaat hoeft dit niet
                ProgData.Main.timerKill.Enabled = false;
                try
                {
                    if (!File.Exists("kill.ini"))
                    {
                        using (File.Create("kill.ini"))
                        { };
                        for (int i = 0; i < 60; i++)
                        {
                            Thread.Sleep(1000);
                            labelNieuwRoosterDatum.Text = $" {60 - i}            ";
                            labelNieuwRoosterDatum.Refresh();
                        }
                        labelNieuwRoosterDatum.Text = "";
                    }
                }
                catch
                {
                    MessageBox.Show($"Kan kill.ini niet schrijven!");
                    ProgData.Main.timerKill.Enabled = true;
                }
            }
            return !ProgData.Main.timerKill.Enabled;
        }

        private void buttonUitlegCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Als u een persoon heeft verhuisd van rooster,\nKunt u dit cancelen tot op moment einde " +
                "maand van start verhuizing.\nBij Start nieuwe maand is wordt verhuizing defenitief.\n\n" +
                "Let op, veranderingen die tussen start en cancel verhuizing zijn gemaakt, verdwijnen");
        }

        private void buttonHelpImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bij verhuizing of bij Export kan/worden de ingevulde afwijkingen tov standaard rooster opgeslagen," +
                "Met import kan je deze weer in bezetting terug zetten. Gebruik je bv bij verhuizing naar nieuwe sectie, of " +
                "als verhuizing roetine fout is gegaan. Tevens kan je dagen voor de import nog bekijken.");
        }

        private void ShowVeranderingenVerhuis()
        {
            //BindingList<VeranderingenVerhuis> tabel = new BindingList<VeranderingenVerhuis>();
            //dgv.Dock = DockStyle.Fill;
            //dgv.Visible = true;
            //dgv.DataSource = tabel;
            //dgv.Update();
            //dgv.Refresh();
            
        }

        private void buttonCloseAdminPanel_Click(object sender, EventArgs e)
        {
            panelAdmin.Visible = false;
        }

        private void buttonExtraAdmin_Click(object sender, EventArgs e)
        {
            panelAdmin.Visible = true;
        }

        private void buttonExport_Click_1(object sender, EventArgs e)
        {
            if (textBoxPersNum.Text != "")
            {
                string start = $"1/{ProgData.igekozenmaand}/{ProgData.igekozenjaar}";
                MessageBox.Show("Bewaar van geselecteerd persoon de komende 6 maanden\n" +
                    "de afwijkingen. Start datum " + start);
                ProgData.GekozenKleur = ProgData.Get_Gebruiker_Kleur(textBoxPersNum.Text);
                Bewaar_oude_afwijkingen(int.Parse(textBoxPersNum.Text), 1, ProgData.igekozenmaand, ProgData.igekozenjaar);
            }
        }
    }
}