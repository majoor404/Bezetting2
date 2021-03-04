using Bezetting2.Data;
using Bezetting2.Invoer;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class DagAfwijkingInvoerForm : Form
    {
        public DateTime _verzoekdag; // gebruik voor ed-o ed-m ed-n , gevuld door MainformBezetting bij aanroep

        public DagAfwijkingInvoerForm()
        {
            InitializeComponent();
        }

        private void FormDagAfwijkingInvoer_Shown(object sender, EventArgs e)
        {
            ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur, 15);
            try
            {
                veranderingen ver = ProgData.ListVeranderingen.Last(a => (a._naam == labelNaam.Text) && (a._datumafwijking == labelDatum.Text));
                if (!string.IsNullOrEmpty(ver._afwijking))
                {
                    textBoxAfwijking.Text = ver._afwijking;
                    textBoxRede.Text = ver._rede;
                }
                else
                {
                    textBoxAfwijking.Text = "";
                    textBoxRede.Text = "";
                }
            }
            catch { }

            textBoxAfwijking.Focus();
        }

        private void ListBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxAfwijking.Text = listBoxItems.SelectedItem.ToString();
        }

        private void ButtonVoerIn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxAfwijking.Text))
            {
                textBoxAfwijking.Text = textBoxAfwijking.Text.ToUpper();
                ProgData.RegelAfwijking(labelNaam.Text, labelDatum.Text, textBoxAfwijking.Text, textBoxRede.Text, this.Text, ProgData.GekozenKleur);
                ProgData.NachtErVoorVrij(labelNaam.Text, labelDatum.Text, textBoxAfwijking.Text);
                //string eerste_2 = "";

                string eerste_2 = textBoxAfwijking.Text.Length >= 2 ? textBoxAfwijking.Text.Substring(0, 2) : textBoxAfwijking.Text;

                if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD" || eerste_2 =="DD")
                {
                    ProgData.VulInLooptExtraDienst(textBoxAfwijking.Text, _verzoekdag, labelNaam.Text);
                }
            }
            else
            {
                MessageBox.Show("Vul afwijking in of kies uit lijst." +
                    "\nOf kies knop cancel afwijking als u huidige wilt verwijderen.");
            }
        }

        private void ButtonHistory_Click(object sender, EventArgs e)
        {
            HistoryForm his = new HistoryForm();
            his.ShowDialog();
        }

        private void ButtonCancelInvoer_Click(object sender, EventArgs e)
        {
            ProgData.RegelAfwijking(labelNaam.Text, labelDatum.Text, "", "Verwijderd", this.Text, ProgData.GekozenKleur);
        }

        private void TextBoxAfwijking_TextChanged(object sender, EventArgs e)
        {
            buttonVoerIn.Enabled = textBoxAfwijking.Text.Length > 0;
        }

        private void ButtonReeks_Click(object sender, EventArgs e)
        {
            AfwijkingInvoerReeksForm afwReeks = new AfwijkingInvoerReeksForm();

            afwReeks.labelNaam.Text = labelNaam.Text;
            afwReeks.labelDatum.Text = labelDatum.Text;
            afwReeks.labelMaand.Text = labelMaand.Text;
            afwReeks.labelPersoneelnr.Text = labelPersoneelnr.Text;
            afwReeks.textBoxAfwijking.Text = textBoxAfwijking.Text;
            afwReeks.Text = Text;
            afwReeks.ShowDialog();
            Close();
        }
    }
}