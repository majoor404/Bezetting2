using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezetting2.InlogGebeuren
{
    public partial class RechtenInstellenForm : Form
    {
        public RechtenInstellenForm()
        {
            InitializeComponent();
        }

        private void checkBoxAllePloegen_CheckedChanged(object sender, EventArgs e)
        {
            int recht = GetRecht();
            if (checkBoxAllePloegen.Checked)
                recht += 50;
            labelRechtenNivo.Text = recht.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // reset wachtwoord (maak gelijk aan personeel nummer
            personeel persoon = ProgData.personeel_lijst.First(a => a._persnummer.ToString() == labelPersoneelNummer.Text);
            // encrypt pass
            persoon._passwoord = ProgData.Scramble("verander_nu");
            ProgData.Save_Namen_lijst();
            MessageBox.Show("Wachtwoord is gereset");
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

        private void radioButton0_CheckedChanged(object sender, EventArgs e)
        {
            int recht = GetRecht();
            if (checkBoxAllePloegen.Checked)
                recht += 50;
            labelRechtenNivo.Text = recht.ToString();
        }
    }
}
