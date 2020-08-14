using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bezetting2
{
    class ModuleDatum
    {
        // rooster op 1-1-2015
        string[] rooster_volgorde
            = {"N", "N", "", "", "", "O", "O", "M", "M", "", "N",
            "N", "", "", "", "O", "O", "M", "M", "" ,"N", "N",
            "", "", "", "O", "O", "M", "M", ""};
        
        public string GetDienst(string rooster, DateTime datum, string ploeg = "")
        {
            // elke 10 dagen komt rooster terug.
            // zonder leapyear zou je dus %10 kunnen gebruiken van aantal dagen verschil
            // tussen DateTime(2007, 1, 1) en gevraagde dag.

            // echter elke tussen 28 feb en 1 maart als er geen leapyear is schuift tabel
            // 1 dag.

            // rooster volgorde is gemaakt op kleur rood, dus een offset maken afhankelijk gevraagde kleur

            string ret = "";
            int schuif_tabel_leapyear = 0;
            int schuif_tabel_standaard = 0;
            int schuif_tabel_ploegkleur = 0;

            switch (ploeg)
            {
                case "Blauw":
                    schuif_tabel_ploegkleur = 8;
                    break;
                case "Groen":
                    schuif_tabel_ploegkleur = 4;
                    break;
                case "Wit":
                    schuif_tabel_ploegkleur = 6;
                    break;
                case "Geel":
                    schuif_tabel_ploegkleur = 2;
                    break;
                default:
                    schuif_tabel_ploegkleur = 0;
                    break;
            }
            
            DateTime start_datum_tabel = new DateTime(2015, 1, 1);  // op deze datum begon rood met 1ste nacht

            // aantal dagen tussen start_datum_tabel en gevraagde
            System.TimeSpan diff1 = datum.Subtract(start_datum_tabel);
            int dag = diff1.Days;

            for (int i = 2015; i <= datum.Year; i++)
            {
                DateTime achtentwintigfeb = new DateTime(i, 2, 28);

                if (datum > achtentwintigfeb)
                    schuif_tabel_standaard++; // schuif elke 28 feb 1 op

                if (DateTime.IsLeapYear(i) && datum > achtentwintigfeb)
                    schuif_tabel_leapyear--; // behalve bij schrikkeljaar
            }

            dag = dag % 10;

            dag += schuif_tabel_leapyear + schuif_tabel_standaard + schuif_tabel_ploegkleur;
            if (dag > 10)
                dag -= 10;

            ret = rooster_volgorde[dag];

            return ret;
        }
        
        public string GetDag(DateTime datum)
        {
            string dag = "";
            switch (datum.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    dag = "Z";
                    break;
                case DayOfWeek.Monday:
                    dag = "M";
                    break;
                case DayOfWeek.Tuesday:
                    dag = "D";
                    break;
                case DayOfWeek.Wednesday:
                    dag = "W";
                    break;
                case DayOfWeek.Thursday:
                    dag = "D";
                    break;
                case DayOfWeek.Friday:
                    dag = "V";
                    break;
                case DayOfWeek.Saturday:
                    dag = "Z";
                    break;
                default:
                    dag = "*";
                    break;
            }

            return dag;
        }
    }
}
