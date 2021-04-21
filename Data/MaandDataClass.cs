using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace Bezetting2.Data
{
    public class MaandDataClass
    {
        // als file zelfde datum heeft als deze, hoef ik niet te laden.
        private DateTime laaste_versie;
        private DateTime laaste_versie_van_huidige_kleur;
        private string laaste_path;
        public bool InVoerOudeData = false;

        [Serializable]
        public class Item
        {
            public Item() { }
            public DateTime ingevoerd_op_ { get; set; }
            public DateTime datum_ { get; set; }
            public string personeel_nr_ { get; set; }
            public string afwijking_ { get; set; }
            public string invoerdoor_ { get; set; }
            public string rede_ { get; set; }
            public string item4_ { get; set; }
            public string item5_ { get; set; }
        }

        public List<Item> MaandDataLijst = new List<Item>();
        private DateTime saveTime = DateTime.Now;

        public void Voeg_toe(string dag, string personeel_nr, string afwijking, string invoer_door, string rede, string reserve1, string reserve2)
        {
            DateTime dat = new DateTime(ProgData.igekozenjaar, ProgData.igekozenmaand, int.Parse(dag));
            var nieuw = new Item();
            nieuw.datum_ = dat;                         // datum dat afwijking plaats vind
            nieuw.personeel_nr_ = personeel_nr;
            nieuw.afwijking_ = afwijking;
            nieuw.invoerdoor_ = invoer_door;
            nieuw.rede_ = rede;
            nieuw.item4_ = reserve1;
            nieuw.item5_ = reserve2;
            nieuw.ingevoerd_op_ = DateTime.Now;         // ingevoerd op
            MaandDataLijst.Add(nieuw);
        }

        public void Load(string kleur)
        {
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.igekozenjaar;

            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_Maand_Data.bin");

            //als kleur weg, en niet bestaat, mag ik altijd een nieuwe maken
            if(kleur == "Weg")
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show($"Omdat kleur weg is en path niet bestaat,\n" +
                        $" maak ik nieuwe aan. {path}");
                    SaveLeegPloeg(kleur, path);
                }
            }

            if (File.Exists(path))
            {
                var veranderd = File.GetLastWriteTime(path);
                if (veranderd != laaste_versie || path != laaste_path)
                {
                    // geef netwerk de tijd om voorgaande te regelen ;-(
                    // tijd is meer dan een sec anders                    
                    Thread.Sleep(30);
                    // laden
                    try
                    {
                        using (Stream stream = File.Open(path, FileMode.Open))
                        {
                            MaandDataLijst.Clear();
                            BinaryFormatter bin = new BinaryFormatter();
                            MaandDataLijst = (List<Item>)bin.Deserialize(stream);
                        }
                    }
                    catch
                    {
                        MessageBox.Show($"kon {path} niet laden!, netwerk druk\nEven pauze...");
                        Thread.Sleep(1000);
                    }

                    if (kleur == ProgData.GekozenKleur)
                        laaste_versie_van_huidige_kleur = veranderd;

                    laaste_versie = veranderd;
                    laaste_path = path;
                }
            }
            else
            {
                if (!InVoerOudeData && TestZekerWetenNietAanwezig(5, kleur))
                {
                    Load(kleur);
                }
                else
                {
                    if (ProgData.RechtenHuidigeGebruiker == 101)
                    {
                        DialogResult dialogResult = MessageBox.Show($"Kijk eerst of file\n" +
                            $"{path}\n" +
                            $"Werkelijk niet bestaat\n" +
                            $"Zo niet, druk op Ja!\n" +
                            $"Er wordt dan een nieuwe LEGE maand data gemaakt voor deze ploeg.", "Vraagje", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            SaveLeegPloeg(kleur, path);
                        }
                    }
                    else
                    {

                        string melding = $"Kan {jaar}\\{maand}\\{kleur}_Maand_Data.bin niet laden," +
                            $"\nlaat een Admin van u groep dit oplossen!\n" +
                            $"Deze file kan alleen gecreerd worden bij maken van juiste directory.\n" +
                            $"Of door een Admin";
                        MessageBox.Show(melding);
                        Process.GetCurrentProcess().Kill();
                    }
                }

            }
        }

        public void Save(string kleur, int try_again)
        {
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.igekozenjaar;
            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_Maand_Data.bin");

            // als ik hier snel weer kom ( > 2 sec ?), dan even wachten ivm netwerk traagheid
            var diffInSeconds = (DateTime.Now - saveTime).TotalSeconds;
            if (diffInSeconds < 2)
                Thread.Sleep(300);

            if (try_again < 0)
            {
                MessageBox.Show($"Kon bestand \n{path}\nNiet saven?\nExit.");
                Process.GetCurrentProcess().Kill();
            }

            try
            {
                using (Stream stream = File.Open(path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, MaandDataLijst);
                }
                saveTime = DateTime.Now;
            }
            catch
            {
                Thread.Sleep(300);
                Save(kleur, --try_again);
            }
        }

        public bool TestNieuweFile(string kleur)
        {
            var path = Path.GetFullPath($"{ProgData.igekozenjaar}\\{ProgData.igekozenmaand}\\{kleur}_Maand_Data.bin");

            if (File.Exists(path))
            {
                var veranderd = File.GetLastWriteTime(path);
                if (veranderd != laaste_versie_van_huidige_kleur)
                {
                    return true;
                }
            }
            return false;
        }

        private bool TestZekerWetenNietAanwezig(int aantal, string kleur)
        {
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.igekozenjaar;

            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_Maand_Data.bin");

            for (int i = 0; i < aantal; i++)
            {
                if (ProgData.TestNetwerkBeschikbaar(15))
                {
                    if (File.Exists(path))
                        return true;
                }
                else
                {
                    MessageBox.Show("Netwerk problemen, Exit!");
                    Process.GetCurrentProcess().Kill();
                }
                Thread.Sleep(1000);
            }
            return false;
        }

        public void SaveLeegPloeg(string kleur)
        {
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.igekozenjaar;
            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_Maand_Data.bin");
            MaandDataLijst.Clear();
            try
            {
                using (Stream stream = File.Open(path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, MaandDataLijst);
                }
            }
            catch
            {
            }
        }

        public void SaveLeegPloeg(string kleur, string path)
        {
            MaandDataLijst.Clear();
            try
            {
                using (Stream stream = File.Open(path, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, MaandDataLijst);
                }
            }
            catch
            {
            }
        }

        public void VeranderInvoerDatum(string datum)   // bij omzetten versie 2.0 naar 2.1 nodig
        {
            string[] subs;
            if (datum.Contains("-"))
            {
                subs = datum.Split('-');
            }
            else
            {
                subs = datum.Split('/');
            }

            DateTime Dat = new DateTime(int.Parse(subs[2]), int.Parse(subs[1]), int.Parse(subs[0]));
            MaandDataLijst[MaandDataLijst.Count - 1].ingevoerd_op_ = Dat;
        }
    }
}
