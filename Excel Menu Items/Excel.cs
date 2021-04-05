using Bezetting2.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using static Bezetting2.DatumVijfPloegUtils;

namespace Bezetting2
{
    public partial class MainFormBezetting2 : Form
    {

        static List<ClassTelAfwijkingenVanPersoon> LijstclassTelAfwijkingenVanPersoons = new List<ClassTelAfwijkingenVanPersoon>();
        public class ClassTelAfwijkingenVanPersoon
        {
            public ClassTelAfwijkingenVanPersoon(int persnr, string afwijking)
            {
                Persnr_ = persnr;
                Afwijking_ = afwijking;
                Aantal_ = 1;
            }

            static public void voeg_toe(int persnr, string afwijking)
            {
                try
                {
                    // als gevonden dan optellen
                    ClassTelAfwijkingenVanPersoon persoon = LijstclassTelAfwijkingenVanPersoons.First(a => a.Persnr_ == persnr && a.Afwijking_ == afwijking);
                    persoon.Aantal_++;
                }
                catch
                {
                    // zo niet dan toevoegen nieuwe
                    ClassTelAfwijkingenVanPersoon a = new ClassTelAfwijkingenVanPersoon(persnr, afwijking);
                    LijstclassTelAfwijkingenVanPersoons.Add(a);
                }


            }

            public string Afwijking_ { get; set; }
            public int Aantal_ { get; set; }
            public int Persnr_ { get; set; }
        }

        private void AfwijkingenTovRoosterIngelogdPersoonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int bewaar_maand = ProgData.igekozenmaand;
            int bewaar_jaar = ProgData.igekozenjaar;

            ListClassTelAfwijkingen.Clear();

            //string locatie = @"BezData\\telnietmee.ini";
            //ListTelNietMeeNamen = File.ReadAllLines(locatie).ToList();

            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel._Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Range oRng;

            for (int i = 1; i < 13; i++)    // maanden
            {
                ProgData.igekozenmaand = i;
                GetAfwijkingenPersoonInEenMaand(ProgData.Huidige_Gebruiker_Personeel_nummer, ProgData.ihuidigjaar, ProgData.igekozenmaand);
            }
            ProgData.igekozenmaand = bewaar_maand;
            ProgData.igekozenjaar = bewaar_jaar;

            try
            {
                //Start Excel and get Application object.
                xlApp = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                string file = Path.GetFullPath("BezData\\OverzichtPersoon.xls");

                xlWorkBook = xlApp.Workbooks.Open(file, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, true, 0, true, 1, 0);

                Microsoft.Office.Interop.Excel.Worksheet excelSheet = xlWorkBook.ActiveSheet;

                // schoonvegen

                // eerste getal is regel
                // tweede kolom

                oRng = excelSheet.Range[excelSheet.Cells[1, 2], excelSheet.Cells[3, 2]];
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[3, 5], excelSheet.Cells[34, 6]];
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[21, 4], excelSheet.Cells[34, 4]];
                oRng.ClearContents();
                oRng.Value = null;

                excelSheet.Cells[1, 2] = ProgData.Sgekozenjaar();
                excelSheet.Cells[2, 2] = ProgData.Get_Gebruiker_Kleur(ProgData.Huidige_Gebruiker_Personeel_nummer);
                excelSheet.Cells[3, 2] = ProgData.Get_Gebruiker_Naam(ProgData.Huidige_Gebruiker_Personeel_nummer);

                int row = 21;
                int vaste_regel;

