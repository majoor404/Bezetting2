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
using System.Threading;
using System.Windows.Forms;
using static Bezetting2.Data.MaandDataClass;
using static Bezetting2.DatumVijfPloegUtils;

/*/===================================================================================================

Niet op 64 bits compeleren, daar office implatatie dan niet werkt.

kleur_Maand_Data.bin heeft de maand data, dat is de MaandDataLijst.
Deze bestaat uit Item met de verander data van maand overzicht.

//=================================================================================================*/

namespace Bezetting2
{
    public partial class MainFormBezetting2 : Form
    {
        public int kolom_breed = 49;
        public const int y_as_eerste_lijn = 136;
        public const int y_as_add_lijn = 4;
        private bool kill = false;
        public bool WindowUpdateViewScreen = true;

        private readonly ToolTip mTooltip = new ToolTip();
        private Point mLastPos = new Point(-1, -1);

        private readonly Color Weekend_ = Color.LightSkyBlue;
        private readonly Color Feestdag_ = Color.LightSalmon;
        private readonly Color Huidigedag_ = Color.LightSteelBlue; // Lavender;
        private readonly Color MaandButton_ = Color.LightSkyBlue;
        private readonly Color Werkplek_ = Color.LightGray;
        private readonly Color MinimaalPersonen_ = Color.LightPink;
        private readonly Color HoverNaam_ = Color.LightGreen;
        private Color Kleur_Standaard_Font = Color.Black;
        private int aantal_regels_gekleurd = 0;

        public List<ClassTelPlekGewerkt> ListClassTelPlekGewerkt = new List<ClassTelPlekGewerkt>();
        public List<string> ListTelNamen = new List<string>();
        public List<string> ListTelWerkPlek = new List<string>();
        public List<ClassTelVuilwerk> ListClassTelVuilwerk = new List<ClassTelVuilwerk>();
        public List<string> ListVuilwerkData = new List<string>();
        private DateTime dag_gekozen;
        public List<ClassTelAfwijkingen> ListClassTelAfwijkingen = new List<ClassTelAfwijkingen>();

        private readonly List<string> ListTelNietMeeNamen = new List<string>();

        // quick menu
        readonly QuickInvoerForm quick = new QuickInvoerForm();

        public class ClassTelAfwijkingen
        {
            public ClassTelAfwijkingen(string afwijking, int aantal, bool toekomst)
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

        private readonly InstellingenProgrammaForm instellingen_programma = new InstellingenProgrammaForm();

        public MainFormBezetting2()
        {
            InitializeComponent();

            ProgData._inlognaam = IngelogdPersNr;
            ProgData._toegangnivo = ToegangNivo;

            ProgData.RechtenHuidigeGebruiker = 0; // alleen lezen
            ProgData.Huidige_Gebruiker_Personeel_nummer = "Niemand Ingelogd";
            WindowUpdateViewScreen = true;

            // test lees rechten
            if (!File.Exists("BezData\\Programdata.ini"))
            {
                MessageBox.Show("Geen lees rechten of BezData\\Programdata.ini niet aanwezig, exit");
                Close();
            }
            // test schrijf rechten
            string loc = Path.GetFullPath("Bezetting2.exe");
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(loc);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show($"Debug test write toegang : {loc}");
                MessageBox.Show("Geen schrijf rechten , exit");
                Close();

            }
            InstellingenProg.LeesProgrammaData();

            if (!File.Exists("BezData\\popupmenu.ini"))
                MaakLeegPopUpMenu();

            if (!Directory.Exists("Backup"))
                Directory.CreateDirectory("Backup");

            Kleur_Standaard_Font = buttonRefresh.ForeColor;
        }

        // start programma
        private void MainFormBezetting2_Shown(object sender, EventArgs e)
        {
            if (File.Exists("kill.ini"))
            {
                DateTime creation = File.GetCreationTime(@"kill.ini");
                DateTime nu_tijd = DateTime.Now;
                creation = creation.AddMinutes(30);
                if (creation < nu_tijd)
                {
                    MessageBox.Show("Oud stop bericht gevonden (meer dan 30 minuten), waarschijnlijk netwerk problemen gehad\nVerwijder deze en ga door.");
                    File.Delete("kill.ini");
                }
                else
                {
                    Close();
                }
            }


            ruilOverwerkToolStripMenuItem.Visible = InstellingenProg._GebruikExtraRuil;
            snipperDagAanvraagToolStripMenuItem.Visible = InstellingenProg._GebruikSnipper;

            ProgData.Main = this;

            if (!ProgData.TestNetwerkBeschikbaar(15))
                Close();

            WindowUpdateViewScreen = false; // gekozen kleur is nog niet bekend, en bij andere maand/jaar wilt die al gaan tekenen op beeld

            DateTime nu = DateTime.Now;

            ProgData.ihuidigemaand = nu.Month;
            ProgData.igekozenmaand = nu.Month;

            ProgData.igekozenjaar = nu.Year;
            ProgData.ihuidigjaar = nu.Year;

            DateTime backup = nu;
            ProgData.backup_zipnaam_huidige_maand = $"Backup\\maand_{backup.Month}.zip";
            backup = backup.AddMonths(1);
            ProgData.backup_zipnaam_maand_verder = $"Backup\\maand_{backup.Month}.zip";
            backup = backup.AddMonths(1);
            ProgData.backup_zipnaam_2maanden_verder = $"Backup\\maand_{backup.Month}.zip";

            Random rnd = new Random();
            ProgData.backup_time = rnd.Next(60);

            KleurMaandButton();

            string dienst = "N";
            if (nu.Hour > 5 && nu.Hour < 14)
                dienst = "O";
            if (nu.Hour > 13 && nu.Hour < 22)
                dienst = "M";

            if (nu.Hour < 6 && dienst == "N")
                nu = nu.AddDays(-1);

            comboBoxKleurKeuze.Text = GetKleurDieWerkt(ProgData.GekozenRooster(), nu, dienst);

            if (comboBoxKleurKeuze.Text == "DD")
                comboBoxKleurKeuze.Enabled = false;

            // als scherm te groot is voor kleine monitors, aanpassen
            int ourScreenWidth = Screen.FromControl(this).WorkingArea.Width;
            if (ourScreenWidth < 1920)
            {
                this.Size = new Size(1220, 730);
                View.Size = new Size(1080, 550);
                View.Font = new Font("Microsoft Sans Serif", 8);

                kolom_breed = 30;
                CenterToScreen();

                foreach (System.Windows.Forms.Button button in this.Controls.OfType<System.Windows.Forms.Button>())
                {
                    button.Font = new Font("Microsoft Sans Serif", 8);
                    if (button.Width > 100)
                    {
                        button.Width = 70;
                    }
                    else
                    {
                        button.Width = 30;
                        if (button.Text == ">")
                            button.Left -= 20;
                    }

                }
                foreach (System.Windows.Forms.ComboBox combo in this.Controls.OfType<System.Windows.Forms.ComboBox>())
                {
                    combo.Width = 70;
                }

                panelColor.Width = 74;

                foreach (System.Windows.Forms.NumericUpDown num in this.Controls.OfType<System.Windows.Forms.NumericUpDown>())
                {
                    num.Width = 70;
                }

                View.Left -= 40;
            }

            WindowUpdateViewScreen = true;
            VulViewScherm();

            if (ProgData.LeesLijnen())
                ZetLijnen();

            // auto inlog
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string autoinlogfile = $"{directory}\\bezetting2.log";
            if (File.Exists(autoinlogfile))
            {
                try
                {
                    List<string> inlognaam = File.ReadAllLines(autoinlogfile).ToList();
                    ProgData.Huidige_Gebruiker_Personeel_nummer = inlognaam[0];
                    ProgData.RechtenHuidigeGebruiker = int.Parse(inlognaam[1]);
                    ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = ProgData.Get_Gebruiker_Kleur(inlognaam[0]);

                    // test of gebruiker nog bestaat
                    ProgData.Laad_LijstNamen();
                    try
                    {
                        personeel persoon = ProgData.LijstPersoneel.First(b => b._persnummer.ToString() == inlognaam[0].ToString());

                    }
                    catch
                    {
                        const string message = "Auto inlog naam bestaat niet in in deze bezetting personeel lijst!, auto inlog verwijderen?";
                        const string caption = "Vraag";
                        var result = MessageBox.Show(message, caption,
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            File.Delete(autoinlogfile);
                        }
                        ProgData.Huidige_Gebruiker_Personeel_nummer = "";
                        ProgData.RechtenHuidigeGebruiker = 0;
                        ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = ProgData.GekozenKleur;
                    }


                }
                catch (IOException)
                {
                }
            }
            ProgData.CheckDubbelAchterNaam();
            Refresh();
        }

