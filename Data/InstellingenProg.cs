using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Bezetting2.Data
{
    // data opslaan als ini, zodat bij aanpasingen je clas kan blijven lezen
    public class InstellingenProg
    {
        public static bool _GebruikExtraRuil
        {
            get { return bool.Parse(ProgrammaData[41]); }
            set { ProgrammaData[41] = value.ToString(); }
        }

        public static bool _GebruikSnipper
        {
            get { return bool.Parse(ProgrammaData[42]); }
            set { ProgrammaData[42] = value.ToString(); }
        }

        public static int _MinimaalAantalPersonen
        {
            get
            {
                try
                {
                    return int.Parse(ProgrammaData[43]);
                }
                catch
                {
                    return 0;
                };
            }
            set { ProgrammaData[43] = value.ToString(CultureInfo.CurrentCulture); }
        }

        public static string _Rooster
        {
            get
            {
                try
                {
                    return ProgrammaData[44];
                }
                catch
                {
                    return "5pl";
                };
            }
            set { ProgrammaData[44] = value.ToString(); }
        }

        public static bool _TelVakAlsVK
        {
            get { return bool.Parse(ProgrammaData[45]); }
            set { ProgrammaData[45] = value.ToString(); }
        }

        public static bool _Wachtoverzicht2Dagen
        {
            get { return bool.Parse(ProgrammaData[46]); }
            set { ProgrammaData[46] = value.ToString(); }
        }

        public static string _LocatieKalender
        {
            get { return ProgrammaData[47]; }
            set { ProgrammaData[47] = value.ToString(); }

        }

        public static bool _GebruikWachtOverzicht
        {
            get { return bool.Parse(ProgrammaData[48]); }
            set { ProgrammaData[48] = value.ToString(); }
        }

        public static List<string> ProgrammaData = new List<string>();

        public static void LeesProgrammaData()
        {
            ProgrammaData.Clear();
            try
            {
                // strings to class data
                // als niet bestaat maak lege aan.
                if (!File.Exists("BezData\\Programdata.ini"))
                {
                    for (int i = 0; i < 100; i++)
                    {
                        ProgrammaData.Add(true.ToString());
                    }
                    ProgrammaData[46] = "False"; // als niet bestaat standaard 1 wachtopverzicht.
                    // maak lege aan
                    SaveProgrammaData();
                }
                ProgrammaData = File.ReadAllLines("BezData\\Programdata.ini").ToList();
            }
            catch (IOException)
            {
                MessageBox.Show("LeesProgrammaData() error");
            }
        }

        public static void SaveProgrammaData()
        {
            try
            {
                // tot ProgrammaData[40] is data wachoverzicht
                ProgrammaData[41] = _GebruikExtraRuil.ToString();
                ProgrammaData[42] = _GebruikSnipper.ToString();
                File.WriteAllLines("BezData\\Programdata.ini", ProgrammaData);
            }
            catch (IOException)
            {
                MessageBox.Show("SaveProgrammaData() error");
            }
        }





    }
}