                foreach (ClassTelAfwijkingen afwijkingen in ListClassTelAfwijkingen)
                {
                    switch (afwijkingen._Afwijking)
                    {
                        case "VK":
                            vaste_regel = 3;
                            break;

                        case "8OI":
                            vaste_regel = 4;
                            break;

                        case "Z":
                            vaste_regel = 5;
                            break;

                        case "VF":
                            vaste_regel = 6;
                            break;

                        case "GP":
                            vaste_regel = 7;
                            break;

                        case "OPLO":
                            vaste_regel = 8;
                            break;

                        case "OPL":
                            vaste_regel = 9;
                            break;

                        case "K":
                            vaste_regel = 10;
                            break;

                        case "ADV":
                            vaste_regel = 11;
                            break;

                        case "BV":
                            vaste_regel = 12;
                            break;

                        case "V":
                            vaste_regel = 13;
                            break;

                        case "W":
                            vaste_regel = 14;
                            break;

                        case "1/2 VK":
                            vaste_regel = 15;
                            break;

                        case "*":
                            vaste_regel = 16;
                            break;

                        case "ED":
                            vaste_regel = 17;
                            break;

                        case "RD":
                            vaste_regel = 18;
                            break;

                        case "VD":
                            vaste_regel = 19;
                            break;

                        default:
                            vaste_regel = 0;
                            break;
                    }

                    if (vaste_regel == 0)
                    {
                        vaste_regel = row;
                        excelSheet.Cells[vaste_regel, 4] = afwijkingen._Afwijking;
                        row++;
                    }

                    if (afwijkingen._Toekomst)
                    {
                        excelSheet.Cells[vaste_regel, 6] = afwijkingen._Aantal;
                    }
                    else
                    {
                        excelSheet.Cells[vaste_regel, 5] = afwijkingen._Aantal;
                    }
                }

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void GetAfwijkingenPersoonInEenMaand(string persnum, int jaar, int maand)
        {
            string kleur = ProgData.Get_Gebruiker_Kleur(persnum);
            string naam = ProgData.Get_Gebruiker_Naam(persnum);
            int AantaldagenDezeMaand = DateTime.DaysInMonth(jaar, maand);
            string[] deze_maand_overzicht_persoon = new string[33];

            //eerst vanuit ploegbezetting een string list maken met afwijkingen en normaal schema van persoon
            if (File.Exists(ProgData.LijstWerkdagPloeg_Locatie(kleur)))
            {
                ProgData.LaadLijstWerkdagPloeg(kleur, 15);
                foreach (werkdag dag in ProgData.LijstWerkdagPloeg)
                {
                    if (dag._naam == naam)
                    {
                        if (dag._dagnummer < AantaldagenDezeMaand + 1)
                        {
                            dag_gekozen = new DateTime(jaar, maand, dag._dagnummer);

                            // get en zet eerst orginele dienst
                            string wacht = GetDienst(ProgData.GekozenRooster(), dag_gekozen, kleur);
                            if (!string.IsNullOrEmpty(wacht))
                            {
                                deze_maand_overzicht_persoon[dag._dagnummer] = "W"; // Werkdag
                            }
                            else
                            {
                                deze_maand_overzicht_persoon[dag._dagnummer] = "V"; // Rooster vrij
                            }

                            // daarna overschrijven als die afwijkt van ""
                            if (!string.IsNullOrEmpty(dag._afwijkingdienst))
                            {
                                string dum = dag._afwijkingdienst.ToUpper();
                                if (dum.Length > 3 && dum.Substring(0, 3) == "ED-")
                                {
                                    dum = "ED";
                                }
                                if (dum.Length > 3 && dum.Substring(0, 3) == "RD-")
                                {
                                    dum = "RD";
                                }
                                if (dum.Length > 3 && dum.Substring(0, 3) == "VD-")
                                {
                                    dum = "VD";
                                }
                                if (InstellingenProg._TelVakAlsVK && dum == "VAK")
                                {
                                    dum = "VK";
                                }

                                if (dum == "A")
                                {
                                    dum = "VK";
                                }

                                deze_maand_overzicht_persoon[dag._dagnummer] = dum;
                            }

                            if (ListTelNietMeeNamen.Contains(deze_maand_overzicht_persoon[dag._dagnummer]))
                            {
                                deze_maand_overzicht_persoon[dag._dagnummer] = "W"; // Werkdag
                            }
                        }
                    }
                }
            }

            // lijst strings nu klaar, nu tellen.
            //int dagen = DateTime.DaysInMonth(jaar, maand);
            for (int q = 1; q < AantaldagenDezeMaand + 1; q++)
            {
                if (deze_maand_overzicht_persoon[q] != null)
                {
                    dag_gekozen = new DateTime(ProgData.ihuidigjaar, ProgData.igekozenmaand, q);
                    bool toekomst = dag_gekozen > DateTime.Now;

                    ClassTelAfwijkingen afwijking_dum;
                    try
                    {
                        afwijking_dum = ListClassTelAfwijkingen.Find(x => (x._Afwijking == deze_maand_overzicht_persoon[q] && x._Toekomst == toekomst));
                        if (afwijking_dum != null)
                        {
                            afwijking_dum._Aantal++;
                        }
                        else
                        {
                            ListClassTelAfwijkingen.Add(new ClassTelAfwijkingen(deze_maand_overzicht_persoon[q], 1, toekomst));
                        }
                    }
                    catch { }

                    //if (!ListAfwijkingen.Contains(deze_maand_overzicht_persoon[q]))
                    //{
                    //    ListAfwijkingen.Add(deze_maand_overzicht_persoon[q]);
                    //    ListTelling.Add(0);
                    //}

                    //int index = ListAfwijkingen.FindIndex(a => a.Contains(deze_maand_overzicht_persoon[q]));
                    //ListTelling[index]++;
                }
            }
        }

