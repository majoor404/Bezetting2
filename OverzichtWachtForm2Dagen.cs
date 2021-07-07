// 19-6-21

using Bezetting2.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static Bezetting2.DatumVijfPloegUtils;

namespace Bezetting2
{
    public partial class OverzichtWachtForm2Dagen : Form
    {
        private ListBox sourse;
        private ListBox broer;
        private int sourse_index;
        public List<Invoerveld> opbouw = new List<Invoerveld>();
        private WerkPlek WP = new WerkPlek();
        private DateTime dat;
        private DateTime dat2;
        //private DateTime huidig;

        private string gekozen_naam1, gekozen_naam2;
        private ListBox gekozen_listbox1, gekozen_listbox2;

        public class Invoerveld
        {
            //  public invoerveld() { }
            public Invoerveld(Label lb, ListBox ln, ListBox la, int dag)
            {
                _Label = lb;
                _ListNaam = ln;
                _ListAfw = la;
                _Dag = dag;
            }
            public Label _Label { get; set; }
            public ListBox _ListNaam { get; set; }
            public ListBox _ListAfw { get; set; }
            public int _Dag { get; set; }   // eerste of 2de dag op formulier
        } // hulp class om snel broer listbox te vinden van afwijking van naam

        public OverzichtWachtForm2Dagen()
        {
            InitializeComponent();
        }
        private void OverzichtWachtForm2_Shown(object sender, EventArgs e)
        {
            buttonOpmerking.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
            buttonOpmerking2.Enabled = ProgData.RechtenHuidigeGebruiker > 24;

            // als ToegangNivo hoog genoeg, vrijgave edit
            if (ProgData.RechtenHuidigeGebruiker > 100)
            {
                foreach (CheckBox check in this.Controls.OfType<CheckBox>())
                {
                    check.Visible = true;
                }

                MessageBox.Show("Gebruikers rechten > 100, ADMIN!");
            }

            opbouw.Clear();

            opbouw.Add(new Invoerveld(label1, listBox1, listBoxAfw, 1));
            opbouw.Add(new Invoerveld(label2, listBox2, listBox22, 1));
            opbouw.Add(new Invoerveld(label3, listBox3, listBox23, 1));
            opbouw.Add(new Invoerveld(label4, listBox4, listBox24, 1));
            opbouw.Add(new Invoerveld(label5, listBox5, listBox25, 1));
            opbouw.Add(new Invoerveld(label6, listBox6, listBox26, 1));
            opbouw.Add(new Invoerveld(label7, listBox7, listBox27, 1));
            opbouw.Add(new Invoerveld(label8, listBox8, listBox28, 1));
            opbouw.Add(new Invoerveld(label9, listBox9, listBox29, 1));
            opbouw.Add(new Invoerveld(label10, listBox10, listBox30, 1));
            opbouw.Add(new Invoerveld(label11, listBox11, listBox31, 1));
            opbouw.Add(new Invoerveld(label12, listBox12, listBox32, 1));
            opbouw.Add(new Invoerveld(label13, listBox13, listBox33, 1));
            opbouw.Add(new Invoerveld(label14, listBox14, listBox34, 1));
            opbouw.Add(new Invoerveld(label15, listBox15, listBox35, 1));
            opbouw.Add(new Invoerveld(label16, listBox16, listBox36, 1));
            opbouw.Add(new Invoerveld(label17, listBox17, listBox37, 1));
            opbouw.Add(new Invoerveld(label18, listBox18, listBox38, 1));
            opbouw.Add(new Invoerveld(label19, listBox19, listBox39, 1));
            opbouw.Add(new Invoerveld(label20, listBox20, listBox40, 1));
            opbouw.Add(new Invoerveld(label21, listBox21, listBox41, 1));

            opbouw.Add(new Invoerveld(label42, listBox83, listBoxAfw2, 2));

            opbouw.Add(new Invoerveld(label41, listBox81, listBox77, 2));
            opbouw.Add(new Invoerveld(label40, listBox80, listBox76, 2));
            opbouw.Add(new Invoerveld(label39, listBox79, listBox75, 2));
            opbouw.Add(new Invoerveld(label38, listBox78, listBox74, 2));

            opbouw.Add(new Invoerveld(label37, listBox73, listBox69, 2));
            opbouw.Add(new Invoerveld(label36, listBox72, listBox68, 2));
            opbouw.Add(new Invoerveld(label35, listBox71, listBox67, 2));
            opbouw.Add(new Invoerveld(label34, listBox70, listBox66, 2));

            opbouw.Add(new Invoerveld(label33, listBox65, listBox57, 2));
            opbouw.Add(new Invoerveld(label32, listBox64, listBox56, 2));
            opbouw.Add(new Invoerveld(label31, listBox63, listBox55, 2));
            opbouw.Add(new Invoerveld(label30, listBox62, listBox54, 2));

            opbouw.Add(new Invoerveld(label29, listBox61, listBox53, 2));
            opbouw.Add(new Invoerveld(label28, listBox60, listBox52, 2));
            opbouw.Add(new Invoerveld(label27, listBox59, listBox51, 2));
            opbouw.Add(new Invoerveld(label26, listBox58, listBox50, 2));

            opbouw.Add(new Invoerveld(label22, listBox46, listBox45, 2));
            opbouw.Add(new Invoerveld(label23, listBox47, listBox44, 2));
            opbouw.Add(new Invoerveld(label24, listBox48, listBox43, 2));
            opbouw.Add(new Invoerveld(label25, listBox49, listBox42, 2));


            LaadDataFormulier();

            // zet datum en kleur in beeld

            dat = DateTime.Now;

            //ProgData.GekozenKleur = ProgData.Main.comboBoxKleurKeuze.Text;

            ViewUpdate();
        }
        private void ListBox29_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                ListBox Sender = (ListBox)sender;
                string afw = Sender.Items[e.Index].ToString().ToUpper();
                if (afw.Length > 3) afw = afw.Substring(0, 3);
                if (afw.Length > 2 && afw.Substring(0, 2) == "EV")
                    afw = "EV";
                if (afw.Length > 2 && afw.Substring(0, 2) == "VK") // bv VK1 VK2 enz
                    afw = "VK";

