using Bezetting2.Data;
using Bezetting2.Invoer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;


namespace Bezetting2
{
    public partial class MainFormBezetting2 : Form
    {
        public const int kolom_breed = 49;
        public const int y_as_eerste_lijn = 150;
        public const int y_as_add_lijn = 4;
        private bool kill = false;

        private Color _Weekend = Color.LightSkyBlue;
        private Color _Feestdag = Color.LightSalmon;
        private Color _Huidigedag = Color.Lavender;
        private Color _MaandButton = Color.LightSkyBlue;
        private Color _Werkplek = Color.LightGray;
        private Color _MinimaalPersonen = Color.LightPink;

        InstellingenProgrammaForm instellingen_programma = new InstellingenProgrammaForm();

        public MainFormBezetting2()
        {
            InitializeComponent();
            ProgData._inlognaam = IngelogdPersNr;
            ProgData._toegangnivo = ToegangNivo;

            ProgData.RechtenHuidigeGebruiker = 0; // alleen lezen
            ProgData.Huidige_Gebruiker_Personeel_nummer = "Niemand Ingelogd";

            InstellingenProg.LeesProgrammaData();
        }

        // start programma
        private void MainFormBezetting2_Shown(object sender, EventArgs e)
        {
            if (File.Exists("kill.ini"))
                Close();

            ruilOverwerkToolStripMenuItem.Enabled = InstellingenProg._GebruikExtraRuil;
            snipperDagAanvraagToolStripMenuItem.Enabled = InstellingenProg._GebruikSnipper;

            ProgData.Main = this;
            DateTime nu = DateTime.Now;

            ProgData.ihuidigemaand = nu.Month;
            ProgData.igekozenmaand = nu.Month;

            ProgData.igekozenjaar = nu.Year;
            ProgData.ihuidigjaar = nu.Year;

            KleurMaandButton();

            // get huidige kleur op
            string dienst = "N";
            if (nu.Hour > 5 && nu.Hour < 14)
                dienst = "O";
            if (nu.Hour > 13 && nu.Hour < 22)
                dienst = "M";

            if (nu.Hour < 6 && dienst == "N")
                nu = nu.AddDays(-1);

            comboBoxKleurKeuze.Text = ProgData.MDatum.GetKleurDieWerkt(nu, dienst);

            //comboBoxKleurKeuze.Text = "Blauw"; // roept ook VulViewScherm(); aan.
            //VulViewScherm(); aanroep door comboBoxKleurKeuze.Text = "Blauw"

            if (ProgData.LeesLijnen())
                ZetLijnen();
        }

        private void importNamenOudeVersieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgData.personeel_lijst.Clear();
            openFileDialog.FileName = "personeel.csv";

