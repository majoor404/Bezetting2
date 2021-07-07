using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bezetting2.Data
{
    [Serializable]
    public class WerkPlek
    {
        public string naam_ { get; set; }
        public string werkplek_ { get; set; }
        public int dagnummer_ { get; set; }

        public static List<WerkPlek> LijstWerkPlekPloeg = new List<WerkPlek>();

        public WerkPlek()
        {
        }
       
        
        public static void AddWerkPlek(string naam,string werkplek, int dag)
        {
            WerkPlek wp = new WerkPlek();
            wp.dagnummer_ = dag;
            wp.naam_ = naam;
            wp.werkplek_ = werkplek;
            LijstWerkPlekPloeg.Add(wp);
        }

        public static void LaadWerkPlek(string Kleur, int maand, int jaar)
        {
            string file = Path.GetFullPath($"{jaar}\\{maand}\\{Kleur}_WerkPlek.bin");
            LijstWerkPlekPloeg.Clear();
            if (File.Exists(file))
            {
                try
                {
                    using (Stream stream = File.Open(file, FileMode.Open))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        try
                        {
                            LijstWerkPlekPloeg = (List<WerkPlek>)bin.Deserialize(stream);
                            stream.Dispose();
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
                catch{}
            }
        }

        public static void SafeWerkPlek(string Kleur, int maand, int jaar)
        {
            string file = Path.GetFullPath($"{jaar}\\{maand}\\{Kleur}_WerkPlek.bin");

            //var diffInSeconds = (DateTime.Now - saveSaveTime).TotalSeconds;
            //saveSaveTime = DateTime.Now;

            //_ = diffInSeconds < 1 ? saveSaveTel++ : saveSaveTel = 0;

            //if (saveSaveTel > 4)
            //{
            //    Thread.Sleep(600);
            //    saveSaveTel = 0;
            //}

            if (!string.IsNullOrEmpty(Kleur))
            {
                try
                {
                    using (Stream stream = File.Open(file, FileMode.OpenOrCreate))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, LijstWerkPlekPloeg);
                    }
                }
                catch{}
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

        public static void SetWerkPlek(string naam, int Dag, string werkplek)
        {
            try
            {
                WerkPlek wp = LijstWerkPlekPloeg.First(a => a.naam_ == naam && a.dagnummer_ == Dag);
                wp.werkplek_ = werkplek;
            }
            catch 
            {
                AddWerkPlek(naam, werkplek, Dag);
            }
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

        public static void DeleteWerkPlek(string naam, int Dag)
        {
            for (int i = 0; i < LijstWerkPlekPloeg.Count; i++)
            {
                if (LijstWerkPlekPloeg[i].dagnummer_ == Dag && LijstWerkPlekPloeg[i].naam_ == naam)
                {
                    LijstWerkPlekPloeg.RemoveAt(i);
                    break;
                }
            }
        }
    }


}
