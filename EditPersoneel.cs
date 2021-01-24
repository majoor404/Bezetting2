using Bezetting2.Data;
using Bezetting2.InlogGebeuren;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Bezetting2.DatumVijfPloegUtils;

namespace Bezetting2
{
	public partial class EditPersoneel : Form
	{
		private int selpersnummer;
		private int rechten;
		// sla de veranderingen op van persoon in tijdelijke lijst
		readonly List<VeranderingenVerhuis> VeranderingenLijstTemp = new List<VeranderingenVerhuis>();

		public EditPersoneel()
		{
			InitializeComponent();
		}

		private void EditPersoneel_Shown(object sender, EventArgs e)
		{
			comboBoxFilter.Enabled = true;
			ProgData.Lees_Namen_lijst();
			ViewNamen.Items.Clear();
			comboBoxFilter.Items.Clear();
			string[] arr = new string[5];
			ListViewItem itm;
			foreach (personeel a in ProgData.ListPersoneel)
			{
				arr[0] = a._persnummer.ToString();
				arr[1] = a._achternaam;
				arr[2] = a._voornaam;
				arr[3] = a._kleur;
				arr[4] = a._rechten.ToString();
				itm = new ListViewItem(arr);
				ViewNamen.Items.Add(itm);

				if (a._kleur != null && !comboBoxFilter.Items.Contains(a._kleur))
					comboBoxFilter.Items.Add(a._kleur);
			}
			comboBoxFilter.Text = "";
			comboBoxFilter.Items.Add("");
			textBoxPersNum.Text = "";
			textBoxAchterNaam.Text = "";
			textBoxVoorNaam.Text = "";
			textBoxAdres.Text = "";
			textBoxPostcode.Text = "";
			textBoxWoonplaats.Text = "";
			textBoxEmailThuis.Text = "";
			textBoxEmailWerk.Text = "";
			textBoxTelThuis.Text = "";
			textBoxTelMobPrive.Text = "";
			textBoxTelMobWerk.Text = "";
			textBoxAdresCodeWerk.Text = "";
			textBoxTelWerk.Text = "";
			comboBoxKleur.Text = "";
			textBoxFuntie.Text = "";
			textBoxWerkplek.Text = "";
			LabelRoosterNieuw.Text = "";
			labelNieuwRoosterDatum.Text = "";
			vuilwerk.Checked = false;

			// als rechten 50, dan alleen eigen kleur
			if (ProgData.RechtenHuidigeGebruiker < 51)
			{
				// haal kleur van gebruiker
				comboBoxFilter.Text = ProgData.Huidige_Gebruiker_Werkt_Op_Kleur;
				comboBoxFilter.Enabled = false;
			}
			if (ProgData.RechtenHuidigeGebruiker > 100)
			{
				// direct edit ploeg rooster als admin
				MessageBox.Show("als Admin nu direct rooster wissel mogelijk");
				comboBoxKleur.Enabled = true;
			}
			textBoxPersNum.Enabled = false;
			textBoxAchterNaam.Enabled = false;
		}

		// filter aangepast
		private void ComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			ViewNamen.Items.Clear();
			string[] arr = new string[5];
			ListViewItem itm;
			foreach (personeel a in ProgData.ListPersoneel)
			{
				if (a._kleur == comboBoxFilter.Text || string.IsNullOrEmpty(comboBoxFilter.Text))
				{
					arr[0] = a._persnummer.ToString();
					arr[1] = a._achternaam;
					arr[2] = a._voornaam;
					arr[3] = a._kleur;
					arr[4] = a._rechten.ToString();
					itm = new ListViewItem(arr);
					ViewNamen.Items.Add(itm);
				}
			}
			textBoxPersNum.Text = "";
			textBoxAchterNaam.Text = "";
			textBoxVoorNaam.Text = "";
			textBoxAdres.Text = "";
			textBoxPostcode.Text = "";
			textBoxWoonplaats.Text = "";
			textBoxEmailThuis.Text = "";
			textBoxEmailWerk.Text = "";
			textBoxTelThuis.Text = "";
			textBoxTelMobPrive.Text = "";
			textBoxTelMobWerk.Text = "";
			textBoxAdresCodeWerk.Text = "";
			textBoxTelWerk.Text = "";
			comboBoxKleur.Text = "";
			textBoxFuntie.Text = "";
			textBoxWerkplek.Text = "";
			LabelRoosterNieuw.Text = "";
			vuilwerk.Checked = false;
		}

