using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Bezetting2.Data
{
    public class MaandDataClass
    {
        // als file zelfde datum heeft als deze, hoef ik niet te laden.
        private DateTime laaste_versie;

        [Serializable]
        public class Item
        {
            public Item() {}
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
        
        // twee versie's, afhankelijk wat we meegeven.
        public void Voeg_toe(DateTime datum, string personeel_nr, string afwijking, string invoer_door, string rede, string reserve1, string reserve2)
        {
            var nieuw = new Item();
            nieuw.datum_ = datum;                       // datum dat afwijking plaats vind
            nieuw.personeel_nr_ = personeel_nr;
            nieuw.afwijking_ = afwijking;
            nieuw.invoerdoor_ = invoer_door;
            nieuw.rede_ = rede;
            nieuw.item4_ = reserve1;
            nieuw.item5_ = reserve2;
            nieuw.ingevoerd_op_ = DateTime.Now;         // ingevoerd op
            MaandDataLijst.Add(nieuw);
        }

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
            
            DateTime Dat = new DateTime(int.Parse(subs[2]),int.Parse(subs[1]),int.Parse(subs[0]));
            MaandDataLijst[MaandDataLijst.Count-1].ingevoerd_op_ = Dat;
        }

        public void Load(string kleur)
        {
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.igekozenjaar;
            
            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_MaandData.bin");

            ProgData.Zetom_naar_versie21(kleur);

            if (File.Exists(path))
            {
                var veranderd = File.GetLastWriteTime(path);
                if (veranderd != laaste_versie)
                {
                    // laden
                    MaandDataLijst.Clear();
                    try
                    {
                        using (Stream stream = File.Open(path, FileMode.Open))
                        {
                            BinaryFormatter bin = new BinaryFormatter();
                            MaandDataLijst = (List<Item>)bin.Deserialize(stream);
                        }
                    }
                    catch { }

                    laaste_versie = veranderd;
                }
            }
            else
            {
                if(ProgData.TestNetwerkBeschikbaar(15))
                {
                    // save lege
                    Save(kleur);
                    //MessageBox.Show($"{ProgData.igekozenjaar}\\{ProgData.igekozenmaand}\\{kleur}_MaandData.bin bestond niet!\nNu aangemaakt");
                }
                else
                {
                    MessageBox.Show($"Netwerk problemen!");
                }
                  
            }
        }

        public void Save(string kleur)
        {
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.igekozenjaar;
            
            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_MaandData.bin");
                try
                {
                    using (Stream stream = File.Open(path, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, MaandDataLijst);
                        laaste_versie = File.GetLastWriteTime(path);
                    }
                }
                catch
                {
                    //
                }
        }

        public void NieuweMaandDataLijst()
        {
            MaandDataLijst.Clear();
            Save(ProgData.GekozenKleur);
        }
    }
}
