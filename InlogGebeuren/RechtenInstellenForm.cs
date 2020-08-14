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

        private void trackBarRechten_ValueChanged(object sender, EventArgs e)
        {
            int recht = (int)trackBarRechten.Value;
            if (checkBoxAllePloegen.Checked)
                recht += 50;
            labelRechtenNivo.Text = recht.ToString();
        }

        private void checkBoxAllePloegen_CheckedChanged(object sender, EventArgs e)
        {
            int recht = (int)trackBarRechten.Value;
            if (checkBoxAllePloegen.Checked)
                recht += 50;
            labelRechtenNivo.Text = recht.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // reset wachtwoord (maak gelijk aan personeel nummer
            personeel persoon = ProgData.personeel_lijst.First(a => a._persnummer.ToString() == labelPersoneelNummer.Text);
            // encrypt pass
            persoon._passwoord = ProgData.Scramble(labelPersoneelNummer.Text);
            ProgData.Save_Namen_lijst();
            MessageBox.Show("Wachtwoord is gereset naar personeel nummer");

        }
    }
}
