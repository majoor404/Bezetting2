using Bezetting2.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class MainFormBezetting2 : Form
    {
        private void afwijkingenTovRoosterIngelogdPersoonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int bewaar_maand = ProgData.igekozenmaand;
            int bewaar_jaar = ProgData.igekozenjaar;

            ListClassTelAfwijkingen.Clear();

            string locatie = @"telnietmee.ini";
            ListTelNietMeeNamen = File.ReadAllLines(locatie).ToList();

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
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.Visible = true;

                //Get a new workbook.
                string file = Path.GetFullPath("OverzichtPersoon.xls");

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
                oRng = excelSheet.Range[excelSheet.Cells[20, 4], excelSheet.Cells[34, 4]];
                oRng.ClearContents();
                oRng.Value = null;

                excelSheet.Cells[1, 2] = ProgData.sgekozenjaar();
                excelSheet.Cells[2, 2] = ProgData.Get_Gebruiker_Kleur(ProgData.Huidige_Gebruiker_Personeel_nummer);
                excelSheet.Cells[3, 2] = ProgData.Get_Gebruiker_Naam(ProgData.Huidige_Gebruiker_Personeel_nummer);


                int row = 20;
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
                        case "A":
                            vaste_regel = 5;
                            break;
                        case "Z":
                            vaste_regel = 6;
                            break;
                        case "VF":
                            vaste_regel = 7;
                            break;
                        case "GP":
                            vaste_regel = 8;
                            break;
                        case "OPLO":
                            vaste_regel = 9;
                            break;
                        case "OPL":
                            vaste_regel = 10;
                            break;
                        case "K":
                            vaste_regel = 11;
                            break;
                        case "ADV":
                            vaste_regel = 12;
                            break;
                        case "BV":
                            vaste_regel = 13;
                            break;
                        case "V":
                            vaste_regel = 14;
                            break;
                        case "W":
                            vaste_regel = 15;
                            break;
                        case "1/2 VK":
                            vaste_regel = 16;
                            break;
                        case "*":
                            vaste_regel = 17;
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
            string kleur = ProgData.Get_Gebruiker_Kleur(ProgData.Huidige_Gebruiker_Personeel_nummer);
            string naam = ProgData.Get_Gebruiker_Naam(ProgData.Huidige_Gebruiker_Personeel_nummer);

            //eerst vanuit ploegbezetting een string list maken met afwijkingen en normaal schema van persoon
            if (File.Exists(ProgData.Ploeg_Bezetting_Locatie(kleur)))
            {
                ProgData.LoadPloegBezetting(kleur);
                foreach (werkdag dag in ProgData.ListWerkdagPloeg)
                {
                    if (dag._naam == naam)
                    {
                        dag_gekozen = new DateTime(ProgData.ihuidigjaar, ProgData.igekozenmaand, dag._dagnummer);

                        // get en zet eerst orginele dienst
                        string wacht = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), dag_gekozen, kleur);
                        if (wacht != "")
                        {
                            deze_maand_overzicht_persoon[dag._dagnummer] = "W"; // Werkdag
                        }
                        else
                        {
                            deze_maand_overzicht_persoon[dag._dagnummer] = "V"; // Rooster vrij
                        }
                        // daarna overschrijven als die afwijkt van ""
                        if (dag._afwijkingdienst != "")
                        {
                            deze_maand_overzicht_persoon[dag._dagnummer] = dag._afwijkingdienst;
                        }

                        if (ListTelNietMeeNamen.Contains(deze_maand_overzicht_persoon[dag._dagnummer]))
                        {
                            deze_maand_overzicht_persoon[dag._dagnummer] = "W"; // Werkdag
                        }
                    }
                }
            }

            // lijst strings nu klaar, nu tellen.
            int dagen = DateTime.DaysInMonth(jaar, maand);
            for (int q = 1; q < dagen + 1; q++)
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
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlApp.Visible = true;

                //Get a new workbook.
                string file = Path.GetFullPath("vuilwerk.xls");

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

                excelSheet.Cells[1, 2] = ProgData.sgekozenmaand();
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
                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = true;

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                // Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Waar gewerkt in Jaar : ";
                oSheet.Cells[1, 2] = ProgData.sgekozenjaar();
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
                    //var test = Tel[i]._PlekTelPlek;
                    //var test2 = TelWerkPlek.IndexOf(test);      // y coord
                    //var test4 = Tel[i]._NaamTelPlek;
                    //var test5 = TelNamen.IndexOf(test4);      // x coord
                    //var test3 = Tel[i]._AantalTelPlek;      // inhoud
                    oSheet.Cells[ListTelNamen.IndexOf(ListClassTelPlekGewerkt[i]._NaamTelPlek) + 4,
                        ListTelWerkPlek.IndexOf(ListClassTelPlekGewerkt[i]._PlekTelPlek) + 2] = ListClassTelPlekGewerkt[i]._AantalTelPlek;
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
    }
}
