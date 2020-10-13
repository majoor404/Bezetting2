﻿using Bezetting2.Data;
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
        public static MainFormBezetting2 Main;

        public static string GekozenRooster = "5pl";  // later aanpassen als kantoor dienst of zo

        public static ToolStripStatusLabel _inlognaam;
        public static ToolStripStatusLabel _toegangnivo;

        public static List<string> Lijnen = new List<string>();

        public static List<personeel> personeel_lijst = new List<personeel>();
        public static List<personeel> kleur_personeel_lijst = new List<personeel>();
        public static string ReloadSpeed1 = "";
        public static string ReloadSpeed2 = "";

        public static string _LooptExtra_Locatie;
        public static List<LooptExtraDienst> LooptExtra_lijst = new List<LooptExtraDienst>();

        public static List<string> werkgroep_personeel = new List<string>();
        public static ModuleDatum MDatum = new ModuleDatum();

        public static List<veranderingen> Veranderingen_Lijst = new List<veranderingen>();

        public static List<werkdag> Bezetting_Ploeg_Lijst = new List<werkdag>();

        public static string _RuilExtra_Locatie;
        public static List<AanvraagRuilExtra> RuilExtra_Lijst = new List<AanvraagRuilExtra>();

        public static string _Snipper_Locatie;
        public static List<SnipperAanvraag> Snipper_Lijst = new List<SnipperAanvraag>();

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
                personeel persoon = personeel_lijst.First(a => a._persnummer == personeel_nr);
                return persoon._achternaam;
            }
            catch
            {
                return "";
            }
        }
        public static void LoadPloegNamenLijst()
        {
            Main.labelDebug.Text = "Load Ploeg Namen Lijst";
            // om tijd te winnen alleen laden als er andere ploeg gevraagt wordt
            if (ReloadSpeed2 != GekozenKleur)
            {
                kleur_personeel_lijst.Clear();
                try
                {
                    //string _Ploeg_Namen_Locatie = Path.GetFullPath(GetDir() + "\\" + _GekozenKleur + "_namen.bin");
                    using (Stream stream = File.Open(Ploeg_Namen_Locatie(), FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();

                        kleur_personeel_lijst = (List<personeel>)bin.Deserialize(stream);
                    }
                }
                catch (IOException)
                {
                }
                // haal werkgroepen op
                MaakWerkPlekkenLijst();
            }
            ReloadSpeed2 = GekozenKleur;
        }
        public static void SavePloegNamenLijst()
        {
            Main.labelDebug.Text = "Save Ploeg Namen Lijst";
            if (GekozenKleur != "")
            {
                try
                {
                    string Locatie = Ploeg_Namen_Locatie();
                    using (Stream stream = File.Open(Locatie, FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, kleur_personeel_lijst);
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("SavePloegNamenLijst() error");
                }
            }
        }
        public static void MaakPloegNamenLijst(string kleur)
        {
            DateTime dezemaand = new DateTime(igekozenjaar, igekozenmaand, 1, 0, 0, 0, 0, 0);
            kleur_personeel_lijst.Clear();

            // query om juiste mensen te vinden
            IEnumerable<personeel> ploeg_gekozen = from a in personeel_lijst
                                                   where (a._kleur == kleur) || (a._nieuwkleur == kleur)
                                                   select a;

            foreach (personeel a in ploeg_gekozen)
            {
                if(a._nieuwkleur != "")
                {
                    DateTime verhuismaand = new DateTime(a._verhuisdatum.Year, a._verhuisdatum.Month, 1, 0, 0, 0, 0, 0);
                    if(a._kleur == kleur)
                    {
                        if (verhuismaand >= dezemaand)
                            kleur_personeel_lijst.Add(a);
                    }
                    else
                    {
                        if (verhuismaand <= dezemaand)
                            kleur_personeel_lijst.Add(a);
                    }
                }
                else 
                { 
                    kleur_personeel_lijst.Add(a);
                }
            }

            //kleur_personeel.Sort();
            // sorteer kleur_personeel
            kleur_personeel_lijst.Sort(delegate (personeel x, personeel y)
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
            Bezetting_Ploeg_Lijst.Clear();
            // elke dag in deze maand
            int aantal_dagen_deze_maand = DateTime.DaysInMonth(igekozenjaar, igekozenmaand);
            // elke persoon
            foreach (personeel a in kleur_personeel_lijst)
            {
                for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
                {
                    DateTime dat = new DateTime(igekozenjaar, igekozenmaand, i);
                    werkdag dag = new werkdag();
                    dag._naam = a._achternaam;
                    dag._standaarddienst = MDatum.GetDienst(GekozenRooster, dat, a._kleur);
                    dag._werkplek = "";
                    dag._afwijkingdienst = "";
                    dag._dagnummer = i;
                    Bezetting_Ploeg_Lijst.Add(dag);
                }
            }
            SavePloegBezetting(kleur);
        }
        public static void SavePloegBezetting(string kleur)
        {
            Main.labelDebug.Text = "Save Ploeg Bezetting";
            if (kleur != "")
            {
                try
                {
                    string file = Ploeg_Bezetting_Locatie(kleur);
                    using (Stream stream = File.Open(file, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, Bezetting_Ploeg_Lijst);
                    }
                }
                catch
                {
                    MessageBox.Show($"SavePloegBezetting() error\n{Ploeg_Bezetting_Locatie(kleur)}");
                }
            }

        }
        public static void LoadPloegBezetting(string kleur)
        {
                Main.labelDebug.Text = "Load Ploeg Bezetting";
                Bezetting_Ploeg_Lijst.Clear();
                try
                {
                    using (Stream stream = File.Open(Ploeg_Bezetting_Locatie(kleur), FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        try
                        {
                            Bezetting_Ploeg_Lijst = (List<werkdag>)bin.Deserialize(stream);
                        }
                        catch
                        {
                            MessageBox.Show("Deserialize(stream) error, gebruik repareer tool als Admin");
                        }
                    }
                }
                catch
                {
                    MessageBox.Show($"LoadPloegBezettingLijst() error\n{Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)}");
                }


            //if (Bezetting_Ploeg_Lijst.Count == 0)
            //    MessageBox.Show("bezetting ploeg lijst is leeg ?");
        }
        public static void LoadVeranderingenPloeg()
        {
            Main.labelDebug.Text = "Load Ploeg Veranderingen";
            Veranderingen_Lijst.Clear();
            try
            {
                using (Stream stream = File.Open(Ploeg_Veranderingen_Locatie(), FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    Veranderingen_Lijst = (List<veranderingen>)bin.Deserialize(stream);
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
        public static void SaveVeranderingenPloeg()
        {
            Main.labelDebug.Text = "Save veranderingen Ploeg";
            if (GekozenKleur != "")
            {
                try
                {
                    using (Stream stream = File.Open(Ploeg_Veranderingen_Locatie(), FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, Veranderingen_Lijst);
                        Main.labelDebug.Text = "";
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show($"SaveVeranderingenPloegLijst() error\n{Ploeg_Veranderingen_Locatie()}");
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
                    bin.Serialize(stream, personeel_lijst);
                }
            }
            catch
            {
                MessageBox.Show("Save_Namen_lijst() error");
            }
 
        }
        public static void Lees_Namen_lijst()
        {
            Main.labelDebug.Text = "Load Namen Lijst";
            personeel_lijst.Clear();
                try
                {
                    using (Stream stream = File.Open("personeel.bin", FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        personeel_lijst = (List<personeel>)bin.Deserialize(stream);
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Lees_Namen_lijst() error");
                }
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
        static void MaakWerkPlekkenLijst()
        {
            werkgroep_personeel.Clear();
            foreach (personeel a in kleur_personeel_lijst)
            {
                if (!werkgroep_personeel.Contains(a._werkgroep))
                    werkgroep_personeel.Add(a._werkgroep);
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
        static public void RegelAfwijking(string naam, string dagnr, string afwijking, string rede, string invoerdoor , string kleur)
        {
            LoadPloegBezetting(kleur);
            try
            {
                werkdag ver = Bezetting_Ploeg_Lijst.First(a => (a._naam == naam) && (a._dagnummer.ToString() == dagnr));
                ver._afwijkingdienst = afwijking;
                SavePloegBezetting(kleur);

                // vul historie/afwijkingen aan
                LoadVeranderingenPloeg();
                veranderingen verander = new veranderingen();
                verander._naam = naam;
                verander._afwijking = afwijking;
                verander._datumafwijking = dagnr;
                verander._datuminvoer = DateTime.Now.ToShortDateString();
                verander._rede = rede; // de rede
                verander._invoerdoor = invoerdoor;
                Veranderingen_Lijst.Add(verander);
                SaveVeranderingenPloeg();
                ChangeData = true;
            }
            catch
            {
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
            RegelAfwijking(naam, dagnr, afwijking, rede, invoerdoor , kleur);
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
            RuilExtra_Lijst.Clear();
            _RuilExtra_Locatie = Path.GetFullPath(dir + "\\ruilextra.bin");
            try
            {
                using (Stream stream = File.Open(_RuilExtra_Locatie, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    RuilExtra_Lijst = (List<AanvraagRuilExtra>)bin.Deserialize(stream);
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
                    bin.Serialize(st, RuilExtra_Lijst);
                }
            }
            catch
            {
                MessageBox.Show("SaveExtraRuilLijst() error");
            }
        }
        public static void LoadSnipperLijst(string dir)
        {
            Snipper_Lijst.Clear();
            _Snipper_Locatie = Path.GetFullPath(dir + "\\snipper.bin");
            try
            {
                using (Stream stream = File.Open(_Snipper_Locatie, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    Snipper_Lijst = (List<SnipperAanvraag>)bin.Deserialize(stream);
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
                    bin.Serialize(st, Snipper_Lijst);
                }
            }
            catch
            {
                MessageBox.Show("SaveSnipperLijst() error");
            }
        }
        public static void LoadLooptExtraLijst(string dir, string kleur)
        {
            LooptExtra_lijst.Clear();
            _LooptExtra_Locatie = Path.GetFullPath(dir + "\\" + kleur + "_extra.bin");
            try
            {
                using (Stream stream = File.Open(_LooptExtra_Locatie, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    LooptExtra_lijst = (List<LooptExtraDienst>)bin.Deserialize(stream);
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
                    bin.Serialize(st, LooptExtra_lijst);
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
        public static string Ploeg_Veranderingen_Locatie()
        {
            return Path.GetFullPath($"{_igekozenjaar.ToString()}\\{igekozenmaand.ToString()}\\{GekozenKleur}_afwijkingen.bin");
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
        public static bool Bestaat_Gebruiker(string nummer)
        {
            try
            {
                int personeel_nr = int.Parse(nummer);
                personeel persoon = personeel_lijst.First(a => a._persnummer == personeel_nr);
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
                personeel persoon = personeel_lijst.First(a => a._persnummer == personeel_nr);
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
                personeel persoon = personeel_lijst.First(a => a._persnummer == personeel_nr);
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
                personeel persoon = personeel_lijst.First(a => a._achternaam == naam);
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
                SavePloegNamenLijst();

                LoadPloegBezetting(kleur);
                GekozenKleur = kleur;
            }
        }

        public static void Backup()
        {
            string startPath = GetDirectoryBezettingMaand(DateTime.Now);
            ZipFile.CreateFromDirectory(startPath, backup_zipnaam);
        }
    }
}

