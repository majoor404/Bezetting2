using Bezetting2.Data;
using System;
using System.Windows.Forms;
using static Bezetting2.Data.MaandDataClass;

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
            ProgData.MaandData.Load(ProgData.GekozenKleur);

            int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.igekozenjaar, ProgData.igekozenmaand);
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
            ProgData.MaandData.Load(ProgData.GekozenKleur);

            listViewHis.Items.Clear();
            string[] regel = new string[6];
            
            foreach (Item a in ProgData.MaandData.MaandDataLijst)
            {
                var dat = a.datum_.Day.ToString();
                if (comboBoxDag.Text == dat || string.IsNullOrEmpty(comboBoxDag.Text))
                {
                    regel[1] = a.afwijking_;
                    regel[2] = a.datum_.ToShortDateString();
                    regel[4] = a.ingevoerd_op_.ToShortDateString(); ;
                    regel[3] = a.invoerdoor_;
                    regel[0] = ProgData.Get_Gebruiker_Naam(a.personeel_nr_);
                    regel[5] = a.rede_;
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