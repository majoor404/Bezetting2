using Bezetting2.Data;
using System;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class HistoryForm : Form
    {
        public HistoryForm()
        {
            InitializeComponent();
        }

        private void History_Shown(object sender, EventArgs e)
        {
            ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur);

            int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);
            comboBoxDag.Items.Clear();
            comboBoxDag.Items.Add("");

            for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
            {
                comboBoxDag.Items.Add(i.ToString());
            }
            buttonFilter_Click(this, null);
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur);
            listViewHis.Items.Clear();
            string[] regel = new string[6];
            foreach (veranderingen a in ProgData.ListVeranderingen)
            {
                if (comboBoxDag.Text == a._datumafwijking || string.IsNullOrEmpty(comboBoxDag.Text))
                {
                    regel[1] = a._afwijking;
                    regel[2] = a._datumafwijking;
                    regel[4] = a._datuminvoer;
                    regel[3] = ProgData.Get_Gebruiker_Naam(a._invoerdoor);
                    regel[0] = a._naam;
                    regel[5] = a._rede;
                    ListViewItem listItem = new ListViewItem(regel);
                    listViewHis.Items.Add(listItem);
                }
            }
        }

        private void comboBoxDag_TextChanged(object sender, EventArgs e)
        {
            buttonFilter_Click(this, null);
        }
    }
}