                switch (afw)
                {
                    case "Z":
                    case "VK":
                    case "8OI":
                    case "VRI":
                    case "VAK":
                    case "VF":
                    case "OPL":
                    case "OI8":
                    case "EV":
                    case "GP":
                    case "X":
                        e.Graphics.FillRectangle(Brushes.Lavender, e.Bounds);
                        break;
                    case "ED-":
                    case "VD-":
                    case "RD-":
                        e.Graphics.FillRectangle(Brushes.LightSalmon, e.Bounds);
                        break;
                    default:
                        e.DrawBackground();
                        break;
                }

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(Sender.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
                }
            }
        }
        private void ListBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (CheckRechten())
            {
                if (e.Data.GetDataPresent(DataFormats.Text))
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        private void ListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (CheckRechten())
            {
                // voor dubbel, check rechter muis
                if (e.Button == MouseButtons.Right)
                {
                    if (sender.GetType() == typeof(ListBox))
                    {
                        sourse = (ListBox)sender;
                        int index = sourse.IndexFromPoint(e.X, e.Y);
                        sourse_index = index;

                        Point start = new Point(sourse.Location.X, sourse.Location.Y);
                        start.X = start.X + e.Location.X;
                        start.Y = start.Y + e.Location.Y;
                        buttonSplitDag1.Location = start;

                        // bewaar data voor als je op 2 maal invullen knop drukt
                        if (index > -1)
                        {
                            buttonSplitDag1.Visible = true;
                            gekozen_naam1 = sourse.Items[index].ToString();
                            gekozen_listbox1 = sourse;

                            // delete +naam
                            //var first = string.IsNullOrEmpty(gekozen_naam1) ? (char?)null : gekozen_naam1[0];
                            //if (first == '+')
                            //{
                            //    buttonSplitDag1.Visible = false;
                            //    MessageBox.Show($"Verwijder {gekozen_naam1}");
                            //    WerkPlek.SetWerkPlek(gekozen_naam1, dat.Day, "!@#$%$");
                            //    WerkPlek.SafeWerkPlek(ProgData.GekozenKleur, dat.Month, dat.Year);
                            //    gekozen_listbox1.Items.RemoveAt(sourse_index);
                            //    UpdateAfwijkingListBox(gekozen_listbox1, dat.Day);
                            //}
                            /////////////////////
                        }
                        else
                        {
                            buttonSplitDag1.Visible = false;
                        }
                    }
                }
                else
                {
                    try
                    {
                        sourse = (ListBox)sender;
                        int index = sourse.IndexFromPoint(e.X, e.Y);
                        sourse_index = index;
                        if (index > -1)
                        {
                            string s = sourse.Items[index].ToString();
                            //DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.Move);
                            _ = DoDragDrop(s, DragDropEffects.Move);
                        }
                    }
                    catch { }
                }
            }
        }
        private void listBox81_MouseDown(object sender, MouseEventArgs e)
        {
            if (CheckRechten())
            {
                // voor dubbel, check rechter muis
                if (e.Button == MouseButtons.Right)
                {
                    if (sender.GetType() == typeof(ListBox))
                    {
                        sourse = (ListBox)sender;
                        int index = sourse.IndexFromPoint(e.X, e.Y);
                        sourse_index = index;

                        Point start = new Point(sourse.Location.X, sourse.Location.Y);
                        start.X = start.X + e.Location.X;
                        start.Y = start.Y + e.Location.Y;
                        buttonSplitDag2.Location = start;
                        // bewaar data voor als je op 2 maal invullen knop drukt
                        if (index > -1)
                        {
                            buttonSplitDag2.Visible = true;
                            gekozen_naam2 = sourse.Items[index].ToString();
                            gekozen_listbox2 = sourse;
                        }
                        else
                        {
                            buttonSplitDag2.Visible = false;
                            gekozen_naam2 = "";
                        }
                    }
                }
                else
                {
                    try
                    {
                        sourse = (ListBox)sender;
                        int index = sourse.IndexFromPoint(e.X, e.Y);
                        sourse_index = index;
                        if (index > -1)
                        {
                            string s = sourse.Items[index].ToString();
                            //DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.Move);
                            _ = DoDragDrop(s, DragDropEffects.Move);
                        }
                    }
                    catch { }
                }
            }
        }
        private void ListBox2_DragDrop(object sender, DragEventArgs e)
        {
            if (CheckRechten())
            {
                ListBox tb = (ListBox)sender;

                // alleen verpaalsen als zelfde panel (dag)
                //var panel1 = tb.Parent;
                //var panel2 = sourse.Parent;
                if (tb.Parent == sourse.Parent)
                {
                    if (tb != sourse)
                    {
                        tb.Items.Add(e.Data.GetData(DataFormats.Text).ToString());

                        // remove van oude locatie
                        if (sourse_index > -1)
                            sourse.Items.RemoveAt(sourse_index);

                        if (tb.Parent == PanelDag1)
                        {
                            UpdateAfwijkingListBox(tb, dat.Day);
                            UpdateAfwijkingListBox(sourse, dat.Day);
                        }
                        else
                        {
                            UpdateAfwijkingListBox(tb, dat2.Day);
                            UpdateAfwijkingListBox(sourse, dat2.Day);
                        }

                    }
                }
                SaveData();
            }
        }
        private static bool CheckRechten()
        {
            bool ret = false;
            if (ProgData.RechtenHuidigeGebruiker > 25) ret = true;

            if (ProgData.RechtenHuidigeGebruiker == 25 && ProgData.Huidige_Gebruiker_Werkt_Op_Kleur() == ProgData.GekozenKleur) ret = true;

            return ret;
        }
        private void UpdateAfwijkingListBox(ListBox box, int dag_)
        {
            Invoerveld veld = opbouw.First(a => (a._ListNaam == box));
            broer = veld._ListAfw;
            broer.Items.Clear();

            bool afwijking = false;
            for (int i = 0; i < box.Items.Count; i++)
            {
                // voor elke persoon in lijst
                string naam = box.Items[i].ToString();

                // als dag == day.Day dan eerste datum

                if (dag_ == dat.Day)
                {
                    var first = string.IsNullOrEmpty(naam) ? (char?)null : naam[0];
                    if (first == '+')
                        naam = naam.Substring(1, naam.Length - 1);

                    var nummer = ProgData.Get_Gebruiker_Nummer(naam);
                    DateTime datum = new DateTime(dat.Year, dat.Month, dat.Day);
                    string afwijking_ = ProgData.GetLaatsteAfwijkingPersoon(ProgData.Get_Gebruiker_Kleur(nummer, datum),
                        nummer, datum);
                    broer.Items.Add(afwijking_);
                    if (!string.IsNullOrEmpty(afwijking_))
                        afwijking = true;
                }
                else
                {
                    var first = string.IsNullOrEmpty(naam) ? (char?)null : naam[0];
                    if (first == '+')
                        naam = naam.Substring(1, naam.Length - 1);

                    var nummer = ProgData.Get_Gebruiker_Nummer(naam);
                    DateTime datum = new DateTime(dat2.Year, dat2.Month, dat2.Day);
                    string afwijking_ = ProgData.GetLaatsteAfwijkingPersoon(ProgData.Get_Gebruiker_Kleur(nummer, datum),
                        nummer, datum);
                    broer.Items.Add(afwijking_);
                    if (!string.IsNullOrEmpty(afwijking_))
                        afwijking = true;
                }
            }
            broer.Visible = afwijking;
        }
        private void LaadDataFormulier()
        {
            // zet juiste vakjes aan
            try
            {
                label1.Text = label42.Text = InstellingenProg.ProgrammaData[00];
                label2.Text = label41.Text = InstellingenProg.ProgrammaData[01];
                label3.Text = label40.Text = InstellingenProg.ProgrammaData[02];
                label4.Text = label39.Text = InstellingenProg.ProgrammaData[03];
                label5.Text = label38.Text = InstellingenProg.ProgrammaData[04];
                label6.Text = label37.Text = InstellingenProg.ProgrammaData[05];
                label7.Text = label36.Text = InstellingenProg.ProgrammaData[06];
                label8.Text = label35.Text = InstellingenProg.ProgrammaData[07];
                label9.Text = label34.Text = InstellingenProg.ProgrammaData[08];
                label10.Text = label33.Text = InstellingenProg.ProgrammaData[09];
                label11.Text = label32.Text = InstellingenProg.ProgrammaData[10];
                label12.Text = label31.Text = InstellingenProg.ProgrammaData[11];
                label13.Text = label30.Text = InstellingenProg.ProgrammaData[12];
                label14.Text = label29.Text = InstellingenProg.ProgrammaData[13];
                label15.Text = label28.Text = InstellingenProg.ProgrammaData[14];
                label16.Text = label27.Text = InstellingenProg.ProgrammaData[15];
                label17.Text = label26.Text = InstellingenProg.ProgrammaData[16];
                label18.Text = label22.Text = InstellingenProg.ProgrammaData[17];
                label19.Text = label23.Text = InstellingenProg.ProgrammaData[18];
                label20.Text = label24.Text = InstellingenProg.ProgrammaData[19];
                label21.Text = label25.Text = InstellingenProg.ProgrammaData[20];

                listBox2.Visible = listBox22.Visible = listBox81.Visible = listBox77.Visible = bool.Parse(InstellingenProg.ProgrammaData[21]);
                listBox3.Visible = listBox23.Visible = listBox80.Visible = listBox76.Visible = bool.Parse(InstellingenProg.ProgrammaData[22]);
                listBox4.Visible = listBox24.Visible = listBox79.Visible = listBox75.Visible = bool.Parse(InstellingenProg.ProgrammaData[23]);
                listBox5.Visible = listBox25.Visible = listBox78.Visible = listBox74.Visible = bool.Parse(InstellingenProg.ProgrammaData[24]);

                listBox6.Visible = listBox26.Visible = listBox73.Visible = listBox69.Visible = bool.Parse(InstellingenProg.ProgrammaData[25]);
                listBox7.Visible = listBox27.Visible = listBox72.Visible = listBox68.Visible = bool.Parse(InstellingenProg.ProgrammaData[26]);
                listBox8.Visible = listBox28.Visible = listBox71.Visible = listBox67.Visible = bool.Parse(InstellingenProg.ProgrammaData[27]);
                listBox9.Visible = listBox29.Visible = listBox70.Visible = listBox66.Visible = bool.Parse(InstellingenProg.ProgrammaData[28]);

                listBox10.Visible = listBox30.Visible = listBox65.Visible = listBox57.Visible = bool.Parse(InstellingenProg.ProgrammaData[29]);
                listBox11.Visible = listBox31.Visible = listBox64.Visible = listBox56.Visible = bool.Parse(InstellingenProg.ProgrammaData[30]);
                listBox12.Visible = listBox32.Visible = listBox63.Visible = listBox55.Visible = bool.Parse(InstellingenProg.ProgrammaData[31]);
                listBox13.Visible = listBox33.Visible = listBox62.Visible = listBox54.Visible = bool.Parse(InstellingenProg.ProgrammaData[32]);

                listBox14.Visible = listBox34.Visible = listBox61.Visible = listBox53.Visible = bool.Parse(InstellingenProg.ProgrammaData[33]);
                listBox15.Visible = listBox35.Visible = listBox60.Visible = listBox52.Visible = bool.Parse(InstellingenProg.ProgrammaData[34]);
                listBox16.Visible = listBox36.Visible = listBox59.Visible = listBox51.Visible = bool.Parse(InstellingenProg.ProgrammaData[35]);
                listBox17.Visible = listBox37.Visible = listBox58.Visible = listBox50.Visible = bool.Parse(InstellingenProg.ProgrammaData[36]);

                listBox18.Visible = listBox38.Visible = listBox46.Visible = listBox45.Visible = bool.Parse(InstellingenProg.ProgrammaData[37]);
                listBox19.Visible = listBox39.Visible = listBox47.Visible = listBox44.Visible = bool.Parse(InstellingenProg.ProgrammaData[38]);
                listBox20.Visible = listBox40.Visible = listBox48.Visible = listBox43.Visible = bool.Parse(InstellingenProg.ProgrammaData[39]);
                listBox21.Visible = listBox41.Visible = listBox49.Visible = listBox42.Visible = bool.Parse(InstellingenProg.ProgrammaData[40]);

                label2.Visible = label41.Visible = listBox2.Visible;
                label3.Visible = label40.Visible = listBox3.Visible;
                label4.Visible = label39.Visible = listBox4.Visible;
                label5.Visible = label38.Visible = listBox5.Visible;

                label6.Visible = label37.Visible = listBox6.Visible;
                label7.Visible = label36.Visible = listBox7.Visible;
                label8.Visible = label35.Visible = listBox8.Visible;
                label9.Visible = label34.Visible = listBox9.Visible;

                label10.Visible = label33.Visible = listBox10.Visible;
                label11.Visible = label32.Visible = listBox11.Visible;
                label12.Visible = label31.Visible = listBox12.Visible;
                label13.Visible = label30.Visible = listBox13.Visible;

                label14.Visible = label29.Visible = listBox14.Visible;
                label15.Visible = label28.Visible = listBox15.Visible;
                label16.Visible = label27.Visible = listBox16.Visible;
                label17.Visible = label26.Visible = listBox17.Visible;

                label18.Visible = label22.Visible = listBox18.Visible;
                label19.Visible = label23.Visible = listBox19.Visible;
                label20.Visible = label24.Visible = listBox20.Visible;
                label21.Visible = label25.Visible = listBox21.Visible;
            }
            catch { }
        }
        private void ViewUpdate()
        {
            dateTimePicker1.Visible = false;
            GaNaarDat.Visible = false;
            labelKleur.Text = ProgData.GekozenKleur;

            switch (ProgData.GekozenKleur)
            {
                case "Blauw":
                    buttonRefresh.BackColor = Color.Blue;
                    break;
                case "Geel":
                    buttonRefresh.BackColor = Color.Yellow;
                    break;
                case "Groen":
                    buttonRefresh.BackColor = Color.Green;
                    break;
                case "Wit":
                    buttonRefresh.BackColor = Color.White;
                    break;
                case "Rood":
                    buttonRefresh.BackColor = Color.Red;
                    break;
                default:
                    buttonRefresh.BackColor = Color.White;
                    break;
            }

            labelDienst.Text = GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            labelDatum.Text = dat.ToLongDateString(); // dat.ToShortDateString();
            //huidig = dat;

            // als labeldienst is leeg, dan vrije dag, ga naar volgende
            if (string.IsNullOrEmpty(labelDienst.Text))
            {
                // kan geen next roetine gebruiken ivm save data daar terwijl deze leeg is.
                dat = dat.AddDays(1);
                labelDienst.Text = GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
                while (string.IsNullOrEmpty(labelDienst.Text))
                {
                    dat = dat.AddDays(1);
                    labelDienst.Text = GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
                }
                labelDatum.Text = dat.ToLongDateString();
            }

            ProgData.igekozenmaand = dat.Month;
            ProgData.igekozenjaar = dat.Year;

            ViewDag1(dat);

            dat2 = dat.AddDays(1);
            labelDienst2.Text = GetDienstLong(ProgData.GekozenRooster(), dat2, ProgData.GekozenKleur);
            labelDatum2.Text = dat2.ToLongDateString(); // dat.ToShortDateString();

            // als labeldienst is leeg, dan vrije dag, ga naar volgende
            if (string.IsNullOrEmpty(labelDienst2.Text))
            {
                // kan geen next roetine gebruiken ivm save data daar terwijl deze leeg is.
                dat2 = dat2.AddDays(1);
                labelDienst2.Text = GetDienstLong(ProgData.GekozenRooster(), dat2, ProgData.GekozenKleur);
                while (string.IsNullOrEmpty(labelDienst2.Text))
                {
                    dat2 = dat2.AddDays(1);
                    labelDienst2.Text = GetDienstLong(ProgData.GekozenRooster(), dat2, ProgData.GekozenKleur);
                }
                labelDatum2.Text = dat2.ToLongDateString();
            }



            // als dag info, kleur button
            string file = $"{dat.Year}\\{dat.Month}\\{labelDatum.Text} - {labelDienst.Text}.txt";
            if (File.Exists(file))
            {
                buttonOpmerking.BackColor = Color.Yellow;
            }
            else
            {
                buttonOpmerking.BackColor = Color.FromArgb(255, 240, 240, 240);
            }

            ProgData.igekozenmaand = dat2.Month;
            ProgData.igekozenjaar = dat2.Year;

            ViewDag2(dat2);

            file = $"{dat2.Year}\\{dat2.Month}\\{labelDatum2.Text} - {labelDienst2.Text}.txt";
            if (File.Exists(file))
            {
                buttonOpmerking2.BackColor = Color.Yellow;
            }
            else
            {
                buttonOpmerking2.BackColor = Color.FromArgb(255, 240, 240, 240);
            }

        }
        private void ButtonNext_Click(object sender, EventArgs e)
        {
            // met drag drop save SaveData();
            dat = dat.AddDays(1);

            string dienst = GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            while (string.IsNullOrEmpty(dienst))
            {
                dat = dat.AddDays(1);
                dienst = GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            }

            ProgData.igekozenmaand = dat.Month;
            ProgData.igekozenjaar = dat.Year;
            ViewUpdate();
        }
        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            // met drag drop save SaveData();
            dat = dat.AddDays(-1);

            string dienst = GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            while (string.IsNullOrEmpty(dienst))
            {
                dat = dat.AddDays(-1);
                dienst = GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            }

            ProgData.igekozenmaand = dat.Month;
            ProgData.igekozenjaar = dat.Year;

            ViewUpdate();
        }
        private void ButtonNu_Click(object sender, EventArgs e)
        {
            //SaveData();
            dat = DateTime.Now;
            OverzichtWachtForm2_Shown(this, null);
        }
        private void ViewDag1(DateTime dat)
        {
            ProgData.igekozenjaar = dat.Year;
            ProgData.igekozenmaand = dat.Month;
            WerkPlek.LaadWerkPlek(ProgData.GekozenKleur, ProgData.igekozenmaand, ProgData.igekozenjaar);

            foreach (ListBox box in PanelDag1.Controls.OfType<ListBox>())
            {
                box.Items.Clear();
                int tag = int.Parse(box.Tag.ToString());
                if (tag == 0)
                    box.Visible = false;
            }

            try
            {
                // extra diensten in overzicht zetten
                string dir = ProgData.GetDirectoryBezettingMaand(dat);
                ProgData.LoadLooptExtraLijst(dir, ProgData.GekozenKleur);
                if (ProgData.ListLooptExtra.Count > 0)
                {
                    foreach (LooptExtraDienst naam in ProgData.ListLooptExtra)
                    {
                        if (naam._datum.ToShortDateString() == dat.ToShortDateString())
                        {
                            listBox1.Items.Add(naam._naam);
                        }
                    }
                }

                DateTime datum = new DateTime(dat.Year, dat.Month, dat.Day);
                // orginele namen van die kleur in listbox1 zetten
                foreach (personeel man in ProgData.AlleMensen.LijstPersoonKleur)
                {
                    try
                    {
                        var nummer = ProgData.Get_Gebruiker_Nummer(man._achternaam);
                        string afwijking_ = ProgData.GetLaatsteAfwijkingPersoon(ProgData.Get_Gebruiker_Kleur(nummer),
                            nummer, datum);

                        afwijking_ = afwijking_.ToUpper();

                        if (afwijking_.Length > 3)
                            afwijking_ = afwijking_.Substring(0, 3);

                        // als ED VD of RD dan hoort hij niet thuis op eigen wacht
                        if (!(afwijking_ == "ED-" || afwijking_ == "RD-" || afwijking_ == "VD-"))
                            listBox1.Items.Add(man._achternaam);
                    }
                    catch { }
                }

                //26 - 6 - 21 UpdateAfwijkingListBox(listBox1,dat.Day);

                // haal uit WP de werkplekken, en verplaats als nodig
                for (int i = listBox1.Items.Count - 1; i > -1; i--)
                {
                    string naam = listBox1.Items[i].ToString();
                    string werkplek = WerkPlek.GetWerkPlek(naam, dat.Day);
                    if (werkplek != "" && werkplek != label1.Text)
                    {
                        //move
                        listBox1.Items.RemoveAt(i);
                        //26 - 6 - 21 UpdateAfwijkingListBox(listBox1,dat.Day);

                        Invoerveld veld = opbouw.First(a => (a._Label.Text == werkplek && a._Dag == 1));
                        veld._ListNaam.Items.Add(naam);
                        UpdateAfwijkingListBox(veld._ListNaam, dat.Day);
                    }

                    // nu de dubbele, naam begind met +
                    naam = "+" + naam;
                    werkplek = WerkPlek.GetWerkPlek(naam, dat.Day);
                    if (werkplek != "" && werkplek != "!@#$%$") // ook op plek namen
                    {
                            Invoerveld veld = opbouw.First(a => (a._Label.Text == werkplek));
                            veld._ListNaam.Items.Add(naam);
                            UpdateAfwijkingListBox(veld._ListNaam, dat.Day);
                    }

                }

                UpdateAfwijkingListBox(listBox1, dat.Day); //26 - 6 - 21 
            }
            catch { }
        }
        private void ViewDag2(DateTime dat)
        {
            ProgData.igekozenjaar = dat.Year;
            ProgData.igekozenmaand = dat.Month;
            WerkPlek.LaadWerkPlek(ProgData.GekozenKleur, ProgData.igekozenmaand, ProgData.igekozenjaar);

            foreach (ListBox box in PanelDag2.Controls.OfType<ListBox>())
            {
                box.Items.Clear();
                int tag = int.Parse(box.Tag.ToString());
                if (tag == 0)
                    box.Visible = false;
            }

            try
            {
                // extra diensten in overzicht zetten
                string dir = ProgData.GetDirectoryBezettingMaand(dat);
                ProgData.LoadLooptExtraLijst(dir, ProgData.GekozenKleur);
                if (ProgData.ListLooptExtra.Count > 0)
                {
                    foreach (LooptExtraDienst naam in ProgData.ListLooptExtra)
                    {
                        if (naam._datum.ToShortDateString() == dat.ToShortDateString())
                        {
                            listBox83.Items.Add(naam._naam);
                        }
                    }
                }

                DateTime datum = new DateTime(dat.Year, dat.Month, dat.Day);
                // orginele namen van die kleur in listbox1 zetten
                foreach (personeel man in ProgData.AlleMensen.LijstPersoonKleur)
                {
                    try
                    {
                        var nummer = ProgData.Get_Gebruiker_Nummer(man._achternaam);
                        string afwijking_ = ProgData.GetLaatsteAfwijkingPersoon(ProgData.Get_Gebruiker_Kleur(nummer),
                            nummer, datum);

                        afwijking_ = afwijking_.ToUpper();

                        if (afwijking_.Length > 3)
                            afwijking_ = afwijking_.Substring(0, 3);

                        // als ED VD of RD dan hoort hij niet thuis op eigen wacht
                        if (!(afwijking_ == "ED-" || afwijking_ == "RD-" || afwijking_ == "VD-"))
                            listBox83.Items.Add(man._achternaam);
                    }
                    catch { }
                }


                // haal uit WP de werkplekken, en verplaats als nodig
                for (int i = listBox83.Items.Count - 1; i > -1; i--)
                {
                    string naam = listBox83.Items[i].ToString();
                    string werkplek = WerkPlek.GetWerkPlek(naam, dat.Day);
                    if (werkplek != "" && werkplek != label42.Text)
                    {
                        //move
                        listBox83.Items.RemoveAt(i);
                        //26 - 6 - 21 UpdateAfwijkingListBox(listBox83, dat.Day);

                        Invoerveld veld = opbouw.First(a => (a._Label.Text == werkplek && a._Dag == 2));
                        veld._ListNaam.Items.Add(naam);
                        UpdateAfwijkingListBox(veld._ListNaam, dat.Day);
                    }

                    // nu de dubbele, naam begind met +
                    naam = "+" + naam;
                    werkplek = WerkPlek.GetWerkPlek(naam, dat.Day);
                    if (werkplek != "" && werkplek != "!@#$%$") // ook op plek namen
                    {
                        Invoerveld veld = opbouw.First(a => (a._Label.Text == werkplek && a._Dag == 2));
                        veld._ListNaam.Items.Add(naam);
                        UpdateAfwijkingListBox(veld._ListNaam, dat.Day);
                    }

                }
                UpdateAfwijkingListBox(listBox83, dat.Day);
            }
            catch { }
        }
        private void SaveData()
        {
            if (CheckRechten())
            {
                ProgData.igekozenmaand = dat.Month;
                ProgData.igekozenjaar = dat.Year;

                WerkPlek.LaadWerkPlek(ProgData.GekozenKleur, dat.Month, dat.Year);
                foreach (ListBox box in PanelDag1.Controls.OfType<ListBox>())
                {
                    if ((box.Tag != null))
                    {
                        int tag = int.Parse(box.Tag.ToString());
                        // staat er een naam in listbox ?
                        if (tag == 1 && box.Items.Count > 0/* && box != listBox1*/)
                        {
                            for (int i = 0; i < box.Items.Count; i++)
                            {
                                Invoerveld veld = opbouw.First(a => (a._ListNaam == box && a._Dag == 1));
                                string werkplek = veld._Label.Text;
                                // pak naam
                                string naam = box.Items[i].ToString();
                                // save werkdag bij persoon
                                WerkPlek.SetWerkPlek(naam, dat.Day, werkplek);
                            }
                        }
                    }
                }
                WerkPlek.SafeWerkPlek(ProgData.GekozenKleur, dat.Month, dat.Year);


                ProgData.igekozenmaand = dat2.Month;
                ProgData.igekozenjaar = dat2.Year;
                WerkPlek.LaadWerkPlek(ProgData.GekozenKleur, ProgData.igekozenmaand, ProgData.igekozenjaar);

                foreach (ListBox box in PanelDag2.Controls.OfType<ListBox>())
                {
                    if ((box.Tag != null))
                    {
                        int tag = int.Parse(box.Tag.ToString());
                        // staat er een naam in listbox ?
                        if (tag == 1 && box.Items.Count > 0/* && box != listBox83*/)
                        {
                            for (int i = 0; i < box.Items.Count; i++)
                            {
                                Invoerveld veld = opbouw.First(a => (a._ListNaam == box && a._Dag == 2));
                                string werkplek = veld._Label.Text;
                                // pak naam
                                string naam = box.Items[i].ToString();
                                // haal juiste werkdag bij persoon
                                WerkPlek.SetWerkPlek(naam, dat2.Day, werkplek);
                            }
                        }
                    }
                }
                WerkPlek.SafeWerkPlek(ProgData.GekozenKleur, dat2.Month, dat2.Year);
                CaptureMyScreen();
            }
        }
        private void CaptureMyScreen()
        {
            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save(ProgData.GetLocatieOverzichtPlaatje(dat), ImageFormat.Jpeg);
            }
        }
        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            // copy werkplek data 1 dag verder.
            DateTime Volgende_werk_dag = dat;// huidig;
            Volgende_werk_dag = Volgende_werk_dag.AddDays(1);
            string dienst = GetDienstLong(ProgData.GekozenRooster(), Volgende_werk_dag, ProgData.GekozenKleur);
            while (string.IsNullOrEmpty(dienst))
            {
                Volgende_werk_dag = Volgende_werk_dag.AddDays(1);
                dienst = GetDienstLong(ProgData.GekozenRooster(), Volgende_werk_dag, ProgData.GekozenKleur);
            }

            if (Volgende_werk_dag.Month == dat.Month)
            {

                foreach (personeel man in ProgData.AlleMensen.LijstPersoonKleur)
                {
                    string plek = WerkPlek.GetWerkPlek(man._achternaam, dat.Day);
                    WerkPlek.SetWerkPlek(man._achternaam, Volgende_werk_dag.Day, plek);
                    plek = WerkPlek.GetWerkPlek("+" + man._achternaam, dat.Day);
                    if(plek !="")
                        WerkPlek.SetWerkPlek("+" + man._achternaam, Volgende_werk_dag.Day, plek);
                }
                WerkPlek.SafeWerkPlek(ProgData.GekozenKleur, Volgende_werk_dag.Month, Volgende_werk_dag.Year);
                Thread.Sleep(500);
                ButtonNext_Click(this, null);
            }
            else
            {
                // voor gebruik met copy knopje als nieuw maand, dus eerst tijdelijk opslaan
                List<string> LijstWerkPlekPloegCopy = new List<string>();

                foreach (personeel man in ProgData.AlleMensen.LijstPersoonKleur)
                {
                    string plek = WerkPlek.GetWerkPlek(man._achternaam, dat.Day);
                    LijstWerkPlekPloegCopy.Add(man._achternaam);
                    LijstWerkPlekPloegCopy.Add(plek);
                    plek = WerkPlek.GetWerkPlek("+" + man._achternaam, dat.Day);
                    if (plek != "")
                    {
                        LijstWerkPlekPloegCopy.Add("+" + man._achternaam);
                        LijstWerkPlekPloegCopy.Add(plek);
                    }
                }

                WerkPlek.LaadWerkPlek(ProgData.GekozenKleur, Volgende_werk_dag.Month, Volgende_werk_dag.Year);
                for (int i = 0; i < LijstWerkPlekPloegCopy.Count; i += 2)
                {
                    WerkPlek.SetWerkPlek(LijstWerkPlekPloegCopy[i], Volgende_werk_dag.Day, LijstWerkPlekPloegCopy[i + 1]);
                }
                WerkPlek.SafeWerkPlek(ProgData.GekozenKleur, Volgende_werk_dag.Month, Volgende_werk_dag.Year);
                Thread.Sleep(500);
                ButtonNext_Click(this, null);
            }
        }
        private void ButtonOpmerking_Click(object sender, EventArgs e)
        {
            //string file = $"{ProgData.Igekozenjaar}\\{ProgData.igekozenmaand}\\{labelDatum.Text} - {labelDienst.Text}.txt";
            string file = $"{dat.Year}\\{dat.Month}\\{labelDatum.Text} - {labelDienst.Text}.txt";
            if (!File.Exists(file))
            {
                File.Create(file).Dispose();
            }
            Process.Start(file);
            buttonOpmerking.BackColor = Color.Yellow;
        }
        private void buttonOpmerkingDag2_Click(object sender, EventArgs e)
        {
            string file = $"{dat2.Year}\\{dat2.Month}\\{labelDatum2.Text} - {labelDienst2.Text}.txt";
            if (!File.Exists(file))
            {
                File.Create(file).Dispose();
            }
            Process.Start(file);
            buttonOpmerking2.BackColor = Color.Yellow;
        }
        private void GaNaarDatumButton_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker1.Visible = !dateTimePicker1.Visible;
            GaNaarDat.Visible = dateTimePicker1.Visible;
        }
        private void GaNaarDat_Click(object sender, EventArgs e)
        {
            dat = dateTimePicker1.Value;
            ViewUpdate();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ViewUpdate();
        }

        private void buttonSplitDag1_MouseLeave(object sender, EventArgs e)
        {
            buttonSplitDag1.Visible = false;
        }

        private void buttonSplitDag2_MouseLeave(object sender, EventArgs e)
        {
            buttonSplitDag2.Visible = false;
        }

        private void buttonSplitDag1_Click(object sender, EventArgs e)
        {
            buttonSplitDag1.Visible = false;
            var first = string.IsNullOrEmpty(gekozen_naam1) ? (char?)null : gekozen_naam1[0];
            if (first != '+')
            {
                // test of +naam al bestaat
                bool test = WerkPlek.CheckWerkPlek("+" + gekozen_naam1, dat.Day);
                //gekozen_naam
                if (!test)
                {
                    gekozen_listbox1.Items.Add("+" + gekozen_naam1);
                    SaveData();
                }
            }
            else
            {
                // delete de naam met +
                WerkPlek.DeleteWerkPlek(gekozen_naam1, dat.Day);
                WerkPlek.SafeWerkPlek(ProgData.GekozenKleur, dat.Month, dat.Year);
                for (int i = 0; i < gekozen_listbox1.Items.Count; i++)
                {
                    if (gekozen_listbox1.Items[i].ToString() == gekozen_naam1)
                        gekozen_listbox1.Items.RemoveAt(i);
                }
                SaveData();
                ViewUpdate();
            }
        }

        private void buttonSplitDag2_Click(object sender, EventArgs e)
        {
            buttonSplitDag2.Visible = false;
            var first = string.IsNullOrEmpty(gekozen_naam2) ? (char?)null : gekozen_naam2[0];
            if (first != '+')
            {
                // test of +naam al bestaat
                bool test = WerkPlek.CheckWerkPlek("+" + gekozen_naam2, dat2.Day);
                //gekozen_naam
                if (!test)
                {
                    gekozen_listbox2.Items.Add("+" + gekozen_naam2);
                    SaveData();
                }
            }
            else
            {
                // delete de naam met +
                WerkPlek.DeleteWerkPlek(gekozen_naam2, dat2.Day);
                WerkPlek.SafeWerkPlek(ProgData.GekozenKleur, dat2.Month, dat2.Year);
                for (int i = 0; i < gekozen_listbox2.Items.Count; i++)
                {
                    if (gekozen_listbox2.Items[i].ToString() == gekozen_naam2)
                        gekozen_listbox2.Items.RemoveAt(i);
                }
                SaveData();
                ViewUpdate();
            }
        }
    }
}
