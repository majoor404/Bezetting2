using Bezetting2.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

/*****************************************************************************

SavePloegBezetting(string kleur)
LoadPloegBezetting(string kleur)
ListWerkdagPloeg
Ploeg_Bezetting_Locatie(kleur)	\\{kleur}_bezetting.bin"

LoadPloegNamenLijst()
SavePloegNamenLijst()
ListPersoneelKleur
Ploeg_Namen_Locatie();		\\{GekozenKleur}_namen.bin"

LoadVeranderingenPloeg()
SaveVeranderingenPloeg()
ListVeranderingen
Ploeg_Veranderingen_Locatie()	\\{GekozenKleur}_afwijkingen.bin"

Save_Namen_lijst()
Lees_Namen_lijst()
ListPersoneel
				"personeel.bin"

 * ****************************************************************************/

namespace Bezetting2
{
    public class ProgData
    {
        public enum Kleur
        {
            Blauw,
            Wit,
            Geel,
            Groen,
            Rood
        }

        private static bool ChangeData = false;
        public static bool Disable_error_Meldingen = false;
        public static MainFormBezetting2 Main;

        public static ToolStripStatusLabel _inlognaam;
        public static ToolStripStatusLabel _toegangnivo;

        public static List<string> Lijnen = new List<string>();

        public static List<personeel> ListPersoneel = new List<personeel>();
        public static List<personeel> ListPersoneelKleur = new List<personeel>();
        public static string ReloadSpeed1 = "";
        public static string ReloadSpeed2 = "";

        public static string _LooptExtra_Locatie;
        public static List<LooptExtraDienst> ListLooptExtra = new List<LooptExtraDienst>();

        public static List<string> ListWerkgroepPersoneel = new List<string>();
        public static ModuleDatum MDatum = new ModuleDatum();

        public static List<veranderingen> ListVeranderingen = new List<veranderingen>();

        public static List<werkdag> ListWerkdagPloeg = new List<werkdag>();

        public static string _RuilExtra_Locatie;
        public static List<AanvraagRuilExtra> ListAanvraagRuilExtra = new List<AanvraagRuilExtra>();

        public static string _Snipper_Locatie;
        public static List<SnipperAanvraag> ListSnipperAanvraag = new List<SnipperAanvraag>();

        public static string Huidige_Gebruiker_Werkt_Op_Kleur;

        public static int ihuidigemaand;
        public static int igekozenmaand;

        public static int ihuidigjaar;
        private static int _igekozenjaar; // Backing

        //public static class ZipFile;
        public static string backup_zipnaam;

        public static int backup_time;

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

        public static string sgekozenjaar()
        {
            return _igekozenjaar.ToString();
        }

        public static string sgekozenmaand()
        {
            DateTime t = new DateTime(_igekozenjaar, igekozenmaand, 1);
            return t.ToString("MMMM");
        }

        public static string GekozenKleur; // Backing field

        public static string GetDir()
        {
            return sgekozenjaar() + "\\" + igekozenmaand.ToString(); // maand als nummer
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
            }
        }

        public static string Huidige_Gebruiker_Naam()
        {
            try
            {
                int personeel_nr = int.Parse(_inlognaam.Text);
                personeel persoon = ListPersoneel.First(a => a._persnummer == personeel_nr);
                return persoon._achternaam;
            }
            catch
            {
                return "";
            }
        }

