using Bezetting2.Data;
using System;
using System.Collections.Generic;
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
"BezData\\personeel.bin"

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

        //once you have the path you get the directory with:
        //public static string DataDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

        public static bool Disable_error_Meldingen = false;
        public static MainFormBezetting2 Main;

        public static bool ScreenCapture = true;

        public static ToolStripStatusLabel _inlognaam;
        public static ToolStripStatusLabel _toegangnivo;

        public static AllVerCain chain = new AllVerCain();

        public static List<string> Lijnen = new List<string>();

        public static List<personeel> ListPersoneel = new List<personeel>();
        public static List<personeel> ListPersoneelKleur = new List<personeel>();

        public static string _LooptExtra_Locatie;
        public static List<LooptExtraDienst> ListLooptExtra = new List<LooptExtraDienst>();

        public static List<string> ListWerkgroepPersoneel = new List<string>();

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

        public static string backup_zipnaam;

        public static int backup_time;

        public static int Igekozenjaar
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
                    Main.Text = $"Bezetting 2.0          Ingelogd :   {Huidige_Gebruiker_Naam()} -- {_inlognaam.Text}";
                }

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

        public static void LoadPloegNamenLijst(string kleur, int try_again)
        {
            if (try_again < 0)
            {
                MessageBox.Show("LoadPloegNamenLijst() lukt niet!, te vaak geprobeerd.");
            }
            Main.labelDebug.Text = "Load Ploeg Namen Lijst";

            try
            {
                //string _Ploeg_Namen_Locatie = Path.GetFullPath(GetDir() + "\\" + _GekozenKleur + "_namen.bin");
                using (Stream stream = File.Open(Ploeg_Namen_Locatie(kleur), FileMode.Open))
                {
                    ListPersoneelKleur.Clear();
                    BinaryFormatter bin = new BinaryFormatter();
                    ListPersoneelKleur = (List<personeel>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
                Main.labelDebug.Text = "catch LoadPloegNamenLijst() error, try again";
                Thread.Sleep(300);
                LoadPloegNamenLijst(kleur, --try_again);
            }
            // haal werkgroepen op
            MaakWerkPlekkenLijst();
            Main.labelDebug.Text = "";
        }

        public static void SavePloegNamenLijst(string kleur, int try_again)
        {
            if (try_again < 0 && !Disable_error_Meldingen)
            {
                MessageBox.Show("SavePloegNamenLijst() error na 15 keer, " + Ploeg_Namen_Locatie(kleur));
            }
            Main.labelDebug.Text = "Save Ploeg Namen Lijst";
            if (!string.IsNullOrEmpty(kleur))
            {
                try
                {
                    string Locatie = Ploeg_Namen_Locatie(kleur);
                    using (Stream stream = File.Open(Locatie, FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, ListPersoneelKleur);
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(300);
                    SavePloegNamenLijst(kleur, --try_again);
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
                if (!string.IsNullOrEmpty(a._nieuwkleur) && kleur != a._nieuwkleur)
                {
                    //als nieuwkleur aanwezig, kijk of hij in die bezetting_Ploeg_lijst voorkomt,
                    //regel dat anders, als file niet bestaat, gaat dat vanzelf
                    if (File.Exists(Ploeg_Bezetting_Locatie(a._nieuwkleur)))
                    {
                        LoadPloegBezetting(a._nieuwkleur, 15);
                        MaakNieuweCollegaInBezettingAan(a._achternaam, a._nieuwkleur, Igekozenjaar, igekozenmaand, 1);
                        
                        //// check of naam er in zit
                        //try
                        //{
                        //    werkdag ver = ListWerkdagPloeg.First(x => (x._naam == a._achternaam));
                        //}
                        //catch
                        //{
                        //    // deze persoon bestaat niet in bezetting, dus toevoegen
                        //    // elke dag in deze maand
                        //    int aantal_dagen_deze_maand = DateTime.DaysInMonth(Igekozenjaar, igekozenmaand);
                        //    for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
                        //    {
                        //        DateTime dat = new DateTime(Igekozenjaar, igekozenmaand, i);
                        //        werkdag dag = new werkdag
                        //        {
                        //            _naam = a._achternaam,
                        //            //_standaarddienst = MDatum.GetDienst(GekozenRooster(), dat, a._nieuwkleur),
                        //            _standaarddienst = GetDienst(GekozenRooster(), dat, a._nieuwkleur),
                        //            _werkplek = "",
                        //            _afwijkingdienst = "",
                        //            _dagnummer = i
                        //        };
                        //        ListWerkdagPloeg.Add(dag);
                        //    }
                        //    SavePloegBezetting(a._nieuwkleur, 15);
                        //}
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
                MaakLegeBezetting(kleur, true); // in deze roetine wordt het ook opgeslagen
            }
        }

        public static void MaakLegeBezetting(string kleur, bool MetVeranderLijst)
        {
            if (TestNetwerkBeschikbaar(15))
            {
                ListWerkdagPloeg.Clear();
                // elke dag in deze maand
                int aantal_dagen_deze_maand = DateTime.DaysInMonth(Igekozenjaar, igekozenmaand);
                // elke persoon
                foreach (personeel a in ListPersoneelKleur)
                {
                    for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
                    {
                        DateTime dat = new DateTime(Igekozenjaar, igekozenmaand, i);
                        werkdag dag = new werkdag
                        {
                            _naam = a._achternaam,
                            //_standaarddienst = MDatum.GetDienst(GekozenRooster(), dat, a._kleur),
                            _standaarddienst = GetDienst(GekozenRooster(), dat, a._kleur),
                            _werkplek = "",
                            _afwijkingdienst = "",
                            _dagnummer = i
                        };
                        ListWerkdagPloeg.Add(dag);
                    }
                }
                SavePloegBezetting(kleur, 15);

                // bij nieuwe bezetting hoort ook een nieuwe verander lijst
                if (MetVeranderLijst)
                {
                    ListVeranderingen.Clear(); // stel er bestond er nog 1, niet overschrijven.
                    SaveVeranderingenPloeg(kleur, 15);
                }
            }
            else
            {
                MessageBox.Show("Kan niet schrijven en/of lezen op locatie, netwerk problemen ?, Exit");
                Main.Close();
            }
        }

        public static void SavePloegBezetting(string kleur, int try_again)
        {
            if (try_again < 0 && !Disable_error_Meldingen)
            {
                MessageBox.Show($"SavePloegBezetting() error na 15 keer, \n{Ploeg_Bezetting_Locatie(kleur)}");
            }

            Main.labelDebug.Text = "Save Ploeg Bezetting";
            if (!string.IsNullOrEmpty(kleur))
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
                    SavePloegBezetting(kleur, --try_again);
                }
            }
        }

        public static void LoadPloegBezetting(string kleur, int try_again)
        {
            CheckFiles(kleur);

            if (try_again < 0)
            {
                MessageBox.Show("Tevaak is load ploeg bezetting laden niet gelukt, netwerk probleem ?");
            }
            Main.labelDebug.Text = "Load Ploeg Bezetting";

            try
            {
                using (Stream stream = File.Open(Ploeg_Bezetting_Locatie(kleur), FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    try
                    {
                        ListWerkdagPloeg.Clear();
                        ListWerkdagPloeg = (List<werkdag>)bin.Deserialize(stream);
                        stream.Dispose();
                    }
                    catch
                    {
                        MessageBox.Show("Deserialize(stream) error, gebruik repareer tool als Admin");
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Dispose();
                    }
                }
            }
            catch (IOException)
            {
                //Main.labelDebug.Text = " LoadPloegBezettingLijst() error, try again";
                Thread.Sleep(300);
                LoadPloegBezetting(kleur, --try_again);
            }
            catch
            {
                if (!Disable_error_Meldingen)
                    MessageBox.Show($"LoadPloegBezettingLijst() error\n{Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)}");
            }
        }

        public static void LoadVeranderingenPloeg(string kleur, int try_again)
        {
            if (try_again < 0)
            {
                MessageBox.Show("Tevaak is Ploeg Veranderingen laden niet gelukt, netwerk probleem ?, prog maakt nieuwe aan.");
            }
            else
            {
                Main.labelDebug.Text = "Load Ploeg Veranderingen";
                
                var file = Ploeg_Veranderingen_Locatie(kleur);

                if (!File.Exists(file))
                { 
                    MessageBox.Show($"verander lijst {file}  bestond niet!, checkfiles zou dit hebben moeten afvangen!");
                    //ListVeranderingen.Clear();
                    //SaveVeranderingenPloeg(kleur, 15);
                }

                try
                {
                    using (Stream stream = File.Open(file , FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        ListVeranderingen.Clear();
                        ListVeranderingen = (List<veranderingen>)bin.Deserialize(stream);
                        Main.labelDebug.Text = "";
                    }
                }
                catch
                {
                    Thread.Sleep(300);
                    LoadVeranderingenPloeg(kleur, --try_again);
                }
            }
        }

        public static void SaveVeranderingenPloeg(string kleur, int try_again)
        {
            if (try_again < 0 && !Disable_error_Meldingen)
            {
                MessageBox.Show($"SaveVeranderingenPloeg() error na 15 keer, \n{Ploeg_Veranderingen_Locatie(kleur)}");
            }


            Main.labelDebug.Text = "Save veranderingen Ploeg";
            if (!string.IsNullOrEmpty(GekozenKleur))
            {
                try
                {
                    using (Stream stream = File.Open(Ploeg_Veranderingen_Locatie(kleur), FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, ListVeranderingen);
                        Main.labelDebug.Text = "";
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(300);
                    SaveVeranderingenPloeg(kleur, --try_again);
                }
            }
        }

        public static void Save_Namen_lijst()
        {
            Main.labelDebug.Text = "Save Namen Lijst";
            try
            {
                if (File.Exists("BezData\\personeel.bin"))
                {
                    long length = new FileInfo("BezData\\personeel.bin").Length;
                    if (length > 0)
                    {
                        if (!Directory.Exists("Backup"))
                            Directory.CreateDirectory("Backup");

                        string s = DateTime.Now.ToString("MM-dd-yyyy HH-mm");
                        /**/
                        string nieuw_naam = Directory.GetCurrentDirectory() + @"\Backup\personeel" + s + ".bin";
                        File.Copy("BezData\\personeel.bin", nieuw_naam, true);  // overwrite oude file

                        List<FileInfo> files = new DirectoryInfo("Backup").EnumerateFiles()
                                        .OrderByDescending(f => f.CreationTime)
                                        .Skip(15)
                                        .ToList();
                        files.ForEach(f => f.Delete());
                    }
                    Main.labelDebug.Text = "";
                }

                using (Stream stream = File.Open("BezData\\personeel.bin", FileMode.Create))
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
            try
            {
                using (Stream stream = File.Open("BezData\\personeel.bin", FileMode.Open))
                {
                    ListPersoneel.Clear();
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
            DateTime gevraagd = new DateTime(Igekozenjaar, igekozenmaand, 1, 0, 0, 0, 0, 0);
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
            LoadPloegBezetting(kleur, 15);
            TestNaamInBezetting(naam, dagnr, kleur);
            try
            {
                /// test
                chain.Load();
                chain.voeg_toe(AllVerCain.soort_.dag_inhoud, dagnr, Get_Gebruiker_Nummer(naam), afwijking, rede, invoerdoor, kleur);
                chain.Save();
                ///

                werkdag ver = ListWerkdagPloeg.First(a => (a._naam == naam) && (a._dagnummer.ToString() == dagnr));
                ver._afwijkingdienst = afwijking;
                SavePloegBezetting(kleur, 15);

                LoadVeranderingenPloeg(kleur, 15);

                veranderingen verander = new veranderingen
                {
                    _naam = naam,
                    _afwijking = afwijking,
                    _datumafwijking = dagnr,
                    _datuminvoer = DateTime.Now.ToShortDateString(),
                    _rede = rede, // de rede
                    _invoerdoor = invoerdoor
                };
                ListVeranderingen.Add(verander);
                SaveVeranderingenPloeg(kleur, 15);
            }
            catch
            {
                if (!Disable_error_Meldingen)
                    MessageBox.Show($"Kan naam {naam} niet vinden in ploeg voor wijzeging. Kleur :  {kleur} Dag : {dagnr} Maand : {igekozenmaand} Jaar : {Igekozenjaar}");
            }
        }

        static public void RegelAfwijkingOpDatumEnKleur(DateTime datum, string kleur, string naam, string dagnr, string afwijking, string rede, string invoerdoor ,bool Update_screen = true)
        {
            Main.WindowUpdateViewScreen = Update_screen;
            // zet datum goed en kleur goed
            string bewaar_kleur = GekozenKleur;
            int bewaar_maand = igekozenmaand;
            int bewaar_jaar = Igekozenjaar;
            // zet nieuwe kleur en datum
            if (GekozenKleur != kleur)
                GekozenKleur = kleur;
            if (igekozenmaand != datum.Month)
                igekozenmaand = datum.Month;
            if (Igekozenjaar != datum.Year)
                Igekozenjaar = datum.Year;

            // bestaat kleur en maand jaar file's ?
            ProgData.CheckFiles(kleur);

            // roep afwijking roetine aan
            RegelAfwijking(naam, dagnr, afwijking, rede, invoerdoor, kleur);
            // datum terug en kleur goed
            if (GekozenKleur != bewaar_kleur)
                GekozenKleur = bewaar_kleur;
            if (igekozenmaand != bewaar_maand)
                igekozenmaand = bewaar_maand;
            if (Igekozenjaar != bewaar_jaar)
                Igekozenjaar = bewaar_jaar;

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
            return Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\{kleur}_bezetting.bin");
        }

        public static string Ploeg_Namen_Locatie(string kleur)
        {
            return Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\{kleur}_namen.bin");
        }

        public static string Ploeg_Veranderingen_Locatie(string kleur)
        {
            //var test = Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\{kleur}_afwijkingen.bin");
            return Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}\\{kleur}_afwijkingen.bin");
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
                if (TestNetwerkBeschikbaar(15))
                {
                    _ = Directory.CreateDirectory(Path.GetFullPath($"{_igekozenjaar}\\{igekozenmaand}"));

                    MaakPloegNamenLijst(kleur);
                    SavePloegNamenLijst(kleur, 15);

                    LoadPloegBezetting(kleur, 15);
                    GekozenKleur = kleur;
                }
                else
                {
                    Main.Close();
                }
            }
            
            if (!File.Exists(Ploeg_Veranderingen_Locatie(kleur)))
            {
                MessageBox.Show($"verander lijst {Ploeg_Veranderingen_Locatie(kleur)}  bestond niet, maak deze aan.");
                ListVeranderingen.Clear();
                SaveVeranderingenPloeg(kleur, 15);
            }

        }

        public static void Backup()
        {
            string startPath = GetDirectoryBezettingMaand(DateTime.Now);

            ZipFile.CreateFromDirectory(startPath, backup_zipnaam);
        }

        public static void NachtErVoorVrij(string gekozen_naam, string dagnr, string afwijking)
        {
            // kijk of afwijking op vrije dag was, en dag ervoor Nacht, dan 
            if (GekozenRooster() != "DD")
            {
                DateTime dag_er_voor = new DateTime(Igekozenjaar, igekozenmaand, int.Parse(dagnr));
                dag_er_voor = dag_er_voor.AddDays(-1);
                if (/*MDatum.*/GetDienst(GekozenRooster(), dag_er_voor, GekozenKleur) == "N")
                {
                    if (afwijking == "VK" || afwijking == "8OI" || afwijking == "A" ||
                        afwijking == "VRIJ" || afwijking == "VAK" || afwijking == "VF"
                        || afwijking == "ED-N")
                    {

                    }
                    else
                    {
                        DialogResult dialogResult = MessageBox.Show("Nacht er voor VRIJ zetten?", "Vraagje", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            igekozenmaand = dag_er_voor.Month;
                            Igekozenjaar = dag_er_voor.Year;

                            ProgData.RegelAfwijking(gekozen_naam, dag_er_voor.Day.ToString(), "VRIJ", "IVM WERKDAG MORGEN", "Rooster Regel", ProgData.GekozenKleur);

                            dag_er_voor = dag_er_voor.AddDays(1);
                            igekozenmaand = dag_er_voor.Month;
                            Igekozenjaar = dag_er_voor.Year;

                        }
                    }
                }
            }
        }

        private static void TestNaamInBezetting(string naam, string dagnr, string kleur)
        {
            if (kleur != ProgData.GekozenKleur)
            {
                MessageBox.Show("Error kleur gevraagde kleur wijkt af van gekozen kleur, meld het aan programeur a.u.b");
            }

            try
            {
                werkdag ver = ListWerkdagPloeg.First(a => (a._naam == naam) && (a._dagnummer.ToString() == dagnr));
            }
            catch
            {
                //MessageBox.Show($"Naam : {naam} bestond niet in Listwerkdag van Ploeg {ProgData.GekozenKleur}, probeer op te lossen!");

                MaakNieuweCollegaInBezettingAan(naam, kleur, ProgData.Igekozenjaar, ProgData.igekozenmaand, 1);

                //// test of blauw_bezetting.bin bestaat
                //string Locatie = Path.GetFullPath(ProgData.GetDir() + "\\" + ProgData.GekozenKleur + "_afwijkingen.bin");
                //if (File.Exists(ProgData.Ploeg_Veranderingen_Locatie(ProgData.GekozenKleur)))
                //{
                //    File.Delete(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur));
                //    ProgData.MaakLegeBezetting(ProgData.GekozenKleur, false);
                //    ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur, 15);
                //    ProgData.LoadPloegBezetting(ProgData.GekozenKleur, 15);
                //    foreach (veranderingen verander in ProgData.ListVeranderingen)
                //    {
                //        werkdag ver = ProgData.ListWerkdagPloeg.First(a => (a._naam == verander._naam) && (a._dagnummer.ToString() == verander._datumafwijking));
                //        ver._afwijkingdienst = verander._afwijking;
                //    }
                //    ProgData.SavePloegBezetting(ProgData.GekozenKleur, 15);
                //    Main.VulViewScherm();
                //}
                //else
                //{
                //    MessageBox.Show("Error 123432");
                //}
            }
        }

        public static bool TestNetwerkBeschikbaar(int test)
        {
            Main.labelDebug.Text = "Test Netwerk";


            if (test == 0)
            {
                MessageBox.Show("Kan niet schrijven en/of lezen op locatie, netwerk problemen ?, Exit");
                return false;
            }

            try
            {
                File.WriteAllText("TestNetWerk.txt", "");
                if (File.Exists("TestNetWerk.txt"))
                {
                    File.Delete("TestNetWerk.txt");
                    Main.labelDebug.Text = "";
                    return true;
                }
            }
            catch
            {
                Thread.Sleep(300);
                TestNetwerkBeschikbaar(test--);
            }
            MessageBox.Show("Kan niet schrijven en/of lezen op locatie, netwerk problemen ?, Exit");
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

        public static void MaakNieuweCollegaInBezettingAan(string Naam, string Kleur, int jaar, int maand, int aantal_maanden)
        {
            // als bij verhuizing er een persoon bij komt, moet in deze toevoegen aan ListWerkdagPloeg,
            // echter alleen als deze nog niet bestaat.
            // check of naam er in zit, en op die dag van de maand.

            DateTime dumm = new DateTime(jaar, maand, 1);

            int save_jaar = Igekozenjaar;
            int save_maand = igekozenmaand;

            Igekozenjaar = jaar;
            igekozenmaand = maand;

            for (int m = 0; m < aantal_maanden; m++)
            {
                if (m != 0)
                    dumm = dumm.AddMonths(1);

                Igekozenjaar = dumm.Year;
                igekozenmaand = dumm.Month;

                int aantal_dagen_dezemaand = DateTime.DaysInMonth(Igekozenjaar, igekozenmaand);

                ProgData.LoadPloegBezetting(Kleur, 15);

                for (int i = 1; i < aantal_dagen_dezemaand + 1; i++)
                {
                    DateTime dat = new DateTime(Igekozenjaar, igekozenmaand, i);

                    try
                    {
                        werkdag ver = ListWerkdagPloeg.First(x => (x._naam == Naam && x._dagnummer == i));
                    }
                    catch
                    {
                        werkdag dag = new werkdag
                        {
                            _naam = Naam,
                            _standaarddienst = GetDienst(InstellingenProg._Rooster, dat, Kleur),
                            _werkplek = "",
                            _afwijkingdienst = "",
                            _dagnummer = i
                        };
                        ProgData.ListWerkdagPloeg.Add(dag);
                    }
                }
                ProgData.SavePloegBezetting(Kleur, 15);
            }
            Igekozenjaar = save_jaar;
            igekozenmaand = save_maand;
        }
    }
}

