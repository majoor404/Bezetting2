using System;

namespace Bezetting2
{
    public class ModuleDatum
    {
        // rooster op 1-1-2015 kleur rood
        private string[] rooster_volgorde
            = {"N", "N", "", "", "", "O", "O", "M", "M", "", "N",
            "N", "", "", "", "O", "O", "M", "M", "" ,"N", "N",
            "", "", "", "O", "O", "M", "M", ""};

        private string[] rooster_volgorde_long
            = {"Eerste Nacht", "Tweede Nacht", "", "", "", "Eerste Ochtend", "Tweede Ochtend", "Eerste Middag", "Tweede Middag", "", "Eerste Nacht",
            "Tweede Nacht", "", "", "", "Eerste Ochtend", "Tweede Ochtend", "Eerste Middag", "Tweede Middag", "" ,"Eerste Nacht", "Tweede Nacht",
            "", "", "", "Eerste Ochtend", "Tweede Ochtend", "Eerste Middag", "Tweede Middag", ""};

        public string GetDienstLong(string rooster, DateTime datum, string ploeg)
        {
            if (rooster == "dd")
                return "Dagdienst";

            int index = RekenTabelIndex(rooster, datum, ploeg);

            string ret = "";

            ret = rooster_volgorde_long[index];

            return ret;
        }

        public string GetDienst(string rooster, DateTime datum, string ploeg)
        {
            if (rooster == "dd")
            {
                return GetDag(datum) == "Z" ? "" : "-";
            }

            int index = RekenTabelIndex(rooster, datum, ploeg);

            string ret = "";

            ret = rooster_volgorde[index];

            return ret;
        }

        private int RekenTabelIndex(string rooster, DateTime datum, string ploeg)
        {
            // elke 10 dagen komt rooster terug.
            // zonder leapyear zou je dus %10 kunnen gebruiken van aantal dagen verschil
            // tussen DateTime(2007, 1, 1) en gevraagde dag.

            // echter elke tussen 28 feb en 1 maart als er geen leapyear is schuift tabel
            // 1 dag.

            // rooster volgorde is gemaakt op kleur rood, dus een offset maken afhankelijk gevraagde kleur

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

            return dag;
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

        // bepaal welke kleur er werkt op datum en dienst

        public string GetKleurDieWerkt(DateTime datum, string dienst)
        {
            string dienst_ = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), datum, "Blauw");
            if (dienst == dienst_)
                return "Blauw";
            dienst_ = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), datum, "Geel");
            if (dienst == dienst_)
                return "Geel";
            dienst_ = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), datum, "Wit");
            if (dienst == dienst_)
                return "Wit";
            dienst_ = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), datum, "Groen");
            if (dienst == dienst_)
                return "Groen";
            return "Rood";
        }
    }
}