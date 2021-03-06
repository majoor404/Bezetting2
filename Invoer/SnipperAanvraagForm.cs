﻿using Bezetting2.Data;
using System;
using System.Linq;
using System.Windows.Forms;
using static Bezetting2.DatumVijfPloegUtils;

namespace Bezetting2.Invoer
{
    public partial class SnipperAanvraagForm : Form
    {
        private ListViewColumnSorter lvwColumnSorter;

        public SnipperAanvraagForm()
        {
            InitializeComponent();
            lvwColumnSorter = new ListViewColumnSorter();
            this.listViewSnipper.ListViewItemSorter = lvwColumnSorter;
        }

        private void SnipperAanvraagForm_Shown(object sender, EventArgs e)
        {
            buttonVraagAan.Enabled = labelNaam.Text != "Niemand Ingelogd";
            LaadGevraagdeSnipper();
            buttonKeurGoed.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
            DateTimePicker1_ValueChanged(this, null);
            comboBox1.Text = "";
            textBoxRede.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        private void ButtonVraagAan_Click(object sender, EventArgs e)
        {
            SnipperAanvraag snip = new SnipperAanvraag();

            if (string.IsNullOrEmpty(labelDienst.Text))
            {
                MessageBox.Show("Deze dag bent u al vrij!");
            }
            else
            {
                snip._datum = dateTimePicker1.Value.Date;
                snip._dienst = labelDienst.Text;
                snip._hoe = comboBox1.Text;
                snip._naam = labelNaamFull.Text;
                snip._rede = textBoxRede.Text;
                snip._kleur = comboBoxKleur.Text;
                snip._persnr = ProgData.Get_Gebruiker_Nummer(snip._naam);

                ProgData.LoadSnipperLijst(snip._datum.Year.ToString() + "\\" + snip._datum.Month.ToString());
                bool staat_die_al_in_lijst = false;
                foreach (SnipperAanvraag a in ProgData.ListSnipperAanvraag)
                {
                    if (a._datum.Date == snip._datum.Date
                       && a._naam == snip._naam &&
                       a._rede_coordinator != "Zelf Gecanceld")
                        staat_die_al_in_lijst = true;
                }

                if (!staat_die_al_in_lijst && !string.IsNullOrEmpty(comboBox1.Text))
                {
                    ProgData.ListSnipperAanvraag.Add(snip);

                    ProgData.SaveSnipperLijst(snip._datum.Year.ToString() + "\\" + snip._datum.Month.ToString());
                }
                else
                {
                    MessageBox.Show("staat al in de lijst of niet volledig ingevuld");
                }
                SnipperAanvraagForm_Shown(this, null);
            }
        }

        private void LaadGevraagdeSnipper()
        {
            listViewSnipper.Items.Clear();
            string[] info = new string[8];
            DateTime nu = DateTime.Now;

            nu = nu.AddMonths(-1); // vorige maand

            for (int i = 0; i < 10; i++)
            {
                LaadGevraagdeSnipperMaand(info, nu);
                nu = nu.AddMonths(1); // huidige maand
            }
        }

        private void LaadGevraagdeSnipperMaand(string[] info, DateTime nu)
        {
            ProgData.LoadSnipperLijst(nu.Year.ToString() + "\\" + nu.Month.ToString());
            foreach (SnipperAanvraag a in ProgData.ListSnipperAanvraag)
            {
                info[0] = a._datum.ToString("dd/MM/yyyy");
                info[1] = a._naam;
                info[2] = a._kleur;
                info[3] = a._dienst;
                info[4] = a._hoe;
                info[5] = a._rede;
                info[6] = a._Coorcinator;
                info[7] = a._rede_coordinator;

                if (!checkBoxVerbergIngevulde.Checked)
                {
                    ListViewItem item_info = new ListViewItem(info);
                    listViewSnipper.Items.Add(item_info);
                }
                else
                {
                    if (info[7] == null)
                    {
                        ListViewItem item_info = new ListViewItem(info);
                        listViewSnipper.Items.Add(item_info);
                    }
                }
            }
        }

        private void ButtonKeurGoed_Click(object sender, EventArgs e)
        {
            if (listViewSnipper.SelectedItems.Count > 0)
            {
                int index = listViewSnipper.Items.IndexOf(listViewSnipper.SelectedItems[0]);
                if (index > -1)
                {
                    string dat = listViewSnipper.Items[index].SubItems[0].Text;

                    string dir = ProgData.GetDirectoryBezettingMaand(dat);
                    ProgData.LoadSnipperLijst(dir);

                    string naam = listViewSnipper.Items[index].SubItems[1].Text;
                    string datum = listViewSnipper.Items[index].SubItems[0].Text;
                    try
                    {
                        SnipperAanvraag ver = ProgData.ListSnipperAanvraag.First(a => (a._naam == naam) && (a._datum.ToString("dd/MM/yyyy") == datum));
                        ver._Coorcinator = labelNaamFull.Text;
                        ver._rede_coordinator = "Goed Gekeurd";
                        ProgData.SaveSnipperLijst(dir);
                        // invoer in maand overzicht
                        DialogResult dialogResult = MessageBox.Show("Invoeren in Bezetting programma?", "Vraagje", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes)
                        {
                            VulAanvraagInForm invul = new VulAanvraagInForm();

                            invul.labelNaam.Text = listViewSnipper.Items[index].SubItems[1].Text;
                            invul.labelKleur.Text = listViewSnipper.Items[index].SubItems[2].Text;
                            invul.labelDienst.Text = listViewSnipper.Items[index].SubItems[3].Text;
                            invul.labelDatum.Text = listViewSnipper.Items[index].SubItems[0].Text;
                            invul.textBoxRede.Text = listViewSnipper.Items[index].SubItems[5].Text;
                            invul.textBoxAfwijking.Text = listViewSnipper.Items[index].SubItems[4].Text;

                            invul.ShowDialog();

                            ProgData.RegelAfwijkingOpDatumEnKleur(ver._datum, ver._kleur, ver._persnr, ver._datum.Day.ToString(), invul.textBoxAfwijking.Text, invul.textBoxRede.Text, $"via aanvraag programma {labelNaamFull.Text}");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("kon goed keuring niet opslaan");
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een item");
            }
            SnipperAanvraagForm_Shown(this, null);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (listViewSnipper.SelectedItems.Count > 0)
            {
                int index = listViewSnipper.Items.IndexOf(listViewSnipper.SelectedItems[0]);
                if (index > -1)
                {
                    // haal juiste data
                    string dat = listViewSnipper.Items[index].SubItems[0].Text;
                    string dir = ProgData.GetDirectoryBezettingMaand(dat);
                    ProgData.LoadSnipperLijst(dir);

                    if (listViewSnipper.Items[index].SubItems[1].Text == labelNaamFull.Text)
                    {
                        string naam = listViewSnipper.Items[index].SubItems[1].Text;
                        string datum = listViewSnipper.Items[index].SubItems[0].Text;
                        // eigen verzoek cancel
                        try
                        {
                            SnipperAanvraag ver = ProgData.ListSnipperAanvraag.First(a => (a._naam == naam) && (a._datum.ToString("dd/MM/yyyy") == datum));
                            if (string.IsNullOrEmpty(ver._rede_coordinator))
                            {
                                ver._Coorcinator = labelNaamFull.Text;
                                ver._rede_coordinator = "Zelf Gecanceld";
                                ProgData.SaveSnipperLijst(dir);
                            }
                            else
                            {
                                MessageBox.Show("Coordinator heeft hem al geaccodeerd, kan niks aanpassen");
                            }
                        }
                        catch
                        {
                            MessageBox.Show("kon niet cancelen");
                        }

                        //SnipperAanvraagForm_Shown(this, null);
                    }
                    else
                    {
                        // afgekeurd 
                        string naam = listViewSnipper.Items[index].SubItems[1].Text;
                        string datum = listViewSnipper.Items[index].SubItems[0].Text;
                        //

                        if (ProgData.RechtenHuidigeGebruiker > 1)
                        {
                            try
                            {
                                SnipperAanvraag ver = ProgData.ListSnipperAanvraag.First(a => (a._naam == naam) && (a._datum.ToString("dd/MM/yyyy") == datum));
                                ver._Coorcinator = labelNaamFull.Text;
                                ver._rede_coordinator = "Afgekeurd";
                                ProgData.SaveSnipperLijst(dir);

                                MessageBox.Show("Als hij eerst was goedgekeurd,\n" +
                                    "kan deze staan in maand overzicht,\n" +
                                    "even bekijken en anders daar verwijderen!");
                            }
                            catch
                            {
                                MessageBox.Show("afkeur niet gelukt");
                            }
                        }
                        else // rechten is 1, dus alleen cancel als het je eigen naam is!
                        {
                            if(naam == ProgData.Huidige_Gebruiker_Naam())
                            {
                                SnipperAanvraag ver = ProgData.ListSnipperAanvraag.First(a => (a._naam == naam) && (a._datum.ToString("dd/MM/yyyy") == datum));
                                ver._Coorcinator = labelNaamFull.Text;
                                ver._rede_coordinator = "Zelf Gecanceld";
                                ProgData.SaveSnipperLijst(dir);
                            }
                            else
                            {
                                MessageBox.Show("Alleen je eigen aanvraag kan je verwijderen");
                            }
                        }
                    }
                }
            }
            SnipperAanvraagForm_Shown(this, null);
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            labelDienst.Text = GetDienstLong(ProgData.GekozenRooster(), dateTimePicker1.Value, comboBoxKleur.Text);
        }

        private void checkBoxVerbergIngevulde_CheckedChanged(object sender, EventArgs e)
        {
            SnipperAanvraagForm_Shown(this, null);
        }

        private void listViewSnipper_ColumnClick(object sender, ColumnClickEventArgs e)
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
            this.listViewSnipper.Sort();
        }
    }
}