		// geklikt op naam in overzicht
		private void ViewNamen_SelectedIndexChanged(object sender, EventArgs e)
		{
			//_ = comboBoxFilter.Text;
			comboBoxKleur.Enabled = false;
			textBoxPersNum.Enabled = false;
			textBoxAchterNaam.Enabled = false;
			selpersnummer = 0;
			try
			{
				// ik kom hier elke keer 2 maal bij klikken
				// als er al 1 gesellcteerd is en ik klik een andere
				// zie je eerst geen item geklikt, dan de volgende.
				// dus niks in catch invullen
				string test = ViewNamen.SelectedItems[0].SubItems[0].Text;
				selpersnummer = int.Parse(ViewNamen.SelectedItems[0].SubItems[0].Text);
			}
			catch
			{
			}
			foreach (personeel a in ProgData.ListPersoneel)
			{
				if (a._persnummer == selpersnummer)
				{
					textBoxPersNum.Text = a._persnummer.ToString();
					textBoxAchterNaam.Text = a._achternaam;
					textBoxVoorNaam.Text = a._voornaam;
					textBoxAdres.Text = a._adres;
					textBoxPostcode.Text = a._postcode;
					textBoxWoonplaats.Text = a._woonplaats;
					textBoxEmailThuis.Text = a._emailthuis;
					textBoxEmailWerk.Text = a._emailwerk;
					textBoxTelThuis.Text = a._telthuis;
					textBoxTelMobPrive.Text = a._tel06prive;
					textBoxTelMobWerk.Text = a._tel06werk;
					textBoxAdresCodeWerk.Text = a._adrescodewerk;
					textBoxTelWerk.Text = a._telwerk;
					comboBoxKleur.Text = a._kleur;
					textBoxFuntie.Text = a._funtie;
					textBoxWerkplek.Text = a._werkgroep;
					vuilwerk.Checked = bool.Parse(a._vuilwerk);

					LabelRoosterNieuw.Text = a._nieuwkleur;
					if (!string.IsNullOrEmpty(LabelRoosterNieuw.Text))
					{
						labelNieuwRoosterDatum.Text = a._verhuisdatum.ToString("d");
						button1.Enabled = false;
					}
					else
					{
						labelNieuwRoosterDatum.Text = "";
						button1.Enabled = true;
					}
					rechten = a._rechten;
				}
			}
		}

		private void ButtonClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void ButtonSave_Click(object sender, EventArgs e)
		{
			try
			{
				personeel persoon_gekozen = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
				ProgData.ListPersoneel.Remove(persoon_gekozen);
				ButtonVoegToe_Click(this, null);
				EditPersoneel_Shown(this, null);
			}
			catch
			{
				MessageBox.Show("Kan personeel nummer niet vinden om record te veranderen.");
			}
		}

		// verhuis bericht naar andere kleur
		private void VerhuisKnop_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Gebruik dit alleen als gebruiker langer als een maand verhuisd, dus eind datum ligt maand verder dan nu!");
			DialogResult dialogResult = MessageBox.Show("De ingevulde dagen worden 11 maanden doorgecopyeerd naar nieuwe kleur.\nAndere verdwijnen!, doorgaan ?", "Vraagje", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				VerhuisForm verhuis = new VerhuisForm();
				verhuis.labelNaam.Text = textBoxAchterNaam.Text;
				verhuis.labelPersoneelNummer.Text = textBoxPersNum.Text;
				verhuis.labelHuidigRooster.Text = comboBoxKleur.Text;
				verhuis.ShowDialog();

				MessageBox.Show("Geduld, copyeren van dagen");

				try
				{
					personeel persoon_gekozen = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);

					// zat op deze kleur, maar gaat naar nieuwe
					// dus zet kruisjes dat hij niet meer aanwezig is.
					persoon_gekozen._verhuisdatum = verhuis.dateTimeVerhuisDatum.Value;
					persoon_gekozen._nieuwkleur = verhuis.comboBoxNieuwRooster.Text;
					ProgData.Save_Namen_lijst();

					int eerste_dag_weg = persoon_gekozen._verhuisdatum.Day;

					LabelRoosterNieuw.Text = persoon_gekozen._nieuwkleur;

					string GekozenKleurInBeeld = ProgData.GekozenKleur;

					// zet path goed van kleur ploeg
					ProgData.GekozenKleur = persoon_gekozen._kleur;
					// zet maand en jaar goed van verhuis datum
					ProgData.igekozenmaand = persoon_gekozen._verhuisdatum.Month;
					ProgData.Igekozenjaar = persoon_gekozen._verhuisdatum.Year;

					// gevraagde afwijkingen/vakantie's op oude wacht, zodat ze kunnen verhuizen naar nieuwe
					// meegeven eerste dag.
					if(persoon_gekozen._kleur != "Nieuw")
						Bewaar_oude_afwijkingen(persoon_gekozen._achternaam, eerste_dag_weg, persoon_gekozen._verhuisdatum.Month, persoon_gekozen._verhuisdatum.Year);

					int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);

