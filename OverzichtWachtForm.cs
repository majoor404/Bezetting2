using Bezetting2.Data;
using Bezetting2.Invoer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class OverzichtWachtForm : Form
    {
        private ListBox sourse;
        private ListBox broer;
        private int sourse_index;
        public List<invoerveld> opbouw = new List<invoerveld>();

        public class invoerveld
        {
            //            public invoerveld() { }
            public invoerveld(Label lb, ListBox ln, ListBox la)
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

        private void listBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (CheckRechten())
            {
                if (e.Data.GetDataPresent(DataFormats.Text))
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (CheckRechten())
            {
                //if (e.Button == MouseButtons.Right)
                //{
                //    sourse = (ListBox)sender;
                //    Point start = sourse.PointToScreen(new Point(e.X, e.Y));
                //    QuickInvoerForm quick = new QuickInvoerForm();
                //    quick.Location = new System.Drawing.Point(start.X,start.Y);
                //    quick.ShowDialog();
                //}
                //else
                //{
                sourse = (ListBox)sender;
                int index = sourse.IndexFromPoint(e.X, e.Y);
                sourse_index = index;
                if (index > -1)
                {
                    string s = sourse.Items[index].ToString();
                    DragDropEffects dde1 = DoDragDrop(s, DragDropEffects.Move);
                }
                //}
            }
        }

        private void listBox2_DragDrop(object sender, DragEventArgs e)
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
                    //UpdateAfwijking();
                }
            }
        }

        private void OverzichtWachtForm_Shown(object sender, EventArgs e)
        {
            // als ToegangNivo hoog genoeg, vrijgave edit
            if (ProgData.RechtenHuidigeGebruiker > 100)
            {
                foreach (CheckBox check in this.Controls.OfType<CheckBox>())
                {
                    check.Visible = true;
                }

                MessageBox.Show("In edit mode, Gebruikers rechten > 100");
            }

            opbouw.Clear();

            opbouw.Add(new invoerveld(label1, listBox1, listBoxAfw));
            opbouw.Add(new invoerveld(label2, listBox2, listBox22));
            opbouw.Add(new invoerveld(label3, listBox3, listBox23));
            opbouw.Add(new invoerveld(label4, listBox4, listBox24));
            opbouw.Add(new invoerveld(label5, listBox5, listBox25));
            opbouw.Add(new invoerveld(label6, listBox6, listBox26));
            opbouw.Add(new invoerveld(label7, listBox7, listBox27));
            opbouw.Add(new invoerveld(label8, listBox8, listBox28));
            opbouw.Add(new invoerveld(label9, listBox9, listBox29));
            opbouw.Add(new invoerveld(label10, listBox10, listBox30));
            opbouw.Add(new invoerveld(label11, listBox11, listBox31));
            opbouw.Add(new invoerveld(label12, listBox12, listBox32));
            opbouw.Add(new invoerveld(label13, listBox13, listBox33));
            opbouw.Add(new invoerveld(label14, listBox14, listBox34));
            opbouw.Add(new invoerveld(label15, listBox15, listBox35));
            opbouw.Add(new invoerveld(label16, listBox16, listBox36));
            opbouw.Add(new invoerveld(label17, listBox17, listBox37));
            opbouw.Add(new invoerveld(label18, listBox18, listBox38));
            opbouw.Add(new invoerveld(label19, listBox19, listBox39));
            opbouw.Add(new invoerveld(label20, listBox20, listBox40));
            opbouw.Add(new invoerveld(label21, listBox21, listBox41));

            LaadDataFormulier();

            // zet datum en kleur in beeld

            dat = DateTime.Now;

            //ProgData.GekozenKleur = ProgData.Main.comboBoxKleurKeuze.Text;

            ViewUpdate();
        }

        // geklikt op label, als in edit mode vraag nieuwe tekst
        private void label2_Click(object sender, EventArgs e)
        {
            // geklikt op label, als in edit mode vraag nieuwe tekst
            if (ProgData.RechtenHuidigeGebruiker > 80)
            {
                Label s = (Label)sender;
                NieuwLabel n = new NieuwLabel();
                n.labelOudeLabel.Text = s.Text;
                n.ShowDialog();
                s.Text = n.textBox1.Text;
                if (s.Text == "")
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
        private void checkBox11_CheckedChanged(object sender, EventArgs e)
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

            InstellingenProg.SaveProgrammaData();
        }

        private void UpdateAfwijkingListBox(ListBox box)
        {
            invoerveld veld = opbouw.First(a => (a._ListNaam == box));
            broer = veld._ListAfw;
            broer.Items.Clear();

            bool afwijking = false;
            for (int i = 0; i < box.Items.Count; i++)
            {
                // voor elke persoon in lijst
                string naam = box.Items[i].ToString();
                werkdag ver = ProgData.ListWerkdagPloeg.First(a => (a._naam == naam) && (a._dagnummer.ToString() == dat.Day.ToString()));
                broer.Items.Add(ver._afwijkingdienst);
                if (ver._afwijkingdienst != "")
                    afwijking = true;
            }
            broer.Visible = afwijking;
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            SaveData();
            dat = dat.AddDays(-1);
            string dienst = ProgData.MDatum.GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            while (dienst == "")
            {
                dat = dat.AddDays(-1);
                dienst = ProgData.MDatum.GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            }

            ProgData.igekozenjaar = dat.Year;
            ProgData.igekozenmaand = dat.Month;

            ViewUpdate();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            SaveData();
            dat = dat.AddDays(1);
            string dienst = ProgData.MDatum.GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            while (dienst == "")
            {
                dat = dat.AddDays(1);
                dienst = ProgData.MDatum.GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
            }

            ProgData.igekozenjaar = dat.Year;
            ProgData.igekozenmaand = dat.Month;

            ViewUpdate();
        }

        private void OverzichtWachtForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            if (CheckRechten())
            {
                ProgData.LoadPloegBezetting(ProgData.GekozenKleur);
                foreach (ListBox box in this.Controls.OfType<ListBox>())
                {
                    if ((box.Tag != null))
                    {
                        int tag = int.Parse(box.Tag.ToString());
                        // staat er een naam in listbox ?
                        if (tag < 22 && box.Items.Count > 0)
                        {
                            for (int i = 0; i < box.Items.Count; i++)
                            {
                                // pak naam
                                string naam = box.Items[i].ToString();
                                // haal juiste werkdag bij persoon
                                werkdag ver = ProgData.ListWerkdagPloeg.First(a => (a._naam == naam) && (a._dagnummer.ToString() == dat.Day.ToString()));
                                if (box.Tag.ToString() == "1")
                                {
                                    ver._werkplek = "";
                                }
                                else
                                {
                                    // save werkplek
                                    invoerveld veld = opbouw.First(a => (a._ListNaam == box));
                                    ver._werkplek = veld._Label.Text;
                                }
                            }
                        }
                    }
                }
                ProgData.SavePloegBezetting(ProgData.GekozenKleur);

                CaptureMyScreen();
            }
        }

        private void ViewUpdate()
        {
            ProgData.LoadPloegBezetting(ProgData.GekozenKleur);

            labelDatum.Text = dat.ToLongDateString(); // dat.ToShortDateString();

            //labelKleur.Text = ProgData.GekozenKleur;

            labelDienst.Text = ProgData.MDatum.GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);

            // als labeldienst is leeg, dan vrije dag, ga naar volgende
            if (labelDienst.Text == "")
            {
                // kan geen next roetine gebruiken ivm save data daar terwijl deze leeg is.
                dat = dat.AddDays(1);
                labelDienst.Text = ProgData.MDatum.GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
                while (labelDienst.Text == "")
                {
                    dat = dat.AddDays(1);
                    labelDienst.Text = ProgData.MDatum.GetDienstLong(ProgData.GekozenRooster(), dat, ProgData.GekozenKleur);
                }
            }

            foreach (ListBox box in this.Controls.OfType<ListBox>())
            {
                box.Items.Clear();
            }

            try
            {
                foreach (personeel man in ProgData.ListPersoneelKleur)
                {
                    werkdag ver = ProgData.ListWerkdagPloeg.First(aa => (aa._naam == man._achternaam) && (aa._dagnummer == dat.Day));
                    if (ver._werkplek == "")
                    {
                        listBox1.Items.Add(man._achternaam);
                        UpdateAfwijkingListBox(listBox1);
                    }
                    else
                    {
                        invoerveld veld = opbouw.First(a => (a._Label.Text == ver._werkplek));
                        veld._ListNaam.Items.Add(man._achternaam);

                        UpdateAfwijkingListBox(veld._ListNaam);
                    }
                }

                foreach (ListBox box in this.Controls.OfType<ListBox>())
                {
                    // als op werkplek geen personeel, dan invisible afwijking
                    int tag = int.Parse(box.Tag.ToString());
                    if (tag < 22)
                    {
                        if (box.Items.Count == 0)
                        {
                            invoerveld veld = opbouw.First(a => (a._ListNaam == box));
                            veld._ListAfw.Visible = false;
                        }
                    }
                }
            }
            catch { }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            List<string> temp1 = new List<string>();
            List<string> temp2 = new List<string>();
            List<string> temp3 = new List<string>();
            List<string> temp4 = new List<string>();
            List<string> temp5 = new List<string>();
            List<string> temp6 = new List<string>();
            List<string> temp7 = new List<string>();
            List<string> temp8 = new List<string>();
            List<string> temp9 = new List<string>();
            List<string> temp10 = new List<string>();
            List<string> temp11 = new List<string>();
            List<string> temp12 = new List<string>();
            List<string> temp13 = new List<string>();
            List<string> temp14 = new List<string>();
            List<string> temp15 = new List<string>();
            List<string> temp16 = new List<string>();
            List<string> temp17 = new List<string>();
            List<string> temp18 = new List<string>();
            List<string> temp19 = new List<string>();
            List<string> temp20 = new List<string>();
            List<string> temp21 = new List<string>();

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                temp1.Add(listBox1.Items[i].ToString());
            }
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                temp2.Add(listBox2.Items[i].ToString());
            }
            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                temp3.Add(listBox3.Items[i].ToString());
            }
            for (int i = 0; i < listBox4.Items.Count; i++)
            {
                temp4.Add(listBox4.Items[i].ToString());
            }
            for (int i = 0; i < listBox5.Items.Count; i++)
            {
                temp5.Add(listBox5.Items[i].ToString());
            }
            for (int i = 0; i < listBox6.Items.Count; i++)
            {
                temp6.Add(listBox6.Items[i].ToString());
            }
            for (int i = 0; i < listBox7.Items.Count; i++)
            {
                temp7.Add(listBox7.Items[i].ToString());
            }
            for (int i = 0; i < listBox8.Items.Count; i++)
            {
                temp8.Add(listBox8.Items[i].ToString());
            }
            for (int i = 0; i < listBox9.Items.Count; i++)
            {
                temp9.Add(listBox9.Items[i].ToString());
            }
            for (int i = 0; i < listBox10.Items.Count; i++)
            {
                temp10.Add(listBox10.Items[i].ToString());
            }
            for (int i = 0; i < listBox11.Items.Count; i++)
            {
                temp11.Add(listBox11.Items[i].ToString());
            }
            for (int i = 0; i < listBox12.Items.Count; i++)
            {
                temp12.Add(listBox12.Items[i].ToString());
            }
            for (int i = 0; i < listBox13.Items.Count; i++)
            {
                temp13.Add(listBox13.Items[i].ToString());
            }
            for (int i = 0; i < listBox14.Items.Count; i++)
            {
                temp14.Add(listBox14.Items[i].ToString());
            }
            for (int i = 0; i < listBox15.Items.Count; i++)
            {
                temp15.Add(listBox15.Items[i].ToString());
            }
            for (int i = 0; i < listBox16.Items.Count; i++)
            {
                temp16.Add(listBox16.Items[i].ToString());
            }
            for (int i = 0; i < listBox17.Items.Count; i++)
            {
                temp17.Add(listBox17.Items[i].ToString());
            }
            for (int i = 0; i < listBox18.Items.Count; i++)
            {
                temp18.Add(listBox18.Items[i].ToString());
            }
            for (int i = 0; i < listBox19.Items.Count; i++)
            {
                temp19.Add(listBox19.Items[i].ToString());
            }
            for (int i = 0; i < listBox20.Items.Count; i++)
            {
                temp20.Add(listBox20.Items[i].ToString());
            }
            for (int i = 0; i < listBox21.Items.Count; i++)
            {
                temp21.Add(listBox21.Items[i].ToString());
            }

            // ga naar volgende dag
            buttonNext_Click(this, null);
            foreach (ListBox box in this.Controls.OfType<ListBox>())
            {
                box.Items.Clear();
            }

            for (int i = 0; i < temp1.Count; i++)
            {
                listBox1.Items.Add(temp1[i]);
            }
            for (int i = 0; i < temp2.Count; i++)
            {
                listBox2.Items.Add(temp2[i]);
            }
            for (int i = 0; i < temp3.Count; i++)
            {
                listBox3.Items.Add(temp3[i]);
            }
            for (int i = 0; i < temp4.Count; i++)
            {
                listBox4.Items.Add(temp4[i]);
            }
            for (int i = 0; i < temp5.Count; i++)
            {
                listBox5.Items.Add(temp5[i]);
            }
            for (int i = 0; i < temp6.Count; i++)
            {
                listBox6.Items.Add(temp6[i]);
            }
            for (int i = 0; i < temp7.Count; i++)
            {
                listBox7.Items.Add(temp7[i]);
            }
            for (int i = 0; i < temp8.Count; i++)
            {
                listBox8.Items.Add(temp8[i]);
            }
            for (int i = 0; i < temp9.Count; i++)
            {
                listBox9.Items.Add(temp9[i]);
            }
            for (int i = 0; i < temp10.Count; i++)
            {
                listBox10.Items.Add(temp10[i]);
            }
            for (int i = 0; i < temp11.Count; i++)
            {
                listBox11.Items.Add(temp11[i]);
            }
            for (int i = 0; i < temp12.Count; i++)
            {
                listBox12.Items.Add(temp12[i]);
            }
            for (int i = 0; i < temp13.Count; i++)
            {
                listBox13.Items.Add(temp13[i]);
            }
            for (int i = 0; i < temp14.Count; i++)
            {
                listBox14.Items.Add(temp14[i]);
            }
            for (int i = 0; i < temp15.Count; i++)
            {
                listBox15.Items.Add(temp15[i]);
            }
            for (int i = 0; i < temp16.Count; i++)
            {
                listBox16.Items.Add(temp16[i]);
            }
            for (int i = 0; i < temp17.Count; i++)
            {
                listBox17.Items.Add(temp17[i]);
            }
            for (int i = 0; i < temp18.Count; i++)
            {
                listBox18.Items.Add(temp18[i]);
            }
            for (int i = 0; i < temp19.Count; i++)
            {
                listBox19.Items.Add(temp19[i]);
            }
            for (int i = 0; i < temp20.Count; i++)
            {
                listBox20.Items.Add(temp20[i]);
            }
            for (int i = 0; i < temp21.Count; i++)
            {
                listBox21.Items.Add(temp21[i]);
            }
            SaveData();
            ViewUpdate();
        }

        private bool CheckRechten()
        {
            bool ret = false;
            if (ProgData.RechtenHuidigeGebruiker > 25) ret = true;

            if (ProgData.RechtenHuidigeGebruiker == 25 && ProgData.Huidige_Gebruiker_Werkt_Op_Kleur == ProgData.GekozenKleur) ret = true;

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

                listBox2.Visible = listBox22.Visible = bool.Parse(InstellingenProg.ProgrammaData[21]);
                listBox3.Visible = listBox23.Visible = bool.Parse(InstellingenProg.ProgrammaData[22]);
                listBox4.Visible = listBox24.Visible = bool.Parse(InstellingenProg.ProgrammaData[23]);
                listBox5.Visible = listBox25.Visible = bool.Parse(InstellingenProg.ProgrammaData[24]);
                listBox6.Visible = listBox26.Visible = bool.Parse(InstellingenProg.ProgrammaData[25]);
                listBox7.Visible = listBox27.Visible = bool.Parse(InstellingenProg.ProgrammaData[26]);
                listBox8.Visible = listBox28.Visible = bool.Parse(InstellingenProg.ProgrammaData[27]);
                listBox9.Visible = listBox29.Visible = bool.Parse(InstellingenProg.ProgrammaData[28]);
                listBox10.Visible = listBox30.Visible = bool.Parse(InstellingenProg.ProgrammaData[29]);
                listBox11.Visible = listBox31.Visible = bool.Parse(InstellingenProg.ProgrammaData[30]);
                listBox12.Visible = listBox32.Visible = bool.Parse(InstellingenProg.ProgrammaData[31]);
                listBox13.Visible = listBox33.Visible = bool.Parse(InstellingenProg.ProgrammaData[32]);
                listBox14.Visible = listBox34.Visible = bool.Parse(InstellingenProg.ProgrammaData[33]);
                listBox15.Visible = listBox35.Visible = bool.Parse(InstellingenProg.ProgrammaData[34]);
                listBox16.Visible = listBox36.Visible = bool.Parse(InstellingenProg.ProgrammaData[35]);
                listBox17.Visible = listBox37.Visible = bool.Parse(InstellingenProg.ProgrammaData[36]);
                listBox18.Visible = listBox38.Visible = bool.Parse(InstellingenProg.ProgrammaData[37]);
                listBox19.Visible = listBox39.Visible = bool.Parse(InstellingenProg.ProgrammaData[38]);
                listBox20.Visible = listBox40.Visible = bool.Parse(InstellingenProg.ProgrammaData[39]);
                listBox21.Visible = listBox41.Visible = bool.Parse(InstellingenProg.ProgrammaData[40]);

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
            catch { }
        }

        private void listBox1_MouseLeave(object sender, EventArgs e)
        {
            sourse = (ListBox)sender;
            if (sourse.SelectedIndex > -1)
                sourse.SelectedIndex = -1;
        }

        private void buttonNu_Click(object sender, EventArgs e)
        {
            SaveData();
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
    }
}