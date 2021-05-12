using System;
using System.Linq;
using System.Windows.Forms;

namespace Bezetting2.InlogGebeuren
{
    public partial class RechtenInstellenForm : Form
    {
        public RechtenInstellenForm()
        {
            InitializeComponent();
        }

        private void CheckBoxAllePloegen_CheckedChanged(object sender, EventArgs e)
        {
            int recht = GetRecht();
            labelRechtenNivo.Text = recht.ToString();
            panel25.Visible = GetRecht() > 24 && GetRecht() < 28;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // reset wachtwoord (maak gelijk aan personeel nummer
            personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == labelPersoneelNummer.Text);
            // encrypt pass
            persoon._passwoord = ProgData.Scramble("verander_nu");
            //ProgData.Save_LijstNamen();
            ProgData.AlleMensen.Save();
        }

        private int GetRecht()
        {
            int ret = 0;
            if (radioButton50.Checked)
                ret = 50;
            if (radioButton25.Checked)
                ret = 25;
            if (radioButton0.Checked)
                ret = 0;
            if (checkBoxAllePloegen.Checked)
                ret += 50;
            if(ret == 25)
            {
                if (checkBoxAlleenZelf.Checked)
                    ret = 26;
                if (checkBoxAlleenAndere.Checked)
                    ret = 27;
            }
            return ret;
        }

        private void RadioButton0_CheckedChanged(object sender, EventArgs e)
        {
            int recht = GetRecht();
            labelRechtenNivo.Text = recht.ToString();
            panel25.Visible = GetRecht() > 24 && GetRecht() < 28;
        }

        //save button
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                personeel persoon = ProgData.AlleMensen.LijstPersonen.First(a => a._persnummer.ToString() == labelPersoneelNummer.Text);

                if (GetRecht() > 0 && string.IsNullOrEmpty(persoon._passwoord))
                    Button2_Click(this, null); // = reset wachtwoord
            }
            catch
            {
                MessageBox.Show("Persoon niet gevonden, eerst saven voordat rechten worden ingesteld.");
            }
        }

        private void RechtenInstellenForm_Shown(object sender, EventArgs e)
        {
            int rechten = int.Parse(labelRechtenNivo.Text);
            if (rechten > 51)
                checkBoxAllePloegen.Checked = true;
            if (rechten == 0)
                radioButton0.Checked = true;
            if (rechten > 24)
                radioButton25.Checked = true;
            if (rechten > 49)
                radioButton50.Checked = true;

            panel25.Visible = GetRecht() > 24 && GetRecht() < 28;

            if (rechten == 26)
                checkBoxAlleenZelf.Checked = true;
            if (rechten == 27)
                checkBoxAlleenAndere.Checked = true;

            if (ProgData.RechtenHuidigeGebruiker == 25)
            {
                checkBoxAllePloegen.Enabled = false;
                radioButton0.Enabled = true;
                radioButton25.Enabled = true;
                radioButton50.Enabled = false;
            }
            if (ProgData.RechtenHuidigeGebruiker == 50)
            {
                checkBoxAllePloegen.Enabled = false;
                radioButton0.Enabled = true;
                radioButton25.Enabled = true;
                radioButton50.Enabled = true;
            }
        }

        private void checkBoxAlleenZelf_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxAlleenAndere.Checked && checkBoxAlleenZelf.Checked)
                checkBoxAlleenAndere.Checked = false;
            int recht = GetRecht();
            labelRechtenNivo.Text = recht.ToString();
            panel25.Visible = GetRecht() > 24 && GetRecht() < 28;
        }

        private void checkBoxAlleenAndere_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAlleenAndere.Checked && checkBoxAlleenZelf.Checked)
                checkBoxAlleenZelf.Checked = false;
            int recht = GetRecht();
            labelRechtenNivo.Text = recht.ToString();
            panel25.Visible = GetRecht() > 24 && GetRecht() < 28;
        }
    }
}