					if (ProgData.GekozenKleur != "Nieuw") // als nieuw persoon, dan hoef je niet X te zetten bij weg gaan ploeg
					{
						for (int i = eerste_dag_weg; i < aantal_dagen_deze_maand + 1; i++)
						{
							ProgData.RegelAfwijking(persoon_gekozen._achternaam, i.ToString(), "X", "Rooster Wissel", "Verhuizing", ProgData.GekozenKleur);
						}
					}

					// tevens 12 maanden verder de X
					for (int maandenverder = 1; maandenverder < 12; maandenverder++)
					{

						DateTime dummy = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, 1);
						dummy = dummy.AddMonths(1);
						aantal_dagen_deze_maand = DateTime.DaysInMonth(dummy.Year, dummy.Month);
						ProgData.Igekozenjaar = dummy.Year;
						ProgData.igekozenmaand = dummy.Month;

						if (ProgData.GekozenKleur != "Nieuw") // als nieuw persoon, dan hoef je niet X te zetten bij weg gaan ploeg
						{
							for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
							{
								ProgData.RegelAfwijking(persoon_gekozen._achternaam, i.ToString(), "X", "Rooster Wissel", "Verhuizing", ProgData.GekozenKleur);
							}
						}
					}

					// komt van andere kleur
					//GekozenKleurInBeeld = ProgData.GekozenKleur;

					ProgData.GekozenKleur = persoon_gekozen._nieuwkleur;
					// zet maand en jaar goed van verhuis datum
					ProgData.igekozenmaand = persoon_gekozen._verhuisdatum.Month;
					ProgData.Igekozenjaar = persoon_gekozen._verhuisdatum.Year;

					ProgData.MaakPloegNamenLijst(ProgData.GekozenKleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
					ProgData.SavePloegNamenLijst(ProgData.GekozenKleur,15);     // save ploegbezetting (de mensen)

					// moet nieuwe collega toevoegen aan bezetting

					int aantal_dagen_dezemaand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);
					ProgData.LoadPloegBezetting(ProgData.GekozenKleur, 15); // want nieuwe kleur gekozen
																			// maak ruimte voor nieuwe collega in werkdag tabel
					for (int i = 1; i < aantal_dagen_dezemaand + 1; i++)
					{
						DateTime dat = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, i);
						werkdag dag = new werkdag
						{
							_naam = persoon_gekozen._achternaam,
							_standaarddienst = GetDienst(ProgData.GekozenRooster(), dat, persoon_gekozen._nieuwkleur),
							_werkplek = "",
							_afwijkingdienst = "",
							_dagnummer = i
						};
						ProgData.ListWerkdagPloeg.Add(dag);
					}
					ProgData.SavePloegBezetting(ProgData.GekozenKleur, 15);

					for (int i = 1; i < eerste_dag_weg; i++)
					{
						DateTime dat = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, i);
						ProgData.RegelAfwijkingOpDatumEnKleur(dat, persoon_gekozen._nieuwkleur, persoon_gekozen._achternaam, i.ToString(), "X", "Rooster Wissel", "Verhuizing");
					}

					LabelRoosterNieuw.Text = persoon_gekozen._nieuwkleur;
					if (!string.IsNullOrEmpty(LabelRoosterNieuw.Text))
					{
						labelNieuwRoosterDatum.Text = persoon_gekozen._verhuisdatum.ToString("d");
						button1.Enabled = false;
					}
					else
					{
						labelNieuwRoosterDatum.Text = "";
						button1.Enabled = true;
					}
					ProgData.Save_Namen_lijst();

					// gevraagde afwijkingen/vakantie's op oude wacht, zodat ze kunnen verhuizen naar nieuwe
					// meegeven eerste dag.
					Restore_oude_afwijkingen(persoon_gekozen._nieuwkleur);

					MessageBox.Show("Klaar met Verhuis");

					ProgData.GekozenKleur = GekozenKleurInBeeld;
				}
				catch { }
			}// vraagje 
		}

		private void ButtonCancelVerhuis_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(LabelRoosterNieuw.Text))
			{

				int maand = ProgData.igekozenmaand;
				int jaar = ProgData.Igekozenjaar;
				string kleur = ProgData.GekozenKleur;

				personeel persoon_gekozen = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);

				DateTime verhuisdatum_was = persoon_gekozen._verhuisdatum;
				string kleur_was = persoon_gekozen._nieuwkleur;
				string kleur_terug = persoon_gekozen._kleur;

				DateTime dum = new DateTime(9999, 1, 1);
				persoon_gekozen._verhuisdatum = dum;
				persoon_gekozen._nieuwkleur = "";
				labelNieuwRoosterDatum.Text = "";
				LabelRoosterNieuw.Text = "";
				ProgData.Save_Namen_lijst();

				// get data van nieuwkleur
				ProgData.GekozenKleur = kleur_was;
				Bewaar_oude_afwijkingen(persoon_gekozen._achternaam, verhuisdatum_was.Day,
					verhuisdatum_was.Month, verhuisdatum_was.Year);

				int eerste_dag_weg = persoon_gekozen._verhuisdatum.Day;

				// zet path goed van kleur ploeg
				ProgData.GekozenKleur = kleur_terug;
				ProgData.Igekozenjaar = verhuisdatum_was.Year;
				ProgData.igekozenmaand = verhuisdatum_was.Month;

				int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);

				for (int i = eerste_dag_weg; i < aantal_dagen_deze_maand + 1; i++)
				{
					ProgData.RegelAfwijking(persoon_gekozen._achternaam, i.ToString(), "", "Rooster Wissel Cancel", "Verhuizing", kleur_terug);
				}


				// tevens 12 maanden verder de X
				for (int maandenverder = 1; maandenverder < 12; maandenverder++)
				{

					DateTime dummy = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, 1);
					dummy = dummy.AddMonths(1);
					aantal_dagen_deze_maand = DateTime.DaysInMonth(dummy.Year, dummy.Month);
					ProgData.Igekozenjaar = dummy.Year;
					ProgData.igekozenmaand = dummy.Month;

					for (int i = 1; i < aantal_dagen_deze_maand + 1; i++)
					{
						ProgData.RegelAfwijking(persoon_gekozen._achternaam, i.ToString(), "", "Rooster Wissel Cancel", "Verhuizing", kleur_terug);
					}
				}

				Restore_oude_afwijkingen(kleur_terug);

				//MessageBox.Show("Kruisjes in bezetting met de hand weghalen!");
				ProgData.Igekozenjaar = jaar;
				ProgData.igekozenmaand = maand;
				ProgData.GekozenKleur = kleur;
				EditPersoneel_Shown(this, null);

				MessageBox.Show("Klaar met cancel verhuis");
			}
		}

		private void ButtonRechten_Click(object sender, EventArgs e)
		{
			RechtenInstellenForm recht = new RechtenInstellenForm();
			recht.labelNaam.Text = textBoxAchterNaam.Text;
			recht.labelPersoneelNummer.Text = textBoxPersNum.Text;
			recht.labelRechtenNivo.Text = rechten.ToString();
			if (rechten > 51)
				recht.checkBoxAllePloegen.Checked = true;
			if (rechten == 0)
				recht.radioButton0.Checked = true;
			if (rechten > 24)
				recht.radioButton25.Checked = true;
			if(rechten> 49)
				recht.radioButton50.Checked = true;

			DialogResult dialog = recht.ShowDialog();
			if (dialog == DialogResult.OK)
			{
				try
				{
					personeel persoon = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
					persoon._rechten = int.Parse(recht.labelRechtenNivo.Text);
					ProgData.Save_Namen_lijst();
					ComboBoxFilter_SelectedIndexChanged(this, null);
					// als rechten aangepast dan auto inlog verwijderen, zou zelfde persoon kunnen zijn.
					// als ander auto inlog dan helaas ook even weg
					string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
					string autoinlogfile = $"{directory}\\bezetting2.log";
					File.Delete(autoinlogfile);
				}
				catch
				{
					MessageBox.Show("Persoon niet gevonden, eerst opslaan");
				}
			}

		}

		private void ButtonDelete_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("Delete deze naam ?", "Delete", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				personeel persoon = ProgData.ListPersoneel.First(a => a._persnummer.ToString() == textBoxPersNum.Text);
				ProgData.ListPersoneel.Remove(persoon);
				ProgData.Save_Namen_lijst();
				EditPersoneel_Shown(this, null);
			}
		}

		private void ButtonVoegToe_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(comboBoxKleur.Text))
			{
				MessageBox.Show("Op welke wacht of kleur gaat persoon lopen?");
			}
			else
			{
				comboBoxKleur.Enabled = false;
				textBoxPersNum.Enabled = false;
				textBoxAchterNaam.Enabled = false;
				buttonSave.Enabled = true;
				buttonDelete.Enabled = true;
				button1.Enabled = true;
				buttonCancelVerhuis.Enabled = true;

				personeel a = new personeel
				{
					_persnummer = int.Parse(textBoxPersNum.Text),
					_achternaam = textBoxAchterNaam.Text,
					_voornaam = textBoxVoorNaam.Text,
					_adres = textBoxAdres.Text,
					_postcode = textBoxPostcode.Text,
					_woonplaats = textBoxWoonplaats.Text,
					_emailthuis = textBoxEmailThuis.Text,
					_emailwerk = textBoxEmailWerk.Text,
					_telthuis = textBoxTelThuis.Text,
					_tel06prive = textBoxTelMobPrive.Text,
					_tel06werk = textBoxTelMobWerk.Text,
					_adrescodewerk = textBoxAdresCodeWerk.Text,
					_telwerk = textBoxTelWerk.Text,
					_vuilwerk = vuilwerk.Checked.ToString()
				};

				a._kleur = comboBoxKleur.Text;

				if (ProgData.RechtenHuidigeGebruiker > 100)
				{
					// direct edit ploeg rooster als admin
					a._kleur = comboBoxKleur.Text;
				}

				a._funtie = textBoxFuntie.Text;
				a._werkgroep = textBoxWerkplek.Text;
				a._rechten = 0;
				ProgData.ListPersoneel.Add(a);
				ProgData.Save_Namen_lijst();

				ProgData.MaakPloegNamenLijst(comboBoxKleur.Text);
				
				// voeg nieuw naam toe in listwerkdagploeg
				ProgData.AddNaamInBezetting(comboBoxKleur.Text, textBoxAchterNaam.Text);

				EditPersoneel_Shown(this, null);
			}
		}

		private void ButtonNieuw_Click(object sender, EventArgs e)
		{
			buttonSave.Enabled = false;
			buttonDelete.Enabled = false;
			button1.Enabled = false;
			buttonCancelVerhuis.Enabled = false;
			textBoxPersNum.Text = "";
			textBoxAchterNaam.Text = "";
			textBoxVoorNaam.Text = "";
			textBoxAdres.Text = "";
			textBoxPostcode.Text = "";
			textBoxWoonplaats.Text = "";
			textBoxEmailThuis.Text = "";
			textBoxEmailWerk.Text = "";
			textBoxTelThuis.Text = "";
			textBoxTelMobPrive.Text = "";
			textBoxTelMobWerk.Text = "";
			textBoxAdresCodeWerk.Text = "";
			textBoxTelWerk.Text = "";
			comboBoxKleur.Text = "";
			textBoxFuntie.Text = "";
			textBoxWerkplek.Text = "";
			LabelRoosterNieuw.Text = "";
			vuilwerk.Checked = false;
			comboBoxKleur.Enabled = true;
			textBoxPersNum.Enabled = true;
			textBoxAchterNaam.Enabled = true;
			MessageBox.Show("Na invoeren naam enz, druk op voeg toe.");
			/*\nDoe daarna kleur verhuizing naar juiste kleur/plek.*/
		}
		// gevraagde afwijkingen/vakantie's op oude wacht, zodat ze kunnen verhuizen naar nieuwe
		private void Bewaar_oude_afwijkingen(string personeel_naam, int eerste_dag, int maand, int jaar)
		{
			VeranderingenLijstTemp.Clear();

			// sla de veranderingen op van persoon in tijdelijke lijst
			// save maand/jaar
			int backupjaar = ProgData.Igekozenjaar;
			int backupmaand = ProgData.igekozenmaand;

			ProgData.Igekozenjaar = jaar;
			ProgData.igekozenmaand = maand;
			int aantal_dagen_dezemaand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);
			
			ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur, 15);

			IEnumerable<veranderingen> veranderingen = from s in ProgData.ListVeranderingen
										where s._naam == personeel_naam
										select s;


			foreach (veranderingen Item in veranderingen)
			{
				int I = int.Parse(Item._datumafwijking);

				if (I >= eerste_dag) {
					VeranderingenVerhuis verhuisje = new VeranderingenVerhuis
					{
						Maand_ = ProgData.igekozenmaand.ToString(),
						Jaar_ = ProgData.Igekozenjaar.ToString(),
						_afwijking = Item._afwijking,
						_datumafwijking = Item._datumafwijking,
						_datuminvoer = Item._datuminvoer,
						_invoerdoor = Item._invoerdoor,
						_naam = Item._naam,
						_rede = Item._rede
					};

					VeranderingenLijstTemp.Add(verhuisje);
				}
			}

			// en nu volgende 10 maanden
			DateTime datum = new DateTime(jaar, maand, 1);
			for (int i = 0; i < 10; i++)
			{
				datum = datum.AddMonths(1);
				ProgData.Igekozenjaar = datum.Year;
				ProgData.igekozenmaand = datum.Month;
				aantal_dagen_dezemaand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);

				if (File.Exists(ProgData.Ploeg_Veranderingen_Locatie(ProgData.GekozenKleur)))
				{
					ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur, 15);

					veranderingen = from s in ProgData.ListVeranderingen
									where s._naam == personeel_naam
									select s;

					foreach (veranderingen Item in veranderingen)
					{
						VeranderingenVerhuis verhuisje = new VeranderingenVerhuis
						{
							Maand_ = ProgData.igekozenmaand.ToString(),
							Jaar_ = ProgData.Igekozenjaar.ToString(),
							_afwijking = Item._afwijking,
							_datumafwijking = Item._datumafwijking,
							_datuminvoer = Item._datuminvoer,
							_invoerdoor = Item._invoerdoor,
							_naam = Item._naam,
							_rede = Item._rede
						};

						VeranderingenLijstTemp.Add(verhuisje);
					}
				}
			}

			// restore maand jaar
			ProgData.igekozenmaand = backupmaand;
			ProgData.Igekozenjaar = backupjaar;
		}
		private void Restore_oude_afwijkingen(string nieuwekleur)
		{
			// save maand/jaar
			int backupjaar = ProgData.Igekozenjaar;
			int backupmaand = ProgData.igekozenmaand;
			ProgData.GekozenKleur = nieuwekleur;
			string temp = labelNieuwRoosterDatum.Text;
			int t = 0;
			foreach (VeranderingenVerhuis ver in VeranderingenLijstTemp)
			{
				labelNieuwRoosterDatum.Text = t.ToString();
				labelNieuwRoosterDatum.Refresh();
				DateTime dat = new DateTime(int.Parse(ver.Jaar_), int.Parse(ver.Maand_), int.Parse(ver._datumafwijking));
				string invoerdoor = $"Verhuis: {ver._invoerdoor}";
				if (ver._afwijking == "X")
					ver._afwijking = "";
				ProgData.RegelAfwijkingOpDatumEnKleur(dat, nieuwekleur, ver._naam, ver._datumafwijking, ver._afwijking, ver._rede, invoerdoor);
			}
			labelNieuwRoosterDatum.Text = temp;
			// restore maand jaar
			ProgData.Igekozenjaar = backupjaar;
			ProgData.igekozenmaand = backupmaand;
		}
	}
}