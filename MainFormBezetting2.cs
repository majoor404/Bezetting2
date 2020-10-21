using Bezetting2.Data;
using Bezetting2.Invoer;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;


namespace Bezetting2
{
    public partial class MainFormBezetting2 : Form
    {
        public const int kolom_breed = 49;
        public const int y_as_eerste_lijn = 150;
        public const int y_as_add_lijn = 4;
        private bool kill = false;
        private bool WindowUpdateViewScreen = true;



        private Color _Weekend = Color.LightSkyBlue;
        private Color _Feestdag = Color.LightSalmon;
        private Color _Huidigedag = Color.Lavender;
        private Color _MaandButton = Color.LightSkyBlue;
        private Color _Werkplek = Color.LightGray;
        private Color _MinimaalPersonen = Color.LightPink;

        public List<ClassTelPlekGewerkt> ListClassTelPlekGewerkt = new List<ClassTelPlekGewerkt>();
        public List<string> ListTelNamen = new List<string>();
        public List<string> ListTelWerkPlek = new List<string>();
        public List<ClassTelVuilwerk> ListClassTelVuilwerk = new List<ClassTelVuilwerk>();
        public List<string> ListVuilwerkData = new List<string>();
        string[] deze_maand_overzicht_persoon = new string[33];
        DateTime dag_gekozen;
        public List<ClassTelAfwijkingen> ListClassTelAfwijkingen = new List<ClassTelAfwijkingen>();

        List<string> ListTelNietMeeNamen = new List<string>();

        public class ClassTelAfwijkingen
        {
            public ClassTelAfwijkingen(string afwijking,int aantal,bool toekomst)
            {
                _Afwijking = afwijking;
                _Aantal = aantal;
                _Toekomst = toekomst;
            }
            public string _Afwijking { get; set; }
            public int _Aantal { get; set; }
            public bool _Toekomst { get; set; }

        }
        public class ClassTelPlekGewerkt
        {

            public ClassTelPlekGewerkt(string naam, string plek)
            {
                _NaamTelPlek = naam;
                _PlekTelPlek = plek;
                _AantalTelPlek = 1;
            }
            public string _NaamTelPlek { get; set; }
            public string _PlekTelPlek { get; set; }
            public int _AantalTelPlek { get; set; }
        }

        public class ClassTelVuilwerk
        {

            public ClassTelVuilwerk(string naam, string dag)
            {
                _NaamTelVuil = naam;
                _DagTelVuil = dag;
            }
            public string _NaamTelVuil { get; set; }
            public string _DagTelVuil { get; set; }
        }

        InstellingenProgrammaForm instellingen_programma = new InstellingenProgrammaForm();

        public MainFormBezetting2()
        {
            InitializeComponent();
            ProgData._inlognaam = IngelogdPersNr;
            ProgData._toegangnivo = ToegangNivo;

            ProgData.RechtenHuidigeGebruiker = 0; // alleen lezen
            ProgData.Huidige_Gebruiker_Personeel_nummer = "Niemand Ingelogd";
            WindowUpdateViewScreen = true;

            InstellingenProg.LeesProgrammaData();
        }

        // start programma
        private void MainFormBezetting2_Shown(object sender, EventArgs e)
        {
            if (File.Exists("kill.ini"))
                Close();

            ruilOverwerkToolStripMenuItem.Visible = InstellingenProg._GebruikExtraRuil;
            snipperDagAanvraagToolStripMenuItem.Visible = InstellingenProg._GebruikSnipper;

            ProgData.Main = this;
            DateTime nu = DateTime.Now;

            ProgData.ihuidigemaand = nu.Month;
            ProgData.igekozenmaand = nu.Month;

            ProgData.igekozenjaar = nu.Year;
            ProgData.ihuidigjaar = nu.Year;

            ProgData.backup_zipnaam = "Backup\\" + nu.ToShortDateString() + ".zip";

            Random rnd = new Random();
            ProgData.backup_time = rnd.Next(60);

            KleurMaandButton();

            if (InstellingenProg._Rooster == "5pl")
            {
                // get huidige kleur op
                string dienst = "N";
                if (nu.Hour > 5 && nu.Hour < 14)
                    dienst = "O";
                if (nu.Hour > 13 && nu.Hour < 22)
                    dienst = "M";

                if (nu.Hour < 6 && dienst == "N")
                    nu = nu.AddDays(-1);

                comboBoxKleurKeuze.Text = ProgData.MDatum.GetKleurDieWerkt(nu, dienst);
            }
            else
            {
                comboBoxKleurKeuze.Text = "DD";
            }

            //comboBoxKleurKeuze.Text = "Blauw"; // roept ook VulViewScherm(); aan.
            //VulViewScherm(); aanroep door comboBoxKleurKeuze.Text = "Blauw"

            if (ProgData.LeesLijnen())
                ZetLijnen();
        }