        private void ZetGevondenDataTellingVuilWerktInExcel()
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel._Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                xlApp = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                string file = Path.GetFullPath("BezData\\vuilwerk.xls");

                /*
                 public Microsoft.Office.Interop.Excel.Workbook Open
                (string Filename,
                object UpdateLinks,
                object ReadOnly,
                object Format,
                object Password,
                object WriteResPassword,
                object IgnoreReadOnlyRecommended,
                object Origin,
                object Delimiter,
                object Editable,
                object Notify,
                object Converter,
                object AddToMru,
                object Local,
                object CorruptLoad);
                */

                xlWorkBook = xlApp.Workbooks.Open(file, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, true, 0, true, 1, 0);

                Microsoft.Office.Interop.Excel.Worksheet excelSheet = xlWorkBook.ActiveSheet;

                // schoonvegen
                oRng = excelSheet.Range[excelSheet.Cells[11, 1], excelSheet.Cells[37, 33]];
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[11, 35], excelSheet.Cells[37, 36]];
                oRng.ClearContents();
                oRng.Value = null;

                int row = 11;
                int aantalvuilwerkcodes = ListVuilwerkData.Count / 3;

                // invullen
                foreach (ClassTelVuilwerk afw in ListClassTelVuilwerk)
                {
                    string cellValue = (string)(excelSheet.Cells[row, 2] as Microsoft.Office.Interop.Excel.Range).Value;

                    // voor eerste persoon
                    if (cellValue == null)
                    {
                        excelSheet.Cells[row, 1] = ProgData.Get_Gebruiker_Nummer(afw._NaamTelVuil);
                        excelSheet.Cells[row, 2] = afw._NaamTelVuil;
                        cellValue = afw._NaamTelVuil;

                        for (int i = 0; i < aantalvuilwerkcodes; i++)
                        {
                            excelSheet.Cells[row + i, 35] = ListVuilwerkData[1 + (i * 3)];
                            excelSheet.Cells[row + i, 36] = ListVuilwerkData[2 + (i * 3)];
                        }
                    }

                    if (cellValue != afw._NaamTelVuil)  // nieuwe naam
                    {
                        row += aantalvuilwerkcodes;

                        // nieuwe naam
                        excelSheet.Cells[row, 1] = ProgData.Get_Gebruiker_Nummer(afw._NaamTelVuil);
                        excelSheet.Cells[row, 2] = afw._NaamTelVuil;
                        cellValue = afw._NaamTelVuil;
                        for (int i = 0; i < aantalvuilwerkcodes; i++)
                        {
                            excelSheet.Cells[row + i, 35] = ListVuilwerkData[1 + (i * 3)];
                            excelSheet.Cells[row + i, 36] = ListVuilwerkData[2 + (i * 3)];
                        }
                    }

                    int dag = int.Parse(afw._DagTelVuil);
                    for (int i = 0; i < aantalvuilwerkcodes; i++)
                    {
                        excelSheet.Cells[row + i, dag + 2] = ListVuilwerkData[i * 3];
                    }
                }

                excelSheet.Cells[1, 2] = ProgData.Sgekozenmaand();
                excelSheet.Cells[6, 2] = ProgData.GekozenKleur;

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void ZetGevondenDataTellingWaarGewerktInExcel()
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                // Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Waar gewerkt in Jaar : ";
                oSheet.Cells[1, 2] = ProgData.Sgekozenjaar();
                oSheet.Cells[2, 1] = "Ploegkleur : ";
                oSheet.Cells[2, 2] = ProgData.GekozenKleur;

                for (int i = 0; i < ListTelWerkPlek.Count; i++)
                {
                    oSheet.Cells[3, i + 2] = ListTelWerkPlek[i];
                }

                oSheet.get_Range("A1", "Z2").Font.Bold = true;

                for (int i = 0; i < ListTelNamen.Count; i++)
                {
                    oSheet.Cells[i + 4, 1] = ListTelNamen[i];
                }

                for (int i = 0; i < ListClassTelPlekGewerkt.Count; i++)
                {
                    var test = ListClassTelPlekGewerkt[i]._PlekTelPlek;
                    var test2 = ListTelWerkPlek.IndexOf(test);      // y coord
                    var test4 = ListClassTelPlekGewerkt[i]._NaamTelPlek;
                    var test5 = ListTelNamen.IndexOf(test4);      // x coord    als naam niet gevonden, dan antwoord -1 bv een extra dienst
                    var test3 = ListClassTelPlekGewerkt[i]._AantalTelPlek;      // inhoud

                    if (test5 > -1)
                    {
                        var row = ListTelNamen.IndexOf(ListClassTelPlekGewerkt[i]._NaamTelPlek) + 4;
                        var col = ListTelWerkPlek.IndexOf(ListClassTelPlekGewerkt[i]._PlekTelPlek) + 2;
                        oSheet.Cells[row, col] = ListClassTelPlekGewerkt[i]._AantalTelPlek;
                    }
                }

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "Z3");
                oRng.EntireColumn.AutoFit();

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void VuilwerkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("BezData\\vuilwerk.ini"))
            {
                ListVuilwerkData.Clear();
                ListVuilwerkData = File.ReadAllLines("BezData\\vuilwerk.ini").ToList();

                List<string> TelNietMeeNamen = new List<string>();
                string locatie = @"BezData\\telnietmee.ini";
                try
                {
                    TelNietMeeNamen = File.ReadAllLines(locatie).ToList();
                }
                catch { }

                // eerst maar lijst maken wie vuilwerk verdiend.
                ListClassTelVuilwerk.Clear();
                int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);

                string korte_afwijking = "";

                foreach (personeel a in ProgData.AlleMensen.LijstPersoonKleur)
                {
                    if ((a._vuilwerk == "True"))
                    {
                        for (int i = 0; i < View.Items.Count; i++) // alle namen/rows
                        {
                            if (a._achternaam == View.Items[i].Text) // gevonden naam
                            {
                                for (int d = 1; d < aantal_dagen_deze_maand + 1; d++)
                                {
                                    bool rechtop = false;

                                    string afwijking = View.Items[i].SubItems[d].Text;
                                    string dienst = View.Items[3].SubItems[d].Text;

                                    if (string.IsNullOrEmpty(afwijking)) rechtop = true;
                                    if (afwijking == "*") rechtop = true;

                                    if (TelNietMeeNamen.Contains(afwijking))
                                        rechtop = true;

                                    if (afwijking.Length > 3)
                                        korte_afwijking = afwijking.Substring(0, 2);

                                    if (korte_afwijking == "ED")
                                    {
                                        rechtop = true;
                                        dienst = "X"; // extra
                                    }

                                    if (korte_afwijking == "VD")
                                    {
                                        rechtop = true;
                                        dienst = "X"; // extra
                                    }
                                    if (korte_afwijking == "RD")
                                    {
                                        rechtop = true;
                                        dienst = "X"; // extra
                                    }

                                    if (rechtop && !string.IsNullOrEmpty(dienst))
                                    {
                                        ClassTelVuilwerk afw = new ClassTelVuilwerk(a._achternaam, d.ToString());
                                        ListClassTelVuilwerk.Add(afw);   // recht op vuilwerk
                                        korte_afwijking = "";
                                    }
                                }
                            }
                        }
                    }
                }
                ZetGevondenDataTellingVuilWerktInExcel();
            }
            else // geen BezData\\vuilwerk.ini
            {
                MessageBox.Show("Geen BezData\\vuilwerk.ini gevonden!");
            }
        }

        private void TellingWaarGewerktToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListTelNamen.Clear();
                ListTelWerkPlek.Clear();
                ListClassTelPlekGewerkt.Clear();
                // bewaar huidige maand en kleur
                int bewaar_maand = ProgData.igekozenmaand;

                for (int i = 1; i < 13; i++)    // maanden
                {
                    ProgData.igekozenmaand = i;
                    if (File.Exists(ProgData.LijstWerkdagPloeg_Locatie(ProgData.GekozenKleur)))
                    {
                        ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);
                        ProgData.AlleMensen.HaalPloegNamenOpKleur(ProgData.GekozenKleur);
                        //ProgData.LaadLijstPersoneelKleur(ProgData.GekozenKleur, 15);

                        foreach (personeel a in ProgData.AlleMensen.LijstPersoonKleur)
                        {
                            if (!ListTelNamen.Contains(a._achternaam))
                                ListTelNamen.Add(a._achternaam);
                        }

                        foreach (werkdag a in ProgData.LijstWerkdagPloeg)
                        {
                            if (!ListTelWerkPlek.Contains(a._werkplek) && !string.IsNullOrEmpty(a._werkplek))
                                ListTelWerkPlek.Add(a._werkplek);

                            if (!string.IsNullOrEmpty(a._werkplek))
                            {
                                ClassTelPlekGewerkt tel = new ClassTelPlekGewerkt(a._naam, a._werkplek);
                                try
                                {
                                    ClassTelPlekGewerkt gevonden = ListClassTelPlekGewerkt.First(b => b._NaamTelPlek == a._naam && b._PlekTelPlek == a._werkplek);
                                    gevonden._AantalTelPlek++;
                                }
                                catch
                                {
                                    ListClassTelPlekGewerkt.Add(tel);
                                }
                            }
                        }
                    }
                }
                ZetGevondenDataTellingWaarGewerktInExcel();
                ProgData.igekozenmaand = bewaar_maand;
                ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);
                ProgData.AlleMensen.HaalPloegNamenOpKleur(ProgData.GekozenKleur);
                //ProgData.LaadLijstPersoneelKleur(ProgData.GekozenKleur, 15);
            }
            catch
            {
            }
        }

        private void NietMeeTelLijstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("BezData\\telnietmee.ini");
        }

        public void ToExcelNamenEnAdressen(string kleur)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Achter Naam";
                oSheet.Cells[1, 2] = "Voor Naam";
                oSheet.Cells[1, 3] = "Adres";
                oSheet.Cells[1, 4] = "Postcode";
                oSheet.Cells[1, 5] = "Plaats";
                oSheet.Cells[1, 6] = "E-mail";

                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.get_Range("A1", "F1").Font.Bold = true;
                oSheet.get_Range("A1", "F1").VerticalAlignment =
                Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                int start_row = 2;
                foreach (personeel a in ProgData.AlleMensen.LijstPersonen)
                {
                    if (a._kleur == kleur || kleur == "Allemaal")
                    {
                        oSheet.Cells[start_row, 1] = a._achternaam;
                        oSheet.Cells[start_row, 2] = a._voornaam;
                        oSheet.Cells[start_row, 3] = a._adres;
                        oSheet.Cells[start_row, 4] = a._postcode;
                        oSheet.Cells[start_row, 5] = a._woonplaats;
                        oSheet.Cells[start_row, 6] = a._emailthuis;
                        start_row++;
                    }
                }

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "F1");
                oRng.EntireColumn.AutoFit();

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void BlauwToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToExcelNamenEnAdressen("Blauw");
        }

        private void GroenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToExcelNamenEnAdressen("Groen");
        }

        private void WitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToExcelNamenEnAdressen("Wit");
        }

        private void RoodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToExcelNamenEnAdressen("Rood");
        }

        private void AllemaalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToExcelNamenEnAdressen("Allemaal");
        }

        private void AfwijkingTovRoosterPloegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel._Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Range oRng;

            List<int> Namen = new List<int>();
            int bewaar_maand = ProgData.igekozenmaand;
            // haal eerst de namen die dit jaar op kleur x gelopen hebben
            for (int i = 1; i < 13; i++)
            {
                ProgData.igekozenmaand = i;
                if (File.Exists(ProgData.Ploeg_Namen_Locatie(ProgData.GekozenKleur)))
                {
                    ProgData.AlleMensen.HaalPloegNamenOpKleur(ProgData.GekozenKleur);
                    //ProgData.LaadLijstPersoneelKleur(ProgData.GekozenKleur, 15);
                    if (ProgData.AlleMensen.LijstPersoonKleur.Count > 0)
                    {
                        foreach (personeel a in ProgData.AlleMensen.LijstPersoonKleur)
                        {
                            if (!Namen.Contains(a._persnummer))
                                Namen.Add(a._persnummer);
                        }
                    }
                }
            }

            try
            {
                //Start Excel and get Application object.
                xlApp = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                string file = Path.GetFullPath("BezData\\OverzichtPloeg.xls");

                xlWorkBook = xlApp.Workbooks.Open(file, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, true, 0, true, 1, 0);

                Microsoft.Office.Interop.Excel.Worksheet excelSheet = xlWorkBook.ActiveSheet;

                // schoonvegen

                // eerste getal is regel
                // tweede kolom

                oRng = excelSheet.Range[excelSheet.Cells[1, 2], excelSheet.Cells[3, 2]]; // B1 B3
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[4, 1], excelSheet.Cells[50, 1]]; // A4 A50
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[4, 4], excelSheet.Cells[50, 5]]; // D4 E50
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[4, 8], excelSheet.Cells[50, 9]]; // H4 I50
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[4, 12], excelSheet.Cells[50, 13]]; // L4 M50
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[4, 16], excelSheet.Cells[50, 17]]; // P4 Q50
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[4, 20], excelSheet.Cells[50, 21]]; // T4 U50
                oRng.ClearContents();
                oRng.Value = null;
                oRng = excelSheet.Range[excelSheet.Cells[4, 24], excelSheet.Cells[50, 25]]; // X4 Y50
                oRng.ClearContents();
                oRng.Value = null;


                int row = 4;
                foreach (int naam in Namen)
                {
                    ListClassTelAfwijkingen.Clear();

                    for (int i = 1; i < 13; i++)
                    {
                        ProgData.igekozenmaand = i;
                        GetAfwijkingenPersoonInEenMaand(naam.ToString(CultureInfo.CurrentCulture), ProgData.igekozenjaar, i);
                        // data nu in ListClassTelAfwijkingen
                    }

                    if (ListClassTelAfwijkingen.Count > 0)
                    {
                        int[] tel_op = new int[12];
                        foreach (ClassTelAfwijkingen afw in ListClassTelAfwijkingen)
                        {
                            switch (afw._Afwijking)
                            {
                                case "VK":
                                case "A":
                                case "8OI":
                                case "VRIJ":
                                    if (afw._Toekomst)
                                    {
                                        tel_op[1] = tel_op[1] + afw._Aantal;
                                    }
                                    else
                                    {
                                        tel_op[0] = tel_op[0] + afw._Aantal;
                                    }
                                    break;
                                case "OPL":
                                case "OPLO":
                                    if (afw._Toekomst)
                                    {
                                        tel_op[3] = tel_op[3] + afw._Aantal;
                                    }
                                    else
                                    {
                                        tel_op[2] = tel_op[2] + afw._Aantal;
                                    }
                                    break;
                                case "Z":
                                    if (afw._Toekomst)
                                    {
                                        tel_op[5] = tel_op[5] + afw._Aantal;
                                    }
                                    else
                                    {
                                        tel_op[4] = tel_op[4] + afw._Aantal;
                                    }
                                    break;
                                case "ED":
                                case "RD":
                                case "VD":
                                case "K":
                                    if (afw._Toekomst)
                                    {
                                        tel_op[7] = tel_op[7] + afw._Aantal;
                                    }
                                    else
                                    {
                                        tel_op[6] = tel_op[6] + afw._Aantal;
                                    }
                                    break;
                                case "GP":
                                    if (afw._Toekomst)
                                    {
                                        tel_op[9] = tel_op[9] + afw._Aantal;
                                    }
                                    else
                                    {
                                        tel_op[8] = tel_op[8] + afw._Aantal;
                                    }
                                    break;
                                case "BV":
                                case "VF":
                                    if (afw._Toekomst)
                                    {
                                        tel_op[11] = tel_op[11] + afw._Aantal;
                                    }
                                    else
                                    {
                                        tel_op[10] = tel_op[10] + afw._Aantal;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        excelSheet.Cells[row, 4] = tel_op[0];
                        excelSheet.Cells[row, 5] = tel_op[1];
                        excelSheet.Cells[row, 8] = tel_op[2];
                        excelSheet.Cells[row, 9] = tel_op[3];
                        excelSheet.Cells[row, 12] = tel_op[4];
                        excelSheet.Cells[row, 13] = tel_op[5];
                        excelSheet.Cells[row, 16] = tel_op[6];
                        excelSheet.Cells[row, 17] = tel_op[7];
                        excelSheet.Cells[row, 20] = tel_op[8];
                        excelSheet.Cells[row, 21] = tel_op[9];
                        excelSheet.Cells[row, 24] = tel_op[10];
                        excelSheet.Cells[row, 25] = tel_op[11];


                        excelSheet.Cells[row, 1] = ProgData.Get_Gebruiker_Naam(naam.ToString(CultureInfo.CurrentCulture));

                    }
                    row++;

                }
                excelSheet.Cells[1, 2] = ProgData.Sgekozenjaar();
                excelSheet.Cells[2, 2] = ProgData.GekozenKleur;
                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
            ProgData.igekozenmaand = bewaar_maand;
        }

        private void jaarOverzichtNaarExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //naar maand 1
            ProgData.SaveDatum();
            ProgData.igekozenmaand = 1;
            VulViewScherm();

            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                int aantal_row = 20;    // nog bedenken
                int aantal_col = 35;    // maakt niet uit aantal dagen + wat extra

                for (int row = 0; row < aantal_row; row++)
                {
                    for (int col = 0; col < aantal_col; col++)
                    {
                        try
                        {
                            oSheet.Cells[row + 1, col + 1] = View.Items[row].SubItems[col].Text;

                            if (View.Items[row].SubItems[col].BackColor == Werkplek_)
                            {
                                var columnHeadingsRange = oSheet.Range[
                                                          oSheet.Cells[row + 1, col + 1],
                                                          oSheet.Cells[row + 1, col + 1]];

                                columnHeadingsRange.Interior.Color = Werkplek_;
                            }
                            if (View.Items[row].SubItems[col].BackColor == Weekend_)
                            {
                                var columnHeadingsRange = oSheet.Range[
                                                          oSheet.Cells[row + 1, col + 1],
                                                          oSheet.Cells[row + 1, col + 1]];

                                columnHeadingsRange.Interior.Color = Weekend_;
                            }

                        }
                        catch { }
                    }
                }

                oRng = oSheet.get_Range("A1", "A100");
                oRng.EntireColumn.ColumnWidth = 20;
                oRng.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                oRng = oSheet.get_Range("B1", "AZ100");
                oRng.EntireColumn.ColumnWidth = 6;
                oRng.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                oSheet.Name = View.Items[2].SubItems[0].Text;

                for (int i = 0; i < 11; i++)
                {
                    // volgende maanden
                    oWB.Sheets.Add(After: oWB.Sheets[oWB.Sheets.Count]);
                    oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oXL.Sheets[i + 2];
                    VolgendeMaandToolStripMenuItem_Click(this, null);
                    VulViewScherm();
                    View.Refresh();

                    for (int row = 0; row < aantal_row; row++)
                    {
                        for (int col = 0; col < aantal_col; col++)
                        {
                            try
                            {
                                oSheet.Cells[row + 1, col + 1] = View.Items[row].SubItems[col].Text;

                                if (View.Items[row].SubItems[col].BackColor == Werkplek_)
                                {
                                    var columnHeadingsRange = oSheet.Range[
                                                              oSheet.Cells[row + 1, col + 1],
                                                              oSheet.Cells[row + 1, col + 1]];

                                    columnHeadingsRange.Interior.Color = Werkplek_;
                                }
                                if (View.Items[row].SubItems[col].BackColor == Weekend_)
                                {
                                    var columnHeadingsRange = oSheet.Range[
                                                              oSheet.Cells[row + 1, col + 1],
                                                              oSheet.Cells[row + 1, col + 1]];

                                    columnHeadingsRange.Interior.Color = Weekend_;
                                }

                            }
                            catch { }
                        }
                    }

                    oRng = oSheet.get_Range("A1", "A100");
                    oRng.EntireColumn.ColumnWidth = 20;
                    oRng.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                    oRng = oSheet.get_Range("B1", "AZ100");
                    oRng.EntireColumn.ColumnWidth = 6;
                    oRng.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    oSheet.Name = View.Items[2].SubItems[0].Text;
                }
                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
            ProgData.ReturnDatum();
        }

        private void MaandenOverzichtNaarExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;

            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                int aantal_row = 20;    // nog bedenken
                int aantal_col = 35;    // maakt niet uit aantal dagen + wat extra

                for (int row = 0; row < aantal_row; row++)
                {
                    for (int col = 0; col < aantal_col; col++)
                    {
                        try
                        {
                            oSheet.Cells[row + 1, col + 1] = View.Items[row].SubItems[col].Text;

                            if (View.Items[row].SubItems[col].BackColor == Werkplek_)
                            {
                                var columnHeadingsRange = oSheet.Range[
                                                          oSheet.Cells[row + 1, col + 1],
                                                          oSheet.Cells[row + 1, col + 1]];

                                columnHeadingsRange.Interior.Color = Werkplek_;
                            }
                            if (View.Items[row].SubItems[col].BackColor == Weekend_)
                            {
                                var columnHeadingsRange = oSheet.Range[
                                                          oSheet.Cells[row + 1, col + 1],
                                                          oSheet.Cells[row + 1, col + 1]];

                                columnHeadingsRange.Interior.Color = Weekend_;
                            }

                        }
                        catch { }
                    }
                }

                oRng = oSheet.get_Range("A1", "A100");
                oRng.EntireColumn.ColumnWidth = 20;
                oRng.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                oRng = oSheet.get_Range("B1", "AZ100");
                oRng.EntireColumn.ColumnWidth = 6;
                oRng.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                oSheet.Name = View.Items[2].SubItems[0].Text;

                // volgende maand
                oWB.Sheets.Add(After: oWB.Sheets[oWB.Sheets.Count]);
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oXL.Sheets["Blad2"];
                VolgendeMaandToolStripMenuItem_Click(this, null);
                View.Refresh();

                for (int row = 0; row < aantal_row; row++)
                {
                    for (int col = 0; col < aantal_col; col++)
                    {
                        try
                        {
                            oSheet.Cells[row + 1, col + 1] = View.Items[row].SubItems[col].Text;

                            if (View.Items[row].SubItems[col].BackColor == Werkplek_)
                            {
                                var columnHeadingsRange = oSheet.Range[
                                                          oSheet.Cells[row + 1, col + 1],
                                                          oSheet.Cells[row + 1, col + 1]];

                                columnHeadingsRange.Interior.Color = Werkplek_;
                            }
                            if (View.Items[row].SubItems[col].BackColor == Weekend_)
                            {
                                var columnHeadingsRange = oSheet.Range[
                                                          oSheet.Cells[row + 1, col + 1],
                                                          oSheet.Cells[row + 1, col + 1]];

                                columnHeadingsRange.Interior.Color = Weekend_;
                            }

                        }
                        catch { }
                    }
                }

                oRng = oSheet.get_Range("A1", "A100");
                oRng.EntireColumn.ColumnWidth = 20;
                oRng.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                oRng = oSheet.get_Range("B1", "AZ100");
                oRng.EntireColumn.ColumnWidth = 6;
                oRng.EntireColumn.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                oSheet.Name = View.Items[2].SubItems[0].Text;

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
        }

        public void PloegTotalenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<int> Namen = new List<int>();
            List<string> Afwijking = new List<string>();

            ProgData.SaveDatum();

            // haal eerst de namen die dit jaar op kleur x gelopen hebben
            // dus lijst met namen
            DebugPanelShow("Verzamel namen deze kleur");
            for (int i = 1; i < 13; i++)
            {
                ProgData.igekozenmaand = i;
                if (File.Exists(ProgData.Ploeg_Namen_Locatie(ProgData.GekozenKleur)))
                {
                    ProgData.AlleMensen.HaalPloegNamenOpKleur(ProgData.GekozenKleur);
                    if (ProgData.AlleMensen.LijstPersoonKleur.Count > 0)
                    {
                        foreach (personeel a in ProgData.AlleMensen.LijstPersoonKleur)
                        {
                            if (!Namen.Contains(a._persnummer))
                                Namen.Add(a._persnummer);
                        }
                    }
                }
            }

            LijstclassTelAfwijkingenVanPersoons.Clear();
            // nu de afwijkingen van die personen.
            DebugWrite("Verzamel Afwijkingen van deze personen dit jaar");
            for (int i = 1; i < 13; i++)
            {
                ProgData.igekozenmaand = i;
                int aantal_dagen = DateTime.DaysInMonth(ProgData.igekozenjaar, i);
                foreach (int a in Namen)
                {
                    string naam = ProgData.Get_Gebruiker_Naam(a.ToString());
                    DebugWrite($"Verzamel Afwijkingen van persoon dit jaar {naam}");
                    for (int q = 1; q < aantal_dagen+1; q++)
                    {
                        DateTime dat = new DateTime(ProgData.igekozenjaar, i, q);
                        string afwijking = ProgData.GetLaatsteAfwijkingPersoon(ProgData.GekozenKleur, a.ToString(), dat);
                        if (!Afwijking.Contains(afwijking) && afwijking != "")
                        {
                            Afwijking.Add(afwijking);
                        }
                        if (afwijking != "")
                        {
                            DebugWrite($"Afwijkingen van persoon dit jaar, moment..... {naam}  {afwijking}");
                            ClassTelAfwijkingenVanPersoon.voeg_toe(a, afwijking);
                        }
                    } //dagen
                } // namen
            } // maanden

            DebugPanelEnd();

            try
            {

                Microsoft.Office.Interop.Excel.Application oXL;
                Microsoft.Office.Interop.Excel._Workbook oWB;
                Microsoft.Office.Interop.Excel._Worksheet oSheet;
                Microsoft.Office.Interop.Excel.Range oRng;

                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application
                {
                    Visible = true
                };

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                oSheet.Cells[1, 1] = ProgData.Sgekozenjaar();
                oSheet.Cells[2, 1] = ProgData.GekozenKleur;


                for (int i = 0; i < Afwijking.Count; i++)
                {
                    oSheet.Cells[3, i + 2] = Afwijking[i];
                }

                oSheet.get_Range("A1", "Z2").Font.Bold = true;

                for (int i = 0; i < Namen.Count; i++)
                {
                    oSheet.Cells[i + 4, 1] = ProgData.Get_Gebruiker_Naam(Namen[i].ToString());
                }

                foreach (ClassTelAfwijkingenVanPersoon a in LijstclassTelAfwijkingenVanPersoons)
                {
                    var test = a.Afwijking_;
                    var test2 = Afwijking.IndexOf(test);      // y coord

                    var test4 = a.Persnr_;
                    var test5 = Namen.IndexOf(test4);      // x coord    als naam niet gevonden, dan antwoord -1 bv een extra dienst
                    var test3 = a.Aantal_.ToString();      // inhoud

                    if (test5 > -1)
                    {
                        var row = test5 + 4;
                        var col = test2 + 2;
                        oSheet.Cells[row, col] = test3;
                    }
                }

                //AutoFit columns A:D.
                oRng = oSheet.get_Range("A1", "Z3");
                oRng.EntireColumn.AutoFit();


                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (Exception theException)
            {
                String errorMessage;
                errorMessage = "Error: ";
                errorMessage = String.Concat(errorMessage, theException.Message);
                errorMessage = String.Concat(errorMessage, " Line: ");
                errorMessage = String.Concat(errorMessage, theException.Source);

                MessageBox.Show(errorMessage, "Error");
            }
            ProgData.ReturnDatum();

        }
    }
}
