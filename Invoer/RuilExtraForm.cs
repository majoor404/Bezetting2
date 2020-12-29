using Bezetting2.Data;
using System;
using System.Linq;
using System.Windows.Forms;
using static Bezetting2.DatumVijfPloegUtils;

namespace Bezetting2.Invoer
{
    public partial class RuilExtraForm : Form
    {
        public RuilExtraForm()
        {
            InitializeComponent();
        }

        private void RuilExtraForm_Shown(object sender, EventArgs e)
        {
            LaadGevraagdeDiensten();

            // laad werkplekken
            comboBoxWerkplek.Items.Clear();
            for (int i = 1; i < 21; i++)
            {
                if (bool.Parse(InstellingenProg.ProgrammaData[i + 20]))
                    comboBoxWerkplek.Items.Add(InstellingenProg.ProgrammaData[i]);
            }

            comboBoxPloeg.SelectedIndex = 0;
            //comboBoxPloeg.Text = ProgData.GekozenKleur;
            //comboBoxPloeg.SelectedIndex = Enum.GetValues(typeof(ProgData.Kleur));

            buttonVulDienst.Enabled = ProgData.RechtenHuidigeGebruiker > 24;

            textBoxAanvraagVoor.Text = "";
            textBoxAanvraagVoor.Focus();
        }

        private void MonthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            // bepaal afhankelijk van datum en ploeg kleur dienst
            comboBoxDienst.Text = GetDienstLong(ProgData.GekozenRooster(), monthCalendar1.SelectionStart, comboBoxPloeg.Text);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool ok = true;

            int vanploeg = 0;
            if (checkBoxBL.Checked)
                vanploeg += 1;
            if (checkBoxWI.Checked)
                vanploeg += 2;
            if (checkBoxGE.Checked)
                vanploeg += 4;
            if (checkBoxGR.Checked)
                vanploeg += 8;
            if (checkBoxRO.Checked)
                vanploeg += 16;
            // vraag aan
            if (string.IsNullOrEmpty(comboBoxPloeg.Text))
            {
                MessageBox.Show("Geen ploeg gekozen");
                ok = false;
            }
            if (string.IsNullOrEmpty(comboBoxDienst.Text))
            {
                MessageBox.Show("Geen Dienst gekozen");
                ok = false;
            }
            if (string.IsNullOrEmpty(comboBoxWerkplek.Text))
            {
                MessageBox.Show("Geen ploeg gekozen");
                ok = false;
            }
            if (string.IsNullOrEmpty(textBoxAanvraagVoor.Text))
            {
                MessageBox.Show("Geen aanvraag voor gekozen");
                ok = false;
            }
            if (vanploeg < 1)
            {
                MessageBox.Show("Geen ploeg gekozen die ruil kan maken");
                ok = false;
            }

            if (ok)
            {
                AanvraagRuilExtra re = new AanvraagRuilExtra
                {
                    _naamAanvraagDoor = labelNaam.Text,
                    _naamAanvraagVoor = textBoxAanvraagVoor.Text,
                    _extra = radioButton1.Checked,
                    _datum = monthCalendar1.SelectionStart,
                    _dienst = comboBoxDienst.Text,
                    _werkplek = comboBoxWerkplek.Text,
                    _ploeg = comboBoxPloeg.Text,
                    _vanploeg = vanploeg
                };

                ProgData.LoadExtraRuilLijst(re._datum.Year.ToString() + "\\" + re._datum.Month.ToString());
                ProgData.ListAanvraagRuilExtra.Add(re);
                ProgData.SaveExtraRuilLijst(re._datum.Year.ToString() + "\\" + re._datum.Month.ToString());
                RuilExtraForm_Shown(this, null);
            }
        }

        private void LaadGevraagdeDiensten()
        {
            listViewExtra.Items.Clear();
            string[] info = new string[9];
            DateTime nu = DateTime.Now;

            nu = nu.AddMonths(-1); // vorige maand

            for (int i = 0; i < 10; i++)
            {
                LaadGevraagdeDienstenMaand(info, nu);
                nu = nu.AddMonths(1); // huidige maand
            }
        }