        private void importNamenOudeVersieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgData.ListPersoneel.Clear();
            //openFileDialog.Filter = "(*.Bez)|*.Bez";
            MessageBox.Show("Delete eerst directory's van toekomst");
            MessageBox.Show("Let op, alle oude personeel gaat weg, open Bezetting5ploegen....Bez");

            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                OpenDataBase_en_Voer_Oude_Namen_In(openFileDialog.FileName);
                ProgData.Save_Namen_lijst();
            }
            MessageBox.Show("Klaar, druk op refresh");
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
            importOudeVeranderDataOudeVersieToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
            nietMeeTelLijstToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;

            vuilwerkToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
            tellingWaarGewerktToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
            namenAdressenEMailToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
            afwijkingenTovRoosterIngelogdPersoonToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 0;
            afwijkingTovRoosterPloegToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
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
            if (WindowUpdateViewScreen)
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
                    rooster[i] = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), datum, ProgData.GekozenKleur);
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
                int aantal_rows = ProgData.ListPersoneelKleur.Count();
                string dag_string;
                for (int col = 1; col < aantal_dagen + 1; col++)
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
                            if (View.Items[row].SubItems[col].BackColor != _Werkplek)
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
                        if (View.Items[i].SubItems[DateTime.Now.Day].BackColor != _Werkplek)
                            View.Items[i].SubItems[DateTime.Now.Day].BackColor = _Huidigedag;
                    }
                }

                // maand in beeld
                View.Items[2].UseItemStyleForSubItems = false;
                View.Items[2].SubItems[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);
                View.Items[2].SubItems[0].Text = ProgData.sgekozenmaand().ToUpper();

                // Jaar in beeld
                View.Items[3].UseItemStyleForSubItems = false;
                View.Items[3].SubItems[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);
                View.Items[3].SubItems[0].Text = ProgData.sgekozenjaar().ToUpper();

                LijnenWeg();
                if (ProgData.LeesLijnen())
                    ZetLijnen();

                labelDebug.Text = "";
            }
        }

        private void ButtonJan_Click(object sender, EventArgs e)
        {
            ProgData.CaptureMainScreen();
            System.Windows.Forms.Button myButton = (System.Windows.Forms.Button)sender;
            ProgData.igekozenmaand = int.Parse(myButton.Tag.ToString());

            KleurMaandButton();
            VulViewScherm();
        }

        private void numericUpDownJaar_ValueChanged(object sender, EventArgs e)
        {
            ProgData.igekozenjaar = (int)numericUpDownJaar.Value;
            ProgData.CaptureMainScreen();
            VulViewScherm();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            WindowUpdateViewScreen = true;
            //ProgData.ReloadSpeed1 = "";
            //ProgData.ReloadSpeed2 = "";
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
            foreach (System.Windows.Forms.Button button in this.Controls.OfType<System.Windows.Forms.Button>())
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
            int aantal_rows = ProgData.ListPersoneelKleur.Count();
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
                    for (int row = 0; row < aantal_rows + 5 + ProgData.ListWerkgroepPersoneel.Count; row++)
                    {
                        //this is very Important
                        View.Items[row].UseItemStyleForSubItems = false;
                        // Now you can Change the Particular Cell Property of Style
                        if (View.Items[row].SubItems[col].BackColor != _Werkplek)
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
                    ProgData.ListPersoneelKleur.Clear();
                    ProgData.ListWerkgroepPersoneel.Clear();
                }
                else
                {
                    // 1) Haal Ploeg Bezetting
                    ProgData.LoadPloegNamenLijst();

                    // 2) Zet ploeg en werkplek op scherm
                    for (int i = 0; i < ProgData.ListWerkgroepPersoneel.Count; i++)
                    {
                        // eerst naam werkplek
                        string[] werkplek = new string[33];
                        werkplek[0] = ProgData.ListWerkgroepPersoneel[i];
                        ListViewItem item = new ListViewItem(werkplek);
                        View.Items.Add(item);
                        View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                        for (int grijzebalk_werkplek = 0; grijzebalk_werkplek < 33; grijzebalk_werkplek++)
                        {
                            View.Items[View.Items.Count - 1].SubItems[grijzebalk_werkplek].BackColor = _Werkplek;
                        }
                        // zet in View
                        foreach (personeel a in ProgData.ListPersoneelKleur)
                        {
                            string[] naamlijst = new string[33];
                            if (a._werkgroep == ProgData.ListWerkgroepPersoneel[i])
                            {
                                naamlijst[0] = a._achternaam;
                                ListViewItem item_naam = new ListViewItem(naamlijst);
                                View.Items.Add(item_naam);
                            }
                        }
                    }

                    // Vul bezetting op scherm
                    if (File.Exists(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)))
                    {
                        ProgData.LoadPloegBezetting(ProgData.GekozenKleur);
                        foreach (werkdag a in ProgData.ListWerkdagPloeg)
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
                    int aantal_mensen = ProgData.ListPersoneelKleur.Count;

                    List<string> TelNietMeeNamen = new List<string>();
                    string locatie = @"telnietmee.ini";
                    try
                    {
                        TelNietMeeNamen = File.ReadAllLines(locatie).ToList();
                    }
                    catch { }

                    for (dag = 1; dag < aantal_dagen_deze_maand + 1; dag++) // aantal dagen
                    {
                        string wacht = View.Items[3].SubItems[dag].Text;
                        aantal_mensen = ProgData.ListPersoneelKleur.Count;
                        for (int i = 4; i < View.Items.Count; i++) // alle namen/rows
                        {
                            if (View.Items[i].SubItems[dag].Text != "")
                                aantal_mensen--;
                            if (TelNietMeeNamen.Contains(View.Items[i].SubItems[dag].Text))
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
                    if (ProgData.ListLooptExtra.Count > 0)
                    {
                        // extra diensten regel
                        string[] extralijst = new string[aantal_dagen_deze_maand + 1];
                        extralijst[0] = "Extra dienst";
                        ListViewItem item_extra = new ListViewItem(extralijst);
                        View.Items.Add(item_extra);
                        foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
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
                //if (!File.Exists(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)))
                if (!File.Exists(ProgData.Ploeg_Veranderingen_Locatie()))
                {
                    ProgData.MaakLegeBezetting(ProgData.sgekozenjaar(), ProgData.igekozenmaand.ToString(), ProgData.GekozenKleur); // in deze roetine wordt het ook opgeslagen

                }

                CheckEnDealVerhuizing();

                // 1) Haal Ploeg Bezetting
                ProgData.LoadPloegNamenLijst();

                // 2) Zet ploeg en werkplek op scherm
                for (int i = 0; i < ProgData.ListWerkgroepPersoneel.Count; i++)
                {
                    // eerst naam werkplek
                    string[] werkplek = new string[33];
                    werkplek[0] = ProgData.ListWerkgroepPersoneel[i];
                    ListViewItem item = new ListViewItem(werkplek);
                    View.Items.Add(item);
                    View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                    for (int grijzebalk_werkplek = 0; grijzebalk_werkplek < 33; grijzebalk_werkplek++)
                    {
                        View.Items[View.Items.Count - 1].SubItems[grijzebalk_werkplek].BackColor = _Werkplek;
                    }

                    // zet in View
                    foreach (personeel a in ProgData.ListPersoneelKleur)
                    {
                        string[] naamlijst = new string[33];
                        if (a._werkgroep == ProgData.ListWerkgroepPersoneel[i])
                        {
                            naamlijst[0] = a._achternaam;
                            ListViewItem item_naam = new ListViewItem(naamlijst);
                            View.Items.Add(item_naam);
                        }
                    }
                }

                // Vul bezetting op scherm
                if (File.Exists(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)))
                {
                    ProgData.LoadPloegBezetting(ProgData.GekozenKleur);
                    foreach (werkdag a in ProgData.ListWerkdagPloeg)
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
                int aantal_mensen = ProgData.ListPersoneelKleur.Count;

                List<string> TelNietMeeNamen = new List<string>();
                string locatie = @"telnietmee.ini";
                try
                {
                    TelNietMeeNamen = File.ReadAllLines(locatie).ToList();
                }
                catch
                {
                    MessageBox.Show("telnietmee.ini niet gevonden");
                }

                for (dag = 1; dag < aantal_dagen_deze_maand + 1; dag++) // aantal dagen
                {
                    aantal_mensen = ProgData.ListPersoneelKleur.Count;
                    string wacht = View.Items[3].SubItems[dag].Text;
                    for (int i = 4; i < View.Items.Count; i++) // alle namen/rows
                    {
                        if (View.Items[i].SubItems[dag].Text != "")
                            aantal_mensen--;
                        if (TelNietMeeNamen.Contains(View.Items[i].SubItems[dag].Text))
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
                if (ProgData.ListLooptExtra.Count > 0)
                {
                    // extra diensten regel
                    string[] extralijst = new string[aantal_dagen_deze_maand + 1];
                    extralijst[0] = "Extra dienst";
                    ListViewItem item_extra = new ListViewItem(extralijst);
                    View.Items.Add(item_extra);
                    foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
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
            labelDebug.Text = "";
        }

        private void CheckEnDealVerhuizing()
        {
            ProgData.Lees_Namen_lijst();            // lees alle mensen in sectie , personeel_lijst
                                                    // check of er vorige maand mensen zijn verhuisd

            IEnumerable<personeel> persoon = from a in ProgData.ListPersoneel
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
                            his.comboBoxDag.Text = col.ToString();
                            his.ShowDialog();
                        }

                        if (col != 0 && View.Items[row].SubItems[0].BackColor != _Werkplek)
                        {

                            string gekozen_naam = info.Item.SubItems[0].Text;
                            string gekozen_datum = col.ToString();

                            personeel persoon = ProgData.ListPersoneelKleur.First(a => a._achternaam == gekozen_naam);

                            if (e.Button == MouseButtons.Right)
                            {
                                // quick menu
                                QuickInvoerForm quick = new QuickInvoerForm();
                                quick.Location = new System.Drawing.Point(e.Location.X + this.Location.X + 180, e.Location.Y + this.Location.Y + 60);
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
                                            ProgData.RegelAfwijking(gekozen_naam, gekozen_datum, "", "Verwijderd", ProgData.Huidige_Gebruiker_Personeel_nummer, ProgData.GekozenKleur);
                                        }
                                    }
                                    else
                                    {
                                        ProgData.RegelAfwijking(gekozen_naam, gekozen_datum, afwijking, "", ProgData.Huidige_Gebruiker_Personeel_nummer, ProgData.GekozenKleur);
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
            buttonNu_Click(this, null);
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
                            foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
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
            if (ProgData.ListVeranderingen.Count < 1)
                ProgData.LoadVeranderingenPloeg();
            string sdag = dag.ToString();
            try
            {
                if (ProgData.ListVeranderingen.Count > 0)
                {
                    veranderingen ver = ProgData.ListVeranderingen.Last(a => (a._naam == naam) && (a._datumafwijking == sdag));
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
                panel1.Location = new System.Drawing.Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn);
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
                panel2.Location = new System.Drawing.Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (1 * y_as_add_lijn));
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
                panel3.Location = new System.Drawing.Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (2 * y_as_add_lijn));
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
                panel4.Location = new System.Drawing.Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (3 * y_as_add_lijn));
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
            MessageBox.Show($"Ploeg bezetting {0} bestaat niet\nOf is corrupt, Kijken wat ik kan doen", ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur));
            //string Locatie = Path.GetFullPath(ProgData.GetDir() + "\\" + ProgData.GekozenKleur + "_afwijkingen.bin");
            if (File.Exists(ProgData.Ploeg_Veranderingen_Locatie()))
            {
                MessageBox.Show($"Ploeg veranderingen bestaat wel, repareren!");
                File.Delete(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur));
                ProgData.MaakLegeBezetting(ProgData.sgekozenjaar(), ProgData.sgekozenmaand(), ProgData.GekozenKleur);
                ProgData.LoadVeranderingenPloeg();
                ProgData.LoadPloegBezetting(ProgData.GekozenKleur);
                foreach (veranderingen verander in ProgData.ListVeranderingen)
                {
                    werkdag ver = ProgData.ListWerkdagPloeg.First(a => (a._naam == verander._naam) && (a._dagnummer.ToString() == verander._datumafwijking));
                    ver._afwijkingdienst = verander._afwijking;
                }
                ProgData.SavePloegBezetting(ProgData.GekozenKleur);
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

            if (DateTime.Now.Hour == 12 && ProgData.backup_time == DateTime.Now.Minute)
            {
                if (!File.Exists(ProgData.backup_zipnaam))
                {
                    timerKill.Enabled = false;
                    labelDebug.Text = "Dagelijkse Backup, moment.....";
                    ProgData.Backup();
                    timerKill.Enabled = true;
                    labelDebug.Text = "Dagelijkse Backup gelukt";
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

        private void importOudeVeranderDataOudeVersieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            //openFileDialog.Filter = "(*.Bez)|*.Bez";
            MessageBox.Show("Open oude data bez file. (Wijz...Bez)");
            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                MessageBox.Show("Dit gaat tijdje duren, geduld..... (10 min)\nAl ingevulde data wordt overschreven!");
                ProgData.Disable_error_Meldingen = true;
                ProgData.Lees_Namen_lijst();
                OpenDataBase_en_Voer_oude_data_in_Bezetting(openFileDialog.FileName);
                MessageBox.Show("Klaar met invoer, start programma opnieuw op.");
                ProgData.GekozenKleur = "Blauw";
                buttonNu_Click(this, null);
                ProgData.Disable_error_Meldingen = false;
            }
        }

        private void OpenDataBase_en_Voer_oude_data_in_Bezetting(string file)
        {
            WindowUpdateViewScreen = false;
            using (OleDbConnection connection =
                new OleDbConnection($"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \"{file}\"; Jet OLEDB:Database Password = fcl721"))
            {

                object[] meta = new object[12];
                bool read;
                int teller = 0;


                OleDbCommand command = new OleDbCommand("select * from Wijzeging", connection);

                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();
                WindowUpdateViewScreen = false;
                if (reader.Read() == true)
                {
                    DateTime nu = DateTime.Now;
                    nu = nu.AddMonths(-1);
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        int NumberOfColums = reader.GetValues(meta);

                        labelDebug.Text = $"{teller++.ToString()}";
                        labelDebug.Refresh();


                        //Console.Write("{0} ", meta[2].ToString()); // pers nummer persoon
                        //Console.Write("{0} ", meta[6].ToString()); // datum invoer
                        //Console.Write("{0} ", meta[7].ToString()); // datum afwijking
                        //Console.Write("{0} ", meta[5].ToString()); // afwijking
                        //Console.Write("{0} ", meta[9].ToString()); // personeel nummer invoerder
                        //Console.Write("{0} ", meta[11].ToString()); // rede

                        string[] datum = meta[7].ToString().Split('-');
                        datum[2] = datum[2].Substring(0, 4);

                        DateTime gekozen = new DateTime(int.Parse(datum[2]), int.Parse(datum[1]), int.Parse(datum[0]));

                            if ((gekozen > nu) && (ProgData.Bestaat_Gebruiker(meta[2].ToString())))
                            //if (ProgData.Bestaat_Gebruiker(meta[2].ToString()))
                            {
                            try
                            {
                                string kleur = ProgData.Get_Gebruiker_Kleur(meta[2].ToString());

                                // in oude programma is afwijking soms gelijk aan orginele dienst
                                // die hoef ik in te voeren
                                if (meta[5].ToString() == "O" || meta[5].ToString() == "M" || meta[5].ToString() == "N")
                                {
                                    if (ProgData.MDatum.GetDienst("5PL", gekozen, kleur) == meta[5].ToString())
                                    {
                                        kleur = "niet invoeren";
                                    }
                                }

                                if (kleur == "Blauw" || kleur == "Geel" || kleur == "Groen" || kleur == "Rood" || kleur == "Wit" || kleur == "DD")
                                {
                                    string naam = ProgData.Get_Gebruiker_Naam(meta[2].ToString());
                                    string invoer_naam = ProgData.Get_Gebruiker_Naam(meta[9].ToString());
                                    ProgData.RegelAfwijkingOpDatumEnKleur(gekozen, kleur, naam, datum[0], meta[5].ToString(), meta[11].ToString(), "Import " + invoer_naam);
                                }

                            }
                            catch { }
                        }
                        read = reader.Read();
                    } while (read == true);
                }
                reader.Close();
            }
            WindowUpdateViewScreen = true;
        }

        private void OpenDataBase_en_Voer_Oude_Namen_In(string file)
        {
            using (OleDbConnection connection =
                            new OleDbConnection($"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \"{file}\"; Jet OLEDB:Database Password = fcl721"))
            {

                object[] meta = new object[20];
                bool read;

                OleDbCommand command = new OleDbCommand("select * from Adresen", connection);

                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();

                if (reader.Read() == true)
                {
                    DateTime nu = DateTime.Now;
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        int NumberOfColums = reader.GetValues(meta);

                        //Console.Write("{0} ", meta[2].ToString()); // pers nummer persoon
                        //Console.Write("{0} ", meta[6].ToString()); // datum invoer
                        //Console.Write("{0} ", meta[7].ToString()); // datum afwijking
                        //Console.Write("{0} ", meta[5].ToString()); // afwijking
                        //Console.Write("{0} ", meta[9].ToString()); // personeel nummer invoerder
                        //Console.Write("{0} ", meta[11].ToString()); // rede

                        try
                        {
                            personeel p = new personeel();
                            p._persnummer = int.Parse(meta[0].ToString());
                            p._achternaam = meta[1].ToString();
                            p._voornaam = meta[2].ToString();
                            p._adres = meta[3].ToString();
                            p._postcode = meta[4].ToString();
                            p._woonplaats = meta[5].ToString();
                            p._telthuis = meta[6].ToString();
                            p._tel06prive = meta[7].ToString();
                            p._telwerk = meta[8].ToString();
                            p._emailwerk = meta[9].ToString();
                            p._emailthuis = meta[10].ToString();
                            p._adrescodewerk = meta[11].ToString();
                            p._funtie = meta[12].ToString();
                            p._kleur = meta[13].ToString();
                            p._nieuwkleur = "";
                            p._verhuisdatum = DateTime.Now;
                            p._tel06werk = meta[14].ToString();
                            p._werkgroep = meta[15].ToString();
                            p._vuilwerk = meta[16].ToString();
                            p._passwoord = "";
                            p._rechten = 0;
                            p._reserve1 = "";
                            p._reserve2 = "";
                            p._reserve3 = "";
                            p._reserve4 = "";
                            p._reserve5 = "";
                            ProgData.ListPersoneel.Add(p);
                        }
                        catch { }
                        read = reader.Read();
                    } while (read == true);
                }
                reader.Close();
            }
            ProgData.Save_Namen_lijst();
        }

        private void tellingWaarGewerktToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListTelNamen.Clear();
                ListTelWerkPlek.Clear();
                ListClassTelPlekGewerkt.Clear();
                // bewaar huidige maand en kleur
                int bewaar_maand = ProgData.igekozenmaand;

                for (int i = 1; i < 13; i++)    // maanden
                {
                    ProgData.igekozenmaand = i;
                    if (File.Exists(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)))
                    {
                        ProgData.LoadPloegBezetting(ProgData.GekozenKleur);
                        ProgData.LoadPloegNamenLijst();

                        foreach (personeel a in ProgData.ListPersoneelKleur)
                        {
                            if (!ListTelNamen.Contains(a._achternaam))
                                ListTelNamen.Add(a._achternaam);
                        }

                        foreach (werkdag a in ProgData.ListWerkdagPloeg)
                        {
                            if (!ListTelWerkPlek.Contains(a._werkplek) && a._werkplek != "")
                                ListTelWerkPlek.Add(a._werkplek);

                            if (a._werkplek != "")
                            {
                                ClassTelPlekGewerkt tel = new ClassTelPlekGewerkt(a._naam, a._werkplek);
                                try
                                {
                                    ClassTelPlekGewerkt gevonden = ListClassTelPlekGewerkt.First(b => b._NaamTelPlek == a._naam && b._PlekTelPlek == a._werkplek);
                                    gevonden._AantalTelPlek++;
                                }
                                catch
                                {
                                    ListClassTelPlekGewerkt.Add(tel);
                                }
                            }
                        }
                    }
                }
                ZetGevondenDataTellingWaarGewerktInExcel();
                ProgData.igekozenmaand = bewaar_maand;
                ProgData.LoadPloegBezetting(ProgData.GekozenKleur);
                ProgData.LoadPloegNamenLijst();
            }
            catch
            {
            }
        }

        private void namenAdressenEMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Achter Naam";
                oSheet.Cells[1, 2] = "Voor Naam";
                oSheet.Cells[1, 3] = "Adres";
                oSheet.Cells[1, 4] = "Postcode";
                oSheet.Cells[1, 5] = "Plaats";
                oSheet.Cells[1, 6] = "E-mail";


                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "F1").Font.Bold = true;
                oSheet.get_Range("A1", "F1").VerticalAlignment =
                Excel.XlVAlign.xlVAlignCenter;

                int start_row = 2;
                foreach (personeel a in ProgData.ListPersoneel)
                {
                    oSheet.Cells[start_row, 1] = a._achternaam;
                    oSheet.Cells[start_row, 2] = a._voornaam;
                    oSheet.Cells[start_row, 3] = a._adres;
                    oSheet.Cells[start_row, 4] = a._postcode;
                    oSheet.Cells[start_row, 5] = a._woonplaats;
                    oSheet.Cells[start_row, 6] = a._emailthuis;

                    start_row++;
                }

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "F1");
                oRng.EntireColumn.AutoFit();

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }

        }

        private void closeExitStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ZetGevondenDataTellingWaarGewerktInExcel()
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                // Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Waar gewerkt in Jaar : ";
                oSheet.Cells[1, 2] = ProgData.sgekozenjaar();
                oSheet.Cells[2, 1] = "Ploegkleur : ";
                oSheet.Cells[2, 2] = ProgData.GekozenKleur;

                for (int i = 0; i < ListTelWerkPlek.Count; i++)
                {
                    oSheet.Cells[3, i + 2] = ListTelWerkPlek[i];
                }

                oSheet.get_Range("A1", "Z2").Font.Bold = true;

                for (int i = 0; i < ListTelNamen.Count; i++)
                {
                    oSheet.Cells[i + 4, 1] = ListTelNamen[i];
                }

                for (int i = 0; i < ListClassTelPlekGewerkt.Count; i++)
                {
                    //var test = Tel[i]._PlekTelPlek;
                    //var test2 = TelWerkPlek.IndexOf(test);      // y coord
                    //var test4 = Tel[i]._NaamTelPlek;
                    //var test5 = TelNamen.IndexOf(test4);      // x coord
                    //var test3 = Tel[i]._AantalTelPlek;      // inhoud
                    oSheet.Cells[ListTelNamen.IndexOf(ListClassTelPlekGewerkt[i]._NaamTelPlek) + 4,
                        ListTelWerkPlek.IndexOf(ListClassTelPlekGewerkt[i]._PlekTelPlek) + 2] = ListClassTelPlekGewerkt[i]._AantalTelPlek;
                }

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "Z3");
                oRng.EntireColumn.AutoFit();

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void vuilwerkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("vuilwerk.ini"))
            {
                ListVuilwerkData.Clear();
                ListVuilwerkData = File.ReadAllLines("vuilwerk.ini").ToList();

                // eerst maar lijst maken wie vuilwerk verdiend.
                ListClassTelVuilwerk.Clear();
                int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);

                foreach (personeel a in ProgData.ListPersoneelKleur)
                {
                    if ((a._vuilwerk == "True"))
                    {
                        for (int i = 0; i < View.Items.Count; i++) // alle namen/rows
                        {
                            if (a._achternaam == View.Items[i].Text) // gevonden naam
                            {
                                for (int d = 1; d < aantal_dagen_deze_maand + 1; d++)
                                {
                                    bool rechtop = false;

                                    string afwijking = View.Items[i].SubItems[d].Text;
                                    string dienst = View.Items[3].SubItems[d].Text;

                                    if (afwijking == "") rechtop = true;
                                    if (afwijking == "*") rechtop = true;

                                    if (rechtop && dienst != "")
                                    {
                                        ClassTelVuilwerk afw = new ClassTelVuilwerk(a._achternaam, d.ToString());
                                        ListClassTelVuilwerk.Add(afw);   // recht op vuilwerk
                                    }
                                }
                            }
                        }
                    }
                }
                ZetGevondenDataTellingVuilWerktInExcel();
            }
            else // geen vuilwerk.ini
            {
                MessageBox.Show("Geen vuilwerk.ini gevonden!");
            }
        }

        private void ZetGevondenDataTellingVuilWerktInExcel()
        {
            Excel.Application xlApp;
            Excel._Workbook xlWorkBook;
            Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                xlApp = new Excel.Application();
                xlApp.Visible = true;

                //Get a new workbook.
                string file = Path.GetFullPath("vuilwerk.xls");

                /*
                 public Microsoft.Office.Interop.Excel.Workbook Open
                (string Filename, 
                object UpdateLinks, 
                object ReadOnly, 
                object Format, 
                object Password, 
                object WriteResPassword, 
                object IgnoreReadOnlyRecommended, 
                object Origin, 
                object Delimiter, 
                object Editable,
                object Notify, 
                object Converter, 
                object AddToMru, 
                object Local, 
                object CorruptLoad); 
                */

                xlWorkBook = xlApp.Workbooks.Open(file, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, true, 0, true, 1, 0);

                Excel.Worksheet excelSheet = xlWorkBook.ActiveSheet;

                // schoonvegen
                oRng = excelSheet.Range[excelSheet.Cells[11, 1], excelSheet.Cells[37, 33]];
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[11, 35], excelSheet.Cells[37, 36]];
                oRng.ClearContents();
                oRng.Value = null;

                int row = 11;
                int aantalvuilwerkcodes = ListVuilwerkData.Count / 3;

                // invullen
                foreach (ClassTelVuilwerk afw in ListClassTelVuilwerk)
                {
                    string cellValue = (string)(excelSheet.Cells[row, 2] as Excel.Range).Value;

                    // voor eerste persoon
                    if (cellValue == null)
                    {
                        excelSheet.Cells[row, 1] = ProgData.Get_Gebruiker_Nummer(afw._NaamTelVuil);
                        excelSheet.Cells[row, 2] = afw._NaamTelVuil;
                        cellValue = afw._NaamTelVuil;

                        for (int i = 0; i < aantalvuilwerkcodes; i++)
                        {
                            excelSheet.Cells[row + i, 35] = ListVuilwerkData[1 + (i * 3)];
                            excelSheet.Cells[row + i, 36] = ListVuilwerkData[2 + (i * 3)];
                        }

                    }

                    if (cellValue != afw._NaamTelVuil)  // nieuwe naam
                    {
                        row += aantalvuilwerkcodes;

                        // nieuwe naam
                        excelSheet.Cells[row, 1] = ProgData.Get_Gebruiker_Nummer(afw._NaamTelVuil);
                        excelSheet.Cells[row, 2] = afw._NaamTelVuil;
                        cellValue = afw._NaamTelVuil;
                        for (int i = 0; i < aantalvuilwerkcodes; i++)
                        {
                            excelSheet.Cells[row + i, 35] = ListVuilwerkData[1 + (i * 3)];
                            excelSheet.Cells[row + i, 36] = ListVuilwerkData[2 + (i * 3)];
                        }
                    }

                    int dag = int.Parse(afw._DagTelVuil);
                    for (int i = 0; i < aantalvuilwerkcodes; i++)
                    {
                        excelSheet.Cells[row + i, dag + 2] = ListVuilwerkData[i * 3];
                    }
                }

                excelSheet.Cells[1, 2] = ProgData.sgekozenmaand();
                excelSheet.Cells[6, 2] = ProgData.GekozenKleur;

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void nietMeeTelLijstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("telnietmee.ini");
        }

        private void afwijkingenTovRoosterIngelogdPersoonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int bewaar_maand = ProgData.igekozenmaand;
            int bewaar_jaar = ProgData.igekozenjaar;

            ListClassTelAfwijkingen.Clear();

            string locatie = @"telnietmee.ini";
            ListTelNietMeeNamen = File.ReadAllLines(locatie).ToList();

            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            for (int i = 1; i < 13; i++)    // maanden
            {
                ProgData.igekozenmaand = i;
                GetAfwijkingenPersoonInEenMaand(ProgData.Huidige_Gebruiker_Personeel_nummer, ProgData.ihuidigjaar, ProgData.igekozenmaand);
            }
            ProgData.igekozenmaand = bewaar_maand;
            ProgData.igekozenjaar = bewaar_jaar;

            try
            {
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                // Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Afwijkingen tov rooster in Jaar : ";
                oSheet.Cells[1, 2] = ProgData.sgekozenjaar();
                oSheet.Cells[2, 1] = "Ploegkleur : ";
                oSheet.Cells[2, 2] = ProgData.GekozenKleur;
                oSheet.Cells[3, 1] = "Naam : ";
                oSheet.Cells[3, 2] = ProgData.Get_Gebruiker_Naam(ProgData.Huidige_Gebruiker_Personeel_nummer);

                oSheet.get_Range("A1", "B4").Font.Bold = true;

                int row = 6;
                oSheet.Cells[1, 5] = "Aantal            ";
                oSheet.Cells[1, 6] = "Aantal in toekomst";

                foreach (ClassTelAfwijkingen afwijkingen in ListClassTelAfwijkingen)
                {
                    switch (afwijkingen._Afwijking)
                    {
                        case "V":
                            oSheet.Cells[2, 4] = "Vrij volgens rooster  ";
                            if (afwijkingen._Toekomst)
                            {
                                oSheet.Cells[2, 5] = afwijkingen._Aantal;
                            }
                            else
                            {
                                oSheet.Cells[2, 6] = afwijkingen._Aantal;
                            }
                            break;
                        case "W":
                            oSheet.Cells[3, 4] = "Volledig gewerkte dagen zonder afwijking  ";
                            if (afwijkingen._Toekomst)
                            {
                                oSheet.Cells[3, 5] = afwijkingen._Aantal;
                            }
                            else
                            {
                                oSheet.Cells[3, 6] = afwijkingen._Aantal;
                            }
                            break;
                        case "A":
                            oSheet.Cells[4, 4] = "A";
                            if (afwijkingen._Toekomst)
                            {
                                oSheet.Cells[4, 5] = afwijkingen._Aantal;
                            }
                            else
                            {
                                oSheet.Cells[4, 6] = afwijkingen._Aantal;
                            }
                            break;
                        case "VAK":
                            oSheet.Cells[5, 4] = "VAK";
                            if (afwijkingen._Toekomst)
                            {
                                oSheet.Cells[5, 5] = afwijkingen._Aantal;
                            }
                            else
                            {
                                oSheet.Cells[5, 6] = afwijkingen._Aantal;
                            }
                            break;
                        default:
                            oSheet.Cells[row, 4] = afwijkingen._Afwijking;
                            oSheet.Cells[row, 5] = afwijkingen._Aantal;
                            oSheet.Cells[row, 6] = afwijkingen._Toekomst;
                            row++;
                            break;
                    }
                }

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "Z15");
                oRng.EntireColumn.AutoFit();


                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void afwijkingTovRoosterPloegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void GetAfwijkingenPersoonInEenMaand(string persnum, int jaar, int maand)
        {
            string kleur = ProgData.Get_Gebruiker_Kleur(ProgData.Huidige_Gebruiker_Personeel_nummer);
            string naam = ProgData.Get_Gebruiker_Naam(ProgData.Huidige_Gebruiker_Personeel_nummer);

            //eerst vanuit ploegbezetting een string list maken met afwijkingen en normaal schema van persoon
            if (File.Exists(ProgData.Ploeg_Bezetting_Locatie(kleur)))
            {
                ProgData.LoadPloegBezetting(kleur);
                foreach (werkdag dag in ProgData.ListWerkdagPloeg)
                {
                    if (dag._naam == naam)
                    {
                        dag_gekozen = new DateTime(ProgData.ihuidigjaar, ProgData.igekozenmaand, dag._dagnummer);

                        // get en zet eerst orginele dienst
                        string wacht = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), dag_gekozen, kleur);
                        if (wacht != "")
                        {
                            deze_maand_overzicht_persoon[dag._dagnummer] = "W"; // Werkdag
                        }
                        else
                        {
                            deze_maand_overzicht_persoon[dag._dagnummer] = "V"; // Rooster vrij
                        }
                        // daarna overschrijven als die afwijkt van ""
                        if (dag._afwijkingdienst != "")
                        {
                            deze_maand_overzicht_persoon[dag._dagnummer] = dag._afwijkingdienst;
                        }

                        if (ListTelNietMeeNamen.Contains(deze_maand_overzicht_persoon[dag._dagnummer]))
                        {
                            deze_maand_overzicht_persoon[dag._dagnummer] = "W"; // Werkdag
                        }
                    }
                }
            }

            // lijst strings nu klaar, nu tellen.
            int dagen = DateTime.DaysInMonth(jaar, maand);
            for (int q = 1; q < dagen + 1; q++)
            {
                if (deze_maand_overzicht_persoon[q] != null)
                {
                    dag_gekozen = new DateTime(ProgData.ihuidigjaar, ProgData.igekozenmaand, q);
                    bool toekomst = dag_gekozen > DateTime.Now;

                    ClassTelAfwijkingen afwijking_dum;
                    try
                    {
                        afwijking_dum = ListClassTelAfwijkingen.Find(x => (x._Afwijking == deze_maand_overzicht_persoon[q] && x._Toekomst == toekomst));
                        if (afwijking_dum != null)
                        {
                            afwijking_dum._Aantal++;
                        }
                        else
                        {
                            ListClassTelAfwijkingen.Add(new ClassTelAfwijkingen(deze_maand_overzicht_persoon[q], 1, toekomst));
                        }
                    }
                    catch{}
                    
                        
                    //if (!ListAfwijkingen.Contains(deze_maand_overzicht_persoon[q]))
                    //{
                    //    ListAfwijkingen.Add(deze_maand_overzicht_persoon[q]);
                    //    ListTelling.Add(0);
                    //}

                    //int index = ListAfwijkingen.FindIndex(a => a.Contains(deze_maand_overzicht_persoon[q]));
                    //ListTelling[index]++;
                }
            }
        }
    }
}