            MessageBox.Show("Let op, alle oude personeel gaat weg, delete \ndaarna ook met de hand kleur bez en kleur mensen");

            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                TextBox temp = new TextBox();
                temp.Text = File.ReadAllText(openFileDialog.FileName);
                for (int i = 0; i < temp.Lines.Count() - 1; i++)
                {
                    string regel = temp.Lines[i];
                    string[] words;
                    try
                    {
                        words = regel.Split(';');
                        personeel p = new personeel();
                        p._persnummer = int.Parse(words[2]);
                        p._achternaam = words[0];
                        p._voornaam = words[1];
                        p._adres = words[3];
                        p._postcode = words[4];
                        p._woonplaats = words[5];
                        p._telthuis = words[6];
                        p._tel06prive = words[7];
                        p._telwerk = words[8];
                        p._emailwerk = words[9];
                        p._emailthuis = words[10];
                        p._adrescodewerk = words[11];
                        p._funtie = words[12];
                        p._kleur = words[13];
                        p._nieuwkleur = "";
                        p._verhuisdatum = DateTime.Now;
                        p._tel06werk = words[14];
                        p._werkgroep = words[15];
                        p._vuilwerk = words[16];
                        p._passwoord = "";
                        p._rechten = 0;
                        ProgData.personeel_lijst.Add(p);
                    }
                    catch
                    {
                        MessageBox.Show("omzetten personeel.csv gaat fout, verkeerde opbouw ?");
                    }
                }
                ProgData.Save_Namen_lijst();
            }
        }

        private void inloggenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InlogForm log = new InlogForm();
            log.ShowDialog();
            comboBoxKleurKeuze.Text = ProgData.GekozenKleur;
        }

        private void EditPersoneelClick(object sender, EventArgs e)
        {
            ProgData.Lees_Namen_lijst();
            EditPersoneel edit = new EditPersoneel();
            edit.ShowDialog();
            VulViewScherm(); // refresh
        }

        private void ToegangNivo_TextChanged(object sender, EventArgs e)
        {
            // rechten worden gewijzigd, pas dus div menu items aan
            importNamenOudeVersieToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
            editPersoneelToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 45;
            kleurLijnenToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 45;
            repareerPloegAfwijkingToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
            instellingenProgrammaToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
        }

        private void uitloggenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgData.RechtenHuidigeGebruiker = 0; // alleen lezen
            ProgData.Huidige_Gebruiker_Personeel_nummer = "Niemand Ingelogd";
        }

        private void comboBoxKleurKeuze_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProgData.CaptureMainScreen();
            ProgData.GekozenKleur = comboBoxKleurKeuze.Text;
            VulViewScherm();
        }

        public void VulViewScherm()
        {
            View.Columns.Clear();
            View.Items.Clear();

            int aantal_dagen = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);

            View.Columns.Add("", 140, HorizontalAlignment.Left); // namen

            //DateTime nu = DateTime.Now;

            for (int i = 1; i <= aantal_dagen; i++)
            {
                View.Columns.Add("", kolom_breed, HorizontalAlignment.Center);
            }

            // haal rooster
            string[] dagnr = new string[aantal_dagen + 1];
            string[] rooster = new string[aantal_dagen + 1];
            string[] dag = new string[aantal_dagen + 1];
            string[] weeknr = new string[aantal_dagen + 1];
            string[] lijn_regel = new string[aantal_dagen + 1];
            rooster[0] = "";
            dag[0] = "";
            weeknr[0] = "";
            dagnr[0] = "";
            CultureInfo cul = CultureInfo.CurrentCulture;
            DateTime datum = new DateTime();
            for (int i = 1; i < aantal_dagen + 1; i++)
            {
                dagnr[i] = i.ToString();
                datum = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, i);
                rooster[i] = ProgData.MDatum.GetDienst(ProgData.GekozenRooster, datum, ProgData.GekozenKleur);
                dag[i] = ProgData.MDatum.GetDag(datum);
                weeknr[i] = "";
                if (dag[i] == "W")
                {
                    weeknr[i - 1] = "WK";
                    int weekNum = cul.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                    weeknr[i] = weekNum.ToString();
                }
                lijn_regel[i] = "";
            }

            ListViewItem item_weeknr = new ListViewItem(weeknr);
            View.Items.Add(item_weeknr);
            ListViewItem item_dag = new ListViewItem(dag);
            View.Items.Add(item_dag);
            ListViewItem item_dagnr = new ListViewItem(dagnr);
            View.Items.Add(item_dagnr);
            ListViewItem item_rooster = new ListViewItem(rooster);
            View.Items.Add(item_rooster);
            ListViewItem item_lijnregel = new ListViewItem(lijn_regel);
            View.Items.Add(item_lijnregel);

            // vul namen en opgeslagen data als die bestond
            HaalBezetting();

            // kleur weekenden
            //int col = 20;
            //int row = 1;
            int aantal_rows = ProgData.kleur_personeel_lijst.Count();
            string dag_string;
            for (int col = 1; col < aantal_dagen; col++)
            {
                // lees dag
                dag_string = View.Items[1].SubItems[col].Text;
                if (dag_string == "Z") // zaterdag of zondag
                {
                    //for (int row = 0; row < aantal_rows + 4 + ProgData.werkgroep_personeel.Count; row++)
                    for (int row = 0; row < View.Items.Count - 1; row++)
                    {
                        //this is very Important
                        View.Items[row].UseItemStyleForSubItems = false;
                        // Now you can Change the Particular Cell Property of Style
                        View.Items[row].SubItems[col].BackColor = _Weekend;
                    }
                }
            }
            KleurFeestdagen();

            // kleur huidige dag
            if (ProgData.igekozenmaand == DateTime.Now.Month && ProgData.igekozenjaar == DateTime.Now.Year)
            {
                for (int i = 0; i < View.Items.Count - 1; i++)
                {
                    View.Items[i].UseItemStyleForSubItems = false;
                    // Now you can Change the Particular Cell Property of Style
                    View.Items[i].SubItems[DateTime.Now.Day].BackColor = _Huidigedag;

                }
            }

            // maand in beeld
            View.Items[2].UseItemStyleForSubItems = false;
            View.Items[2].SubItems[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);
            View.Items[2].SubItems[0].Text = ProgData.sgekozenmaand().ToUpper();

            LijnenWeg();
            if (ProgData.LeesLijnen())
                ZetLijnen();
        }

        private void ButtonJan_Click(object sender, EventArgs e)
        {
            ProgData.CaptureMainScreen();
            Button myButton = (Button)sender;
            ProgData.igekozenmaand = int.Parse(myButton.Tag.ToString());

            KleurMaandButton();
            VulViewScherm();
        }

        private void numericUpDownJaar_ValueChanged(object sender, EventArgs e)
        {
            ProgData.CaptureMainScreen();
            ProgData.igekozenjaar = (int)numericUpDownJaar.Value;
            VulViewScherm();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            VulViewScherm();
        }

        private void volgendeMaandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime t = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
            t = t.AddMonths(1);

            ProgData.igekozenmaand = t.Month;
            ProgData.igekozenjaar = t.Year;

            KleurMaandButton();
            VulViewScherm();
        }

        private void vorigeMaandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime t = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
            t = t.AddMonths(-1);

            ProgData.igekozenmaand = t.Month;
            ProgData.igekozenjaar = t.Year;

            KleurMaandButton();
            VulViewScherm();
        }

        public void KleurMaandButton()
        {
            int _taghuidig = ProgData.igekozenmaand;  //int.Parse(GevraagdeMaand.Tag.ToString());
            foreach (Button button in this.Controls.OfType<Button>())
            {
                if (button.Tag != null)
                {
                    int _tag = int.Parse(button.Tag.ToString());

                    if (_tag > 0 && _tag < 13 && (_taghuidig != _tag))
                    {
                        button.BackColor = Color.FromArgb(244, 244, 244);
                    }
                    if (_taghuidig == _tag)
                    {
                        button.BackColor = _MaandButton;
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        private void KleurFeestdagen()
        {
            // kleur feestdagen

            // 1 Jan nieuwjaarsdag
            // 27 april Koningsdag 
            // Kerstdag 25 en 26 december
            // bevrijdingsdag 5 mei (eens in 5 jaar)

            // Pasen
            // Hemelsvaartdag = 39 dagen na pasen.
            // Pinksteren

            int maand = ProgData.igekozenmaand;
            int aantal_dagen = DateTime.DaysInMonth((int)numericUpDownJaar.Value, maand);
            int aantal_rows = ProgData.kleur_personeel_lijst.Count();
            //string dag_string;

            DateTime pasen = EasterSunday((int)numericUpDownJaar.Value);
            DateTime pasen2 = pasen.AddDays(1);
            DateTime hemelsvaart = pasen.AddDays(39);
            DateTime pinksteren = hemelsvaart.AddDays(10); // 1ste pinsterdag
            DateTime pinksteren2 = pinksteren.AddDays(1); // 2ste pinsterdag

            // 5 mei elke 5 jaar (start op 2015)
            int modulo = (int)numericUpDownJaar.Value % 5; // als vijf mei is 0, dan vrij

            for (int col = 1; col < aantal_dagen + 1; col++)
            {
                if (
                    (maand == 1 && col == 1) ||
                    (maand == 4 && col == 27) ||
                    (maand == 12 && col == 25) ||
                    (maand == 12 && col == 26) ||
                    (maand == 5 && col == 5 && modulo == 0) ||
                    (maand == pasen.Month && col == pasen.Day) ||
                    (maand == pasen2.Month && col == pasen2.Day) ||
                    (maand == hemelsvaart.Month && col == hemelsvaart.Day) ||
                    (maand == pinksteren.Month && col == pinksteren.Day) ||
                    (maand == pinksteren2.Month && col == pinksteren2.Day)
                    )
                {
                    for (int row = 0; row < aantal_rows + 5 + ProgData.werkgroep_personeel.Count; row++)
                    {
                        //this is very Important
                        View.Items[row].UseItemStyleForSubItems = false;
                        // Now you can Change the Particular Cell Property of Style
                        View.Items[row].SubItems[col].BackColor = _Feestdag;
                    }
                }

            }
        }

        private void HaalBezetting()
        {
            // HaalBezetting Bestaat uit 3 delen
            // afhankelijk wat er gevraagd wordt en waarneer

            // check vooraf of juiste directory bestaat, maak deze anders aan.
            if (!Directory.Exists(ProgData.GetDir()))
                Directory.CreateDirectory(ProgData.GetDir());

            // er zijn nu 3 mogelijkheden, welke afhankelijk ik andere keuze's maak
            // 1 ) gevraagde maand is in verleden van huidige maand
            // 2 ) gevraagde maand is huidige maand
            // 3 ) gevraagde maand is in toekomst
            // ik bepaal dat in roetine WaarInTijd, als uitkomst 1 2 of 3 (zie hierboven)

            int waarintijd = ProgData.WaarInTijd();

            if (waarintijd == 1)
            {
                // verleden
                // hier alleen kijken
                // open oude ploeg bezetting
                // open wijzegingen en laat deze zien
                string Locatie = Path.GetFullPath(ProgData.GetDir() + "\\" + ProgData.GekozenKleur + "_bezetting.bin");
                if (!File.Exists(Locatie))
                {
                    MessageBox.Show("bezetting deze maand bestaat niet, kan dus niks laten zien");
                    ProgData.kleur_personeel_lijst.Clear();
                    ProgData.werkgroep_personeel.Clear();
                }
                else
                {
                    // 1) Haal Ploeg Bezetting
                    ProgData.LoadPloegNamenLijst();

                    // 2) Zet ploeg en werkplek op scherm
                    for (int i = 0; i < ProgData.werkgroep_personeel.Count; i++)
                    {
                        // eerst naam werkplek
                        string[] werkplek = new string[33];
                        werkplek[0] = ProgData.werkgroep_personeel[i];
                        ListViewItem item = new ListViewItem(werkplek);
                        View.Items.Add(item);
                        View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                        View.Items[View.Items.Count - 1].SubItems[0].BackColor = _Werkplek;
                        // zet in View
                        foreach (personeel a in ProgData.kleur_personeel_lijst)
                        {
                            string[] naamlijst = new string[33];
                            if (a._werkgroep == ProgData.werkgroep_personeel[i])
                            {
                                naamlijst[0] = a._achternaam;
                                ListViewItem item_naam = new ListViewItem(naamlijst);
                                View.Items.Add(item_naam);
                            }
                        }
                    }

                    // Vul bezetting op scherm
                    if (File.Exists(ProgData.Ploeg_Bezetting_Locatie()))
                    {
                        ProgData.LoadPloegBezettingLijst();
                        foreach (werkdag a in ProgData.Bezetting_Ploeg_Lijst)
                        {
                            if (a._afwijkingdienst != "")
                            {
                                for (int i = 0; i < View.Items.Count; i++) // alle namen/rows
                                {
                                    if (a._naam == View.Items[i].Text) // gevonden naam
                                    {
                                        View.Items[i].SubItems[a._dagnummer].Text = a._afwijkingdienst;
                                    }
                                }
                            }
                        }
                    }

                    // aantal bezetting regel
                    int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);
                    string[] maxlijst = new string[aantal_dagen_deze_maand + 1];
                    //maxlijst[0] = "Aantal";
                    ListViewItem item_max = new ListViewItem(maxlijst);
                    View.Items.Add(item_max);
                    int dag;
                    int aantal_mensen = ProgData.kleur_personeel_lijst.Count;
                    for (dag = 1; dag < aantal_dagen_deze_maand + 1; dag++) // aantal dagen
                    {
                        string wacht = View.Items[3].SubItems[dag].Text;
                        aantal_mensen = ProgData.kleur_personeel_lijst.Count;
                        for (int i = 4; i < View.Items.Count; i++) // alle namen/rows
                        {
                            if (View.Items[i].SubItems[dag].Text != "")
                                aantal_mensen--;
                            if (View.Items[i].SubItems[dag].Text == wacht + "+")
                                aantal_mensen++;
                        }
                        if (View.Items[3].SubItems[dag].Text != "")
                            View.Items[View.Items.Count - 1].SubItems[dag].Text = aantal_mensen.ToString();
                        if(aantal_mensen < InstellingenProg._MinimaalAantalPersonen)
                        {
                            View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                            View.Items[View.Items.Count - 1].SubItems[dag].BackColor = _MinimaalPersonen;
                        }
                    }

                    // extra diensten
                    DateTime dat = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
                    string dir = ProgData.GetDirectoryBezettingMaand(dat);
                    ProgData.LoadLooptExtraLijst(dir, ProgData.GekozenKleur);
                    if (ProgData.LooptExtra_lijst.Count > 0)
                    {
                        // extra diensten regel
                        string[] extralijst = new string[aantal_dagen_deze_maand + 1];
                        extralijst[0] = "Extra dienst";
                        ListViewItem item_extra = new ListViewItem(extralijst);
                        View.Items.Add(item_extra);
                        foreach (LooptExtraDienst ex in ProgData.LooptExtra_lijst)
                        {
                            int dagy = ex._datum.Day;
                            if (View.Items[View.Items.Count - 1].SubItems[dagy].Text == "")
                            {
                                View.Items[View.Items.Count - 1].SubItems[dagy].Text = "1";
                            }
                            else
                            {
                                int inhoud = int.Parse(View.Items[View.Items.Count - 1].SubItems[dagy].Text);
                                inhoud++;
                                View.Items[View.Items.Count - 1].SubItems[dagy].Text = inhoud.ToString();
                            }
                        }

                    }
                }
            }

            if (waarintijd == 2 || waarintijd == 3)
            {
                // huidig of toekomst

                ProgData.Lees_Namen_lijst();            // lees alle mensen in sectie , personeel_lijst
                ProgData.MaakPloegNamenLijst(ProgData.GekozenKleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
                ProgData.SavePloegNamenLijst();     // save ploegbezetting (de mensen)

                // maak bezettingafwijking.bin voor kleur als die niet bestaat
                // is lijst met werkdagen
                if (!File.Exists(ProgData.Ploeg_Bezetting_Locatie()))
                {
                    ProgData.MaakLegeBezetting(ProgData.sgekozenjaar(), ProgData.igekozenmaand.ToString(), ProgData.GekozenKleur); // in deze roetine wordt het ook opgeslagen
                }

                CheckEnDealVerhuizing();

                // 1) Haal Ploeg Bezetting
                ProgData.LoadPloegNamenLijst();

                // 2) Zet ploeg en werkplek op scherm
                for (int i = 0; i < ProgData.werkgroep_personeel.Count; i++)
                {
                    // eerst naam werkplek
                    string[] werkplek = new string[33];
                    werkplek[0] = ProgData.werkgroep_personeel[i];
                    ListViewItem item = new ListViewItem(werkplek);
                    View.Items.Add(item);
                    View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                    View.Items[View.Items.Count - 1].SubItems[0].BackColor = _Werkplek;
                    // zet in View
                    foreach (personeel a in ProgData.kleur_personeel_lijst)
                    {
                        string[] naamlijst = new string[33];
                        if (a._werkgroep == ProgData.werkgroep_personeel[i])
                        {
                            naamlijst[0] = a._achternaam;
                            ListViewItem item_naam = new ListViewItem(naamlijst);
                            View.Items.Add(item_naam);
                        }
                    }
                }

                // Vul bezetting op scherm
                if (File.Exists(ProgData.Ploeg_Bezetting_Locatie()))
                {
                    ProgData.LoadPloegBezettingLijst();
                    foreach (werkdag a in ProgData.Bezetting_Ploeg_Lijst)
                    {
                        if (a._afwijkingdienst != "")
                        {
                            for (int i = 0; i < View.Items.Count; i++) // alle namen/rows
                            {
                                if (a._naam == View.Items[i].Text) // gevonden naam
                                {
                                    View.Items[i].SubItems[a._dagnummer].Text = a._afwijkingdienst;
                                }
                            }
                        }
                    }
                }

                // aantal bezetting regel
                int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);
                string[] maxlijst = new string[aantal_dagen_deze_maand + 1];
                //maxlijst[0] = "Aantal";
                ListViewItem item_max = new ListViewItem(maxlijst);
                View.Items.Add(item_max);
                int dag;
                int aantal_mensen = ProgData.kleur_personeel_lijst.Count;

                for (dag = 1; dag < aantal_dagen_deze_maand + 1; dag++) // aantal dagen
                {
                    aantal_mensen = ProgData.kleur_personeel_lijst.Count;
                    string wacht = View.Items[3].SubItems[dag].Text;
                    for (int i = 4; i < View.Items.Count; i++) // alle namen/rows
                    {
                        if (View.Items[i].SubItems[dag].Text != "")
                            aantal_mensen--;
                        if (View.Items[i].SubItems[dag].Text == wacht + "+")
                            aantal_mensen++;
                    }
                    if (View.Items[3].SubItems[dag].Text != "")
                        View.Items[View.Items.Count - 1].SubItems[dag].Text = aantal_mensen.ToString();
                    if (aantal_mensen < InstellingenProg._MinimaalAantalPersonen)
                    {
                        View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                        View.Items[View.Items.Count - 1].SubItems[dag].BackColor = _MinimaalPersonen;
                    }
                }

                // extra diensten
                DateTime dat = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
                string dir = ProgData.GetDirectoryBezettingMaand(dat);
                ProgData.LoadLooptExtraLijst(dir, ProgData.GekozenKleur);
                if (ProgData.LooptExtra_lijst.Count > 0)
                {
                    // extra diensten regel
                    string[] extralijst = new string[aantal_dagen_deze_maand + 1];
                    extralijst[0] = "Extra dienst";
                    ListViewItem item_extra = new ListViewItem(extralijst);
                    View.Items.Add(item_extra);
                    foreach (LooptExtraDienst ex in ProgData.LooptExtra_lijst)
                    {
                        int dagy = ex._datum.Day;
                        if (View.Items[View.Items.Count - 1].SubItems[dagy].Text == "")
                        {
                            View.Items[View.Items.Count - 1].SubItems[dagy].Text = "1";
                        }
                        else
                        {
                            int inhoud = int.Parse(View.Items[View.Items.Count - 1].SubItems[dagy].Text);
                            inhoud++;
                            View.Items[View.Items.Count - 1].SubItems[dagy].Text = inhoud.ToString();
                        }
                    }
                }
            }

        }

        private void CheckEnDealVerhuizing()
        {
            ProgData.Lees_Namen_lijst();            // lees alle mensen in sectie , personeel_lijst
                                                    // check of er vorige maand mensen zijn verhuisd

            IEnumerable<personeel> persoon = from a in ProgData.personeel_lijst
                                             where (a._nieuwkleur != "")
                                             select a;

            foreach (personeel a in persoon)
            {
                // als verhuis datum-maand in verleden is tov huidige maand,
                // aanpassen.
                DateTime overgang = new DateTime(a._verhuisdatum.Year, a._verhuisdatum.Month, 1);
                overgang = overgang.AddMonths(1);
                DateTime eerste_maand = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 1);

                if (overgang <= eerste_maand) // roetine CheckEnDealVerhuizing wordt alleen aangeroepen 
                                              // bij waarintijd = 2
                {
                    string bewaar = ProgData.GekozenKleur;
                    if (a._nieuwkleur == null) // geen idee waar dit soms gebeurt
                        a._nieuwkleur = "";
                    if (a._nieuwkleur != "")
                        a._kleur = a._nieuwkleur;
                    a._nieuwkleur = "";
                    if (a._nieuwkleur == null) // geen idee waar dit soms gebeurt
                        a._nieuwkleur = "";
                    ProgData.Save_Namen_lijst();
                    ProgData.GekozenKleur = a._kleur;
                    ProgData.MaakPloegNamenLijst(a._kleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
                    ProgData.SavePloegNamenLijst();         // save ploegbezetting (de mensen)
                    ProgData.GekozenKleur = bewaar;
                }
            }
        }

        /// <summary>
        /// Bepaal datum pasen
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        private static DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        /// <summary>
        /// Geklikt op view scherm, open invoer form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void View_MouseClick(object sender, MouseEventArgs e)
        {
            //Point point = new Point(e.X, e.Y);

            if (ProgData.WaarInTijd() == 1)
            {
                MessageBox.Show("In verleden kunt u alleen kijken, niet meer aanpassen!");
            }
            else
            {
                if (((ProgData.RechtenHuidigeGebruiker > 24) && (ProgData.RechtenHuidigeGebruiker < 51) && (ProgData.Huidige_Gebruiker_Werkt_Op_Kleur == ProgData.GekozenKleur))
                    || ProgData.RechtenHuidigeGebruiker > 51)
                {
                    try
                    {
                        ListViewHitTestInfo info = View.HitTest(e.X, e.Y);
                        int row = info.Item.Index;
                        int col = info.Item.SubItems.IndexOf(info.SubItem);
                        string value = info.Item.SubItems[col].Text;
                        //MessageBox.Show(string.Format("R{0}:C{1} val '{2}'", row, col, value));

                        if (col > 0 && row < 4)
                        {
                            HistoryForm his = new HistoryForm();
                            his.checkBoxFilter.Checked = true;
                            his.comboBoxDag.Text = col.ToString();
                            his.ShowDialog();
                        }

                        if (col != 0 && View.Items[row].SubItems[0].BackColor != _Werkplek)
                        {

                            string gekozen_naam = info.Item.SubItems[0].Text;
                            string gekozen_datum = col.ToString();

                            personeel persoon = ProgData.kleur_personeel_lijst.First(a => a._achternaam == gekozen_naam);

                            if (e.Button == MouseButtons.Right)
                            {
                                // quick menu
                                QuickInvoerForm quick = new QuickInvoerForm();
                                quick.Location = new Point(e.Location.X + this.Location.X + 180, e.Location.Y + this.Location.Y + 60);
                                quick.ShowDialog();
                                if (quick.listBox1.SelectedIndex > -1)
                                {
                                    string afwijking = quick.listBox1.SelectedItem.ToString();
                                    if (afwijking == "Wis")
                                    {
                                        string eerste_2 = "";
                                        if (value.Length > 2)
                                            eerste_2 = value.Substring(0, 2);

                                        if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD")
                                        {
                                            MessageBox.Show("Wis een extra/ruil/verschoven dienst aan met linker muisknop");
                                            //// als ED-O of ED-M of ED-N aanpassing op andere kleur
                                            //// bepaal de kleur die dan loopt.

                                            //// get huidige kleur op
                                            //string dienst = value.Substring(3, 1);
                                            //DateTime _verzoekdag = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, col);
                                            //string gaat_lopen_op_kleur = ProgData.MDatum.GetKleurDieWerkt(_verzoekdag, dienst);
                                            //string dir = ProgData.GetDirectoryBezettingMaand(_verzoekdag);
                                            //ProgData.LoadLooptExtraLijst(dir, gaat_lopen_op_kleur);
                                            //LooptExtraDienst lp = ProgData.LooptExtra_lijst.First(a => (a._naam == gekozen_naam) && (a._datum == _verzoekdag));
                                            //ProgData.LooptExtra_lijst.Remove(lp);
                                            //ProgData.SaveLooptExtraLijst(dir, gaat_lopen_op_kleur);
                                        }
                                        else
                                        {
                                            ProgData.RegelAfwijking(gekozen_naam, gekozen_datum, "", "Verwijderd", ProgData.Huidige_Gebruiker_Personeel_nummer);
                                        }
                                    }
                                    else
                                    {
                                        ProgData.RegelAfwijking(gekozen_naam, gekozen_datum, afwijking, "", ProgData.Huidige_Gebruiker_Personeel_nummer);
                                    }
                                    VulViewScherm();
                                }
                            }
                            else
                            {
                                DagAfwijkingInvoerForm afw = new DagAfwijkingInvoerForm();
                                afw.labelNaam.Text = gekozen_naam;
                                afw.labelDatum.Text = gekozen_datum;
                                afw.labelMaand.Text = ProgData.sgekozenmaand();
                                afw.labelPersoneelnr.Text = persoon._persnummer.ToString();
                                afw.Text = ProgData.Huidige_Gebruiker_Personeel_nummer;
                                // voor ed-o ed-m en ed-n
                                afw._verzoekdag = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, col);
                                afw.ShowDialog();
                                VulViewScherm();
                            }
                        }
                    }
                    catch { }

                }
            }
        }

        private void buttonNu_Click(object sender, EventArgs e)
        {
            DateTime nu = DateTime.Now;

            ProgData.igekozenmaand = ProgData.ihuidigemaand;
            ProgData.igekozenjaar = ProgData.ihuidigjaar;

            KleurMaandButton();
            VulViewScherm();
        }

        private void wachtOverzichtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OverzichtWachtForm wacht = new OverzichtWachtForm();
            ProgData.GekozenKleur = comboBoxKleurKeuze.Text;
            wacht.labelKleur.Text = ProgData.GekozenKleur;
            wacht.ShowDialog();
        }

        private void View_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ListViewItem item = View.GetItemAt(e.X, e.Y);
                ListViewHitTestInfo info = View.HitTest(e.X, e.Y);

                toolStripStatusLabelInfo.Text = "";
                toolStripStatusRedeAfwijking.Text = "";

                if ((item != null) && (info?.SubItem?.Text != ""))
                {
                    int row = info.Item.Index;
                    int col = info.Item.SubItems.IndexOf(info.SubItem);
                    // extra dienst
                    if (row == View.Items.Count - 1)
                    {
                        if (info.SubItem.Text == "1") // 1 extra dienst
                        {
                            foreach (LooptExtraDienst ex in ProgData.LooptExtra_lijst)
                            {
                                if (col == ex._datum.Day)
                                    toolStripStatusLabelInfo.Text = ex._naam;
                            }
                        }
                        else
                        {
                            toolStripStatusLabelInfo.Text = "extra dienst 54321"; // nog te doen
                        }
                    }
                    if (col > 0 && row > 3 && row < View.Items.Count - 1)
                    {
                        toolStripStatusLabelInfo.Text = info.Item.Text + " " + info.SubItem.Text;
                        toolStripStatusRedeAfwijking.Text = GetRedenAfwijking(info.Item.Text, col);
                    }
                    if (col > 0 && row < 5) // feestdag info ?
                    {
                        if (View.Items[row].SubItems[col].BackColor == _Feestdag)
                        {
                            int maand = ProgData.igekozenmaand;
                            DateTime pasen = EasterSunday((int)numericUpDownJaar.Value);
                            DateTime pasen2 = pasen.AddDays(1);
                            DateTime hemelsvaart = pasen.AddDays(39);
                            DateTime pinksteren = hemelsvaart.AddDays(10); // 1ste pinsterdag
                            DateTime pinksteren2 = pinksteren.AddDays(1); // 2ste pinsterdag
                            int modulo = (int)numericUpDownJaar.Value % 5; // als vijf mei is 0, dan vrij
                            if (maand == 1 && col == 1) toolStripStatusRedeAfwijking.Text = "Nieuwsjaar dag";
                            if (maand == 4 && col == 27) toolStripStatusRedeAfwijking.Text = "Konings dag";
                            if (maand == 12 && col == 25) toolStripStatusRedeAfwijking.Text = "Eerste Kerstdag";
                            if (maand == 12 && col == 26) toolStripStatusRedeAfwijking.Text = "Tweede Kerstdag";
                            if (maand == 5 && col == 5 && modulo == 0) toolStripStatusRedeAfwijking.Text = "Bevrijdings dag";
                            if (maand == pasen.Month && col == pasen.Day) toolStripStatusRedeAfwijking.Text = "Eerste Paasdag";
                            if (maand == pasen2.Month && col == pasen2.Day) toolStripStatusRedeAfwijking.Text = "Tweede Paasdag";
                            if (maand == hemelsvaart.Month && col == hemelsvaart.Day) toolStripStatusRedeAfwijking.Text = "Hemelsvaart dag";
                            if (maand == pinksteren.Month && col == pinksteren.Day) toolStripStatusRedeAfwijking.Text = "Eerste Pinsterdag";
                            if (maand == pinksteren2.Month && col == pinksteren2.Day) toolStripStatusRedeAfwijking.Text = "Tweede Pinsterdag";
                        }
                    }
                }
            }
            catch { }
            
           
        }
        private string GetRedenAfwijking(string naam, int dag)
        {
            if (ProgData.Veranderingen_Lijst.Count < 1)
                ProgData.LoadVeranderingenPloegLijst();
            string sdag = dag.ToString();
            try
            {
                if (ProgData.Veranderingen_Lijst.Count > 0)
                {
                    veranderingen ver = ProgData.Veranderingen_Lijst.Last(a => (a._naam == naam) && (a._datumafwijking == sdag));
                    return ver._rede;
                }
            }
            catch { }
            return "";
        }

        private void kleurLijnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // zet kleur lijnen
            ZetKleurLijnen kleurLijnen = new ZetKleurLijnen();
            kleurLijnen.ShowDialog();
            VulViewScherm();
        }
        private void ZetLijnen()
        {
            // lijn 1
            if (bool.Parse(ProgData.Lijnen[0]))
            {
                label1.Visible = true;
                label1.Text = ProgData.Lijnen[4];
                panel5.Visible = true;
                panel1.Visible = true;
                int start = int.Parse(ProgData.Lijnen[8]);
                int stop = int.Parse(ProgData.Lijnen[12]);
                panel1.Location = new Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn);
                panel1.Size = new Size((stop - start + 1) * kolom_breed, 3);
            }
            // lijn 2
            if (bool.Parse(ProgData.Lijnen[1]))
            {
                label2.Visible = true;
                label2.Text = ProgData.Lijnen[5];
                panel6.Visible = true;
                panel2.Visible = true;
                int start = int.Parse(ProgData.Lijnen[9]);
                int stop = int.Parse(ProgData.Lijnen[13]);
                panel2.Location = new Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (1 * y_as_add_lijn));
                panel2.Size = new Size((stop - start + 1) * kolom_breed, 3);
            }
            // lijn 3
            if (bool.Parse(ProgData.Lijnen[2]))
            {
                label3.Visible = true;
                label3.Text = ProgData.Lijnen[6];
                panel7.Visible = true;
                panel3.Visible = true;
                int start = int.Parse(ProgData.Lijnen[10]);
                int stop = int.Parse(ProgData.Lijnen[14]);
                panel3.Location = new Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (2 * y_as_add_lijn));
                panel3.Size = new Size((stop - start + 1) * kolom_breed, 3);
            }
            // lijn 4
            if (bool.Parse(ProgData.Lijnen[3]))
            {
                label4.Visible = true;
                label4.Text = ProgData.Lijnen[7];
                panel8.Visible = true;
                panel4.Visible = true;
                int start = int.Parse(ProgData.Lijnen[11]);
                int stop = int.Parse(ProgData.Lijnen[15]);
                panel4.Location = new Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (3 * y_as_add_lijn));
                panel4.Size = new Size((stop - start + 1) * kolom_breed, 3);
            }
        }
        private void LijnenWeg()
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
        }
        private void repareerPloegAfwijkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test of blauw_bezetting.bin bestaat
            MessageBox.Show($"Ploeg bezetting {0} bestaat niet\nOf is corrupt, Kijken wat ik kan doen", ProgData.Ploeg_Bezetting_Locatie());
            //string Locatie = Path.GetFullPath(ProgData.GetDir() + "\\" + ProgData.GekozenKleur + "_afwijkingen.bin");
            if (File.Exists(ProgData.Ploeg_Veranderingen_Locatie()))
            {
                MessageBox.Show($"Ploeg veranderingen bestaat wel, repareren!");
                File.Delete(ProgData.Ploeg_Bezetting_Locatie());
                ProgData.MaakLegeBezetting(ProgData.sgekozenjaar(), ProgData.sgekozenmaand(), ProgData.GekozenKleur);
                ProgData.LoadVeranderingenPloegLijst();
                ProgData.LoadPloegBezettingLijst();
                foreach (veranderingen verander in ProgData.Veranderingen_Lijst)
                {
                    werkdag ver = ProgData.Bezetting_Ploeg_Lijst.First(a => (a._naam == verander._naam) && (a._dagnummer.ToString() == verander._datumafwijking));
                    ver._afwijkingdienst = verander._afwijking;
                }
                ProgData.SavePloegBezetting();
                VulViewScherm();
            }
        }
        private void timerKill_Tick(object sender, EventArgs e)
        {
            if (File.Exists("kill.ini"))
            {
                // als kill.ini bestaat sluit programma
                if (kill)
                {
                    Close();
                }
                else
                {
                    kill = true;
                    MessageBox.Show("programma wordt gesloten over 30 sec, er is een update, moment");
                }
            }
        }
        private void ruilOverwerkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RuilExtraForm rf = new RuilExtraForm();
            if (ProgData.Huidige_Gebruiker_Personeel_nummer != "Admin")
            {
                if (ProgData.Huidige_Gebruiker_Personeel_nummer != "Niemand Ingelogd")
                {
                    //int personeel_nr = int.Parse(ProgData.Huidige_Gebruiker_Personeel_nummer);
                    //personeel persoon = ProgData.personeel_lijst.First(a => a._persnummer == personeel_nr);
                    rf.labelNaam.Text = ProgData.Huidige_Gebruiker_Naam();
                }
                else
                {
                    rf.labelNaam.Text = "Niet ingelogd";
                }
            }
            else
            {
                rf.labelNaam.Text = "Admin";
            }
            rf.buttonVraagAan.Enabled = ProgData.RechtenHuidigeGebruiker > 0;
            rf.ShowDialog();
        }
        private void snipperDagAanvraagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SnipperAanvraagForm snip = new SnipperAanvraagForm();
            snip.labelNaam.Text = ProgData.Huidige_Gebruiker_Personeel_nummer;
            snip.labelNaamFull.Text = ProgData.Huidige_Gebruiker_Naam();
            snip.ShowDialog();
        }
        private void panel8_DoubleClick(object sender, EventArgs e)
        {
            // juiste inlog
            ProgData.Huidige_Gebruiker_Personeel_nummer = "590588";
            ProgData.RechtenHuidigeGebruiker = 100;
            comboBoxKleurKeuze.SelectedItem = "Blauw";
            ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = "Blauw";
        }
        private void instellingenProgrammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instellingen_programma.ShowDialog();
            MainFormBezetting2_Shown(this, null);
        }
        private void MainFormBezetting2_FormClosing(object sender, FormClosingEventArgs e)
        {
            ProgData.CaptureMainScreen();
        }
    }
}


