using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class History : Form
    {
        public History()
        {
            InitializeComponent();
        }

        private void History_Shown(object sender, EventArgs e)
        {
            ProgData.HaalVeranderList();
            listViewHis.Items.Clear();
            string[] regel = new string[5];
            foreach (veranderingen a in ProgData.verander_lijst)
            {
                regel[1] = a._afwijking;
                regel[2] = a._datumafwijking;
                regel[4] = a._datuminvoer;
                regel[3] = a._invoerdoor;
                regel[0] = a._naam;
                ListViewItem listItem = new ListViewItem(regel);
                listViewHis.Items.Add(listItem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
