using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bezetting2.Data
{
    public class AllVerCain
    {
        public enum soort_
        {
            dag_inhoud,             // bv ipv M een VK
            locatie_inhoud          // van leeg naar PVK
        }

        // als file zelfde datum heeft als deze, hoef ik niet te laden.
        private DateTime laaste_versie;

        [Serializable]
        public class Item
        {
            public Item(soort_ a, DateTime b, string c, string d, string e, string f, string g)
            {
                soort_verandering_ = a;
                datum_ = b;
                personeel_nr_ = c;
                item1_ = d;
                item2_ = e;
                item3_ = f;
                item4_ = g;
            }
            private soort_ soort_verandering_ { get; set; }
            private DateTime datum_ { get; set; }
            private string personeel_nr_ { get; set; }
            private string item1_ { get; set; }
            private string item2_ { get; set; }
            private string item3_ { get; set; }
            private string item4_ { get; set; }
        }

        private List<Item> ketting_verander_lijst = new List<Item>();

        public void voeg_toe(soort_ a, DateTime datum, string personeel_nr, string d, string e, string f, string g)
        {
            ketting_verander_lijst.Add(new Item(a, datum, personeel_nr, d, e, f, g));
        }
        public void voeg_toe(soort_ a, string dag, string personeel_nr, string d, string e, string f, string g)
        {
            DateTime dat = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, int.Parse(dag));
            ketting_verander_lijst.Add(new Item(a, dat, personeel_nr, d, e, f, g));
        }

        public void Load()
        {
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.Igekozenjaar;
            var kleur = ProgData.GekozenKleur;
            
            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_log.bin");
            if (File.Exists(path))
            {
                var veranderd = File.GetLastWriteTime(path);
                if (veranderd != laaste_versie)
                {
                    // laden
                    ketting_verander_lijst.Clear();
                    try
                    {
                        using (Stream stream = File.Open(path, FileMode.Open))
                        {
                            BinaryFormatter bin = new BinaryFormatter();
                            ketting_verander_lijst = (List<Item>)bin.Deserialize(stream);
                        }
                    }
                    catch { }

                    laaste_versie = veranderd;
                }
            }
        }

        public void Save()
        {
            var maand = ProgData.igekozenmaand;
            var jaar = ProgData.Igekozenjaar;
            var kleur = ProgData.GekozenKleur;
            var path = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_log.bin");
                try
                {
                    using (Stream stream = File.Open(path, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, ketting_verander_lijst);
                        laaste_versie = File.GetLastWriteTime(path);
                    }
                }
                catch
                {
                    //
                }
        }
    }
}
