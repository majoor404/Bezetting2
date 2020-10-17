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
            ProgData.LoadVeranderingenPloeg();
            comboBoxIngevoerdDoor.Items.Clear();
            comboBoxIngevoerdDoor.Items.Add("");
            listViewHis.Items.Clear();
            string[] regel = new string[6];
            foreach (veranderingen a in ProgData.Veranderingen_Lijst)
            {
                if (comboBoxDag.Text == a._datumafwijking || comboBoxIngevoerdDoor.Text == a._invoerdoor)
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
                if (!comboBoxIngevoerdDoor.Items.Contains(ProgData.Get_Gebruiker_Naam(a._invoerdoor)))
                    comboBoxIngevoerdDoor.Items.Add(ProgData.Get_Gebruiker_Naam(a._invoerdoor));
            }

            int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);
            comboBoxDag.Items.Clear();
            comboBoxDag.Items.Add("");

            for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
            {
                comboBoxDag.Items.Add(i.ToString());
            }
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            ProgData.LoadVeranderingenPloeg();
            listViewHis.Items.Clear();
            string[] regel = new string[6];
            foreach (veranderingen a in ProgData.Veranderingen_Lijst)
            {
                if (comboBoxDag.Text == a._datumafwijking || 
                    comboBoxIngevoerdDoor.Text == a._invoerdoor ||
                    comboBoxDag.Text == "")
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
    }
}