using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Bezetting2.Data
{
    public class PersoneelOverzicht
    {

        public List<personeel> LijstPersonen = new List<personeel>();
        public List<personeel> LijstPersoonKleur = new List<personeel>();
        public List<string> LijstWerkgroepenPersoneel = new List<string>();

        private DateTime laaste_versie;
        private DateTime laaste_versie_kleur;

        public void Load()
        {
            var path = "BezData\\personeel.bin";
            var veranderd = File.GetLastWriteTime(path);
            if (veranderd != laaste_versie)
            {
                try
                {
                    using (Stream stream = File.Open(path, FileMode.Open))
                    {
                        LijstPersonen.Clear();
                        BinaryFormatter bin = new BinaryFormatter();
                        LijstPersonen = (List<personeel>)bin.Deserialize(stream);
                        laaste_versie = veranderd;
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Laad LijstPersonen error");
                }
            }
        }

        public void Save()
        {
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

                        string nieuw_naam = Directory.GetCurrentDirectory() + @"\Backup\personeel" + s + ".bin";
                        File.Copy("BezData\\personeel.bin", nieuw_naam, true);  // overwrite oude file

                        List<FileInfo> files = new DirectoryInfo("Backup").EnumerateFiles()
                                        .OrderByDescending(f => f.CreationTime)
                                        .Skip(5)
                                        .ToList();
                        files.ForEach(f => f.Delete());
                    }
                }

                using (Stream stream = File.Open("BezData\\personeel.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, LijstPersonen);
                }
            }
            catch
            {
                MessageBox.Show("Save Namen error");
                Process.GetCurrentProcess().Kill();
            }
        }
        public void HaalPloegNamenOpKleur(string kleur)
        {
            // afhankelijk kwa tijdstip, als deze maand of toekomst, dan uit 
            // LijstPersonen halen.
            // Als in verleden, haal van schijf.

            if (ProgData.WaarInTijd() == 1)
            {
                // laad van schijf
                HaalPloegNamenOpKleurVanSchijf(kleur, 15);
            }
            else
            {
                Load();
                LijstPersoonKleur.Clear();

                // voor sort, 

                // als nieuwkleur er niet is, dan persoon
                // als nieuwkleur aanwezig, dan afhankelijk van tijdstip overgang
                // overgang is huidige maand, dan hoor je bij ploeg
                // overgang verleden, dan gebruiken we dit niet

                 

                IEnumerable<personeel> ploeg_gekozen = from a in LijstPersonen
                                                       where (a._kleur == kleur || a._nieuwkleur == kleur)
                                                       select a;

                foreach (personeel a in ploeg_gekozen)
                {
                    if(ProgData.GekozenKleur == a._kleur )
                    {
                        bool nieuwe_wacht = !(a._nieuwkleur == null || a._nieuwkleur == "");
                        if (nieuwe_wacht)
                        {
                            var verhuis_maand = a._verhuisdatum.Month;
                            var verhuis_jaar = a._verhuisdatum.Year;
                            var jaar = ProgData.igekozenjaar;
                            var maand = ProgData.igekozenmaand;
                            if (verhuis_jaar >= jaar && verhuis_maand >= maand)
                                LijstPersoonKleur.Add(a);
                        }
                        else
                        {
                            LijstPersoonKleur.Add(a);
                        }
                    }
                    else
                    {
                        bool nieuwe_wacht = !(a._nieuwkleur == null || a._nieuwkleur == "");
                        if (nieuwe_wacht)
                        {
                            var verhuis_maand = a._verhuisdatum.Month;
                            var verhuis_jaar = a._verhuisdatum.Year;
                            var jaar = ProgData.igekozenjaar;
                            var maand = ProgData.igekozenmaand;
                            if (jaar >= verhuis_jaar && maand >= verhuis_maand)
                                LijstPersoonKleur.Add(a);
                        }
                        else
                        {
                            LijstPersoonKleur.Add(a);
                        }
                    }
                    
                    
                }

                LijstPersoonKleur.Sort(delegate (personeel x, personeel y)
                {
                    return x._achternaam.CompareTo(y._achternaam);
                });

                MaakWerkPlekkenLijst();

                //wordt in backup geregeld, alleen nodig voor maanden in verleden.
                //BewaarPloegNamenOpKleurOpSchijf(kleur, 15);
            }
        }
        public bool HaalPloegNamenOpKleurVanSchijf(string kleur, int try_again)
        {
            string file = ProgData.Ploeg_Namen_Locatie(kleur);
            
            if (!File.Exists(file))
                return false;
            
            if (try_again < 0)
            {
                MessageBox.Show("HaalPloegNamenOpKleurVanSchijf() lukt niet!, te vaak geprobeerd.");
                Process.GetCurrentProcess().Kill();
            }

            try
            {
                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    LijstPersoonKleur.Clear();
                    BinaryFormatter bin = new BinaryFormatter();
                    LijstPersoonKleur = (List<personeel>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
                Thread.Sleep(300);
                HaalPloegNamenOpKleurVanSchijf(kleur, --try_again);
            }
            // haal werkgroepen op
            MaakWerkPlekkenLijst();

            return true;
        }
        public void MaakWerkPlekkenLijst()
        {
            LijstWerkgroepenPersoneel.Clear();
            foreach (personeel a in LijstPersoonKleur)
            {
                if (!LijstWerkgroepenPersoneel.Contains(a._werkgroep))
                    LijstWerkgroepenPersoneel.Add(a._werkgroep);
            }
        }
        public void BewaarPloegNamenOpKleurOpSchijf(string kleur, int try_again)
        {
            string Locatie = ProgData.Ploeg_Namen_Locatie(kleur);

            var veranderdkleur = File.GetLastWriteTime(Locatie);

            //var diffInSeconds = System.Math.Abs((veranderdkleur - laaste_versie_kleur).TotalSeconds);

            if (laaste_versie_kleur != veranderdkleur /* diffInSeconds > 1000*/)
            {
                if (try_again < 0)
                {
                    MessageBox.Show("BewaarPloegNamenOpKleurVanSchijf() error na 15 keer, exit.");
                    Process.GetCurrentProcess().Kill();
                }

                if (!string.IsNullOrEmpty(kleur))
                {
                    try
                    {

                        using (Stream stream = File.Open(Locatie, FileMode.Create))
                        {
                            BinaryFormatter bin = new BinaryFormatter();
                            bin.Serialize(stream, LijstPersoonKleur);
                            laaste_versie_kleur = File.GetLastWriteTime(Locatie);
                            veranderdkleur = laaste_versie_kleur;
                        }
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(300);
                        BewaarPloegNamenOpKleurOpSchijf(kleur, --try_again);
                    }
                }
            }
        }
    }
}
