using Bezetting2.Data;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Bezetting2.Invoer
{
    public partial class SnipperAanvraagForm : Form
    {
        public SnipperAanvraagForm()
        {
            InitializeComponent();
        }

        private void SnipperAanvraagForm_Shown(object sender, EventArgs e)
        {
            buttonVraagAan.Enabled = labelNaam.Text != "Niemand Ingelogd";
            laadGevraagdeSnipper();
            buttonKeurGoed.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
            dateTimePicker1_ValueChanged(this, null);
            comboBox1.Text = "";
            textBoxRede.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        private void buttonVraagAan_Click(object sender, EventArgs e)
        {
            SnipperAanvraag snip = new SnipperAanvraag();

            if (labelDienst.Text == "")
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

                ProgData.LoadSnipperLijst(snip._datum.Year.ToString() + "\\" + snip._datum.Month.ToString());
                bool staat_die_al_in_lijst = false;
                foreach (SnipperAanvraag a in ProgData.ListSnipperAanvraag)
                {
                    if (a._datum.Date == snip._datum.Date
                       && a._naam == snip._naam &&
                       a._rede_coordinator != "Zelf Gecanceld")
                        staat_die_al_in_lijst = true;
                }
                
                if (!staat_die_al_in_lijst && comboBox1.Text != "")
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

        private void laadGevraagdeSnipper()
        {
            listViewSnipper.Items.Clear();
            string[] info = new string[8];
            DateTime nu = DateTime.Now;

            nu = nu.AddMonths(-1); // vorige maand

            for (int i = 0; i < 10; i++)
            {
                laadGevraagdeSnipperMaand(info, nu);
                nu = nu.AddMonths(1); // huidige maand
            }
        }


        void laadGevraagdeSnipperMaand(string[] info, DateTime nu)
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

                ListViewItem item_info = new ListViewItem(info);
                listViewSnipper.Items.Add(item_info);
            }
        }

        private void buttonKeurGoed_Click(object sender, EventArgs e)
        {
            if (listViewSnipper.SelectedItems.Count > 0)
            {

                int index = listViewSnipper.Items.IndexOf(listViewSnipper.SelectedItems[0]);
                if (index > -1)
                {
                    string dat = listViewSnipper.Items[index].SubItems[0].Text;
                    
                    //string jaar = dat.Substring(6, 4);
                    //string maand = dat.Substring(3, 2);
                    //if (maand.Substring(0, 1) == "0")
                    //    maand = maand.Substring(1, 1);

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
                    }
                    catch
                    {
                        MessageBox.Show("kon goed keuring niet opslaan");
                    }
                    SnipperAanvraagForm_Shown(this, null);
                }
            }
            else
            {
                MessageBox.Show("Selecteer eerst een item");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (listViewSnipper.SelectedItems.Count > 0)
            {
                int index = listViewSnipper.Items.IndexOf(listViewSnipper.SelectedItems[0]);
                if (index > -1)
                {
                    // haal juiste data
                    string dat = listViewSnipper.Items[index].SubItems[0].Text;
                    //string jaar = dat.Substring(6, 4);
                    //string maand = dat.Substring(3, 2);
                    //if (maand.Substring(0, 1) == "0")
                    //    maand = maand.Substring(1, 1);
                    //string dir = jaar + "\\" + maand;
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
                            if (ver._rede_coordinator != "")
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

                        SnipperAanvraagForm_Shown(this, null);
                    }
                    else
                    {
                        // afgekeurd
                        string naam = listViewSnipper.Items[index].SubItems[1].Text;
                        string datum = listViewSnipper.Items[index].SubItems[0].Text;
                        // 
                        try
                        {
                            SnipperAanvraag ver = ProgData.ListSnipperAanvraag.First(a => (a._naam == naam) && (a._datum.ToString("dd/MM/yyyy") == datum));
                            ver._Coorcinator = labelNaamFull.Text;
                            ver._rede_coordinator = "Afgekeurt";
                            ProgData.SaveSnipperLijst(dir);
                        }
                        catch
                        {
                            MessageBox.Show("afkeur niet gelukt");
                        }
                    }
                }
            }
        }
        
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            labelDienst.Text = ProgData.MDatum.GetDienstLong(ProgData.GekozenRooster(), dateTimePicker1.Value, comboBoxKleur.Text);
        }
    }
}
