#define ketting

using Bezetting2.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using static Bezetting2.DatumVijfPloegUtils;

namespace Bezetting2
{
    public class ProgData
    {
        public static bool Disable_error_Meldingen = false;
        public static MainFormBezetting2 Main;

        public static bool ScreenCapture = true;

        public static ToolStripStatusLabel _inlognaam;
        public static ToolStripStatusLabel _toegangnivo;

        public static MaandDataClass MaandData = new MaandDataClass();

        public static PersoneelOverzicht AlleMensen = new PersoneelOverzicht();

        public static List<string> Lijnen = new List<string>();

        public static string _LooptExtra_Locatie;
        public static List<LooptExtraDienst> ListLooptExtra = new List<LooptExtraDienst>();
        private static DateTime saveTimeExtra = DateTime.Now;

        //public static List<werkdag> LijstWerkdagPloeg = new List<werkdag>();

        // kan later weg als alles omgezet is.
        public static List<veranderingen> ListVeranderingen = new List<veranderingen>();

        public static string _RuilExtra_Locatie;
        public static List<AanvraagRuilExtra> ListAanvraagRuilExtra = new List<AanvraagRuilExtra>();

        public static string _Snipper_Locatie;
        public static List<SnipperAanvraag> ListSnipperAanvraag = new List<SnipperAanvraag>();

        // public static string Huidige_Gebruiker_Werkt_Op_Kleur;

        public static int ihuidigemaand;

        public static int igekozenmaand;
        private static int BewaarMaand;

        public static int ihuidigjaar;
        private static int _igekozenjaar;  // Backing
        private static int BewaarJaar;

        public static string backup_zipnaam_huidige_dag;
        public static string backup_zipnaam_maand_verder;
        public static string backup_zipnaam_2maanden_verder;

        //public static int backup_time;

        public static int igekozenjaar
        {
            get { return _igekozenjaar; }  // Getter
            set
            {
                _igekozenjaar = value;   // Setter
                Main.numericUpDownJaar.Value = value;
            }
        }

        public static string GekozenRooster()
        {
            return InstellingenProg._Rooster;
        }

        public static string Sgekozenjaar()
        {
            return _igekozenjaar.ToString();
        }

        public static string Sgekozenmaand()
        {
            DateTime t = new DateTime(_igekozenjaar, igekozenmaand, 1);
            return t.ToString("MMMM");
        }

        public static string GekozenKleur; // Backing field

        public static string GetDir()
        {
            return Sgekozenjaar() + "\\" + igekozenmaand.ToString(); // maand als nummer
        }

        public static int RechtenHuidigeGebruiker
        {
            get
            {
                return Int32.Parse(_toegangnivo.Text);
            }

            set
            {
                _toegangnivo.Text = value.ToString();
            }
        }

        public static string Huidige_Gebruiker_Personeel_nummer
        {
            get
            {
                return _inlognaam.Text;
            }
            set
            {
                _inlognaam.Text = value;
                if (Main != null)
                {
                    string dirName = new DirectoryInfo(Directory.GetCurrentDirectory()).Name;
                    Main.Text = $"Bezetting 2.1   {dirName}   Ingelogd :   {Huidige_Gebruiker_Naam()} -- {_inlognaam.Text}";
                }

            }
        }

        public static string Huidige_Gebruiker_Naam()
        {
            try
            {
                int personeel_nr = int.Parse(_inlognaam.Text);
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer == personeel_nr);
                return persoon._achternaam;
            }
            catch
            {
                return "";
            }
        }

        //public static void SaveLijstWerkdagPloeg(string kleur, int try_again)
        //{
        //    if (try_again < 0 && !Disable_error_Meldingen)
        //    {
        //        MessageBox.Show($"SaveLijstWerkdagPloeg() error na 15 keer, \n{LijstWerkdagPloeg_Locatie(kleur)}");
        //    }

        //    if (!string.IsNullOrEmpty(kleur))
        //    {
        //        try
        //        {
        //            string file = LijstWerkdagPloeg_Locatie(kleur);
        //            using (Stream stream = File.Open(file, FileMode.OpenOrCreate))
        //            {
        //                BinaryFormatter bin = new BinaryFormatter();
        //                bin.Serialize(stream, LijstWerkdagPloeg);
        //            }
        //        }
        //        catch
        //        {
        //            Wacht(300, "SaveLijstWerkdagPloeg");
        //            //Thread.Sleep(300);
        //            SaveLijstWerkdagPloeg(kleur, --try_again);
        //        }
        //    }
        //}

