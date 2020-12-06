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
			ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur,15);
			listBoxItems.Enabled = true;
			textBoxAfwijking.Enabled = true;
			buttonHistory.Enabled = true;
			buttonVoerIn.Enabled = true;
			buttonReeks.Enabled = true;
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

			if (textBoxAfwijking.Text.Length > 2)
			{
				string eerste_2 = textBoxAfwijking.Text.Substring(0, 2);

				if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD")
				{
					MessageBox.Show("Omdat hier ruil/extra staat, moet je eerst deze verwijderen als je wat wil wijzigen");
					listBoxItems.Enabled = false;
					textBoxAfwijking.Enabled = false;
					buttonHistory.Enabled = false;
					buttonVoerIn.Enabled = false;
					buttonReeks.Enabled = false;
				}
			}
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
				ProgData.NachtErVoorVrij(labelNaam.Text,labelDatum.Text, textBoxAfwijking.Text);
				string eerste_2 = "";

				if (textBoxAfwijking.Text.Length > 2)
					eerste_2 = textBoxAfwijking.Text.Substring(0, 2);

				if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD")
				{
					// als ED-O of ED-M of ED-N aanpassing op andere kleur, of VD of RD
					// bepaal de kleur die dan loopt.

					// get huidige kleur op
					string dienst = textBoxAfwijking.Text.Substring(3, 1);
					string gaat_lopen_op_kleur = ProgData.MDatum.GetKleurDieWerkt(_verzoekdag, dienst);
					string dir = ProgData.GetDirectoryBezettingMaand(_verzoekdag);
					ProgData.LoadLooptExtraLijst(dir, gaat_lopen_op_kleur);

                    LooptExtraDienst lop = new LooptExtraDienst
                    {
                        _datum = _verzoekdag,
                        _naam = labelNaam.Text,
                        _metcode = textBoxAfwijking.Text
                    };

                    ProgData.ListLooptExtra.Add(lop);
					ProgData.SaveLooptExtraLijst(dir, gaat_lopen_op_kleur);
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
			string eerste_2 = "";
			if (textBoxAfwijking.Text.Length > 2)
				eerste_2 = textBoxAfwijking.Text.Substring(0, 2);

			if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD")
			{
				// als ED-O of ED-M of ED-N aanpassing op andere kleur
				// bepaal de kleur die dan loopt.

				// get huidige kleur op
				string dienst = textBoxAfwijking.Text.Substring(3, 1);
				DateTime _verzoekdag = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, int.Parse(labelDatum.Text));
				string gaat_lopen_op_kleur = ProgData.MDatum.GetKleurDieWerkt(_verzoekdag, dienst);
				string dir = ProgData.GetDirectoryBezettingMaand(_verzoekdag);
				ProgData.LoadLooptExtraLijst(dir, gaat_lopen_op_kleur);
				try
				{
					LooptExtraDienst lp = ProgData.ListLooptExtra.First(a => (a._naam == labelNaam.Text) && (a._datum == _verzoekdag));
					ProgData.ListLooptExtra.Remove(lp);
					ProgData.SaveLooptExtraLijst(dir, gaat_lopen_op_kleur);
				}
				catch { }
			}
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