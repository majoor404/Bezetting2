using System;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class FormDagAfwijkingInvoer : Form
    {
        public FormDagAfwijkingInvoer()
        {
            InitializeComponent();
        }

        private void FormDagAfwijkingInvoer_Shown(object sender, EventArgs e)
        {
            textBoxAfwijking.Text = "";
            textBoxAfwijking.Focus();
        }

        private void listBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAfwijking.Text = listBoxItems.SelectedItem.ToString();
            if (textBoxAfwijking.Text == "Wis")
                textBoxAfwijking.Text = "";
        }

        private void buttonVoerIn_Click(object sender, EventArgs e)
        {
            // haal veranderingen
            ProgData.HaalVeranderList();
            // voer in
            DateTime nu = DateTime.Now;
            veranderingen a = new veranderingen();
            a._afwijking = textBoxAfwijking.Text;
            a._datumafwijking = labelDatum.Text;
            a._naam = labelNaam.Text;
            a._datuminvoer = $"{nu.Day} / {nu.Month}";
            a._invoerdoor = this.Text;
            a._rede = textBoxRede.Text;
            // voeg toe
            ProgData.VoegToe(a);
            // save
            ProgData.SaveVeranderList();
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            History his = new History();
            his.ShowDialog();
        }
    }
}