        private void InloggenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InlogForm log = new InlogForm();
            log.ShowDialog();
            comboBoxKleurKeuze.Text = ProgData.GekozenKleur;
        }

        private void EditPersoneelClick(object sender, EventArgs e)
        {
            ProgData.Laad_LijstNamen();
            EditPersoneel edit = new EditPersoneel();
            edit.ShowDialog();
            ButtonNu_Click(this, null); //refresh op huidige datum
                                        //VulViewScherm(); // refresh
        }

        private void ToegangNivo_TextChanged(object sender, EventArgs e)
        {
            // rechten worden gewijzigd, pas dus div menu items aan
            importNamenOudeVersieToolStripMenuItem.Visible = ProgData.RechtenHuidigeGebruiker > 100;
            editPersoneelToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 45;
            kleurLijnenToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 45;
            instellingenProgrammaToolStripMenuItem.Visible = ProgData.RechtenHuidigeGebruiker > 100;
            importOudeVeranderDataOudeVersieToolStripMenuItem.Visible = ProgData.RechtenHuidigeGebruiker > 100;
            nietMeeTelLijstToolStripMenuItem.Visible = ProgData.RechtenHuidigeGebruiker > 100;
            removeAutoInlogOnderDitWindowsAccountToolStripMenuItem.Visible = ProgData.RechtenHuidigeGebruiker > 100;
            editPopupMenuToolStripMenuItem.Visible = ProgData.RechtenHuidigeGebruiker > 100;

            vuilwerkToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
            tellingWaarGewerktToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
            namenAdressenEMailToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
            afwijkingenTovRoosterIngelogdPersoonToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 0;
            afwijkingTovRoosterPloegToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
            maandenOverzichtNaarExcelToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
            jaarOverzichtNaarExcelToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
            updateExtraDienstenToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
        }