        //public static void LaadLijstWerkdagPloeg(string kleur, int try_again)
        //{
        //    // Als LijstWerkdagPloeg_Locatie(kleur) niet bestaat, zou ik nieuwe kunnen maken,
        //    // maar wat als hij hem niet zou kunnen laden ?
        //    string file = LijstWerkdagPloeg_Locatie(kleur);
        //    if (!File.Exists(file))
        //    {
        //        if (TestNetwerkBeschikbaar(5) && ProgData.Main.WindowUpdateViewScreen)
        //        {
        //            MessageBox.Show($"Maak nieuwe werkdag maand voor kleur {kleur}\n" +
        //                $"{file}");
        //            MaakNieuwPloegBezettingAan(kleur);
        //        }
        //    }

        //    if (try_again < 0)
        //    {
        //        MessageBox.Show($"Tevaak is load ploeg bezetting laden niet gelukt, netwerk probleem ?\n" +
        //            $" {LijstWerkdagPloeg_Locatie(kleur)}");
        //    }

        //    try
        //    {
        //        using (Stream stream = File.Open(file, FileMode.Open))
        //        {
        //            BinaryFormatter bin = new BinaryFormatter();
        //            try
        //            {
        //                LijstWerkdagPloeg.Clear();
        //                LijstWerkdagPloeg = (List<werkdag>)bin.Deserialize(stream);
        //                stream.Dispose();
        //            }
        //            catch
        //            {
        //                MessageBox.Show("Deserialize(stream) LaadLijstWerkdagPloeg error");
        //            }
        //            finally
        //            {
        //                if (stream != null)
        //                    stream.Dispose();
        //            }
        //        }
        //    }
        //    catch (IOException)
        //    {
        //        Wacht(300, "LaadLijstWerkdagPloeg");
        //        //Thread.Sleep(300);
        //        LaadLijstWerkdagPloeg(kleur, --try_again);
        //    }
        //    catch
        //    {
        //        if (!Disable_error_Meldingen)
        //            MessageBox.Show($"LoadPloegBezettingLijst() error\n{LijstWerkdagPloeg_Locatie(ProgData.GekozenKleur)}");
        //    }
        //}

