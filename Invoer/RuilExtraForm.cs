using Bezetting2.Data;
using System;
using System.Linq;
using System.Windows.Forms;
using static Bezetting2.DatumVijfPloegUtils;

namespace Bezetting2.Invoer
{
    public partial class RuilExtraForm : Form
    {
        private ListViewColumnSorter lvwColumnSorter;
        public RuilExtraForm()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listViewExtra.ListViewItemSorter = lvwColumnSorter;
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

            buttonVulDienst.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
            buttonAanvraagIntrekken.Enabled = ProgData.RechtenHuidigeGebruiker > 24;

            label6.Visible = textBoxAanvraagVoor.Visible = !radioButton1.Checked;
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
            if (string.IsNullOrEmpty(textBoxAanvraagVoor.Text) && !radioButton1.Checked)
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
                bool toevoegen = true; // standaard

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

                if (checkBoxVerbergOudeVragen.Checked && a._datum.AddDays(1) < DateTime.Now)
                    toevoegen = false;
                if (radioButton4.Checked && info[2] != "Ruil") // ruil
                    toevoegen = false;
                if (radioButton5.Checked && info[2] != "Extra") // Extra
                    toevoegen = false;
                if (!string.IsNullOrEmpty(info[8]))     // ingevuld of gecanceld
                {
                    string dummy = info[8].Substring(0, 5);
                    if (checkBoxVerbergIngevulde.Checked && dummy != "Zelf ")
                        toevoegen = false;
                    if (checkBoxVerbergCancel.Checked && dummy == "Zelf ")
                        toevoegen = false;
                }

                if (toevoegen) // ruil
                {
                    ListViewItem item_info = new ListViewItem(info);
                    listViewExtra.Items.Add(item_info);
                }
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

        // vul dienst in.
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

        private void listViewExtra_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listViewExtra.Sort();
        }

        private void checkBoxVerbergOudeVragen_CheckedChanged(object sender, EventArgs e)
        {
            LaadGevraagdeDiensten();
        }

        private void buttonAanvraagIntrekken_Click(object sender, EventArgs e)
        {
            if (listViewExtra.SelectedItems.Count > 0)
            {
                int index = listViewExtra.Items.IndexOf(listViewExtra.SelectedItems[0]);
                if (index > -1)
                {
                    string dat = listViewExtra.Items[index].SubItems[4].Text;
                    string jaar = dat.Substring(6, 4);
                    string maand = dat.Substring(3, 2);
                    if (maand.Substring(0, 1) == "0")
                        maand = maand.Substring(1, 1);
                    string dir = jaar + "\\" + maand;

                    ProgData.LoadExtraRuilLijst(dir);

                    try
                    {
                        AanvraagRuilExtra ver;
                        if (listViewExtra.Items[index].SubItems[2].Text == "Ruil")
                        {
                            ver = ProgData.ListAanvraagRuilExtra.First(a => (a._naamAanvraagVoor == listViewExtra.Items[index].SubItems[1].Text) && (a._datum.ToString("dd/MM/yyyy") == dat));
                        }
                        else
                        {
                            ver = ProgData.ListAanvraagRuilExtra.First(a => (a._werkplek == listViewExtra.Items[index].SubItems[6].Text) && (a._ploeg == listViewExtra.Items[index].SubItems[3].Text) && (a._datum.ToString("dd/MM/yyyy") == dat));
                        }
                        if (listViewExtra.Items[index].SubItems[0].Text == ProgData.Huidige_Gebruiker_Naam())
                        {
                            ver._persoonLoopt = $"Zelf ingetrokken, Niet meer nodig, Ingetrokken door {listViewExtra.Items[index].SubItems[0].Text}";
                        }
                        else
                        {
                            ver._persoonLoopt = $"Niet meer nodig, Ingetrokken door {listViewExtra.Items[index].SubItems[0].Text}";
                        }
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

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label6.Visible = textBoxAanvraagVoor.Visible = !radioButton1.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
            {
                listViewExtra.Columns[0].Width = 0;
                listViewExtra.Columns[1].Width = 0;
            }
            else
            {
                listViewExtra.Columns[0].Width = 110;
                listViewExtra.Columns[1].Width = 110;
            }
            RuilExtraForm_Shown(this, null);
        }

        private void checkBoxVerbergIngevulde_CheckedChanged(object sender, EventArgs e)
        {
            RuilExtraForm_Shown(this, null);
        }

        private void checkBoxVerbergCancel_CheckedChanged(object sender, EventArgs e)
        {
            RuilExtraForm_Shown(this, null);
        }
    }
}
