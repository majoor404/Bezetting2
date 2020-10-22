using System;
using System.Windows.Forms;

namespace Bezetting2.Invoer
{
    public partial class ZetKleurLijnen : Form
    {
        public ZetKleurLijnen()
        {
            InitializeComponent();
        }

        private void ZetKleurLijnen_Shown(object sender, System.EventArgs e)
        {
            int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);
            numericUpDown2.Maximum = aantal_dagen_deze_maand;
            numericUpDown3.Maximum = aantal_dagen_deze_maand;
            numericUpDown5.Maximum = aantal_dagen_deze_maand;
            numericUpDown7.Maximum = aantal_dagen_deze_maand;

            if (ProgData.LeesLijnen())
            {
                // was een bestand
                checkBox1.Checked = bool.Parse(ProgData.Lijnen[0]);
                checkBox2.Checked = bool.Parse(ProgData.Lijnen[1]);
                checkBox3.Checked = bool.Parse(ProgData.Lijnen[2]);
                checkBox4.Checked = bool.Parse(ProgData.Lijnen[3]);
                textBox1.Text = ProgData.Lijnen[4];
                textBox2.Text = ProgData.Lijnen[5];
                textBox3.Text = ProgData.Lijnen[6];
                textBox4.Text = ProgData.Lijnen[7];
                numericUpDown1.Value = int.Parse(ProgData.Lijnen[8]);
                numericUpDown4.Value = int.Parse(ProgData.Lijnen[9]);
                numericUpDown6.Value = int.Parse(ProgData.Lijnen[10]);
                numericUpDown8.Value = int.Parse(ProgData.Lijnen[11]);
                numericUpDown2.Value = int.Parse(ProgData.Lijnen[12]);
                numericUpDown3.Value = int.Parse(ProgData.Lijnen[13]);
                numericUpDown5.Value = int.Parse(ProgData.Lijnen[14]);
                numericUpDown7.Value = int.Parse(ProgData.Lijnen[15]);
            }
            else
            {
                // nieuw
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                numericUpDown1.Value = 1;
                numericUpDown4.Value = 1;
                numericUpDown6.Value = 1;
                numericUpDown8.Value = 1;
                numericUpDown2.Value = aantal_dagen_deze_maand;
                numericUpDown3.Value = aantal_dagen_deze_maand;
                numericUpDown5.Value = aantal_dagen_deze_maand;
                numericUpDown7.Value = aantal_dagen_deze_maand;
            }
        }

        // close en save
        private void button1_Click(object sender, EventArgs e)
        {
            // save data
            ProgData.Lijnen.Clear();
            ProgData.Lijnen.Add(checkBox1.Checked.ToString());
            ProgData.Lijnen.Add(checkBox2.Checked.ToString());
            ProgData.Lijnen.Add(checkBox3.Checked.ToString());
            ProgData.Lijnen.Add(checkBox4.Checked.ToString());

            ProgData.Lijnen.Add(textBox1.Text);
            ProgData.Lijnen.Add(textBox2.Text);
            ProgData.Lijnen.Add(textBox3.Text);
            ProgData.Lijnen.Add(textBox4.Text);

            ProgData.Lijnen.Add(numericUpDown1.Value.ToString());
            ProgData.Lijnen.Add(numericUpDown4.Value.ToString());
            ProgData.Lijnen.Add(numericUpDown6.Value.ToString());
            ProgData.Lijnen.Add(numericUpDown8.Value.ToString());

            ProgData.Lijnen.Add(numericUpDown2.Value.ToString());
            ProgData.Lijnen.Add(numericUpDown3.Value.ToString());
            ProgData.Lijnen.Add(numericUpDown5.Value.ToString());
            ProgData.Lijnen.Add(numericUpDown7.Value.ToString());

            ProgData.SaveLijnen();
        }
    }
}