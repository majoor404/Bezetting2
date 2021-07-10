using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Bezetting2.Data
{
    [Serializable]
    public class WerkPlek
    {
        private static DateTime laaste_versie;
        public string naam_ { get; set; }
        public string werkplek_ { get; set; }
        public int dagnummer_ { get; set; }

        public static List<WerkPlek> LijstWerkPlekPloeg = new List<WerkPlek>();

        public WerkPlek()
        {
        }

        public static void AddWerkPlek(string naam, string werkplek, int dag)
        {
            WerkPlek wp = new WerkPlek();
            wp.dagnummer_ = dag;
            wp.naam_ = naam;
            wp.werkplek_ = werkplek;
            LijstWerkPlekPloeg.Add(wp);
        }

        public static void LaadWerkPlek(string kleur, int maand, int jaar)
        {
            string file = Path.GetFullPath($"{jaar}\\{maand}\\{kleur}_WerkPlek.bin");
            var veranderd = File.GetLastWriteTime(file);
            if (veranderd != laaste_versie)
            {
                if (File.Exists(file))
                {
                    try
                    {
                        using (Stream stream = File.Open(file, FileMode.Open))
                        {
                            BinaryFormatter bin = new BinaryFormatter();
                            try
                            {
                                //MessageBox.Show($"Laad nieuwe werkplek data.\n{maand}");
                                LijstWerkPlekPloeg.Clear();
                                LijstWerkPlekPloeg = (List<WerkPlek>)bin.Deserialize(stream);
                                stream.Dispose();
                                laaste_versie = veranderd;
                            }
                            catch
                            {
                            }
                            finally
                            {
                                if (stream != null)
                                    stream.Dispose();
                            }
                        }
                    }
                    catch { }
                }
                else
                {
                    //MessageBox.Show($"{file}\nbestond niet");
                    LijstWerkPlekPloeg.Clear();
                }
            }
        }

        public static string GetWerkPlek(string naam, int Dag)
        {
            string ret = "";
            try
            {
                WerkPlek wp = LijstWerkPlekPloeg.First(a => a.naam_ == naam && a.dagnummer_ == Dag);
                ret = wp.werkplek_;
            }
            catch { }
            return ret;
        }

        public static void SetWerkPlek(string naam, DateTime Datum, string werkplek)
        {
            LaadWerkPlek(ProgData.GekozenKleur, Datum.Month, Datum.Year);
            try
            {
                WerkPlek wp = LijstWerkPlekPloeg.First(a => a.naam_ == naam && a.dagnummer_ == Datum.Day);
                wp.werkplek_ = werkplek;
            }
            catch
            {
                AddWerkPlek(naam, werkplek, Datum.Day);
            }

            string file = Path.GetFullPath($"{Datum.Year}\\{Datum.Month}\\{ProgData.GekozenKleur}_WerkPlek.bin");

            try
            {
                using (Stream stream = File.Open(file, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, LijstWerkPlekPloeg);
                    laaste_versie = File.GetLastWriteTime(file);
                }
            }
            catch { }

        }

        // voor +naam in overzicht
        public static bool CheckWerkPlek(string naam, int Dag)
        {
            try
            {
                WerkPlek wp = LijstWerkPlekPloeg.First(a => a.naam_ == naam && a.dagnummer_ == Dag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteWerkPlek(string naam, DateTime Datum)
        {
            for (int i = 0; i < LijstWerkPlekPloeg.Count; i++)
            {
                if (LijstWerkPlekPloeg[i].dagnummer_ == Datum.Day && LijstWerkPlekPloeg[i].naam_ == naam)
                {
                    LijstWerkPlekPloeg.RemoveAt(i);
                    break;
                }
            }

            string file = Path.GetFullPath($"{Datum.Year}\\{Datum.Month}\\{ProgData.GekozenKleur}_WerkPlek.bin");
            try
            {
                using (Stream stream = File.Open(file, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, LijstWerkPlekPloeg);
                    laaste_versie = File.GetLastWriteTime(file);
                }
            }
            catch { }
        }
    }
}
