using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Bezetting2
{
    public class ProgData
    {
        public static ToolStripStatusLabel _inlognaam;
        public static ToolStripStatusLabel _toegangnivo;
        public static string _bezettingafwijkingnaam;
        public static List<personeel> personeel_lijst = new List<personeel>();

        public static List<veranderingen> verander_lijst = new List<veranderingen>();

        //public static List<personeel> Namen_lijst { get; set; }
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
        public static string Huidige_Gebruiker
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

        public static void HaalVeranderList()
        {
            verander_lijst.Clear();
            try
            {
                using (Stream stream = File.Open(_bezettingafwijkingnaam, FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    verander_lijst = (List<veranderingen>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
            }
        }

        public static void VoegToe(veranderingen a)
        {
            verander_lijst.Add(a);
        }

        public static void SaveVeranderList()
        {
            try
            {
                using (Stream stream = File.Open(_bezettingafwijkingnaam, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, verander_lijst);
                }
            }
            catch (IOException)
            {

            }
        }

        public static void Save_Namen_lijst()
        {
            try
            {
                using (Stream stream = File.Open("personeel.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, personeel_lijst);
                }
            }
            catch (IOException)
            {

            }
        }

        public static void Save_Namen_lijst_in_vorige_maand(DateTime plek)
        {
            try
            {
                string dir_naam = plek.Year.ToString() + "\\" + plek.Month.ToString();
                string locatie = Path.GetFullPath(dir_naam + "\\personeel.bin");
                 
                using (Stream stream = File.Open(locatie, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, personeel_lijst);
                }
            }
            catch (IOException)
            {

            }
        }
    }
}
