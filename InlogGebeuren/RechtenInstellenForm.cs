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
            if (checkBoxAllePloegen.Checked)
                recht += 50;
            labelRechtenNivo.Text = recht.ToString();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // reset wachtwoord (maak gelijk aan personeel nummer
            personeel persoon = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == labelPersoneelNummer.Text);
            // encrypt pass
            persoon._passwoord = ProgData.Scramble("verander_nu");
            ProgData.Save_Namen_lijst();
        }

        private int GetRecht()
        {
            if (radioButton50.Checked)
                return 50;
            if (radioButton25.Checked)
                return 25;
            if (radioButton0.Checked)
                return 0;
            return 0;
        }

        private void RadioButton0_CheckedChanged(object sender, EventArgs e)
        {
            int recht = GetRecht();
            if (checkBoxAllePloegen.Checked)
                recht += 50;
            labelRechtenNivo.Text = recht.ToString();
        }
    }
}