        private void LaadGevraagdeDienstenMaand(string[] info, DateTime nu)
        {
            ProgData.LoadExtraRuilLijst(nu.Year.ToString() + "\\" + nu.Month.ToString());
            foreach (AanvraagRuilExtra a in ProgData.ListAanvraagRuilExtra)
            {
                info[0] = a._naamAanvraagDoor;
                info[1] = a._naamAanvraagVoor;
                info[2] = a._extra ? "Extra" : "Ruil";
                info[3] = a._ploeg;
                info[4] = a._datum.ToString("dd/MM/yyyy");
                info[5] = a._dienst;
                info[6] = a._werkplek;

                int blauw = a._vanploeg & 1;
                int wit = a._vanploeg & 2;
                int geel = a._vanploeg & 4;
                int groen = a._vanploeg & 8;
                int rood = a._vanploeg & 16;

                info[7] = "";
                if (blauw > 0) info[7] = "BL";
                if (wit > 0) info[7] += " WI";
                if (geel > 0) info[7] += " GE";
                if (groen > 0) info[7] += " GR";
                if (rood > 0) info[7] += " RO";

                info[8] = a._persoonLoopt;

                ListViewItem item_info = new ListViewItem(info);
                listViewExtra.Items.Add(item_info);
            }
        }

        private void ComboBoxPloeg_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBoxBL.Enabled = true;
            checkBoxWI.Enabled = true;
            checkBoxGE.Enabled = true;
            checkBoxGR.Enabled = true;
            checkBoxRO.Enabled = true;
            if (comboBoxPloeg.SelectedIndex == 0)
            {
                checkBoxBL.Checked = false;
                checkBoxBL.Enabled = false;
            }
            if (comboBoxPloeg.SelectedIndex == 1)
            {
                checkBoxWI.Checked = false;
                checkBoxWI.Enabled = false;
            }
            if (comboBoxPloeg.SelectedIndex == 2)
            {
                checkBoxGE.Checked = false;
                checkBoxGE.Enabled = false;
            }
            if (comboBoxPloeg.SelectedIndex == 3)
            {
                checkBoxGR.Checked = false;
                checkBoxGR.Enabled = false;
            }
            if (comboBoxPloeg.SelectedIndex == 4)
            {
                checkBoxRO.Checked = false;
                checkBoxRO.Enabled = false;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // vul dienst op
            if (listViewExtra.SelectedItems.Count > 0)
            {
                int index = listViewExtra.Items.IndexOf(listViewExtra.SelectedItems[0]);
                if (index > -1)
                {
                    InvoerLoopExtraDienst invl = new InvoerLoopExtraDienst();
                    invl.labelNaam.Text = listViewExtra.Items[index].SubItems[1].Text;
                    invl.labelDienst.Text = listViewExtra.Items[index].SubItems[5].Text;
                    invl.labelDatum.Text = listViewExtra.Items[index].SubItems[4].Text;
                    invl.labelKleur.Text = listViewExtra.Items[index].SubItems[3].Text;
                    DialogResult res = invl.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        string dat = invl.labelDatum.Text;
                        string jaar = dat.Substring(6, 4);
                        string maand = dat.Substring(3, 2);
                        if (maand.Substring(0, 1) == "0")
                            maand = maand.Substring(1, 1);
                        string dir = jaar + "\\" + maand;

                        ProgData.LoadExtraRuilLijst(dir);

                        try
                        {
                            AanvraagRuilExtra ver = ProgData.ListAanvraagRuilExtra.First(a => (a._naamAanvraagVoor == invl.labelNaam.Text) && (a._datum.ToString("dd/MM/yyyy") == dat));
                            ver._persoonLoopt = invl.textBoxLoopt.Text;
                            ProgData.SaveExtraRuilLijst(dir);
                        }
                        catch
                        {
                            MessageBox.Show("kon extra/ruil invulling niet opslaan");
                        }

                        RuilExtraForm_Shown(this, null);
                    }
                }
            }
        }

        /*
        private void Save()
        {
            try
            {
                File.WriteAllLines("extradiensten.ini", listBoxDiensten.Items.Cast<string>());
            }
            catch (IOException)
            {
                MessageBox.Show("extradiensten.ini save error");
            }
        }

        private void Laad()
        {
            try
            {
                string[] lines = File.ReadAllLines("extradiensten.ini");
                listBoxDiensten.Items.AddRange(lines.ToArray());
            }
            catch { }
        }*/
    }
}