        private void UitloggenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgData.RechtenHuidigeGebruiker = 0; // alleen lezen
            ProgData.Huidige_Gebruiker_Personeel_nummer = "Niemand Ingelogd";
        }

        private void ComboBoxKleurKeuze_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProgData.CaptureMainScreen();
            ProgData.GekozenKleur = comboBoxKleurKeuze.Text;

            if(comboBoxKleurKeuze.Text == "Blauw")
                panelColor.BackColor = Color.Blue;
            if (comboBoxKleurKeuze.Text == "Rood")
                panelColor.BackColor = Color.Red;
            if (comboBoxKleurKeuze.Text == "Geel")
                panelColor.BackColor = Color.Yellow;
            if (comboBoxKleurKeuze.Text == "Groen")
                panelColor.BackColor = Color.Green;
            if (comboBoxKleurKeuze.Text == "Wit")
                panelColor.BackColor = Color.Gray;
            if (comboBoxKleurKeuze.Text == "DD")
                panelColor.BackColor = Color.White;

            VulViewScherm();
        }

        public void VulViewScherm()
        {
            if (WindowUpdateViewScreen)
            {
                View.Columns.Clear();
                View.Items.Clear();

                ProgData.Zetom_naar_versie21(ProgData.GekozenKleur);

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

                // als DD tab gekozen in 5plg rooster
                string roost = ProgData.GekozenRooster();
                if (ProgData.GekozenKleur == "DD")
                    roost = "dd";

                DateTime datum;
                for (int i = 1; i < aantal_dagen + 1; i++)
                {
                    dagnr[i] = i.ToString();
                    datum = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, i);
                    rooster[i] = GetDienst(roost, datum, ProgData.GekozenKleur);
                    dag[i] = GetDag(datum);
                    weeknr[i] = "";
                    if (dag[i] == "W")
                    {
                        weeknr[i - 1] = "WK";
                        int weekNum = cul.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
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
                string dag_string;
                for (int col = 1; col < aantal_dagen + 1; col++)
                {
                    // lees dag
                    dag_string = View.Items[1].SubItems[col].Text;
                    if (dag_string == "Z") // zaterdag of zondag
                    {
                        for (int row = 0; row < aantal_regels_gekleurd - 1; row++)
                        {
                            //this is very Important
                            View.Items[row].UseItemStyleForSubItems = false;
                            // Now you can Change the Particular Cell Property of Style
                            if (View.Items[row].SubItems[col].BackColor != Werkplek_)
                                View.Items[row].SubItems[col].BackColor = Weekend_;
                        }
                    }
                }

                KleurFeestdagen();

                // kleur huidige dag
                if (ProgData.igekozenmaand == DateTime.Now.Month && ProgData.igekozenjaar == DateTime.Now.Year)
                {
                    for (int i = 0; i < aantal_regels_gekleurd - 1; i++)
                    {
                        View.Items[i].UseItemStyleForSubItems = false;
                        // Now you can Change the Particular Cell Property of Style
                        if (View.Items[i].SubItems[DateTime.Now.Day].BackColor != Werkplek_)
                            View.Items[i].SubItems[DateTime.Now.Day].BackColor = Huidigedag_;
                    }
                }

                // maand in beeld
                View.Items[2].UseItemStyleForSubItems = false;
                View.Items[2].SubItems[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);
                View.Items[2].SubItems[0].Text = ProgData.Sgekozenmaand().ToUpper();

                // Jaar in beeld
                View.Items[3].UseItemStyleForSubItems = false;
                View.Items[3].SubItems[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);
                View.Items[3].SubItems[0].Text = ProgData.Sgekozenjaar().ToUpper();

                // Kleur in beeld
                View.Items[4].UseItemStyleForSubItems = false;
                //View.Items[4].SubItems[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);
                View.Items[4].SubItems[0].Text = ProgData.GekozenKleur;

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

        private void NumericUpDownJaar_ValueChanged(object sender, EventArgs e)
        {
            ProgData.CaptureMainScreen();
            ProgData.igekozenjaar = (int)numericUpDownJaar.Value;
            VulViewScherm();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            WindowUpdateViewScreen = true;
            buttonRefresh.ForeColor = Color.Black;
            VulViewScherm();
        }

        private void VolgendeMaandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime t = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
            t = t.AddMonths(1);

            ProgData.igekozenmaand = t.Month;
            ProgData.igekozenjaar = t.Year;

            KleurMaandButton();
            VulViewScherm();
        }

        private void VorigeMaandToolStripMenuItem_Click(object sender, EventArgs e)
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
                        button.BackColor = MaandButton_;
                    }
                }
            }
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
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
            int aantal_rows = ProgData.LijstPersoneelKleur.Count();
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
                    for (int row = 0; row < aantal_regels_gekleurd - 1; row++)
                    {
                        //this is very Important
                        View.Items[row].UseItemStyleForSubItems = false;
                        // Now you can Change the Particular Cell Property of Style
                        if (View.Items[row].SubItems[col].BackColor != Werkplek_)
                            View.Items[row].SubItems[col].BackColor = Feestdag_;
                    }
                }
            }
        }

        private void HaalBezetting()
        {
            // HaalBezetting Bestaat uit 2 delen
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
                    ProgData.LijstPersoneelKleur.Clear();
                    ProgData.LijstWerkgroepenPersoneel.Clear();
                }
                else
                {
                    // 1) Haal Ploeg Bezetting
                    ProgData.LaadLijstPersoneelKleur(ProgData.GekozenKleur, 15);

                    // 2) Zet ploeg en werkplek op scherm
                    for (int i = 0; i < ProgData.LijstWerkgroepenPersoneel.Count; i++)
                    {
                        // eerst naam werkplek
                        string[] werkplek = new string[33];
                        werkplek[0] = ProgData.LijstWerkgroepenPersoneel[i];
                        ListViewItem item = new ListViewItem(werkplek);
                        View.Items.Add(item);
                        View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                        for (int grijzebalk_werkplek = 0; grijzebalk_werkplek < 33; grijzebalk_werkplek++)
                        {
                            View.Items[View.Items.Count - 1].SubItems[grijzebalk_werkplek].BackColor = Werkplek_;
                        }
                        // zet in View
                        foreach (personeel a in ProgData.LijstPersoneelKleur)
                        {
                            string[] naamlijst = new string[33];
                            if (a._werkgroep == ProgData.LijstWerkgroepenPersoneel[i])
                            {
                                naamlijst[0] = a._achternaam;
                                ListViewItem item_naam = new ListViewItem(naamlijst);
                                item_naam.Tag = a._persnummer;
                                View.Items.Add(item_naam);
                            }
                        }
                    }

                    ProgData.MaandData.Load(ProgData.GekozenKleur);
                    foreach (MaandDataClass.Item Item in ProgData.MaandData.MaandDataLijst)
                    {
                        var naam = ProgData.Get_Gebruiker_Naam(Item.personeel_nr_).Trim();
                        var dag_afwijking = Item.datum_.Day;
                        for (int i = 0; i < View.Items.Count; i++) // alle namen/rows
                        {
                            if (naam == View.Items[i].Text.Trim()) // gevonden naam
                            {
                                View.Items[i].SubItems[dag_afwijking].Text = Item.afwijking_;
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
                    int aantal_mensen;// = ProgData.ListPersoneelKleur.Count;

                    List<string> TelNietMeeNamen = new List<string>();
                    string locatie = @"BezData\\telnietmee.ini";
                    try
                    {
                        TelNietMeeNamen = File.ReadAllLines(locatie).ToList();
                    }
                    catch { }

                    for (dag = 1; dag < aantal_dagen_deze_maand + 1; dag++) // aantal dagen
                    {
                        //string wacht = View.Items[3].SubItems[dag].Text;
                        aantal_mensen = ProgData.LijstPersoneelKleur.Count;
                        for (int i = 4; i < View.Items.Count; i++) // alle namen/rows
                        {
                            if (!string.IsNullOrEmpty(View.Items[i].SubItems[dag].Text))
                                aantal_mensen--;
                            if (TelNietMeeNamen.Contains(View.Items[i].SubItems[dag].Text))
                                aantal_mensen++;
                        }
                        if (!string.IsNullOrEmpty(View.Items[3].SubItems[dag].Text))
                            View.Items[View.Items.Count - 1].SubItems[dag].Text = aantal_mensen.ToString();
                        if (aantal_mensen < InstellingenProg._MinimaalAantalPersonen)
                        {
                            View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                            View.Items[View.Items.Count - 1].SubItems[dag].BackColor = MinimaalPersonen_;
                        }
                    }

                    aantal_regels_gekleurd = View.Items.Count;

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

                        //=====================================================================================================
                        // test of in orginele bezetting die persoon nog loopt
                        // anders verwijder uit lijst
                        // sla dit niet op, is toch oude data in verleden

                        string kleur = ProgData.GekozenKleur;

                        for (int i = ProgData.ListLooptExtra.Count - 1; i >= 0; i--)
                        {
                            string loopt_op_kleur = ProgData.Get_Gebruiker_Kleur(ProgData.Get_Gebruiker_Nummer(ProgData.ListLooptExtra[i]._naam));

                            if (loopt_op_kleur != "")
                            {
                                ProgData.LaadLijstWerkdagPloeg(loopt_op_kleur, 15);
                                try
                                {
                                    werkdag ros = ProgData.LijstWerkdagPloeg.Last(a => (a._naam == ProgData.ListLooptExtra[i]._naam) && (a._dagnummer == ProgData.ListLooptExtra[i]._datum.Day));
                                    string eerste_2 = ros._afwijkingdienst.Length >= 2 ? ros._afwijkingdienst.Substring(0, 2) : ros._afwijkingdienst;
                                    if (!(eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD"))
                                    {
                                        ProgData.ListLooptExtra.RemoveAt(i);
                                    }
                                }
                                catch { }
                            }
                        }

                        ProgData.GekozenKleur = kleur;
                        ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

                        //===================================================================================

                        foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
                        {
                            int dagy = ex._datum.Day;
                            if (string.IsNullOrEmpty(View.Items[View.Items.Count - 1].SubItems[dagy].Text))
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

                ProgData.Laad_LijstNamen();            // lees alle mensen in sectie , personeel_lijst
                ProgData.MaakPloegNamenLijst(ProgData.GekozenKleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
                ProgData.SaveLijstPersoneelKleur(ProgData.GekozenKleur, 15);     // save ploegbezetting (de mensen)

                // maak bezettingafwijking.bin voor kleur als die niet bestaat
                // is lijst met werkdagen
                //if (!File.Exists(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)))
                //if (!File.Exists(ProgData.Ploeg_Veranderingen_Locatie(ProgData.GekozenKleur)))
                //{
                //    ProgData.MaakLegeBezetting(ProgData.GekozenKleur, true); // in deze roetine wordt het ook opgeslagen
                //}

                CheckEnDealVerhuizing();

                // 1) Haal Ploeg Bezetting
                ProgData.LaadLijstPersoneelKleur(ProgData.GekozenKleur, 15);

                // 2) Zet ploeg en werkplek op scherm
                for (int i = 0; i < ProgData.LijstWerkgroepenPersoneel.Count; i++)
                {
                    // eerst naam werkplek
                    string[] werkplek = new string[33];
                    werkplek[0] = ProgData.LijstWerkgroepenPersoneel[i];
                    ListViewItem item = new ListViewItem(werkplek);
                    View.Items.Add(item);
                    View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                    for (int grijzebalk_werkplek = 0; grijzebalk_werkplek < 33; grijzebalk_werkplek++)
                    {
                        View.Items[View.Items.Count - 1].SubItems[grijzebalk_werkplek].BackColor = Werkplek_;
                    }

                    // zet in View
                    foreach (personeel a in ProgData.LijstPersoneelKleur)
                    {
                        string[] naamlijst = new string[33];
                        if (a._werkgroep == ProgData.LijstWerkgroepenPersoneel[i])
                        {
                            naamlijst[0] = a._achternaam;
                            ListViewItem item_naam = new ListViewItem(naamlijst);
                            item_naam.Tag = a._persnummer;      // bewaar personeel nummer in tag van item
                            View.Items.Add(item_naam);
                        }
                    }
                }

                ProgData.MaandData.Load(ProgData.GekozenKleur);
                foreach (MaandDataClass.Item Item in ProgData.MaandData.MaandDataLijst)
                {
                    var naam = ProgData.Get_Gebruiker_Naam(Item.personeel_nr_).Trim();
                    var dag_afwijking = Item.datum_.Day;
                    for (int i = 0; i < View.Items.Count; i++) // alle namen/rows
                    {
                        if (naam == View.Items[i].Text.Trim()) // gevonden naam
                        {
                            View.Items[i].SubItems[dag_afwijking].Text = Item.afwijking_;
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
                int aantal_mensen;// = ProgData.ListPersoneelKleur.Count;

                List<string> TelNietMeeNamen = new List<string>();
                string locatie = @"BezData\\telnietmee.ini";
                try
                {
                    TelNietMeeNamen = File.ReadAllLines(locatie).ToList();
                }
                catch
                {
                    MessageBox.Show("BezData\\telnietmee.ini niet gevonden");
                }

                for (dag = 1; dag < aantal_dagen_deze_maand + 1; dag++) // aantal dagen
                {
                    aantal_mensen = ProgData.LijstPersoneelKleur.Count;
                    //string wacht = View.Items[3].SubItems[dag].Text;
                    for (int i = 4; i < View.Items.Count; i++) // alle namen/rows
                    {
                        if (!string.IsNullOrEmpty(View.Items[i].SubItems[dag].Text))
                            aantal_mensen--;
                        if (TelNietMeeNamen.Contains(View.Items[i].SubItems[dag].Text))
                            aantal_mensen++;
                    }
                    if (!string.IsNullOrEmpty(View.Items[3].SubItems[dag].Text))
                        View.Items[View.Items.Count - 1].SubItems[dag].Text = aantal_mensen.ToString();
                    if (aantal_mensen < InstellingenProg._MinimaalAantalPersonen)
                    {
                        View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
                        View.Items[View.Items.Count - 1].SubItems[dag].BackColor = MinimaalPersonen_;
                    }
                }

                aantal_regels_gekleurd = View.Items.Count;

                
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

                    //=====================================================================================================
                    // test of in orginele bezetting die persoon nog loopt
                    // anders verwijder uit lijst

                    string kleur = ProgData.GekozenKleur;

                    for (int i = ProgData.ListLooptExtra.Count - 1; i >= 0; i--)
                    {
                        // ging fout door opslaan naam ipv personeel nummer,
                        // en we hebben bv 2 bakker's

                        string naam = ProgData.ListLooptExtra[i]._naam;
                        string persnr = ProgData.Get_Gebruiker_Nummer(naam);
                        string loopt_op_kleur = ProgData.Get_Gebruiker_Kleur(persnr);

                        if (loopt_op_kleur != "")
                        {
                            try
                            {
                                DateTime datum = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, ProgData.ListLooptExtra[i]._datum.Day);
                                string AfWijkingPersoon = ProgData.GetLaatsteAfwijkingPersoon(loopt_op_kleur, persnr, datum);


                                string eerste_2 = AfWijkingPersoon.Length >= 2 ? AfWijkingPersoon.Substring(0, 2) : AfWijkingPersoon;
                                string dienst = AfWijkingPersoon.Length >= 4 ? AfWijkingPersoon.Substring(3, 1) : AfWijkingPersoon;


                                var dienst_rooster = GetDienst(ProgData.GekozenRooster(), datum, ProgData.GekozenKleur);

                                if (!(eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD" || eerste_2 == "DD"))
                                {
                                    ProgData.ListLooptExtra.RemoveAt(i);
                                }
                                else
                                {
                                    if (dienst != dienst_rooster && dienst != "DD")
                                    {
                                        ProgData.ListLooptExtra.RemoveAt(i);
                                    }
                                }

                            }
                            catch { }
                        }
                    }

                    ProgData.GekozenKleur = kleur;
                    ProgData.SaveLooptExtraLijst(dir, ProgData.GekozenKleur);
                    ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

                    //================================================================================================================

                    foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
                    {
                        int dagy = ex._datum.Day;

                        //string dienst = ros._afwijkingdienst.Length >= 4 ? ros._afwijkingdienst.Substring(3, 1) : ros._afwijkingdienst;
                        //DateTime datum = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, ros._dagnummer);
                        //var dienst_rooster = GetDienst(ProgData.GekozenRooster(), datum, ProgData.GekozenKleur);

                        if (string.IsNullOrEmpty(View.Items[View.Items.Count - 1].SubItems[dagy].Text))
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
            ProgData.Laad_LijstNamen();            // lees alle mensen in sectie , personeel_lijst
                                                   // check of er vorige maand mensen zijn verhuisd

            IEnumerable<personeel> persoon = from a in ProgData.LijstPersoneel
                                             where (!string.IsNullOrEmpty(a._nieuwkleur))
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
                    if (!string.IsNullOrEmpty(a._nieuwkleur))
                        a._kleur = a._nieuwkleur;
                    a._nieuwkleur = "";
                    if (a._nieuwkleur == null) // geen idee waar dit soms gebeurt
                        a._nieuwkleur = "";
                    ProgData.Save_LijstNamen();
                    ProgData.GekozenKleur = a._kleur;
                    ProgData.MaakPloegNamenLijst(a._kleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
                    ProgData.SaveLijstPersoneelKleur(a._kleur, 15);         // save ploegbezetting (de mensen)
                    ProgData.GekozenKleur = bewaar;
                }
            }
        }

        private static DateTime EasterSunday(int year)
        {
            int day;// = 0;
            int month;// = 0;

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

        // Geklikt op view scherm, open invoer form
        private void View_MouseClick(object sender, MouseEventArgs e)
        {
            //Point point = new Point(e.X, e.Y);
            ListViewHitTestInfo info = View.HitTest(e.X, e.Y);
            int row = info.Item.Index;
            int col = info.Item.SubItems.IndexOf(info.SubItem);

            if (ProgData.WaarInTijd() == 1)
            {
                if (col > 0 && row < 4)
                {
                    HistoryForm his = new HistoryForm();
                    his.comboBoxDag.Text = col.ToString();
                    his.ShowDialog();
                }
                else
                {
                    MessageBox.Show("In verleden kunt u alleen kijken, niet meer aanpassen!");
                }
            }
            else
            {
                if (((ProgData.RechtenHuidigeGebruiker > 24) && (ProgData.RechtenHuidigeGebruiker < 51) && (ProgData.Huidige_Gebruiker_Werkt_Op_Kleur == ProgData.GekozenKleur))
                    || ProgData.RechtenHuidigeGebruiker > 51)
                {
                    try
                    {
                        string value = info.Item.SubItems[col].Text;
                        //MessageBox.Show(string.Format("R{0}:C{1} val '{2}'", row, col, value));

                        if (col > 0 && row < 4)
                        {
                            HistoryForm his = new HistoryForm();
                            his.comboBoxDag.Text = col.ToString();
                            his.ShowDialog();
                        }

                        if (col != 0 && View.Items[row].SubItems[0].BackColor != Werkplek_)
                        {
                            string gekozen_naam = info.Item.SubItems[0].Text;
                            string gekozen_datum = col.ToString();

                            personeel persoon = ProgData.LijstPersoneelKleur.First(a => a._achternaam == gekozen_naam);

                            if (e.Button == MouseButtons.Right)
                            {

                                quick.Location = new System.Drawing.Point(e.Location.X + this.Location.X + 180, e.Location.Y + this.Location.Y + 60);
                                quick.ShowDialog();
                                if (quick.listBox1.SelectedIndex > -1)
                                {
                                    string afwijking = quick.listBox1.SelectedItem.ToString();
                                    switch (afwijking.ToUpper())
                                    {
                                        case "WIS":
                                            ProgData.RegelAfwijking(ProgData.Get_Gebruiker_Nummer(gekozen_naam), gekozen_datum, "", "Verwijderd", ProgData.Huidige_Gebruiker_Personeel_nummer, ProgData.GekozenKleur);
                                            break;
                                        default:
                                            ProgData.RegelAfwijking(ProgData.Get_Gebruiker_Nummer(gekozen_naam), gekozen_datum, afwijking, "", ProgData.Huidige_Gebruiker_Personeel_nummer, ProgData.GekozenKleur);
                                            ProgData.NachtErVoorVrij(gekozen_naam, gekozen_datum, afwijking);
                                            break;
                                    }
                                    VulViewScherm();
                                }
                            }
                            else
                            {
                                DagAfwijkingInvoerForm afw = new DagAfwijkingInvoerForm();
                                afw.labelNaam.Text = gekozen_naam;
                                afw.labelDatum.Text = gekozen_datum;
                                afw.labelMaand.Text = ProgData.Sgekozenmaand();
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
                // geen rechten/ingelogt
                else
                {
                    MessageBox.Show("Even inloggen!");
                }
            }
        }

        private void ButtonNu_Click(object sender, EventArgs e)
        {
            //DateTime nu = DateTime.Now;

            ProgData.igekozenmaand = ProgData.ihuidigemaand;
            ProgData.igekozenjaar = ProgData.ihuidigjaar;

            KleurMaandButton();
            VulViewScherm();
        }

        private void WachtOverzichtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test of alle gebruikers op deze ploeg, zijn opgenomen in LijstWerkdagPloeg
            ProgData.LaadLijstPersoneelKleur(ProgData.GekozenKleur, 15);
            ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);
            foreach (personeel a in ProgData.LijstPersoneelKleur)
            {
                var naam = a._achternaam;
                try
                {
                    werkdag ver = ProgData.LijstWerkdagPloeg.First(d => (d._naam == naam));
                }
                catch
                {
                    // naam bestond niet in werkdaglijst, toevoegen
                    ProgData.MaakNieuweCollegaInBezettingAan(naam, ProgData.GekozenKleur, ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
                }
            }



            if (InstellingenProg._Wachtoverzicht2Dagen && ProgData.RechtenHuidigeGebruiker < 101)
            {
                OverzichtWachtForm2Dagen owacht2 = new OverzichtWachtForm2Dagen();
                owacht2.ShowDialog();

                ButtonNu_Click(this, null);
            }
            else
            {
                OverzichtWachtForm wacht = new OverzichtWachtForm();
                ProgData.GekozenKleur = comboBoxKleurKeuze.Text;
                wacht.labelKleur.Text = ProgData.GekozenKleur;
                wacht.ShowDialog();
                ButtonNu_Click(this, null);
            }
        }

        private void View_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ListViewItem item = View.GetItemAt(e.X, e.Y);
                ListViewHitTestInfo info = View.HitTest(e.X, e.Y);

                toolStripStatusLabelInfo.Text = "";
                toolStripStatusRedeAfwijking.Text = "";

                if ((item != null) && (!string.IsNullOrEmpty(info?.SubItem?.Text)))
                {
                    int row = info.Item.Index;
                    int col = info.Item.SubItems.IndexOf(info.SubItem);
                    // extra dienst
                    string aant = View.Items[View.Items.Count - 1].Text;
                    if (row == View.Items.Count - 1 && aant == "Extra dienst")
                    {
                        if (info.SubItem.Text == "1") // 1 extra dienst
                        {
                            foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
                            {
                                if (col == ex._datum.Day)
                                {
                                    toolStripStatusLabelInfo.Text = ProgData.Get_Gebruiker_Naam(ex._naam);
                                    if (!string.IsNullOrEmpty(toolStripStatusLabelInfo.Text) && mLastPos != e.Location)
                                        mTooltip.Show(toolStripStatusLabelInfo.Text, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
                                }
                            }
                        }
                        else
                        {
                            toolStripStatusLabelInfo.Text = "Meer dan 1 extra dienst";
                            string namen = "";
                            foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
                            {
                                if (col == ex._datum.Day)
                                {
                                    namen = $"{namen}{ProgData.Get_Gebruiker_Naam(ex._naam)}\n";

                                }
                            }
                            if (!string.IsNullOrEmpty(toolStripStatusLabelInfo.Text) && mLastPos != e.Location)
                                mTooltip.Show(namen, info.Item.ListView, e.X + 15, e.Y + 15, 3000);
                        }
                    }
                    // rede afwijking
                    if (col > 0 && row > 3 && row < View.Items.Count - 1)
                    {
                        toolStripStatusLabelInfo.Text = info.Item.Text + " " + info.SubItem.Text;
                        toolStripStatusRedeAfwijking.Text = GetRedenAfwijking(info.Item.Text, col);
                        if (!string.IsNullOrEmpty(toolStripStatusRedeAfwijking.Text) && toolStripStatusRedeAfwijking.Text != " " && mLastPos != e.Location)
                        {
                            mTooltip.Show(toolStripStatusRedeAfwijking.Text, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
                        }
                    }
                    // personeel nummer bij naam
                    if (col == 0 && row > 3 && row < View.Items.Count - 1)
                    {
                        string naam = View.Items[row].Text;
                        //string persnummer = ProgData.Get_Gebruiker_Nummer(naam);
                        if (View.Items[row].Tag != null)
                        {
                            string persnummer = View.Items[row].Tag.ToString();
                            if (!string.IsNullOrEmpty(persnummer) && mLastPos != e.Location)
                                mTooltip.Show(persnummer, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
                        }
                    }
                    // feestdag
                    if (col > 0 && row < 5) // feestdag info ?
                    {
                        if (View.Items[row].SubItems[col].BackColor == Feestdag_)
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

                            if (!string.IsNullOrEmpty(toolStripStatusRedeAfwijking.Text) && mLastPos != e.Location)
                                mTooltip.Show(toolStripStatusRedeAfwijking.Text, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
                        }
                    }
                }
                if (item != null && (string.IsNullOrEmpty(info?.SubItem?.Text) && checkBoxHoverNaam.Checked))
                {
                    try
                    {
                        int time = (int)HoverTime.Value * 1000;
                        int row1 = info.Item.Index;
                        mTooltip.Show(View.Items[row1].Text, info.Item.ListView, e.X + 15, e.Y + 15, time);
                    }
                    catch { }
                }
                mLastPos = e.Location;
            }
            catch { }
        }

        private string GetRedenAfwijking(string naam, int dag)
        {
            ProgData.MaandData.Load(ProgData.GekozenKleur);
            string sdag = dag.ToString(CultureInfo.CurrentCulture);
            try
            {
                if (ProgData.MaandData.MaandDataLijst.Count > 0)
                {
                    var persnr = ProgData.Get_Gebruiker_Nummer(naam);
                    Item ver = ProgData.MaandData.MaandDataLijst.Last(a => (a.personeel_nr_ == persnr) && (a.datum_.Day.ToString() == sdag));
                    return ver.rede_;
                }
            }
            catch { }
            return "";

        }

        private void KleurLijnenToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void RepareerPloegAfwijkingToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void TimerKill_Tick(object sender, EventArgs e)
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

            if (ProgData.backup_time == DateTime.Now.Minute)
            {
                if (!File.Exists(ProgData.backup_zipnaam_huidige_maand))
                {
                    timerKill.Enabled = false;
                    labelDebug.Text = "Dagelijkse Backup, moment.....";
                    ProgData.Backup();
                    timerKill.Enabled = true;
                    labelDebug.Text = "Dagelijkse Backup gelukt";
                }
            }

            if (ProgData.MaandData.TestNieuweFile(ProgData.GekozenKleur))
            {
                buttonRefresh.ForeColor = Color.Red;
            }
            else
            {
                buttonRefresh.ForeColor = Kleur_Standaard_Font;
            }
        }

        private void RuilOverwerkToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void SnipperDagAanvraagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SnipperAanvraagForm snip = new SnipperAanvraagForm();
            snip.labelNaam.Text = ProgData.Huidige_Gebruiker_Personeel_nummer;
            snip.labelNaamFull.Text = ProgData.Huidige_Gebruiker_Naam();
            snip.ShowDialog();
        }

        private void InstellingenProgrammaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            instellingen_programma.ShowDialog();
            MainFormBezetting2_Shown(this, null);
        }

        private void MainFormBezetting2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Thread.Sleep(300);
            ProgData.CaptureMainScreen();
        }

        private void ImportOudeVeranderDataOudeVersieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("import oude data?", "vraagje",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {

                ProgData.ScreenCapture = false;
                WindowUpdateViewScreen = false;

                MessageBox.Show("Gaat soms fout als iemand in tussen tijd verhuisd is!");
                MessageBox.Show("Delete de komende 25 maanden in de toekomst, en maak lege");

                DateTime start = DateTime.Now;
                DateTime eind = start.AddMonths(25);
                DeleteDataDir(start, eind);

                openFileDialog.FileName = "";
                openFileDialog.Filter = "(*.Bez)|*.Bez";
                MessageBox.Show("Open oude data bez file. (Wijz...Bez)");
                result = openFileDialog.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    MessageBox.Show("Dit gaat tijdje duren, geduld..... (10 min)\nAl ingevulde data wordt overschreven!\nAls import klaar is sluit programma vanzelf af.");
                    ProgData.Disable_error_Meldingen = true;
                    ProgData.Laad_LijstNamen();
                    OpenDataBase_en_Voer_oude_data_in_Bezetting(openFileDialog.FileName);
                    //MessageBox.Show("Klaar met invoer, start programma opnieuw op.");
                    ProgData.GekozenKleur = "Blauw";
                    ButtonNu_Click(this, null);
                    ProgData.Disable_error_Meldingen = false;
                    Close();
                }

                ProgData.ScreenCapture = true;
                WindowUpdateViewScreen = true;
            }
        }

        private void OpenDataBase_en_Voer_oude_data_in_Bezetting(string file)
        {
            WindowUpdateViewScreen = false;
            using (OleDbConnection connection =
                new OleDbConnection($"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \"{file}\"; Jet OLEDB:Database Password = fcl721"))
            {

                bool read;
                int teller = 0;

                OleDbCommand command = new OleDbCommand("select * from Wijzeging", connection);

                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();
                WindowUpdateViewScreen = false;
                DateTime inladen_vanaf_datum;



                const string message = "Vanaf welke datum invoeren (Yes is huidige datum , No is hele jaar)";
                const string caption = "Vraag";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                // If the no button was pressed ...
                if (result == DialogResult.No)
                {
                    inladen_vanaf_datum = new DateTime(DateTime.Now.Year, 1, 1);
                    inladen_vanaf_datum = inladen_vanaf_datum.AddMonths(-1);
                    // delete dan nog even die directory's
                    DeleteDataDir(inladen_vanaf_datum, DateTime.Now);
                }
                else
                {
                    inladen_vanaf_datum = DateTime.Now;
                }


                //inladen_vanaf_datum = inladen_vanaf_datum.AddMonths(-3);
                ProgData.Laad_LijstNamen();            // lees alle mensen in sectie , personeel_lijst
                                                       // check of er vorige maand mensen zijn verhuisd

                if (reader.Read() == true)
                {
                    do
                    {
                        Application.DoEvents();
                        object[] meta = new object[12]; // zodat ze leeg zijn elke keer ivm vorige data
                                                        // inlezen waarden
                        _ = reader.GetValues(meta);

                        labelDebug.Text = $"{teller++}";
                        labelDebug.Refresh();

                        //Console.Write("{0} ", meta[2].ToString()); // pers nummer persoon
                        //Console.Write("{0} ", meta[3].ToString()); // naam persoon
                        //Console.Write("{0} ", meta[6].ToString()); // datum invoer
                        //Console.Write("{0} ", meta[7].ToString()); // datum afwijking
                        //Console.Write("{0} ", meta[5].ToString()); // afwijking
                        //Console.Write("{0} ", meta[9].ToString()); // personeel nummer invoerder
                        //Console.Write("{0} ", meta[11].ToString()); // rede

                        string[] datum = new string[12];

                        datum = meta[7].ToString().Split('-');
                        datum[2] = datum[2].Substring(0, 4);

                        DateTime datum_afwijking = new DateTime(int.Parse(datum[2]), int.Parse(datum[1]), int.Parse(datum[0]));

                        if ((datum_afwijking > inladen_vanaf_datum) && (ProgData.Bestaat_Gebruiker(meta[2].ToString())))
                        {
                            try
                            {
                                string kleur = ProgData.Get_Gebruiker_Kleur(meta[2].ToString());

                                // in oude programma is afwijking soms gelijk aan orginele dienst
                                // die hoef ik in te voeren
                                if (meta[5].ToString() == "O" || meta[5].ToString() == "M" || meta[5].ToString() == "N")
                                {
                                    if (GetDienst("5PL", datum_afwijking, kleur) == meta[5].ToString())
                                    {
                                        //// als ed vd of rd wordt verwijderd, dan ook in VerwijderLooptExtraDienst
                                        //// check eerst of laatse wijzeging dan ook een ed ve of rd was
                                        //ProgData.Igekozenjaar = datum_afwijking.Year;
                                        //ProgData.igekozenmaand = datum_afwijking.Month;
                                        //ProgData.LoadVeranderingenPloeg(kleur, 15);
                                        //try
                                        //{
                                        //    veranderingen wijz = ProgData.ListVeranderingen.Last(a => (a._naam == meta[3].ToString()) && (a._datumafwijking == datum_afwijking.Day.ToString()));
                                        //    // zo ja verwijder VerwijderLooptExtraDienst
                                        //    string eerste_2 = wijz._afwijking.Length >= 2 ? wijz._afwijking.Substring(0, 2) : wijz._afwijking;
                                        //    if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD")
                                        //    {
                                        //        //ProgData.VerwijderLooptExtraDienst(wijz._afwijking, datum_afwijking, wijz._naam);
                                        //    }
                                        //}
                                        //catch { }

                                        // in oude programma als afwijking werdt verwijderd, werdt orginele wacht ingevuld.
                                        meta[5] = "";
                                    }
                                }

                                if (kleur == "Blauw" || kleur == "Geel" || kleur == "Groen" || kleur == "Rood" || kleur == "Wit" || kleur == "DD")
                                {
                                    string naam = meta[3].ToString();
                                    string invoer_naam = ProgData.Get_Gebruiker_Naam(meta[9].ToString());
                                    string rede = meta[11].ToString();
                                    string afwijking = meta[5].ToString().ToUpper();

                                    //gaat fout als persoon ondertussen op andere kleur zit

                                    IEnumerable<personeel> persoon = from a in ProgData.LijstPersoneel
                                                                     where (a._achternaam == naam)
                                                                     where (!string.IsNullOrEmpty(a._nieuwkleur))
                                                                     select a;

                                    foreach (personeel a in persoon)
                                    {
                                        // als verhuis datum-maand in verleden is tov huidige maand,
                                        // aanpassen.
                                        DateTime overgang = new DateTime(a._verhuisdatum.Year, a._verhuisdatum.Month, a._verhuisdatum.Day);

                                        if (overgang <= datum_afwijking)
                                        {
                                            kleur = a._nieuwkleur;
                                        }
                                    }


                                    labelDebug.Text = $"{teller} {naam} {afwijking}";
                                    labelDebug.Refresh();
                                    //ProgData.RegelAfwijkingOpDatumEnKleur(datum_afwijking, kleur, naam, datum[0], afwijking, rede, "Import " + invoer_naam, false);

                                    //int personeelNr = int.Parse(meta[2].ToString());
                                    ProgData.RegelAfwijkingOpDatumEnKleur(datum_afwijking, kleur, meta[2].ToString(), datum[0], afwijking, rede, "Import " + invoer_naam, false);
                                    //ProgData.RegelAfwijkingOpDatumEnKleurEnPersoneelNummer(datum_afwijking, kleur, personeelNr, datum[0], afwijking, rede, "Import " + invoer_naam, false);

                                    // toevoegen extra ruil of verschoven dienst
                                    string eerste_2 = afwijking.Length >= 2 ? afwijking.Substring(0, 2) : afwijking;
                                    if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD")
                                    {
                                        ProgData.VulInLooptExtraDienst(afwijking, datum_afwijking, naam);
                                    }
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

                // als rechten <> 0 dan vertalen naar nieuwe rechten, en
                // zet passwoord op "verander_nu", zodat bij eerste keer inlog vraag komt aanpassen



                if (reader.Read() == true)
                {
                    //DateTime nu = DateTime.Now;
                    do
                    {
                        System.Windows.Forms.Application.DoEvents();
                        _ = reader.GetValues(meta);


                        try
                        {
                            personeel p = new personeel
                            {
                                _persnummer = int.Parse(meta[0].ToString()),
                                _achternaam = meta[1].ToString(),
                                _voornaam = meta[2].ToString(),
                                _adres = meta[3].ToString(),
                                _postcode = meta[4].ToString(),
                                _woonplaats = meta[5].ToString(),
                                _telthuis = meta[6].ToString(),
                                _tel06prive = meta[7].ToString(),
                                _telwerk = meta[8].ToString(),
                                _emailwerk = meta[9].ToString(),
                                _emailthuis = meta[10].ToString(),
                                _adrescodewerk = meta[11].ToString(),
                                _funtie = meta[12].ToString(),
                                _kleur = meta[13].ToString(),
                                _nieuwkleur = "",
                                _verhuisdatum = DateTime.Now,
                                _tel06werk = meta[14].ToString(),
                                _werkgroep = meta[15].ToString(),
                                _vuilwerk = meta[16].ToString(),
                                _passwoord = "",
                                _rechten = 0,
                                _reserve1 = "",
                                _reserve2 = "",
                                _reserve3 = "",
                                _reserve4 = "",
                                _reserve5 = ""
                            };
                            ProgData.LijstPersoneel.Add(p);
                        }
                        catch { }
                        read = reader.Read();
                    } while (read == true);
                }
                reader.Close();
            }
            ProgData.Save_LijstNamen();
        }

        private void CloseExitStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(300);
            //ProgData.CaptureMainScreen();
            Close();
        }

        private void RemoveAutoInlogOnderDitWindowsAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string autoinlogfile = $"{directory}\\bezetting2.log";
            if (File.Exists(autoinlogfile))
            {
                File.Delete(autoinlogfile);
            }
        }

        private void ImportNamenOudeVersieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("import oude namen?", "vraagje",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.Yes)
            {

                ProgData.LijstPersoneel.Clear();

                MessageBox.Show("Let op, alle oude personeel gaat weg, open Bezetting5ploegen....Bez");
                openFileDialog.FileName = "";
                openFileDialog.Filter = "(*.Bez)|*.Bez";

                result = openFileDialog.ShowDialog(); // Show the dialog.

                if (result == DialogResult.OK) // Test result.
                {
                    OpenDataBase_en_Voer_Oude_Namen_In(openFileDialog.FileName);
                    ProgData.Save_LijstNamen();
                }

                MessageBox.Show("Open nu bijbehorende Rechten en passwoorden, open Lock.mdb");

                openFileDialog.FileName = "Lock.mdb";
                openFileDialog.Filter = "(*.mdb)|*.mdb";
                result = openFileDialog.ShowDialog(); // Show the dialog.


                if (result == DialogResult.OK) // Test result.
                {
                    OpenDataBase_Wachtwoorden_en_Rechten(openFileDialog.FileName);
                }

                MessageBox.Show("Klaar, druk op refresh");
            }
        }

        private void OpenDataBase_Wachtwoorden_en_Rechten(string file)
        {
            using (OleDbConnection connection =
                new OleDbConnection($"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \"{file}\"; Jet OLEDB:Database Password = fcl721"))
            {
                bool read;

                OleDbCommand command = new OleDbCommand("select * from Tabel1", connection);

                connection.Open();
                OleDbDataReader reader = command.ExecuteReader();
                WindowUpdateViewScreen = false;

                ProgData.Laad_LijstNamen();

                if (reader.Read() == true)
                {
                    do
                    {
                        Application.DoEvents();
                        object[] meta = new object[12]; // zodat ze leeg zijn elke keer ivm vorige data
                                                        // inlezen waarden
                        _ = reader.GetValues(meta);

                        try
                        {
                            //							''  onderstaande geld als 1 is
                            //''  Rechten(1) = Mag alles															0 100
                            //''  Rechten(2) = Mag database nieuw jaar maken en Comprimeren enz
                            //''  Rechten(3) = Mag bij iedereen op zijn ploeg een telling vk's enz maken
                            //''  Rechten(4) = Mag op alle ploegen een telling maken
                            //''  Rechten(5) = Mag in maand wijzigen bij zich zelf									4 25
                            //''  Rechten(6) = Mag in maand wijzigen hele ploeg										5 50
                            //''  Rechten(7) = Mag in maand wijzigen alleen zijn werkplek
                            //''  Rechten(8) = Mag in maand wijzigen alle ploegen									7 100
                            //''  Rechten(9) = Mag in namen wijzigen alle ploegen									8 100
                            //''  Rechten(10) = Mag in namen wijzigen eigen ploeg									9 50
                            //''  Rechten(11) = Mag in naam van zichzelf wijzigen									10 25
                            //''  Rechten(12) = Mag met inlogen rechten zetten
                            //''  Rechten(13) = Mag voor iedereen rechten zetten Behalve Rechten(1)
                            //''  Rechten(14) = Mag op eigen ploeg rechten zetten en paswoord veranderen			13 50
                            //''  Rechten(15) = Vuilwerk uitdraaien
                            //''  Rechten(16) = vakantie vullen
                            //''  Rechten(17) = Mag op andere ploegen kijken										

                            string oud_recht = meta[1].ToString();
                            personeel persoon = ProgData.LijstPersoneel.First(b => b._persnummer.ToString() == meta[2].ToString());
                            string naam = persoon._achternaam;
                            int rechten = 0;
                            // mag alles in oude programma
                            // dus hier ook
                            if (oud_recht.Substring(0, 1) == "1")
                                rechten = 100;
                            // nog uitzoeken
                            if (oud_recht.Substring(4, 1) == "1")
                                if (rechten < 25) rechten = 25;
                            if (oud_recht.Substring(5, 1) == "1")
                                if (rechten < 50) rechten = 50;
                            if (oud_recht.Substring(7, 1) == "1")
                                if (rechten < 100) rechten = 100;
                            if (oud_recht.Substring(8, 1) == "1")
                                if (rechten < 100) rechten = 100;
                            if (oud_recht.Substring(9, 1) == "1")
                                if (rechten < 50) rechten = 50;
                            if (oud_recht.Substring(10, 1) == "1")
                                if (rechten < 25) rechten = 25;
                            if (oud_recht.Substring(13, 1) == "1")
                                if (rechten < 50) rechten = 50;

                            if (rechten > 0)
                                persoon._passwoord = ProgData.Scramble("verander_nu");
                            persoon._rechten = rechten;
                        }
                        catch { }

                        //meta[1] = rechten string
                        //meta[2] = personeel nummer


                        read = reader.Read();
                    } while (read == true);
                }
                reader.Close();
                ProgData.Save_LijstNamen();
            }
            WindowUpdateViewScreen = true;
        }

        private void EditPopupMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists("BezData\\popupmenu.ini"))
            {
                MaakLeegPopUpMenu();
            }
            Process.Start("BezData\\popupmenu.ini");
        }

        private void MaakLeegPopUpMenu()
        {
            File.Create("BezData\\popupmenu.ini").Dispose();

            List<string> PopUpNamen = new List<string>
            {
                "Deze regel en Wis niet aanpassen, Geen ED of RD invullen!",
                "WIS",
                "VK",
                "A",
                "8OI",
                "VRIJ",
                "VAK",
                "Z",
                "K",
                "GP",
                "*",
                "OPLO"
            };

            File.WriteAllLines("BezData\\popupmenu.ini", PopUpNamen);
        }

        private void DeleteDataDir(DateTime start, DateTime eind)
        {
            while (eind >= start)
            //for (int i = 0; i < aantal; i++)
            {
                start = start.AddMonths(1);
                string path = Path.GetFullPath($"{start.Year}\\{start.Month}"); // maand als nummer
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true); // delete met inhoud
                }
                labelDebug.Text = $"maak dir {path}";
                labelDebug.Refresh();
                _ = Directory.CreateDirectory(path);
                labelDebug.Text = $"vul met kleuren data : {path}";
                labelDebug.Refresh();
                /*
                ProgData.igekozenjaar = start.Year;
                ProgData.igekozenmaand = start.Month;
                ProgData.MaakPloegNamenLijst("Blauw");
                ProgData.SaveLijstPersoneelKleur("Blauw", 15);
                ProgData.MaakPloegNamenLijst("Rood");
                ProgData.SaveLijstPersoneelKleur("Rood", 15);
                ProgData.MaakPloegNamenLijst("Wit");
                ProgData.SaveLijstPersoneelKleur("Wit", 15);
                ProgData.MaakPloegNamenLijst("Groen");
                ProgData.SaveLijstPersoneelKleur("Groen", 15);
                ProgData.MaakPloegNamenLijst("Geel");
                ProgData.SaveLijstPersoneelKleur("Geel", 15);
                ProgData.MaakPloegNamenLijst("DD");
                ProgData.SaveLijstPersoneelKleur("DD", 15);
                */
            }
        }

        private void checkBoxHoverNaam_CheckedChanged(object sender, EventArgs e)
        {
            HoverTime.Visible = checkBoxHoverNaam.Checked;
        }

        private void MainFormBezetting2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                ButtonRefresh_Click(this, null);
        }

        private void UpdateExtraDienstLijst(string kleur)
        {
            // maak nieuwe extra diensten lijst

            DateTime dat = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
            string dir = ProgData.GetDirectoryBezettingMaand(dat);

            if (kleur == "Blauw")
            {
                ProgData.ListLooptExtra.Clear();
                ProgData.SaveLooptExtraLijst(dir, kleur);
                CheckExtraLopen("Rood",kleur);
                CheckExtraLopen("Geel", kleur);
                CheckExtraLopen("Wit", kleur);
                CheckExtraLopen("Groen", kleur);
                CheckExtraLopen("DD", kleur);
            }
            if (kleur == "Geel")
            {
                ProgData.ListLooptExtra.Clear();
                ProgData.SaveLooptExtraLijst(dir, kleur);
                CheckExtraLopen("Rood", kleur);
                CheckExtraLopen("Blauw", kleur);
                CheckExtraLopen("Wit", kleur);
                CheckExtraLopen("Groen", kleur);
                CheckExtraLopen("DD", kleur);
            }
            if (kleur == "Groen")
            {
                ProgData.ListLooptExtra.Clear();
                ProgData.SaveLooptExtraLijst(dir, kleur);
                CheckExtraLopen("Rood", kleur);
                CheckExtraLopen("Geel", kleur);
                CheckExtraLopen("Wit", kleur);
                CheckExtraLopen("Blauw", kleur);
                CheckExtraLopen("DD", kleur);
            }
            if (kleur == "Wit")
            {
                ProgData.ListLooptExtra.Clear();
                ProgData.SaveLooptExtraLijst(dir, kleur);
                CheckExtraLopen("Rood", kleur);
                CheckExtraLopen("Geel", kleur);
                CheckExtraLopen("Blauw", kleur);
                CheckExtraLopen("Groen", kleur);
                CheckExtraLopen("DD", kleur);
            }
            if (kleur == "Rood")
            {
                ProgData.ListLooptExtra.Clear();
                ProgData.SaveLooptExtraLijst(dir, kleur);
                CheckExtraLopen("Blauw", kleur);
                CheckExtraLopen("Geel", kleur);
                CheckExtraLopen("Wit", kleur);
                CheckExtraLopen("Groen", kleur);
                CheckExtraLopen("DD", kleur);
            }
            if (kleur == "DD")
            {
                ProgData.ListLooptExtra.Clear();
                ProgData.SaveLooptExtraLijst(dir, kleur);
                CheckExtraLopen("Rood", kleur);
                CheckExtraLopen("Geel", kleur);
                CheckExtraLopen("Wit", kleur);
                CheckExtraLopen("Groen", kleur);
                CheckExtraLopen("Blauw", kleur);
            }
        }

        private void CheckExtraLopen(string kleur,string Gaat_lopen_op_kleur)
        {
            ProgData.LaadLijstPersoneelKleur(kleur, 15);
            DateTime dat = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
            int aantal_dagen = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);

            foreach (personeel  pers in ProgData.LijstPersoneelKleur)
            {
                labelDebug.Text = $"{pers._achternaam}                           ";
                labelDebug.Refresh();

                dat = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, 1);
                for (int i = 0; i < aantal_dagen - 1; i++)
                {
                    string afwijking = ProgData.GetLaatsteAfwijkingPersoon(kleur, pers._persnummer.ToString(), dat);

                    string eerste_2 = afwijking.Length >= 2 ? afwijking.Substring(0, 2) : afwijking;

                    if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD" || eerste_2 == "DD")
                    {
                        var dienst = afwijking.Substring(3, 1);
                        var gaat_lopen_op_kleur = GetKleurDieWerkt(ProgData.GekozenRooster(), dat, dienst);
                        if (gaat_lopen_op_kleur == Gaat_lopen_op_kleur)
                        {
                            ProgData.VulInLooptExtraDienst(afwijking, dat, pers._achternaam);
                            Thread.Sleep(300);
                        }
                    }
                    dat = dat.AddDays(1);
                }
            }
        }

        private void updateExtraRuilVerschovenDienstenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help he = new Help();
            he.ShowDialog();
        }

        private void updateExtraDienstenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string message = "Als aantal extra diensten niet klopt op maand overzicht, kan je deze hertellen.\n" +
                "Dit duurt wel even (netwerk traag). U krijgt melding als programma klaar is.";
            const string caption = "Vraag updaten ?";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                UpdateExtraDienstLijst(ProgData.GekozenKleur);
                labelDebug.Text = "";
                labelDebug.Refresh();
                MessageBox.Show("Klaar");
            }
        }
    }
}