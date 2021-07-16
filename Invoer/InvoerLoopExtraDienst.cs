using System;
using System.Windows.Forms;

namespace Bezetting2.Invoer
{
    public partial class InvoerLoopExtraDienst : Form
    {
        public int gekozen_pers;
        public InvoerLoopExtraDienst()
        {
            InitializeComponent();
        }

        private void InvoerLoopExtraDienst_Shown(object sender, System.EventArgs e)
        {
            // vul namen in dropdown
            comboBoxNamen.Items.Clear();
            foreach (personeel a in ProgData.AlleMensen.LijstPersonen)
            {
                comboBoxNamen.Items.Add($"{a._achternaam,-25}{a._kleur,-10}{a._persnummer,-8}");
            }
            comboBoxNamen.Text = "";
        }

        private void comboBoxNamen_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string gekozen = comboBoxNamen.Text;
            if (gekozen.Length > 30)
            {
                gekozen = gekozen.Substring(35, 6); // personeel nummer
            }
            gekozen_pers = Int32.Parse(gekozen);
            buttonInvoer.Focus();
        }
    }
}