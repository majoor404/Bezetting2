using Bezetting2.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
        private DateTime dat;
        private DateTime dat2;


        public class Invoerveld
        {
            //  public invoerveld() { }
            public Invoerveld(Label lb, ListBox ln, ListBox la)
            {
                _Label = lb;
                _ListNaam = ln;
                _ListAfw = la;
            }
            public Label _Label { get; set; }
            public ListBox _ListNaam { get; set; }
            public ListBox _ListAfw { get; set; }
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

            opbouw.Add(new Invoerveld(label1, listBox1, listBoxAfw));
            opbouw.Add(new Invoerveld(label2, listBox2, listBox22));
            opbouw.Add(new Invoerveld(label3, listBox3, listBox23));
            opbouw.Add(new Invoerveld(label4, listBox4, listBox24));
            opbouw.Add(new Invoerveld(label5, listBox5, listBox25));
            opbouw.Add(new Invoerveld(label6, listBox6, listBox26));
            opbouw.Add(new Invoerveld(label7, listBox7, listBox27));
            opbouw.Add(new Invoerveld(label8, listBox8, listBox28));
            opbouw.Add(new Invoerveld(label9, listBox9, listBox29));
            opbouw.Add(new Invoerveld(label10, listBox10, listBox30));
            opbouw.Add(new Invoerveld(label11, listBox11, listBox31));
            opbouw.Add(new Invoerveld(label12, listBox12, listBox32));
            opbouw.Add(new Invoerveld(label13, listBox13, listBox33));
            opbouw.Add(new Invoerveld(label14, listBox14, listBox34));
            opbouw.Add(new Invoerveld(label15, listBox15, listBox35));
            opbouw.Add(new Invoerveld(label16, listBox16, listBox36));
            opbouw.Add(new Invoerveld(label17, listBox17, listBox37));
            opbouw.Add(new Invoerveld(label18, listBox18, listBox38));
            opbouw.Add(new Invoerveld(label19, listBox19, listBox39));
            opbouw.Add(new Invoerveld(label20, listBox20, listBox40));
            opbouw.Add(new Invoerveld(label21, listBox21, listBox41));

            opbouw.Add(new Invoerveld(label42, listBox83, listBoxAfw2));

            opbouw.Add(new Invoerveld(label41, listBox81, listBox77));
            opbouw.Add(new Invoerveld(label40, listBox80, listBox76));
            opbouw.Add(new Invoerveld(label39, listBox79, listBox75));
            opbouw.Add(new Invoerveld(label38, listBox78, listBox74));

            opbouw.Add(new Invoerveld(label37, listBox73, listBox69));
            opbouw.Add(new Invoerveld(label36, listBox72, listBox68));
            opbouw.Add(new Invoerveld(label35, listBox71, listBox67));
            opbouw.Add(new Invoerveld(label34, listBox70, listBox66));

            opbouw.Add(new Invoerveld(label33, listBox65, listBox57));
            opbouw.Add(new Invoerveld(label32, listBox64, listBox56));
            opbouw.Add(new Invoerveld(label31, listBox63, listBox55));
            opbouw.Add(new Invoerveld(label30, listBox62, listBox54));

            opbouw.Add(new Invoerveld(label29, listBox61, listBox53));
            opbouw.Add(new Invoerveld(label28, listBox60, listBox52));
            opbouw.Add(new Invoerveld(label27, listBox59, listBox51));
            opbouw.Add(new Invoerveld(label26, listBox58, listBox50));

            opbouw.Add(new Invoerveld(label22, listBox46, listBox45));
            opbouw.Add(new Invoerveld(label23, listBox47, listBox44));
            opbouw.Add(new Invoerveld(label24, listBox48, listBox43));
            opbouw.Add(new Invoerveld(label25, listBox49, listBox42));


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
            }
        }
        private static bool CheckRechten()
        {
            bool ret = false;
            if (ProgData.RechtenHuidigeGebruiker > 25) ret = true;

            if (ProgData.RechtenHuidigeGebruiker == 25 && ProgData.Huidige_Gebruiker_Werkt_Op_Kleur == ProgData.GekozenKleur) ret = true;

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
                    var nummer = ProgData.Get_Gebruiker_Nummer(naam);
                    DateTime datum = new DateTime(dat.Year, dat.Month, dat.Day);
                    string afwijking_ = ProgData.GetLaatsteAfwijkingPersoon(ProgData.Get_Gebruiker_Kleur(nummer),
                        nummer, datum);
                    broer.Items.Add(afwijking_);
                    if (!string.IsNullOrEmpty(afwijking_))
                        afwijking = true;
                }
                else
                {
                    var nummer = ProgData.Get_Gebruiker_Nummer(naam);
                    DateTime datum = new DateTime(dat2.Year, dat2.Month, dat2.Day);
                    string afwijking_ = ProgData.GetLaatsteAfwijkingPersoon(ProgData.Get_Gebruiker_Kleur(nummer),
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
            labelDienst.Text = GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            labelDatum.Text = dat.ToLongDateString(); // dat.ToShortDateString();

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

            ProgData.igekozenjaar = dat.Year;
            ProgData.igekozenmaand = dat.Month;
            ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

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

            ProgData.igekozenjaar = dat2.Year;
            ProgData.igekozenmaand = dat2.Month;
            ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

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
            SaveData();
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
            SaveData();
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
            SaveData();
            dat = DateTime.Now;
            OverzichtWachtForm2_Shown(this, null);
        }
        private void ViewDag1(DateTime dat)
        {
            ProgData.igekozenjaar = dat.Year;
            ProgData.igekozenmaand = dat.Month;
            ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);
            foreach (ListBox box in PanelDag1.Controls.OfType<ListBox>())
            {
                box.Items.Clear();
            }

            try
            {
                // extra diensten regelen.
                // als extra dienst dan ListPersoneelKleur uitbreiden met die naam, en afwijkingen
                string dir = ProgData.GetDirectoryBezettingMaand(dat);
                ProgData.LoadLooptExtraLijst(dir, ProgData.GekozenKleur);
                if (ProgData.ListLooptExtra.Count > 0)
                {
                    /////////////////////////////////////////////////////////////////////////////////////
                    // VD is bv veranderd in ED
                    // pas dus aan.
                    for (int i = ProgData.ListLooptExtra.Count - 1; i >= 0; i--)
                    {
                        try
                        {
                            werkdag ver = ProgData.LijstWerkdagPloeg.First(aa => (aa._naam == ProgData.ListLooptExtra[i]._naam)
                                                                             && (aa._dagnummer == ProgData.ListLooptExtra[i]._datum.Day)
                                                                             && (aa._afwijkingdienst != ProgData.ListLooptExtra[i]._metcode));
                            ver._afwijkingdienst = ProgData.ListLooptExtra[i]._metcode;
                            ProgData.SaveLijstWerkdagPloeg(ProgData.GekozenKleur, 15);
                        }
                        catch { }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////

                    foreach (LooptExtraDienst naam in ProgData.ListLooptExtra)
                    {
                        if (naam._datum.ToShortDateString() == dat.ToShortDateString())
                        {
                            try
                            {
                                personeel perso = ProgData.LijstPersoneelKleur.First(aa => (aa._achternaam == naam._naam));
                            }
                            catch
                            {
                                personeel extra_man = new personeel
                                {
                                    _achternaam = naam._naam
                                };
                                ProgData.LijstPersoneelKleur.Add(extra_man);
                            }

                            try
                            {
                                werkdag ver = ProgData.LijstWerkdagPloeg.First(aa => (aa._naam == naam._naam) && (aa._dagnummer == dat.Day));
                            }
                            catch
                            {
                                werkdag werkdag_extra_man = new werkdag
                                {
                                    _dagnummer = dat.Day,
                                    _naam = naam._naam,
                                    _werkplek = "",
                                    _afwijkingdienst = naam._metcode
                                };
                                ProgData.LijstWerkdagPloeg.Add(werkdag_extra_man);
                                ProgData.SaveLijstWerkdagPloeg(ProgData.GekozenKleur, 15);
                            }
                        }
                    }
                }

                foreach (personeel man in ProgData.LijstPersoneelKleur)
                {
                    try
                    {
                        // 
                        werkdag ver = ProgData.LijstWerkdagPloeg.First(aa => (aa._naam == man._achternaam) && (aa._dagnummer == dat.Day));

                        if (string.IsNullOrEmpty(ver._werkplek))
                        {
                            listBox1.Items.Add(man._achternaam);
                            UpdateAfwijkingListBox(listBox1, dat.Day);
                        }
                        else
                        {
                            Invoerveld veld = opbouw.First(a => (a._Label.Text == ver._werkplek));
                            veld._ListNaam.Items.Add(man._achternaam);
                            UpdateAfwijkingListBox(veld._ListNaam, dat.Day);
                        }
                    }
                    catch { }
                }

                foreach (ListBox box in PanelDag1.Controls.OfType<ListBox>())
                {
                    // als op werkplek geen personeel, dan invisible afwijking
                    int tag = int.Parse(box.Tag.ToString());
                    if (tag == 1)
                    {
                        if (box.Items.Count == 0)
                        {
                            Invoerveld veld = opbouw.First(a => (a._ListNaam == box));
                            veld._ListAfw.Visible = false;
                        }
                    }
                }
            }
            catch { }
        }
        private void ViewDag2(DateTime dat)
        {
            ProgData.igekozenjaar = dat.Year;
            ProgData.igekozenmaand = dat.Month;
            ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

            foreach (ListBox box in PanelDag2.Controls.OfType<ListBox>())
            {
                box.Items.Clear();
            }

            try
            {
                // extra diensten regelen.
                // als extra dienst dan ListPersoneelKleur uitbreiden met die naam, en afwijkingen
                string dir = ProgData.GetDirectoryBezettingMaand(dat);
                ProgData.LoadLooptExtraLijst(dir, ProgData.GekozenKleur);
                if (ProgData.ListLooptExtra.Count > 0)
                {
                    foreach (LooptExtraDienst naam in ProgData.ListLooptExtra)
                    {
                        if (naam._datum.ToShortDateString() == dat.ToShortDateString())
                        {
                            try
                            {
                                personeel perso = ProgData.LijstPersoneelKleur.First(aa => (aa._achternaam == naam._naam));
                            }
                            catch
                            {
                                personeel extra_man = new personeel
                                {
                                    _achternaam = naam._naam
                                };
                                ProgData.LijstPersoneelKleur.Add(extra_man);
                            }

                            try
                            {
                                werkdag ver = ProgData.LijstWerkdagPloeg.First(aa => (aa._naam == naam._naam) && (aa._dagnummer == dat.Day));
                            }
                            catch
                            {
                                werkdag werkdag_extra_man = new werkdag
                                {
                                    _dagnummer = dat.Day,
                                    _naam = naam._naam,
                                    _werkplek = "",
                                    _afwijkingdienst = naam._metcode
                                };
                                ProgData.LijstWerkdagPloeg.Add(werkdag_extra_man);
                                ProgData.SaveLijstWerkdagPloeg(ProgData.GekozenKleur, 15);
                            }
                        }
                    }
                }

                foreach (personeel man in ProgData.LijstPersoneelKleur)
                {
                    try
                    {
                        // 
                        werkdag ver = ProgData.LijstWerkdagPloeg.First(aa => (aa._naam == man._achternaam) && (aa._dagnummer == dat.Day));

                        if (string.IsNullOrEmpty(ver._werkplek))
                        {
                            listBox83.Items.Add(man._achternaam);
                            UpdateAfwijkingListBox(listBox83, dat.Day);
                        }
                        else
                        {
                            Invoerveld veld = opbouw.Last(a => (a._Label.Text == ver._werkplek));
                            veld._ListNaam.Items.Add(man._achternaam);
                            UpdateAfwijkingListBox(veld._ListNaam, dat.Day);
                        }
                    }
                    catch { }
                }

                foreach (ListBox box in PanelDag2.Controls.OfType<ListBox>())
                {
                    // als op werkplek geen personeel, dan invisible afwijking
                    int tag = int.Parse(box.Tag.ToString());
                    if (tag == 1)
                        if (box.Items.Count == 0)
                        {
                            //try
                            //{
                            Invoerveld veld = opbouw.First(a => (a._ListNaam == box));
                            veld._ListAfw.Visible = false;
                            //}
                            //catch { }
                        }
                }
            }
            catch { }
        }
        private void SaveData()
        {
            if (CheckRechten())
            {
                ProgData.igekozenjaar = dat.Year;
                ProgData.igekozenmaand = dat.Month;
                ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

                foreach (ListBox box in PanelDag1.Controls.OfType<ListBox>())
                {
                    if ((box.Tag != null))
                    {
                        int tag = int.Parse(box.Tag.ToString());
                        // staat er een naam in listbox ?
                        if (tag == 1 && box.Items.Count > 0)
                        {
                            for (int i = 0; i < box.Items.Count; i++)
                            {
                                // pak naam
                                string naam = box.Items[i].ToString();
                                // haal juiste werkdag bij persoon
                                try // als bv ed gecopyeerd vanuit vorige dag kan die niet gevonden worden.
                                {
                                    werkdag ver = ProgData.LijstWerkdagPloeg.First(a => (a._naam == naam) && (a._dagnummer.ToString() == dat.Day.ToString()));
                                    if (box == listBox1)
                                    {
                                        ver._werkplek = "";
                                    }
                                    else
                                    {
                                        // save werkplek
                                        Invoerveld veld = opbouw.First(a => (a._ListNaam == box));
                                        ver._werkplek = veld._Label.Text;
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
                ProgData.SaveLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

                ProgData.igekozenjaar = dat2.Year;
                ProgData.igekozenmaand = dat2.Month;
                ProgData.LaadLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

                foreach (ListBox box in PanelDag2.Controls.OfType<ListBox>())
                {
                    if (box.Tag != null)
                    {
                        int tag = int.Parse(box.Tag.ToString());
                        // staat er een naam in listbox ?
                        if (tag == 1 && box.Items.Count > 0)
                        {
                            for (int i = 0; i < box.Items.Count; i++)
                            {
                                // pak naam
                                string naam = box.Items[i].ToString();
                                // haal juiste werkdag bij persoon
                                try // als bv ed gecopyeerd vanuit vorige dag kan die niet gevonden worden.
                                {
                                    werkdag ver = ProgData.LijstWerkdagPloeg.First(a => (a._naam == naam) && (a._dagnummer.ToString() == dat2.Day.ToString()));
                                    if (box == listBox83)
                                    {
                                        ver._werkplek = "";
                                    }
                                    else
                                    {
                                        // save werkplek
                                        Invoerveld veld = opbouw.First(a => (a._ListNaam == box));
                                        ver._werkplek = veld._Label.Text;
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
                ProgData.SaveLijstWerkdagPloeg(ProgData.GekozenKleur, 15);

                CaptureMyScreen();
            }
        }
        private void OverzichtWachtForm2Dagen_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveData();
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
            foreach (ListBox box in PanelDag2.Controls.OfType<ListBox>())
            {
                box.Items.Clear();
            }

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox83.Items.Add(listBox1.Items[i].ToString());
            }

            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                listBox81.Items.Add(listBox2.Items[i].ToString());
            }
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                listBox80.Items.Add(listBox3.Items[i].ToString());
            }
            for (int i = 0; i < listBox4.Items.Count; i++)
            {
                listBox79.Items.Add(listBox4.Items[i].ToString());
            }
            for (int i = 0; i < listBox5.Items.Count; i++)
            {
                listBox78.Items.Add(listBox5.Items[i].ToString());
            }

            for (int i = 0; i < listBox6.Items.Count; i++)
            {
                listBox73.Items.Add(listBox6.Items[i].ToString());
            }
            for (int i = 0; i < listBox7.Items.Count; i++)
            {
                listBox72.Items.Add(listBox7.Items[i].ToString());
            }
            for (int i = 0; i < listBox8.Items.Count; i++)
            {
                listBox71.Items.Add(listBox8.Items[i].ToString());
            }
            for (int i = 0; i < listBox9.Items.Count; i++)
            {
                listBox70.Items.Add(listBox9.Items[i].ToString());
            }
            // regel 3
            for (int i = 0; i < listBox10.Items.Count; i++)
            {
                listBox65.Items.Add(listBox10.Items[i].ToString());
            }
            for (int i = 0; i < listBox11.Items.Count; i++)
            {
                listBox64.Items.Add(listBox11.Items[i].ToString());
            }
            for (int i = 0; i < listBox12.Items.Count; i++)
            {
                listBox63.Items.Add(listBox12.Items[i].ToString());
            }
            for (int i = 0; i < listBox13.Items.Count; i++)
            {
                listBox62.Items.Add(listBox13.Items[i].ToString());
            }
            // regel 4
            for (int i = 0; i < listBox14.Items.Count; i++)
            {
                listBox61.Items.Add(listBox14.Items[i].ToString());
            }
            for (int i = 0; i < listBox15.Items.Count; i++)
            {
                listBox60.Items.Add(listBox15.Items[i].ToString());
            }
            for (int i = 0; i < listBox16.Items.Count; i++)
            {
                listBox59.Items.Add(listBox16.Items[i].ToString());
            }
            for (int i = 0; i < listBox17.Items.Count; i++)
            {
                listBox58.Items.Add(listBox17.Items[i].ToString());
            }
            // regel 5
            for (int i = 0; i < listBox18.Items.Count; i++)
            {
                listBox46.Items.Add(listBox18.Items[i].ToString());
            }
            for (int i = 0; i < listBox19.Items.Count; i++)
            {
                listBox47.Items.Add(listBox19.Items[i].ToString());
            }
            for (int i = 0; i < listBox20.Items.Count; i++)
            {
                listBox48.Items.Add(listBox20.Items[i].ToString());
            }
            for (int i = 0; i < listBox21.Items.Count; i++)
            {
                listBox49.Items.Add(listBox21.Items[i].ToString());
            }
            SaveData();
            ViewUpdate();
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
            SaveData();
            dat = dateTimePicker1.Value;
            ViewUpdate();
        }
    }
}