        public static int WaarInTijd()
        {
            // er zijn nu 3 mogelijkheden, welke afhankelijk ik andere keuze's maak
            // 1 ) gevraagde maand is in verleden van huidige maand
            // 2 ) gevraagde maand is huidige maand
            // 3 ) gevraagde maand is in toekomst

            // hier ging het fout soms, stel nu 31-8-2020 dan wordt gevraagd als + 1 maand 31-9-2020, en dat bestaat niet
            //DateTime gevraagd = new DateTime(igekozenjaar, igekozenmaand, DateTime.Now.Day, 0, 0, 0, 0, 0);
            DateTime gevraagd = new DateTime(igekozenjaar, igekozenmaand, 1, 0, 0, 0, 0, 0);
            DateTime nu = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, 0, 0);
            if (gevraagd < nu)
                return 1;
            if (gevraagd > nu)
                return 3;
            return 2;
        }

        /// <summary>
        /// Regel de afwijkingen van rooster
        /// </summary>
        /// <param name="naam">Naam van persoon</param>
        /// <param name="dagnr">nummer van de maand</param>
        /// <param name="afwijking">de afwijking</param>
        /// <param name="rede">de rede</param>
        /// <param name="invoerdoor">ingevoerd door</param>
        static public void RegelAfwijking(string personeel_nr, string dagnr, string afwijking, string rede, string invoerdoor, string kleur)
        {
            // bestaat kleur en maand jaar file's ?
            CheckFiles(kleur);

            MaandData.Load(kleur);
            MaandData.Voeg_toe(dagnr, personeel_nr, afwijking, invoerdoor, rede, "", "");
            MaandData.Save(kleur, 15);
        }

        static public void RegelAfwijkingOpDatumEnKleur(DateTime datum, string kleur, string personeel_nr, string dagnr, string afwijking, string rede, string invoerdoor, bool Update_screen = true)
        {
            Main.WindowUpdateViewScreen = Update_screen;
            // zet datum goed en kleur goed
            string bewaar_kleur = ProgData.GekozenKleur; // ???
            int bewaar_maand = igekozenmaand;
            int bewaar_jaar = igekozenjaar;
            // zet nieuwe kleur en datum
            ProgData.GekozenKleur = kleur;
            igekozenmaand = datum.Month;
            igekozenjaar = datum.Year;

            // roep afwijking roetine aan
            RegelAfwijking(personeel_nr, dagnr, afwijking, rede, invoerdoor, kleur);
            // datum terug en kleur goed
            GekozenKleur = bewaar_kleur;
            igekozenmaand = bewaar_maand;
            igekozenjaar = bewaar_jaar;
            Main.WindowUpdateViewScreen = true;
        }

        static public String Scramble(string woord)
        {
            // string test = unscramble("twtvfw8;");
            string ret = "";
            for (int h = 0; h < woord.Length; h++)
            {
                int xx22 = 0;
                xx22 += 2;
                xx22 += woord.Length - h - 1;
                //xx22 += xx22 = (int)woord[h];
                xx22 += (int)woord[h];
                char character = (char)xx22;
                ret += character.ToString();
            }
            return ret;
        }

        static public string Unscramble(string woord)
        {
            string ret = "";
            if (woord == null || woord.Length < 1)
                return ret;
            for (int h = 0; h < woord.Length; h++)
            {
                int xx22;// = 0;
                xx22 = (int)woord[h];
                xx22 -= 2;
                xx22 -= woord.Length - h - 1;
                char character = (char)xx22;
                ret += character.ToString();
            }
            return ret;
        }

        static public bool LeesLijnen()
        {
            Lijnen.Clear();
            try
            {
                Lijnen = File.ReadAllLines(Lijnen_Locatie()).ToList();
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        static public void SaveLijnen()
        {
            try
            {
                File.WriteAllLines(Lijnen_Locatie(), Lijnen);
            }
            catch (IOException)
            {
                MessageBox.Show("SaveLijnen Error()");
            }
        }

        public static void LoadExtraRuilLijst(string dir)
        {
            ListAanvraagRuilExtra.Clear();
            _RuilExtra_Locatie = Path.GetFullPath(dir + "\\ruilextra.bin");
            try
            {
                using (Stream stream = File.Open(_RuilExtra_Locatie, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    ListAanvraagRuilExtra = (List<AanvraagRuilExtra>)bin.Deserialize(stream);
                }
            }
            catch { }
        }

        public static void SaveExtraRuilLijst(string dir)
        {
            _RuilExtra_Locatie = Path.GetFullPath(dir + "\\ruilextra.bin");
            try
            {
                using (Stream st = File.Open(_RuilExtra_Locatie, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(st, ListAanvraagRuilExtra);
                }
            }
            catch
            {
                MessageBox.Show("SaveExtraRuilLijst() error");
            }
        }

        public static void LoadSnipperLijst(string dir)
        {
            ListSnipperAanvraag.Clear();
            _Snipper_Locatie = Path.GetFullPath(dir + "\\snipper.bin");
            try
            {
                using (Stream stream = File.Open(_Snipper_Locatie, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    ListSnipperAanvraag = (List<SnipperAanvraag>)bin.Deserialize(stream);
                }
            }
            catch { }
        }

        public static void SaveSnipperLijst(string dir)
        {
            _Snipper_Locatie = Path.GetFullPath(dir + "\\snipper.bin");
            try
            {
                using (Stream st = File.Open(_Snipper_Locatie, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(st, ListSnipperAanvraag);
                }
            }
            catch
            {
                MessageBox.Show("SaveSnipperLijst() error");
            }
        }

        public static void LoadLooptExtraLijst(string dir, string kleur)
        {
            ListLooptExtra.Clear();
            _LooptExtra_Locatie = Path.GetFullPath(dir + "\\" + kleur + "_extra.bin");
            try
            {
                using (Stream stream = File.Open(_LooptExtra_Locatie, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    ListLooptExtra = (List<LooptExtraDienst>)bin.Deserialize(stream);
                }
            }
            catch { }
        }

        public static void SaveLooptExtraLijst(string dir, string kleur)
        {
            // als ik hier snel weer kom ( < 2 sec ?), dan even wachten ivm netwerk traagheid
            var diffInSeconds = (DateTime.Now - saveTimeExtra).TotalSeconds;
            if (diffInSeconds < 1)
            {
                Wacht(500, "SaveLooptExtraLijst");
            }
            //Thread.Sleep(500);

            _LooptExtra_Locatie = Path.GetFullPath(dir + "\\" + kleur + "_extra.bin");
            try
            {
                using (Stream st = File.Open(_LooptExtra_Locatie, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(st, ListLooptExtra);
                }
                saveTimeExtra = DateTime.Now;
            }
            catch
            {
                MessageBox.Show("SaveLooptExtraLijst() error");
            }
        }

        public static string GetDirectoryBezettingMaand(DateTime datum)
        {
            string dat = datum.ToString("dd/MM/yyyy");
            string jaar = dat.Substring(6, 4);
            string maand = dat.Substring(3, 2);
            if (maand.Substring(0, 1) == "0")
                maand = maand.Substring(1, 1);

            return jaar + "\\" + maand;
        }

        public static string GetDirectoryBezettingMaand(string datum)
        {
            string jaar = datum.Substring(6, 4);
            string maand = datum.Substring(3, 2);
            if (maand.Substring(0, 1) == "0")
                maand = maand.Substring(1, 1);
            return jaar + "\\" + maand;
        }

        //public static string LijstWerkdagPloeg_Locatie(string kleur)
        //{
        //    return Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\{kleur}_bezetting.bin");
        //}

        public static string Ploeg_Namen_Locatie(string kleur)
        {
            return Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\{kleur}_namen.bin");
        }

        public static string Lijnen_Locatie()
        {
            return Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\lijnen.ini");
        }

        public static string GetLocatieOverzichtPlaatje(DateTime datum)
        {
            // datum bv 18-8-2020
            return Path.GetFullPath($"{datum.Year}\\{datum.Month}\\{datum.Day}_overzicht_{GekozenKleur}.jpg");
        }

        public static void CaptureMainScreen()
        {
            try
            {
                var _ScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                var _ScreenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

                if (ScreenCapture && !string.IsNullOrEmpty(GekozenKleur) && _ScreenWidth > 1850 && _ScreenHeight > 1000)
                {
                    Rectangle bounds = ProgData.Main.Bounds;
                    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                        }
                        // haal data uit plaatje, kan niet uit imaand bv, want na opslag wordt pas nieuwe maand getekend.
                        string maand = Main.View.Items[2].SubItems[0].Text;
                        string jaar = Main.View.Items[3].SubItems[0].Text;
                        int imaand = DateTime.ParseExact(maand, "MMMM", CultureInfo.CurrentCulture).Month;
                        string kleur = Main.View.Items[4].SubItems[0].Text;
                        string opslag = Path.GetFullPath($"{jaar}\\{imaand}\\maand_overzicht_{kleur}.jpg");
                        bitmap.Save(opslag, ImageFormat.Jpeg);

                        // als plaatje is van huidige maand, opslaan
                        if (DateTime.Now.Month == imaand && DateTime.Now.Year.ToString() == jaar)
                        {
                            // voor als er problemen zijn met data ;-)
                            opslag = Path.GetFullPath($"{jaar}\\{imaand}\\maand_overzicht_{kleur}_{DateTime.Now.ToShortDateString()}.jpg");
                            bitmap.Save(opslag, ImageFormat.Jpeg);
                        }
                    }
                }
            }
            catch { }
        }

        public static bool Bestaat_Gebruiker(string nummer)
        {
            try
            {
                int personeel_nr = int.Parse(nummer);
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer == personeel_nr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Get_Gebruiker_Kleur(string nummer, DateTime start = default)
        {
            if (start == default)
                start = DateTime.Now;
            try
            {
                int personeel_nr = int.Parse(nummer);
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer == personeel_nr);

                if (!string.IsNullOrEmpty(persoon._nieuwkleur))
                    if (persoon._verhuisdatum <= start)
                        return persoon._nieuwkleur;

                return persoon._kleur;
            }
            catch
            {
                return "";
            }
        }

        public static string Get_Gebruiker_Naam(string nummer)
        {
            try
            {
                int personeel_nr = int.Parse(nummer);
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer == personeel_nr);
                return persoon._achternaam;
            }
            catch
            {
                return nummer;
            }
        }

        public static string Get_Gebruiker_Naam_NieuweKleur(string nummer)
        {
            try
            {
                int personeel_nr = int.Parse(nummer);
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer == personeel_nr);
                return persoon._nieuwkleur;
            }
            catch
            {
                return "";
            }
        }

        public static string Get_Gebruiker_Nummer(string naam)
        {
            try
            {
                //int personeel_nr = int.Parse(nummer);
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._achternaam == naam);
                return persoon._persnummer.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static void CheckFiles(string kleur)
        {

            if (TestNetwerkBeschikbaar(15))
            {
                string path = Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}");
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("Maak nieuwe directory aan, met inhoud");
                    // directory betond niet, dus aanmaken
                    _ = Directory.CreateDirectory(path);
                    // dir aangemaakt, dus weet zeker geen maand data
                    MaandData.SaveLeegPloeg("Blauw");
                    MaandData.SaveLeegPloeg("Rood");
                    MaandData.SaveLeegPloeg("Geel");
                    MaandData.SaveLeegPloeg("Groen");
                    MaandData.SaveLeegPloeg("Wit");
                    MaandData.SaveLeegPloeg("DD");
                }

                var path_oude_werkdag = Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\{kleur}_bezetting.bin");
                if (File.Exists(path_oude_werkdag))
                    File.Delete(path_oude_werkdag);

                // kan in toekomst ooit weg
                // Zekker tot start 2023 laten staan!
                if (File.Exists(Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\{kleur}_afwijkingen.bin")))
                    Zetom_naar_versie21(kleur);
            }
        }
        public static void Backup()
        {
            try
            {
                DateTime backup = DateTime.Now;

                Main.DebugWrite("Backup huidige maand, moment.....");


                backup_zipnaam_huidige_dag = $"Backup\\GemaaktOpDag_{backup.Day}.zip";
                string startPath = GetDirectoryBezettingMaand(backup);
                SchijfLocatieVanBackup(startPath, backup_zipnaam_huidige_dag);
                if (File.Exists(backup_zipnaam_huidige_dag))
                    File.Delete(backup_zipnaam_huidige_dag);
                ZipFile.CreateFromDirectory(startPath, backup_zipnaam_huidige_dag);
                if (File.Exists($"{startPath}\\locatie.txt"))
                    File.Delete($"{startPath}\\locatie.txt");

                Main.DebugWrite("Backup volgende maand, moment.....");

                backup = backup.AddMonths(1);
                ProgData.backup_zipnaam_maand_verder = $"Backup\\maand_{backup.Month}.zip";
                startPath = GetDirectoryBezettingMaand(backup);
                if (File.Exists(backup_zipnaam_maand_verder))
                    File.Delete(backup_zipnaam_maand_verder);
                ZipFile.CreateFromDirectory(startPath, backup_zipnaam_maand_verder);


                Main.DebugWrite("Backup 2 maanden in toekomst, moment.....");
                backup = backup.AddMonths(1);
                ProgData.backup_zipnaam_2maanden_verder = $"Backup\\maand_{backup.Month}.zip";
                startPath = GetDirectoryBezettingMaand(backup);
                if (File.Exists(backup_zipnaam_2maanden_verder))
                    File.Delete(backup_zipnaam_2maanden_verder);
                ZipFile.CreateFromDirectory(startPath, backup_zipnaam_2maanden_verder);

                // maak ploeg namen op schijf.

                // gaat fout op bv 1 april 00:00 dan komen namen van april na verhuizing in
                // maart, en dan mag niet.

                // los ik op door op dag 1 en laatste maand eea niet op te slaan.

                ProgData.SaveDatum();

                backup = DateTime.Now;
                ProgData.igekozenjaar = backup.Year;
                ProgData.igekozenmaand = backup.Month;

                int aantal_dagen_dezemaand = DateTime.DaysInMonth(igekozenjaar, igekozenmaand);

                if (!(backup.Day == 1 || backup.Day == aantal_dagen_dezemaand))
                {

                    Main.DebugWrite("Backup Blauw namen , moment.....");
                    ProgData.AlleMensen.HaalPloegNamenOpKleur("Blauw");
                    ProgData.AlleMensen.BewaarPloegNamenOpKleurOpSchijf("Blauw", 15);

                    Main.DebugWrite("Backup Groen namen , moment.....");
                    ProgData.AlleMensen.HaalPloegNamenOpKleur("Groen");
                    ProgData.AlleMensen.BewaarPloegNamenOpKleurOpSchijf("Groen", 15);

                    Main.DebugWrite("Backup Wit namen , moment.....");
                    ProgData.AlleMensen.HaalPloegNamenOpKleur("Wit");
                    ProgData.AlleMensen.BewaarPloegNamenOpKleurOpSchijf("Wit", 15);

                    Main.DebugWrite("Backup Geel namen , moment.....");
                    ProgData.AlleMensen.HaalPloegNamenOpKleur("Geel");
                    ProgData.AlleMensen.BewaarPloegNamenOpKleurOpSchijf("Geel", 15);

                    Main.DebugWrite("Backup Rood namen , moment.....");
                    ProgData.AlleMensen.HaalPloegNamenOpKleur("Rood");
                    ProgData.AlleMensen.BewaarPloegNamenOpKleurOpSchijf("Rood", 15);

                    Main.DebugWrite("Backup Dagdienst namen , moment.....");
                    ProgData.AlleMensen.HaalPloegNamenOpKleur("DD");
                    ProgData.AlleMensen.BewaarPloegNamenOpKleurOpSchijf("DD", 15);
                }
                ProgData.ReturnDatum();
            }
            catch
            {
                MessageBox.Show("Backup error, meld dit bij Admin!");
            }
        }
        public static void NachtErVoorVrij(string gekozen_naam, string dagnr, string afwijking)
        {
            // kijk of afwijking op vrije dag was, en dag ervoor Nacht, dan 
            if (GekozenRooster() != "DD")
            {
                DateTime dag_er_voor = new DateTime(igekozenjaar, igekozenmaand, int.Parse(dagnr));
                dag_er_voor = dag_er_voor.AddDays(-1);
                if (/*MDatum.*/GetDienst(GekozenRooster(), dag_er_voor, GekozenKleur) == "N")
                {
                    if (afwijking == "VK" || afwijking == "8OI" || afwijking == "A" ||
                        afwijking == "VRIJ" || afwijking == "VAK" || afwijking == "VF"
                        || afwijking == "ED-N" || afwijking == "Z" || afwijking == "*"
                        || afwijking == "GP")
                    {

                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("Nacht er voor VRIJ zetten?", "Vraagje", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            igekozenmaand = dag_er_voor.Month;
                            igekozenjaar = dag_er_voor.Year;

                            ProgData.RegelAfwijking(ProgData.Get_Gebruiker_Nummer(gekozen_naam), dag_er_voor.Day.ToString(), "VRIJ", "IVM WERKDAG MORGEN", "Rooster Regel", ProgData.GekozenKleur);

                            dag_er_voor = dag_er_voor.AddDays(1);
                            igekozenmaand = dag_er_voor.Month;
                            igekozenjaar = dag_er_voor.Year;

                        }
                    }
                }
            }
        }

        public static bool TestNetwerkBeschikbaar(int test)
        {
            if (test == 0)
            {
                MessageBox.Show("Kan niet schrijven en/of lezen op locatie, netwerk problemen ?, Exit");
                Process.GetCurrentProcess().Kill();
                return false;
            }

            try
            {
                File.WriteAllText("TestNetWerk.txt", "");
                if (File.Exists("TestNetWerk.txt"))
                {
                    File.Delete("TestNetWerk.txt");
                    return true;
                }
            }
            catch
            {
                Wacht(300, "TestNetwerkBeschikbaar");
                //Thread.Sleep(300);
                TestNetwerkBeschikbaar(test--);
            }
            MessageBox.Show("Kan niet schrijven en/of lezen op locatie, netwerk problemen ?, Exit");
            Process.GetCurrentProcess().Kill();
            return false;
        }
        public static void VulInLooptExtraDienst(string afwijking, DateTime _verzoekdag, string naam)
        {
            // als ED-O of ED-M of ED-N aanpassing op andere kleur, of VD of RD
            // bepaal de kleur die dan loopt.

            string dienst;
            string gaat_lopen_op_kleur;
            string dir;
            if (afwijking.ToUpper() == "DD")
            {
                dienst = afwijking.ToUpper();
                gaat_lopen_op_kleur = "DD";
                dir = ProgData.GetDirectoryBezettingMaand(_verzoekdag);
            }
            else
            {
                // get huidige kleur op
                dienst = afwijking.Substring(3, 1);
                gaat_lopen_op_kleur = GetKleurDieWerkt(ProgData.GekozenRooster(), _verzoekdag, dienst);
                dir = ProgData.GetDirectoryBezettingMaand(_verzoekdag);
            }

            ProgData.LoadLooptExtraLijst(dir, gaat_lopen_op_kleur);

            LooptExtraDienst lop = new LooptExtraDienst
            {
                _datum = _verzoekdag,
                _naam = naam,
                _metcode = afwijking
            };

            if (!ListLooptExtra.Contains(lop)) // neem soort niet mee in vergelijking, of het een ED of VD is niet belangrijk
            {
                ProgData.ListLooptExtra.Add(lop);
                ProgData.SaveLooptExtraLijst(dir, gaat_lopen_op_kleur);
            }
            else
            {
                // naam en datum staat al in lijst, maar VD is bv veranderd in ED
                // verwijder dus eerst die oude, en plaats dan nieuwe
                for (int i = ProgData.ListLooptExtra.Count - 1; i >= 0; i--)
                {
                    if (ProgData.ListLooptExtra[i]._naam == lop._naam && ProgData.ListLooptExtra[i]._datum == lop._datum)
                        ProgData.ListLooptExtra.RemoveAt(i);
                }
                ProgData.ListLooptExtra.Add(lop);
                ProgData.SaveLooptExtraLijst(dir, gaat_lopen_op_kleur);
            }
        }
        //public static void MaakNieuweCollegaInBezettingAan(string Naam, string Kleur, int jaar, int maand, int aantal_maanden)
        //{
        //    // als bij verhuizing er een persoon bij komt, moet in deze toevoegen aan ListWerkdagPloeg,
        //    // echter alleen als deze nog niet bestaat.
        //    // check of naam er in zit, en op die dag van de maand.

        //    DateTime dumm = new DateTime(jaar, maand, 1);

        //    int save_jaar = igekozenjaar;
        //    int save_maand = igekozenmaand;

        //    igekozenjaar = jaar;
        //    igekozenmaand = maand;

        //    for (int m = 0; m < aantal_maanden; m++)
        //    {
        //        if (m != 0)
        //            dumm = dumm.AddMonths(1);

        //        igekozenjaar = dumm.Year;
        //        igekozenmaand = dumm.Month;

        //        int aantal_dagen_dezemaand = DateTime.DaysInMonth(igekozenjaar, igekozenmaand);

        //        ProgData.LaadLijstWerkdagPloeg(Kleur, 15);

        //        for (int i = 1; i < aantal_dagen_dezemaand + 1; i++)
        //        {
        //            DateTime dat = new DateTime(igekozenjaar, igekozenmaand, i);

        //            try
        //            {
        //                werkdag ver = LijstWerkdagPloeg.First(x => (x._naam == Naam && x._dagnummer == i));
        //            }
        //            catch
        //            {
        //                werkdag dag = new werkdag
        //                {
        //                    _naam = Naam,
        //                    _standaarddienst = GetDienst(InstellingenProg._Rooster, dat, Kleur),
        //                    _werkplek = "",
        //                    _afwijkingdienst = "",
        //                    _dagnummer = i
        //                };
        //                ProgData.LijstWerkdagPloeg.Add(dag);
        //            }
        //        }
        //        ProgData.SaveLijstWerkdagPloeg(Kleur, 15);
        //    }
        //    igekozenjaar = save_jaar;
        //    igekozenmaand = save_maand;
        //}
        public static string GetLaatsteAfwijkingPersoon(string loopt_op_kleur, string persnr, DateTime datum)
        {
            string ret = "";
            MaandData.Load(loopt_op_kleur);
            for (int i = MaandData.MaandDataLijst.Count - 1; i >= 0; i--)
            {
                if (MaandData.MaandDataLijst[i].personeel_nr_ == persnr && MaandData.MaandDataLijst[i].datum_ == datum)
                {
                    return MaandData.MaandDataLijst[i].afwijking_;
                }
            }
            return ret;
        }
        public static void SaveDatum()
        {
            BewaarJaar = igekozenjaar;
            BewaarMaand = igekozenmaand;
        }
        public static void ReturnDatum()
        {
            igekozenjaar = BewaarJaar;
            igekozenmaand = BewaarMaand;
        }
        //public static void MaakNieuwPloegBezettingAan(string kleur)
        //{
        //    string file = LijstWerkdagPloeg_Locatie(kleur);
        //    LijstWerkdagPloeg.Clear();
        //    ProgData.AlleMensen.HaalPloegNamenOpKleur(kleur);
        //    int aantal_dagen_deze_maand = DateTime.DaysInMonth(igekozenjaar, igekozenmaand);
        //    DateTime dat = new DateTime(igekozenjaar, igekozenmaand, 1);

        //    // bij nieuwe maand aanmaken in zelfde maand ging het dus fout ?
        //    // als dat is 1-4-21 en now was 30-4-21 ging het fout, mag dan wel aanmaken
        //    DateTime VorigeMaand = DateTime.Now.AddMonths(-1);
        //    if (dat > VorigeMaand)
        //    {
        //        foreach (personeel a in ProgData.AlleMensen.LijstPersoonKleur)
        //        {
        //            for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
        //            {
        //                dat = new DateTime(igekozenjaar, igekozenmaand, i);
        //                try
        //                {
        //                    werkdag ver = LijstWerkdagPloeg.First(x => (x._naam == a._achternaam && x._dagnummer == i));
        //                }
        //                catch
        //                {
        //                    werkdag dag = new werkdag
        //                    {
        //                        _naam = a._achternaam,
        //                        _standaarddienst = GetDienst(InstellingenProg._Rooster, dat, kleur),
        //                        _werkplek = "",
        //                        _afwijkingdienst = "",
        //                        _dagnummer = i
        //                    };
        //                    ProgData.LijstWerkdagPloeg.Add(dag);
        //                }
        //            }
        //            ProgData.SaveLijstWerkdagPloeg(kleur, 15);
        //        }
        //        SaveLijstWerkdagPloeg(kleur, 15);
        //    }
        //}
        public static void Zetom_naar_versie21(string kleur)
        {
            // zet oude file's om naar nieuwe ketting
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.igekozenjaar;

            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_Maand_Data.bin");
            var path_oud = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_afwijkingen.bin");
            

            if (!File.Exists(path))
            {
                if (File.Exists(path_oud))
                {
                    AlleMensen.Load();  // nodig voor personeel nummer te krijgen hieronder

                    try
                    {
                        using (Stream stream = File.Open(path_oud, FileMode.Open))
                        {
                            BinaryFormatter bin = new BinaryFormatter();
                            ListVeranderingen.Clear();
                            ListVeranderingen = (List<veranderingen>)bin.Deserialize(stream);
                        }
                    }
                    catch { }

                    if (ListVeranderingen.Count == 0)
                    {
                        MaandData.SaveLeegPloeg(kleur);
                    }
                    else
                    {
                        MaandData.MaandDataLijst.Clear();
                        foreach (veranderingen verander in ProgData.ListVeranderingen)
                        {
                            // verander
                            var personeel_nummer = Get_Gebruiker_Nummer(verander._naam);

                            if (!string.IsNullOrEmpty(personeel_nummer))
                            {
                                MaandData.Voeg_toe(verander._datumafwijking,
                                    personeel_nummer, verander._afwijking, verander._invoerdoor, verander._rede, "", "");
                                MaandData.VeranderInvoerDatum(verander._datuminvoer);
                                MaandData.Save(kleur, 15);
                            }
                        }
                    }
                    File.Delete(path_oud);
                }
            }
            else // als nieuwe bestaat kijken of oude bestaat en opruimen! , zou niet mogen, maar oke.
            {
                if (File.Exists(path_oud))
                {
                    MessageBox.Show($"Delete {path_oud}");                    
                    File.Delete(path_oud);
                }
            }

            
        }
        private static void SchijfLocatieVanBackup(string startPath, string filenaam)
        {
            // maak in backup dir een bestand locatie.txt
            // met inhoud startPath

            try
            {
                string helptext = $"{startPath}\\locatie.txt";

                if (File.Exists(helptext))
                {
                    File.Delete(helptext);
                }

                File.Create(helptext).Dispose();

                List<string> loc = new List<string>();

                loc.Add($"Backup met de naam {filenaam},");
                loc.Add($"Is gemaakt vanuit directory {startPath}");

                File.WriteAllLines(helptext, loc);
            }
            catch { }
        }
        public static string Huidige_Gebruiker_Werkt_Op_Kleur()
        {
            return Get_Gebruiker_Kleur(Huidige_Gebruiker_Personeel_nummer);
        }
        public static void Wacht(int tijd, string hulptext)
        {
            // daar nu gehele scherm wordt geupdate,
            // wat ook tijd kost, verlaag ik de tijd
            if (tijd > 100) tijd -= 100;

            Main.panelMoment.Visible = true;
            Main.labelHulpText.Text = hulptext;
            Main.Refresh();
            Thread.Sleep(tijd);
            System.Windows.Forms.Application.DoEvents();
            Main.panelMoment.Visible = false;
            Main.Refresh();
        }
    }
}

