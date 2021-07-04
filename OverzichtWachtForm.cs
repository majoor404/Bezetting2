// 19-6-21

using Bezetting2.Data;
using Bezetting2.Invoer;
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
    public partial class OverzichtWachtForm : Form
    {
        private ListBox sourse;
        private ListBox broer;
        private int sourse_index;
        public List<Invoerveld> opbouw = new List<Invoerveld>();
        private WerkPlek WP = new WerkPlek();
        private DateTime huidig;
        private string gekozen_naam;
        private ListBox gekozen_listbox;

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

        private DateTime dat;

        public OverzichtWachtForm()
        {
            InitializeComponent();
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
                sourse = (ListBox)sender;
                int index = sourse.IndexFromPoint(e.X, e.Y);
                sourse_index = index;

                // voor dubbel, check rechter muis
                if (e.Button == MouseButtons.Right)
                {
                    Point start = new Point(sourse.Location.X, sourse.Location.Y);
                    start.X = start.X + e.Location.X;
                    start.Y = start.Y + e.Location.Y;
                    buttonSplit.Location = start;
                    // bewaar data voor als je op 2 maal invullen knop drukt
                    if (index > -1)
                    {
                        buttonSplit.Visible = true;
                        gekozen_naam = sourse.Items[index].ToString();
                        gekozen_listbox = sourse;
                    }
                    else
                    {
                        buttonSplit.Visible = false;
                        gekozen_naam = "";
                    }
                }
                else
                {
                    if (index > -1)
                    {
                        string s = sourse.Items[index].ToString();
                        _ = DoDragDrop(s, DragDropEffects.Move);
                    }
                }
            }
        }

        private void ListBox2_DragDrop(object sender, DragEventArgs e)
        {
            if (CheckRechten())
            {
                ListBox tb = (ListBox)sender;
                if (tb != sourse)
                {
                    tb.Items.Add(e.Data.GetData(DataFormats.Text).ToString());

                    // remove van oude locatie
                    if (sourse_index > -1)
                        sourse.Items.RemoveAt(sourse_index);

                    UpdateAfwijkingListBox(tb);
                    UpdateAfwijkingListBox(sourse);
                }
                SaveData();
            }
        }

        private void OverzichtWachtForm_Shown(object sender, EventArgs e)
        {
            buttonOpmerking.Enabled = ProgData.RechtenHuidigeGebruiker > 24;

            // als ToegangNivo hoog genoeg, vrijgave edit
            if (ProgData.RechtenHuidigeGebruiker > 100)
            {
                foreach (CheckBox check in this.Controls.OfType<CheckBox>())
                {
                    check.Visible = true;
                }

                MessageBox.Show("Edit mode, Gebruikers is Admin");
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

            LaadDataFormulier(); // zet juiste vakjes aan

            dat = DateTime.Now;

            //ProgData.GekozenKleur = ProgData.Main.comboBoxKleurKeuze.Text;

            if (ProgData.RechtenHuidigeGebruiker < 101)
                ViewUpdate();
        }

        // geklikt op label, als in edit mode vraag nieuwe tekst
        private void Label2_Click(object sender, EventArgs e)
        {
            // geklikt op label, als in edit mode vraag nieuwe tekst
            if (ProgData.RechtenHuidigeGebruiker > 80)
            {
                Label s = (Label)sender;
                NieuwLabel n = new NieuwLabel();
                n.labelOudeLabel.Text = s.Text;
                n.ShowDialog();
                s.Text = n.textBox1.Text;
                if (string.IsNullOrEmpty(s.Text))
                    s.Text = "-";

                InstellingenProg.ProgrammaData[00] = label1.Text;
                InstellingenProg.ProgrammaData[01] = label2.Text;
                InstellingenProg.ProgrammaData[02] = label3.Text;
                InstellingenProg.ProgrammaData[03] = label4.Text;
                InstellingenProg.ProgrammaData[04] = label5.Text;
                InstellingenProg.ProgrammaData[05] = label6.Text;
                InstellingenProg.ProgrammaData[06] = label7.Text;
                InstellingenProg.ProgrammaData[07] = label8.Text;
                InstellingenProg.ProgrammaData[08] = label9.Text;
                InstellingenProg.ProgrammaData[09] = label10.Text;
                InstellingenProg.ProgrammaData[10] = label11.Text;
                InstellingenProg.ProgrammaData[11] = label12.Text;
                InstellingenProg.ProgrammaData[12] = label13.Text;
                InstellingenProg.ProgrammaData[13] = label14.Text;
                InstellingenProg.ProgrammaData[14] = label15.Text;
                InstellingenProg.ProgrammaData[15] = label16.Text;
                InstellingenProg.ProgrammaData[16] = label17.Text;
                InstellingenProg.ProgrammaData[17] = label18.Text;
                InstellingenProg.ProgrammaData[18] = label19.Text;
                InstellingenProg.ProgrammaData[19] = label20.Text;
                InstellingenProg.ProgrammaData[20] = label21.Text;

                InstellingenProg.SaveProgrammaData();
            }
        }

        // listbox not visible als uitgevinkt
        private void CheckBox11_CheckedChanged(object sender, EventArgs e)
        {
            listBox2.Visible = checkBox1.Checked;
            listBox3.Visible = checkBox2.Checked;
            listBox4.Visible = checkBox3.Checked;
            listBox5.Visible = checkBox4.Checked;
            listBox6.Visible = checkBox5.Checked;
            listBox7.Visible = checkBox6.Checked;
            listBox8.Visible = checkBox7.Checked;
            listBox9.Visible = checkBox8.Checked;
            listBox10.Visible = checkBox9.Checked;
            listBox11.Visible = checkBox10.Checked;
            listBox12.Visible = checkBox11.Checked;
            listBox13.Visible = checkBox12.Checked;
            listBox14.Visible = checkBox13.Checked;
            listBox15.Visible = checkBox14.Checked;
            listBox16.Visible = checkBox15.Checked;
            listBox17.Visible = checkBox16.Checked;
            listBox18.Visible = checkBox17.Checked;
            listBox19.Visible = checkBox18.Checked;
            listBox20.Visible = checkBox19.Checked;
            listBox21.Visible = checkBox20.Checked;

            label2.Visible = listBox2.Visible;
            label3.Visible = listBox3.Visible;
            label4.Visible = listBox4.Visible;
            label5.Visible = listBox5.Visible;
            label6.Visible = listBox6.Visible;
            label7.Visible = listBox7.Visible;
            label8.Visible = listBox8.Visible;
            label9.Visible = listBox9.Visible;
            label10.Visible = listBox10.Visible;
            label11.Visible = listBox11.Visible;
            label12.Visible = listBox12.Visible;
            label13.Visible = listBox13.Visible;
            label14.Visible = listBox14.Visible;
            label15.Visible = listBox15.Visible;
            label16.Visible = listBox16.Visible;
            label17.Visible = listBox17.Visible;
            label18.Visible = listBox18.Visible;
            label19.Visible = listBox19.Visible;
            label20.Visible = listBox20.Visible;
            label21.Visible = listBox21.Visible;
        }

        private void UpdateAfwijkingListBox(ListBox box)
        {
            Invoerveld veld = opbouw.First(a => (a._ListNaam == box));
            broer = veld._ListAfw;

            if (box.Items.Count > 0)
            {
                bool afwijking = false;
                broer.Items.Clear();
                for (int i = 0; i < box.Items.Count; i++)
                {
                    // voor elke persoon in lijst
                    string naam = box.Items[i].ToString();

                    var first = string.IsNullOrEmpty(naam) ? (char?)null : naam[0];
                    if (first == '+')
                        naam = naam.Substring(1, naam.Length-1);

                    var nummer = ProgData.Get_Gebruiker_Nummer(naam);
                    DateTime datum = new DateTime(dat.Year, dat.Month, dat.Day);
                    string afwijking_ = ProgData.GetLaatsteAfwijkingPersoon(ProgData.Get_Gebruiker_Kleur(nummer),
                        nummer, datum);

                    broer.Items.Add(afwijking_);
                    if (!string.IsNullOrEmpty(afwijking_))
                        afwijking = true;
                }
                broer.Visible = afwijking;
            }
            else
            {
                broer.Visible = false;
            }
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

        private void OverzichtWachtForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ProgData.RechtenHuidigeGebruiker == 101)
            {
                SaveCheckboxStatus();
                InstellingenProg.SaveProgrammaData();
            }
        }

        private void SaveData()
        {
            if (CheckRechten())
            {
                WerkPlek.LaadWerkPlek(ProgData.GekozenKleur, ProgData.igekozenmaand, ProgData.igekozenjaar);
                foreach (ListBox box in this.Controls.OfType<ListBox>())
                {
                    if ((box.Tag != null))
                    {
                        int tag = int.Parse(box.Tag.ToString());
                        // staat er een naam in listbox ?
                        if (tag < 22 && box.Items.Count > 0/* && box != listBox1*/)
                        {
                            for (int i = 0; i < box.Items.Count; i++)
                            {
                                Invoerveld veld = opbouw.First(a => (a._ListNaam == box));
                                string werkplek = veld._Label.Text;
                                // pak naam
                                string naam = box.Items[i].ToString();
                                // haal juiste werkdag bij persoon
                                WerkPlek.SetWerkPlek(naam, dat.Day, werkplek);
                            }
                        }
                    }
                }
                WerkPlek.SafeWerkPlek(ProgData.GekozenKleur, ProgData.igekozenmaand, ProgData.igekozenjaar);
                CaptureMyScreen();
            }
        }

        private void ViewUpdate()
        {
            dateTimePicker1.Visible = false;
            GaNaarDat.Visible = false;

            huidig = dat;
            labelDatum.Text = dat.ToLongDateString(); // dat.ToShortDateString();

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
            WerkPlek.LaadWerkPlek(ProgData.GekozenKleur, ProgData.igekozenmaand, ProgData.igekozenjaar);

            foreach (ListBox box in this.Controls.OfType<ListBox>())
            {
                box.Items.Clear();
                if (int.Parse(box.Tag.ToString()) > 21)
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
                        if (naam._datum.Date == dat.Date)
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
                                                                                nummer,
                                                                                datum);

                        afwijking_ = afwijking_.ToUpper();

                        if (afwijking_.Length > 3)
                            afwijking_ = afwijking_.Substring(0, 3);

                        // als ED VD of RD dan hoort hij niet thuis op eigen wacht
                        if (!(afwijking_ == "ED-" || afwijking_ == "RD-" || afwijking_ == "VD-"))
                            listBox1.Items.Add(man._achternaam);
                    }
                    catch { }
                }

                //26-6-21 UpdateAfwijkingListBox(listBox1);

                // haal uit WP de werkplekken, en verplaats als nodig
                for (int i = listBox1.Items.Count - 1; i > -1; i--)
                {
                    string naam = listBox1.Items[i].ToString();
                    string werkplek = WerkPlek.GetWerkPlek(naam, dat.Day);
                    if (werkplek != "" && werkplek != label1.Text)
                    {
                        //move
                        listBox1.Items.RemoveAt(i);
                        //26-6-21 UpdateAfwijkingListBox(listBox1);

                        Invoerveld veld = opbouw.First(a => (a._Label.Text == werkplek));
                        veld._ListNaam.Items.Add(naam);
                        UpdateAfwijkingListBox(veld._ListNaam);
                    }
                    
                    // nu de dubbele, naam begind met +
                    naam = "+" + naam;
                    werkplek = WerkPlek.GetWerkPlek(naam, dat.Day);
                    if (werkplek != "") // ook op plek namen
                    {
                        Invoerveld veld = opbouw.First(a => (a._Label.Text == werkplek));
                        veld._ListNaam.Items.Add(naam);
                        UpdateAfwijkingListBox(veld._ListNaam);
                    }
                }

                UpdateAfwijkingListBox(listBox1); //26-6-21


                //als dag info, kleur button
                string file = $"{ProgData.igekozenjaar}\\{ProgData.igekozenmaand}\\{labelDatum.Text} - {labelDienst.Text}.txt";
                if (File.Exists(file))
                {
                    buttonOpmerking.BackColor = Color.Yellow;
                }
                else
                {
                    buttonOpmerking.BackColor = Color.FromArgb(255, 240, 240, 240);
                }
            }
            catch { }
        }

        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            DateTime Volgende_werk_dag = huidig;
            Volgende_werk_dag = Volgende_werk_dag.AddDays(1);
            string dienst = GetDienstLong(ProgData.GekozenRooster(), Volgende_werk_dag, ProgData.GekozenKleur);
            while (string.IsNullOrEmpty(dienst))
            {
                Volgende_werk_dag = Volgende_werk_dag.AddDays(1);
                dienst = GetDienstLong(ProgData.GekozenRooster(), Volgende_werk_dag, ProgData.GekozenKleur);
            }

            if (Volgende_werk_dag.Month == huidig.Month)
            {
                foreach (personeel man in ProgData.AlleMensen.LijstPersoonKleur)
                {
                    string plek = WerkPlek.GetWerkPlek(man._achternaam, huidig.Day);
                    WerkPlek.SetWerkPlek(man._achternaam, Volgende_werk_dag.Day, plek);
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
                    string plek = WerkPlek.GetWerkPlek(man._achternaam, huidig.Day);
                    LijstWerkPlekPloegCopy.Add(man._achternaam);
                    LijstWerkPlekPloegCopy.Add(plek);
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

        private static bool CheckRechten()
        {
            bool ret = false;
            if (ProgData.RechtenHuidigeGebruiker > 25) ret = true;

            if (ProgData.RechtenHuidigeGebruiker == 25 && ProgData.Huidige_Gebruiker_Werkt_Op_Kleur() == ProgData.GekozenKleur) ret = true;

            return ret;
        }

        private void LaadDataFormulier()
        {
            // zet juiste vakjes aan
            try
            {
                label1.Text = InstellingenProg.ProgrammaData[00];
                label2.Text = InstellingenProg.ProgrammaData[01];
                label3.Text = InstellingenProg.ProgrammaData[02];
                label4.Text = InstellingenProg.ProgrammaData[03];
                label5.Text = InstellingenProg.ProgrammaData[04];
                label6.Text = InstellingenProg.ProgrammaData[05];
                label7.Text = InstellingenProg.ProgrammaData[06];
                label8.Text = InstellingenProg.ProgrammaData[07];
                label9.Text = InstellingenProg.ProgrammaData[08];
                label10.Text = InstellingenProg.ProgrammaData[09];
                label11.Text = InstellingenProg.ProgrammaData[10];
                label12.Text = InstellingenProg.ProgrammaData[11];
                label13.Text = InstellingenProg.ProgrammaData[12];
                label14.Text = InstellingenProg.ProgrammaData[13];
                label15.Text = InstellingenProg.ProgrammaData[14];
                label16.Text = InstellingenProg.ProgrammaData[15];
                label17.Text = InstellingenProg.ProgrammaData[16];
                label18.Text = InstellingenProg.ProgrammaData[17];
                label19.Text = InstellingenProg.ProgrammaData[18];
                label20.Text = InstellingenProg.ProgrammaData[19];
                label21.Text = InstellingenProg.ProgrammaData[20];

                checkBox1.Checked = bool.Parse(InstellingenProg.ProgrammaData[21]);
                checkBox2.Checked = bool.Parse(InstellingenProg.ProgrammaData[22]);
                checkBox3.Checked = bool.Parse(InstellingenProg.ProgrammaData[23]);
                checkBox4.Checked = bool.Parse(InstellingenProg.ProgrammaData[24]);
                checkBox5.Checked = bool.Parse(InstellingenProg.ProgrammaData[25]);
                checkBox6.Checked = bool.Parse(InstellingenProg.ProgrammaData[26]);
                checkBox7.Checked = bool.Parse(InstellingenProg.ProgrammaData[27]);
                checkBox8.Checked = bool.Parse(InstellingenProg.ProgrammaData[28]);
                checkBox9.Checked = bool.Parse(InstellingenProg.ProgrammaData[29]);
                checkBox10.Checked = bool.Parse(InstellingenProg.ProgrammaData[30]);
                checkBox11.Checked = bool.Parse(InstellingenProg.ProgrammaData[31]);
                checkBox12.Checked = bool.Parse(InstellingenProg.ProgrammaData[32]);
                checkBox13.Checked = bool.Parse(InstellingenProg.ProgrammaData[33]);
                checkBox14.Checked = bool.Parse(InstellingenProg.ProgrammaData[34]);
                checkBox15.Checked = bool.Parse(InstellingenProg.ProgrammaData[35]);
                checkBox16.Checked = bool.Parse(InstellingenProg.ProgrammaData[36]);
                checkBox17.Checked = bool.Parse(InstellingenProg.ProgrammaData[37]);
                checkBox18.Checked = bool.Parse(InstellingenProg.ProgrammaData[38]);
                checkBox19.Checked = bool.Parse(InstellingenProg.ProgrammaData[39]);
                checkBox20.Checked = bool.Parse(InstellingenProg.ProgrammaData[40]);

                label2.Visible = listBox22.Visible = listBox2.Visible;
                label3.Visible = listBox23.Visible = listBox3.Visible;
                label4.Visible = listBox24.Visible = listBox4.Visible;
                label5.Visible = listBox25.Visible = listBox5.Visible;
                label6.Visible = listBox26.Visible = listBox6.Visible;
                label7.Visible = listBox27.Visible = listBox7.Visible;
                label8.Visible = listBox28.Visible = listBox8.Visible;
                label9.Visible = listBox29.Visible = listBox9.Visible;
                label10.Visible = listBox30.Visible = listBox10.Visible;
                label11.Visible = listBox31.Visible = listBox11.Visible;
                label12.Visible = listBox32.Visible = listBox12.Visible;
                label13.Visible = listBox33.Visible = listBox13.Visible;
                label14.Visible = listBox34.Visible = listBox14.Visible;
                label15.Visible = listBox35.Visible = listBox15.Visible;
                label16.Visible = listBox36.Visible = listBox16.Visible;
                label17.Visible = listBox37.Visible = listBox17.Visible;
                label18.Visible = listBox38.Visible = listBox18.Visible;
                label19.Visible = listBox39.Visible = listBox19.Visible;
                label20.Visible = listBox40.Visible = listBox20.Visible;
                label21.Visible = listBox41.Visible = listBox21.Visible;
            }
            catch { }
        }

        private void ListBox1_MouseLeave(object sender, EventArgs e)
        {
            sourse = (ListBox)sender;
            if (sourse.SelectedIndex > -1)
                sourse.SelectedIndex = -1;
        }

        private void ButtonNu_Click(object sender, EventArgs e)
        {
            //SaveData();
            OverzichtWachtForm_Shown(this, null);
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

        private void ListBox2_DrawItem(object sender, DrawItemEventArgs e)
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

        private void SaveCheckboxStatus()
        {
            InstellingenProg.ProgrammaData[21] = checkBox1.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[22] = checkBox2.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[23] = checkBox3.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[24] = checkBox4.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[25] = checkBox5.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[26] = checkBox6.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[27] = checkBox7.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[28] = checkBox8.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[29] = checkBox9.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[30] = checkBox10.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[31] = checkBox11.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[32] = checkBox12.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[33] = checkBox13.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[34] = checkBox14.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[35] = checkBox15.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[36] = checkBox16.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[37] = checkBox17.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[38] = checkBox18.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[39] = checkBox19.Checked ? true.ToString() : false.ToString();
            InstellingenProg.ProgrammaData[40] = checkBox20.Checked ? true.ToString() : false.ToString();
        }

        private void ButtonOpmerking_Click(object sender, EventArgs e)
        {
            string file = $"{ProgData.igekozenjaar}\\{ProgData.igekozenmaand}\\{labelDatum.Text} - {labelDienst.Text}.txt";
            if (!File.Exists(file))
            {
                File.Create(file).Dispose();
            }
            Process.Start(file);
            buttonOpmerking.BackColor = Color.Yellow;
        }

        private void GaNaarDatumButton_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker1.Visible = !dateTimePicker1.Visible;
            GaNaarDat.Visible = dateTimePicker1.Visible;
        }

        private void GaNaarDat_Click(object sender, EventArgs e)
        {
            //SaveData();
            dat = dateTimePicker1.Value;
            ViewUpdate();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            ViewUpdate();
        }

        private void buttonSplit_MouseLeave(object sender, EventArgs e)
        {
            buttonSplit.Visible = false;
        }

        private void buttonSplit_Click(object sender, EventArgs e)
        {
            buttonSplit.Visible = false;
            var first = string.IsNullOrEmpty(gekozen_naam) ? (char?)null : gekozen_naam[0];
            if (first != '+')
            {
                // test of +naam al bestaat
                bool test = WerkPlek.CheckWerkPlek("+" + gekozen_naam, huidig.Day);
                //gekozen_naam
                if (!test)
                gekozen_listbox.Items.Add("+" + gekozen_naam);
            }
        }
    }
}