        public static void LoadPloegNamenLijst(int try_again)
        {
            if (try_again < 0)
            {
                MessageBox.Show("LoadPloegNamenLijst() lukt niet!, te vaak geprobeerd.");
            }
            Main.labelDebug.Text = "Load Ploeg Namen Lijst";
            ListPersoneelKleur.Clear();
            try
            {
                //string _Ploeg_Namen_Locatie = Path.GetFullPath(GetDir() + "\\" + _GekozenKleur + "_namen.bin");
                using (Stream stream = File.Open(Ploeg_Namen_Locatie(), FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    ListPersoneelKleur = (List<personeel>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
                Main.labelDebug.Text = "catch LoadPloegNamenLijst() error, try again";
                Thread.Sleep(300);
                LoadPloegNamenLijst(try_again--);
            }
            // haal werkgroepen op
            MaakWerkPlekkenLijst();
            Main.labelDebug.Text = "";
        }

        public static void SavePloegNamenLijst(int try_again)
        {
            if (try_again < 0 && !Disable_error_Meldingen)
            {
                MessageBox.Show("SavePloegNamenLijst() error na 30 keer, " + Ploeg_Namen_Locatie());
            }
            Main.labelDebug.Text = "Save Ploeg Namen Lijst";
            if (GekozenKleur != "")
            {
                try
                {
                    string Locatie = Ploeg_Namen_Locatie();
                    using (Stream stream = File.Open(Locatie, FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, ListPersoneelKleur);
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(300);
                    SavePloegNamenLijst(try_again--);
                }
            }
        }

        public static void MaakPloegNamenLijst(string kleur)
        {
            //DateTime dezemaand = new DateTime(igekozenjaar, igekozenmaand, 1, 0, 0, 0, 0, 0);
            ListPersoneelKleur.Clear();

            // query om juiste mensen te vinden
            IEnumerable<personeel> ploeg_gekozen = from a in ListPersoneel
                                                   where (a._kleur == kleur) || (a._nieuwkleur == kleur)
                                                   select a;

            foreach (personeel a in ploeg_gekozen)
            {
                if (a._nieuwkleur != "" && kleur != a._nieuwkleur)
                {
                    //als nieuwkleur aanwezig, kijk of hij in die bezetting_Ploeg_lijst voorkomt,
                    //regel dat anders, als file niet bestaat, gaat dat vanzelf
                    if (File.Exists(Ploeg_Bezetting_Locatie(a._nieuwkleur)))
                    {
                        LoadPloegBezetting(a._nieuwkleur, 30);
                        // check of naam er in zit
                        try
                        {
                            werkdag ver = ListWerkdagPloeg.First(x => (x._naam == a._achternaam));
                        }
                        catch
                        {
                            // deze persoon bestaat niet in bezetting, dus toevoegen
                            // elke dag in deze maand
                            int aantal_dagen_deze_maand = DateTime.DaysInMonth(igekozenjaar, igekozenmaand);
                            for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
                            {
                                DateTime dat = new DateTime(igekozenjaar, igekozenmaand, i);
                                werkdag dag = new werkdag();
                                dag._naam = a._achternaam;
                                dag._standaarddienst = MDatum.GetDienst(GekozenRooster(), dat, a._nieuwkleur);
                                dag._werkplek = "";
                                dag._afwijkingdienst = "";
                                dag._dagnummer = i;
                                ListWerkdagPloeg.Add(dag);
                            }
                            SavePloegBezetting(a._nieuwkleur,30);
                        }
                    }
                }
                ListPersoneelKleur.Add(a);
            }

            //kleur_personeel.Sort();
            // sorteer kleur_personeel
            ListPersoneelKleur.Sort(delegate (personeel x, personeel y)
            {
                return x._achternaam.CompareTo(y._achternaam);
            });

            // haal werkgroepen op
            MaakWerkPlekkenLijst();

            // bij een ploegnamen lijst hoort een bezetting lijst
            if (!File.Exists(Ploeg_Bezetting_Locatie(kleur)))
            {
                MaakLegeBezetting(sgekozenjaar(), igekozenmaand.ToString(), GekozenKleur); // in deze roetine wordt het ook opgeslagen
            }
        }

        public static void MaakLegeBezetting(string jaar, string maand, string kleur)
        {
            ListWerkdagPloeg.Clear();
            // elke dag in deze maand
            int aantal_dagen_deze_maand = DateTime.DaysInMonth(igekozenjaar, igekozenmaand);
            // elke persoon
            foreach (personeel a in ListPersoneelKleur)
            {
                for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
                {
                    DateTime dat = new DateTime(igekozenjaar, igekozenmaand, i);
                    werkdag dag = new werkdag();
                    dag._naam = a._achternaam;
                    dag._standaarddienst = MDatum.GetDienst(GekozenRooster(), dat, a._kleur);
                    dag._werkplek = "";
                    dag._afwijkingdienst = "";
                    dag._dagnummer = i;
                    ListWerkdagPloeg.Add(dag);
                }
            }
            SavePloegBezetting(kleur,30);

            // bij nieuwe bezetting hoort ook een nieuwe verander lijst
            ListVeranderingen.Clear();
            SaveVeranderingenPloeg(30);
        }

        public static void SavePloegBezetting(string kleur, int try_again)
        {
            if (try_again < 0 && !Disable_error_Meldingen)
            {
                MessageBox.Show($"SavePloegBezetting() error na 30 keer, \n{Ploeg_Bezetting_Locatie(kleur)}");
            }

            Main.labelDebug.Text = "Save Ploeg Bezetting";
            if (kleur != "")
            {
                try
                {
                    string file = Ploeg_Bezetting_Locatie(kleur);
                    using (Stream stream = File.Open(file, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, ListWerkdagPloeg);
                    }
                }
                catch
                {
                    Thread.Sleep(300);
                    SavePloegBezetting(kleur,try_again--);
                }
            }
        }

        public static void LoadPloegBezetting(string kleur, int try_again)
        {
            if (try_again < 0)
            {
                MessageBox.Show("Tevaak is load ploeg bezetting laden niet gelukt, netwerk probleem ?");
            }

            Main.labelDebug.Text = "Load Ploeg Bezetting";
            ListWerkdagPloeg.Clear();
            try
            {
                using (Stream stream = File.Open(Ploeg_Bezetting_Locatie(kleur), FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    try
                    {
                        ListWerkdagPloeg = (List<werkdag>)bin.Deserialize(stream);
                        stream.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("Deserialize(stream) error, gebruik repareer tool als Admin");
                    }
                }
            }
            catch (IOException)
            {
                //Main.labelDebug.Text = " LoadPloegBezettingLijst() error, try again";
                Thread.Sleep(300);
                LoadPloegBezetting(kleur, try_again--);
            }
            catch
            {
                if (!Disable_error_Meldingen)
                    MessageBox.Show($"LoadPloegBezettingLijst() error\n{Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)}");
            }
        }

        public static void LoadVeranderingenPloeg(string kleur)
        {
            Main.labelDebug.Text = "Load Ploeg Veranderingen";
            ListVeranderingen.Clear();
            try
            {
                using (Stream stream = File.Open(Ploeg_Veranderingen_Locatie(kleur), FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    ListVeranderingen = (List<veranderingen>)bin.Deserialize(stream);
                    Main.labelDebug.Text = "";
                }
            }
            catch
            {
                //if (File.Exists(Ploeg_Veranderingen_Locatie()))
                //  MessageBox.Show("LoadVeranderingenPloegLijst() error");
                //MessageBox.Show("LoadVeranderingenPloegLijst() error");
                // als hij nog niet bestaat krijg jke elke keer melding, dat hoefd niet
                // wordt vanzelf aangemaakt.
            }
        }

        public static void SaveVeranderingenPloeg(int try_again)
        {
            if (try_again < 0 && !Disable_error_Meldingen)
            {
                MessageBox.Show($"SaveVeranderingenPloeg() error na 30 keer, \n{Ploeg_Veranderingen_Locatie(ProgData.GekozenKleur)}");
            }


            Main.labelDebug.Text = "Save veranderingen Ploeg";
            if (GekozenKleur != "")
            {
                try
                {
                    using (Stream stream = File.Open(Ploeg_Veranderingen_Locatie(ProgData.GekozenKleur), FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, ListVeranderingen);
                        Main.labelDebug.Text = "";
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(300);
                    SaveVeranderingenPloeg(try_again--);
                }
            }
        }

        public static void Save_Namen_lijst()
        {
            Main.labelDebug.Text = "Save Namen Lijst";
            try
            {
                if (File.Exists("personeel.bin"))
                {
                    long length = new FileInfo("personeel.bin").Length;
                    if (length > 0)
                    {
                        if (!Directory.Exists("Backup"))
                            Directory.CreateDirectory("Backup");

                        string s = DateTime.Now.ToString("MM-dd-yyyy HH-mm");
                        /**/
                        string nieuw_naam = Directory.GetCurrentDirectory() + @"\Backup\personeel" + s + ".bin";
                        File.Copy("personeel.bin", nieuw_naam, true);  // overwrite oude file

                        List<FileInfo> files = new DirectoryInfo("Backup").EnumerateFiles()
                                        .OrderByDescending(f => f.CreationTime)
                                        .Skip(15)
                                        .ToList();
                        files.ForEach(f => f.Delete());
                    }
                    Main.labelDebug.Text = "";
                }

                using (Stream stream = File.Open("personeel.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, ListPersoneel);
                }
            }
            catch
            {
                if (!Disable_error_Meldingen)
                    MessageBox.Show("Save_Namen_lijst() error");
            }
        }

        public static void Lees_Namen_lijst()
        {
            Main.labelDebug.Text = "Load Namen Lijst";
            ListPersoneel.Clear();
            try
            {
                using (Stream stream = File.Open("personeel.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    ListPersoneel = (List<personeel>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
                if (!Disable_error_Meldingen)
                    MessageBox.Show("Lees_Namen_lijst() error");
            }
            Main.labelDebug.Text = "";
        }

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

        private static void MaakWerkPlekkenLijst()
        {
            ListWerkgroepPersoneel.Clear();
            foreach (personeel a in ListPersoneelKleur)
            {
                if (!ListWerkgroepPersoneel.Contains(a._werkgroep))
                    ListWerkgroepPersoneel.Add(a._werkgroep);
            }
        }

        /// <summary>
        /// Regel de afwijkingen van rooster
        /// </summary>
        /// <param name="naam">Naam van persoon</param>
        /// <param name="dagnr">nummer van de maand</param>
        /// <param name="afwijking">de afwijking</param>
        /// <param name="rede">de rede</param>
        /// <param name="invoerdoor">ingevoerd door</param>
        static public void RegelAfwijking(string naam, string dagnr, string afwijking, string rede, string invoerdoor, string kleur)
        {
            LoadPloegBezetting(kleur, 30);
            TestNaamInBezetting(naam);
            try
            {
                werkdag ver = ListWerkdagPloeg.First(a => (a._naam == naam) && (a._dagnummer.ToString() == dagnr));
                ver._afwijkingdienst = afwijking;
                SavePloegBezetting(kleur,30);

                LoadVeranderingenPloeg(kleur);
                veranderingen verander = new veranderingen();
                verander._naam = naam;
                verander._afwijking = afwijking;
                verander._datumafwijking = dagnr;
                verander._datuminvoer = DateTime.Now.ToShortDateString();
                verander._rede = rede; // de rede
                verander._invoerdoor = invoerdoor;
                ListVeranderingen.Add(verander);
                SaveVeranderingenPloeg(30);
                
                ChangeData = true; // voor screen shot
            }
            catch
            {
                if (!Disable_error_Meldingen)
                    MessageBox.Show($"Kan naam {naam} niet vinden in ploeg voor wijzeging. Kleur :  {kleur} Dag : {dagnr} Maand : {igekozenmaand} Jaar : {igekozenjaar}");
            }
        }

        static public void RegelAfwijkingOpDatumEnKleur(DateTime datum, string kleur, string naam, string dagnr, string afwijking, string rede, string invoerdoor)
        {
            // zet datum goed en kleur goed
            string bewaar_kleur = GekozenKleur;
            int bewaar_maand = igekozenmaand;
            int bewaar_jaar = igekozenjaar;
            // zet nieuwe kleur en datum
            if (GekozenKleur != kleur)
                GekozenKleur = kleur;
            if (igekozenmaand != datum.Month)
                igekozenmaand = datum.Month;
            if (igekozenjaar != datum.Year)
                igekozenjaar = datum.Year;

            // bestaat kleur en maand jaar file's ?
            ProgData.CheckFiles(kleur);

            // roep afwijking roetine aan
            RegelAfwijking(naam, dagnr, afwijking, rede, invoerdoor, kleur);
            // datum terug en kleur goed
            if (GekozenKleur != bewaar_kleur)
                GekozenKleur = bewaar_kleur;
            if (igekozenmaand != bewaar_maand)
                igekozenmaand = bewaar_maand;
            if (igekozenjaar != bewaar_jaar)
                igekozenjaar = bewaar_jaar;
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
                xx22 += xx22 = (int)woord[h];
                char character = (char)xx22;
                ret = ret + character.ToString();
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
                int xx22 = 0;
                xx22 = (int)woord[h];
                xx22 -= 2;
                xx22 -= woord.Length - h - 1;
                char character = (char)xx22;
                ret = ret + character.ToString();
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
            _LooptExtra_Locatie = Path.GetFullPath(dir + "\\" + kleur + "_extra.bin");
            try
            {
                using (Stream st = File.Open(_LooptExtra_Locatie, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(st, ListLooptExtra);
                }
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

        public static string Ploeg_Bezetting_Locatie(string kleur)
        {
            return Path.GetFullPath($"{_igekozenjaar.ToString()}\\{igekozenmaand.ToString()}\\{kleur}_bezetting.bin");
        }

        public static string Ploeg_Namen_Locatie()
        {
            return Path.GetFullPath($"{_igekozenjaar.ToString()}\\{igekozenmaand.ToString()}\\{GekozenKleur}_namen.bin");
        }

        public static string Ploeg_Veranderingen_Locatie(string kleur)
        {
            return Path.GetFullPath($"{_igekozenjaar.ToString()}\\{igekozenmaand.ToString()}\\{kleur}_afwijkingen.bin");
        }

        public static string Lijnen_Locatie()
        {
            return Path.GetFullPath($"{_igekozenjaar.ToString()}\\{igekozenmaand.ToString()}\\lijnen.ini");
        }

        public static string GetLocatieOverzichtPlaatje(DateTime datum)
        {
            // datum bv 18-8-2020
            return Path.GetFullPath($"{datum.Year.ToString()}\\{datum.Month.ToString()}\\{datum.Day.ToString()}_overzicht_{GekozenKleur}.jpg");
        }

        public static void CaptureMainScreen()
        {
            try
            {
                if (ChangeData && GekozenKleur != "")
                {
                    ChangeData = false;
                    Rectangle bounds = ProgData.Main.Bounds;
                    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                        }
                        string opslag = Path.GetFullPath($"{sgekozenjaar()}\\{igekozenmaand.ToString()}\\maand_overzicht_{GekozenKleur}.jpg");
                        bitmap.Save(opslag, ImageFormat.Jpeg);
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
                personeel persoon = ListPersoneel.First(a => a._persnummer == personeel_nr);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string Get_Gebruiker_Kleur(string nummer)
        {
            try
            {
                int personeel_nr = int.Parse(nummer);
                personeel persoon = ListPersoneel.First(a => a._persnummer == personeel_nr);
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
                personeel persoon = ListPersoneel.First(a => a._persnummer == personeel_nr);
                return persoon._achternaam;
            }
            catch
            {
                return nummer;
            }
        }

        public static string Get_Gebruiker_Nummer(string naam)
        {
            try
            {
                //int personeel_nr = int.Parse(nummer);
                personeel persoon = ListPersoneel.First(a => a._achternaam == naam);
                return persoon._persnummer.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static void CheckFiles(string kleur)
        {
            // check of juiste directory bestaat en gevuld is met juiste file's
            // maak ze anders aan.

            // maak namen.bin en afwijkingen.bin

            if (!File.Exists(Ploeg_Bezetting_Locatie(kleur)))
            {
                Directory.CreateDirectory(Path.GetFullPath($"{_igekozenjaar.ToString()}\\{igekozenmaand.ToString()}"));

                MaakPloegNamenLijst(kleur);
                SavePloegNamenLijst(30);

                LoadPloegBezetting(kleur, 30);
                GekozenKleur = kleur;
            }
        }

        public static void Backup()
        {
            string startPath = GetDirectoryBezettingMaand(DateTime.Now);
            ZipFile.CreateFromDirectory(startPath, backup_zipnaam);
        }

        public static void NachtErVoorVrij(string gekozen_naam, string dagnr)
        {
            // kijk of afwijking op vrije dag was, en dag ervoor Nacht, dan 
            if (GekozenRooster() != "DD")
            {
                DateTime dag_er_voor = new DateTime(igekozenjaar, igekozenmaand, int.Parse(dagnr));
                dag_er_voor = dag_er_voor.AddDays(-1);
                if (MDatum.GetDienst(GekozenRooster(), dag_er_voor, GekozenKleur) == "N")
                {
                    DialogResult dialogResult = MessageBox.Show("Nacht er voor VRIJ zetten?", "Vraagje", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        igekozenmaand = dag_er_voor.Month;
                        igekozenjaar = dag_er_voor.Year;

                        ProgData.RegelAfwijking(gekozen_naam, dag_er_voor.Day.ToString(), "VRIJ", "IVM WERKDAG MORGEN", "Rooster Regel", ProgData.GekozenKleur);

                        dag_er_voor = dag_er_voor.AddDays(1);
                        igekozenmaand = dag_er_voor.Month;
                        igekozenjaar = dag_er_voor.Year;

                    }
                }
            }
        }

        private static void TestNaamInBezetting(string naam)
        {
            try
            {
                werkdag ver = ListWerkdagPloeg.First(a => (a._naam == naam));
            }
            catch
            {
                int aantal_dagen_deze_maand = DateTime.DaysInMonth(igekozenjaar, igekozenmaand);
                for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
                {
                    DateTime dat = new DateTime(igekozenjaar, igekozenmaand, i);
                    werkdag dag = new werkdag();
                    dag._naam = naam;
                    dag._standaarddienst = MDatum.GetDienst(GekozenRooster(), dat, GekozenKleur);
                    dag._werkplek = "";
                    dag._afwijkingdienst = "";
                    dag._dagnummer = i;
                    ListWerkdagPloeg.Add(dag);
                }
                SavePloegBezetting(GekozenKleur,30);
            }
        }
    }
}
