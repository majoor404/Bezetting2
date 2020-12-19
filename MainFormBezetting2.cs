using Bezetting2.Data;
using Bezetting2.Invoer;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class MainFormBezetting2 : Form
	{
		public const int kolom_breed = 49;
		public const int y_as_eerste_lijn = 136;
		public const int y_as_add_lijn = 4;
		private bool kill = false;
		private bool WindowUpdateViewScreen = true;

		private readonly ToolTip mTooltip = new ToolTip();
		private Point mLastPos = new Point(-1, -1);

		private readonly Color Weekend_ = Color.LightSkyBlue;
		private readonly Color Feestdag_ = Color.LightSalmon;
		private readonly Color Huidigedag_ = Color.Lavender;
		private readonly Color MaandButton_ = Color.LightSkyBlue;
		private readonly Color Werkplek_ = Color.LightGray;
		private readonly Color MinimaalPersonen_ = Color.LightPink;
		private readonly Color HoverNaam_ = Color.LightGreen;

		public List<ClassTelPlekGewerkt> ListClassTelPlekGewerkt = new List<ClassTelPlekGewerkt>();
		public List<string> ListTelNamen = new List<string>();
		public List<string> ListTelWerkPlek = new List<string>();
		public List<ClassTelVuilwerk> ListClassTelVuilwerk = new List<ClassTelVuilwerk>();
		public List<string> ListVuilwerkData = new List<string>();
		private DateTime dag_gekozen;
		public List<ClassTelAfwijkingen> ListClassTelAfwijkingen = new List<ClassTelAfwijkingen>();

		private readonly List<string> ListTelNietMeeNamen = new List<string>();

		// quick menu
		readonly QuickInvoerForm quick = new QuickInvoerForm();

		public class ClassTelAfwijkingen
		{
			public ClassTelAfwijkingen(string afwijking, int aantal, bool toekomst)
			{
				_Afwijking = afwijking;
				_Aantal = aantal;
				_Toekomst = toekomst;
			}

			public string _Afwijking { get; set; }
			public int _Aantal { get; set; }
			public bool _Toekomst { get; set; }
		}

		public class ClassTelPlekGewerkt
		{
			public ClassTelPlekGewerkt(string naam, string plek)
			{
				_NaamTelPlek = naam;
				_PlekTelPlek = plek;
				_AantalTelPlek = 1;
			}

			public string _NaamTelPlek { get; set; }
			public string _PlekTelPlek { get; set; }
			public int _AantalTelPlek { get; set; }
		}

		public class ClassTelVuilwerk
		{
			public ClassTelVuilwerk(string naam, string dag)
			{
				_NaamTelVuil = naam;
				_DagTelVuil = dag;
			}

			public string _NaamTelVuil { get; set; }
			public string _DagTelVuil { get; set; }
		}

		private readonly InstellingenProgrammaForm instellingen_programma = new InstellingenProgrammaForm();

		public MainFormBezetting2()
		{
			InitializeComponent();
			ProgData._inlognaam = IngelogdPersNr;
			ProgData._toegangnivo = ToegangNivo;

			ProgData.RechtenHuidigeGebruiker = 0; // alleen lezen
			ProgData.Huidige_Gebruiker_Personeel_nummer = "Niemand Ingelogd";
			WindowUpdateViewScreen = true;
			InstellingenProg.LeesProgrammaData();
		}

		// start programma
		private void MainFormBezetting2_Shown(object sender, EventArgs e)
		{
			if (File.Exists("kill.ini"))
				Close();

			ruilOverwerkToolStripMenuItem.Visible = InstellingenProg._GebruikExtraRuil;
			snipperDagAanvraagToolStripMenuItem.Visible = InstellingenProg._GebruikSnipper;

			ProgData.Main = this;

			if (!ProgData.TestNetwerkBeschikbaar(15))
				Close();

			DateTime nu = DateTime.Now;

			ProgData.ihuidigemaand = nu.Month;
			ProgData.igekozenmaand = nu.Month;

			ProgData.Igekozenjaar = nu.Year;
			ProgData.ihuidigjaar = nu.Year;

			ProgData.backup_zipnaam = "Backup\\" + nu.ToShortDateString() + ".zip";

			Random rnd = new Random();
			ProgData.backup_time = rnd.Next(60);

			KleurMaandButton();

			if (InstellingenProg._Rooster == "5pl")
			{
				// get huidige kleur op
				string dienst = "N";
				if (nu.Hour > 5 && nu.Hour < 14)
					dienst = "O";
				if (nu.Hour > 13 && nu.Hour < 22)
					dienst = "M";

				if (nu.Hour < 6 && dienst == "N")
					nu = nu.AddDays(-1);

				comboBoxKleurKeuze.Text = ProgData.MDatum.GetKleurDieWerkt(nu, dienst);
			}
			else
			{
				comboBoxKleurKeuze.Text = "DD";
				comboBoxKleurKeuze.Enabled = false;
			}

			//comboBoxKleurKeuze.Text = "Blauw"; // roept ook VulViewScherm(); aan.
			//VulViewScherm(); aanroep door comboBoxKleurKeuze.Text = "Blauw"

			if (ProgData.LeesLijnen())
				ZetLijnen();

            // auto inlog
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string autoinlogfile = $"{directory}\\bezetting2.log";
			if (File.Exists(autoinlogfile))
			{
				List<string> inlognaam = new List<string>();
				try
				{
					inlognaam = File.ReadAllLines(autoinlogfile).ToList();
					ProgData.Huidige_Gebruiker_Personeel_nummer = inlognaam[0];
					ProgData.RechtenHuidigeGebruiker = int.Parse(inlognaam[1]);
					ProgData.Huidige_Gebruiker_Werkt_Op_Kleur = ProgData.Get_Gebruiker_Kleur(inlognaam[0]);
				}
				catch (IOException)
				{
				}
			}
			Refresh();
		}

		private void ImportNamenOudeVersieToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ProgData.ListPersoneel.Clear();
			//openFileDialog.Filter = "(*.Bez)|*.Bez";
			MessageBox.Show("Delete eerst directory's van toekomst");
			MessageBox.Show("Let op, alle oude personeel gaat weg, open Bezetting5ploegen....Bez");

			DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.

			if (result == DialogResult.OK) // Test result.
			{
				OpenDataBase_en_Voer_Oude_Namen_In(openFileDialog.FileName);
				ProgData.Save_Namen_lijst();
			}
			MessageBox.Show("Klaar, druk op refresh");
		}

		private void InloggenToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			InlogForm log = new InlogForm();
			log.ShowDialog();
			comboBoxKleurKeuze.Text = ProgData.GekozenKleur;
		}

		private void EditPersoneelClick(object sender, EventArgs e)
		{
			ProgData.Lees_Namen_lijst();
			EditPersoneel edit = new EditPersoneel();
			edit.ShowDialog();
			ButtonNu_Click(this, null); //refresh op huidige datum
			//VulViewScherm(); // refresh
		}

		private void ToegangNivo_TextChanged(object sender, EventArgs e)
		{
			// rechten worden gewijzigd, pas dus div menu items aan
			importNamenOudeVersieToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
			editPersoneelToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 45;
			kleurLijnenToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 45;
			repareerPloegAfwijkingToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
			instellingenProgrammaToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
			importOudeVeranderDataOudeVersieToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
			nietMeeTelLijstToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;
			removeAutoInlogOnderDitWindowsAccountToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 100;

			vuilwerkToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
			tellingWaarGewerktToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
			namenAdressenEMailToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 49;
			afwijkingenTovRoosterIngelogdPersoonToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 0;
			afwijkingTovRoosterPloegToolStripMenuItem.Enabled = ProgData.RechtenHuidigeGebruiker > 24;
			
		}

		private void UitloggenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ProgData.RechtenHuidigeGebruiker = 0; // alleen lezen
			ProgData.Huidige_Gebruiker_Personeel_nummer = "Niemand Ingelogd";
		}

		private void ComboBoxKleurKeuze_SelectedIndexChanged(object sender, EventArgs e)
		{
			ProgData.CaptureMainScreen();
			ProgData.GekozenKleur = comboBoxKleurKeuze.Text;
			VulViewScherm();
		}

		public void VulViewScherm()
		{
			if (WindowUpdateViewScreen)
			{
				View.Columns.Clear();
				View.Items.Clear();

				int aantal_dagen = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);

				View.Columns.Add("", 140, HorizontalAlignment.Left); // namen

				//DateTime nu = DateTime.Now;

				for (int i = 1; i <= aantal_dagen; i++)
				{
					View.Columns.Add("", kolom_breed, HorizontalAlignment.Center);
				}

				// haal rooster
				string[] dagnr = new string[aantal_dagen + 1];
				string[] rooster = new string[aantal_dagen + 1];
				string[] dag = new string[aantal_dagen + 1];
				string[] weeknr = new string[aantal_dagen + 1];
				string[] lijn_regel = new string[aantal_dagen + 1];
				rooster[0] = "";
				dag[0] = "";
				weeknr[0] = "";
				dagnr[0] = "";
				CultureInfo cul = CultureInfo.CurrentCulture;
				DateTime datum;// = new DateTime();
				for (int i = 1; i < aantal_dagen + 1; i++)
				{
					dagnr[i] = i.ToString();
					datum = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, i);
					rooster[i] = ProgData.MDatum.GetDienst(ProgData.GekozenRooster(), datum, ProgData.GekozenKleur);
					dag[i] = ProgData.MDatum.GetDag(datum);
					weeknr[i] = "";
					if (dag[i] == "W")
					{
						weeknr[i - 1] = "WK";
						int weekNum = cul.Calendar.GetWeekOfYear(datum, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
						weeknr[i] = weekNum.ToString();
					}
					lijn_regel[i] = "";
				}

				ListViewItem item_weeknr = new ListViewItem(weeknr);
				View.Items.Add(item_weeknr);
				ListViewItem item_dag = new ListViewItem(dag);
				View.Items.Add(item_dag);
				ListViewItem item_dagnr = new ListViewItem(dagnr);
				View.Items.Add(item_dagnr);
				ListViewItem item_rooster = new ListViewItem(rooster);
				View.Items.Add(item_rooster);
				ListViewItem item_lijnregel = new ListViewItem(lijn_regel);
				View.Items.Add(item_lijnregel);

				// vul namen en opgeslagen data als die bestond
				HaalBezetting();

				// kleur weekenden
				//int col = 20;
				//int row = 1;
				//int aantal_rows = ProgData.ListPersoneelKleur.Count();
				string dag_string;
				for (int col = 1; col < aantal_dagen + 1; col++)
				{
					// lees dag
					dag_string = View.Items[1].SubItems[col].Text;
					if (dag_string == "Z") // zaterdag of zondag
					{
						//for (int row = 0; row < aantal_rows + 4 + ProgData.werkgroep_personeel.Count; row++)
						for (int row = 0; row < View.Items.Count - 1; row++)
						{
							//this is very Important
							View.Items[row].UseItemStyleForSubItems = false;
							// Now you can Change the Particular Cell Property of Style
							if (View.Items[row].SubItems[col].BackColor != Werkplek_)
								View.Items[row].SubItems[col].BackColor = Weekend_;
						}
					}
				}
				KleurFeestdagen();

				// kleur huidige dag
				if (ProgData.igekozenmaand == DateTime.Now.Month && ProgData.Igekozenjaar == DateTime.Now.Year)
				{
					for (int i = 0; i < View.Items.Count - 1; i++)
					{
						View.Items[i].UseItemStyleForSubItems = false;
						// Now you can Change the Particular Cell Property of Style
						if (View.Items[i].SubItems[DateTime.Now.Day].BackColor != Werkplek_)
							View.Items[i].SubItems[DateTime.Now.Day].BackColor = Huidigedag_;
					}
				}

				// maand in beeld
				View.Items[2].UseItemStyleForSubItems = false;
				View.Items[2].SubItems[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);
				View.Items[2].SubItems[0].Text = ProgData.Sgekozenmaand().ToUpper();

				// Jaar in beeld
				View.Items[3].UseItemStyleForSubItems = false;
				View.Items[3].SubItems[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Bold);
				View.Items[3].SubItems[0].Text = ProgData.Sgekozenjaar().ToUpper();

				LijnenWeg();
				if (ProgData.LeesLijnen())
					ZetLijnen();

				labelDebug.Text = "";
			}
		}

		private void ButtonJan_Click(object sender, EventArgs e)
		{
			ProgData.CaptureMainScreen();
			System.Windows.Forms.Button myButton = (System.Windows.Forms.Button)sender;
			ProgData.igekozenmaand = int.Parse(myButton.Tag.ToString());
			KleurMaandButton();
			VulViewScherm();
		}

		private void NumericUpDownJaar_ValueChanged(object sender, EventArgs e)
		{
			ProgData.CaptureMainScreen();
			ProgData.Igekozenjaar = (int)numericUpDownJaar.Value;
			VulViewScherm();
		}

		private void ButtonRefresh_Click(object sender, EventArgs e)
		{
			WindowUpdateViewScreen = true;
			//ProgData.ReloadSpeed1 = "";
			//ProgData.ReloadSpeed2 = "";
			VulViewScherm();
		}

		private void VolgendeMaandToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DateTime t = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, 1);
			t = t.AddMonths(1);

			ProgData.igekozenmaand = t.Month;
			ProgData.Igekozenjaar = t.Year;

			KleurMaandButton();
			VulViewScherm();
		}

		private void VorigeMaandToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DateTime t = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, 1);
			t = t.AddMonths(-1);

			ProgData.igekozenmaand = t.Month;
			ProgData.Igekozenjaar = t.Year;

			KleurMaandButton();
			VulViewScherm();
		}

		public void KleurMaandButton()
		{
			int _taghuidig = ProgData.igekozenmaand;  //int.Parse(GevraagdeMaand.Tag.ToString());
			foreach (System.Windows.Forms.Button button in this.Controls.OfType<System.Windows.Forms.Button>())
			{
				if (button.Tag != null)
				{
					int _tag = int.Parse(button.Tag.ToString());

					if (_tag > 0 && _tag < 13 && (_taghuidig != _tag))
					{
						button.BackColor = Color.FromArgb(244, 244, 244);
					}
					if (_taghuidig == _tag)
					{
						button.BackColor = MaandButton_;
					}
				}
			}
		}

		private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			About a = new About();
			a.ShowDialog();
		}

		private void KleurFeestdagen()
		{
			// kleur feestdagen

			// 1 Jan nieuwjaarsdag
			// 27 april Koningsdag
			// Kerstdag 25 en 26 december
			// bevrijdingsdag 5 mei (eens in 5 jaar)

			// Pasen
			// Hemelsvaartdag = 39 dagen na pasen.
			// Pinksteren

			int maand = ProgData.igekozenmaand;
			int aantal_dagen = DateTime.DaysInMonth((int)numericUpDownJaar.Value, maand);
			int aantal_rows = ProgData.ListPersoneelKleur.Count();
			//string dag_string;

			DateTime pasen = EasterSunday((int)numericUpDownJaar.Value);
			DateTime pasen2 = pasen.AddDays(1);
			DateTime hemelsvaart = pasen.AddDays(39);
			DateTime pinksteren = hemelsvaart.AddDays(10); // 1ste pinsterdag
			DateTime pinksteren2 = pinksteren.AddDays(1); // 2ste pinsterdag

			// 5 mei elke 5 jaar (start op 2015)
			int modulo = (int)numericUpDownJaar.Value % 5; // als vijf mei is 0, dan vrij

			for (int col = 1; col < aantal_dagen + 1; col++)
			{
				if (
					(maand == 1 && col == 1) ||
					(maand == 4 && col == 27) ||
					(maand == 12 && col == 25) ||
					(maand == 12 && col == 26) ||
					(maand == 5 && col == 5 && modulo == 0) ||
					(maand == pasen.Month && col == pasen.Day) ||
					(maand == pasen2.Month && col == pasen2.Day) ||
					(maand == hemelsvaart.Month && col == hemelsvaart.Day) ||
					(maand == pinksteren.Month && col == pinksteren.Day) ||
					(maand == pinksteren2.Month && col == pinksteren2.Day)
					)
				{
					for (int row = 0; row < aantal_rows + 5 + ProgData.ListWerkgroepPersoneel.Count; row++)
					{
						//this is very Important
						View.Items[row].UseItemStyleForSubItems = false;
						// Now you can Change the Particular Cell Property of Style
						if (View.Items[row].SubItems[col].BackColor != Werkplek_)
							View.Items[row].SubItems[col].BackColor = Feestdag_;
					}
				}
			}
		}

		private void HaalBezetting()
		{
			// HaalBezetting Bestaat uit 3 delen
			// afhankelijk wat er gevraagd wordt en waarneer

			// check vooraf of juiste directory bestaat, maak deze anders aan.
			if (!Directory.Exists(ProgData.GetDir()))
				Directory.CreateDirectory(ProgData.GetDir());

			// er zijn nu 3 mogelijkheden, welke afhankelijk ik andere keuze's maak
			// 1 ) gevraagde maand is in verleden van huidige maand
			// 2 ) gevraagde maand is huidige maand
			// 3 ) gevraagde maand is in toekomst
			// ik bepaal dat in roetine WaarInTijd, als uitkomst 1 2 of 3 (zie hierboven)

			int waarintijd = ProgData.WaarInTijd();

			if (waarintijd == 1)
			{
				// verleden
				// hier alleen kijken
				// open oude ploeg bezetting
				// open wijzegingen en laat deze zien
				string Locatie = Path.GetFullPath(ProgData.GetDir() + "\\" + ProgData.GekozenKleur + "_bezetting.bin");
				if (!File.Exists(Locatie))
				{
					MessageBox.Show("bezetting deze maand bestaat niet, kan dus niks laten zien");
					ProgData.ListPersoneelKleur.Clear();
					ProgData.ListWerkgroepPersoneel.Clear();
				}
				else
				{
					// 1) Haal Ploeg Bezetting
					ProgData.LoadPloegNamenLijst(ProgData.GekozenKleur,15);

					// 2) Zet ploeg en werkplek op scherm
					for (int i = 0; i < ProgData.ListWerkgroepPersoneel.Count; i++)
					{
						// eerst naam werkplek
						string[] werkplek = new string[33];
						werkplek[0] = ProgData.ListWerkgroepPersoneel[i];
						ListViewItem item = new ListViewItem(werkplek);
						View.Items.Add(item);
						View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
						for (int grijzebalk_werkplek = 0; grijzebalk_werkplek < 33; grijzebalk_werkplek++)
						{
							View.Items[View.Items.Count - 1].SubItems[grijzebalk_werkplek].BackColor = Werkplek_;
						}
						// zet in View
						foreach (personeel a in ProgData.ListPersoneelKleur)
						{
							string[] naamlijst = new string[33];
							if (a._werkgroep == ProgData.ListWerkgroepPersoneel[i])
							{
								naamlijst[0] = a._achternaam;
								ListViewItem item_naam = new ListViewItem(naamlijst);
								View.Items.Add(item_naam);
							}
						}
					}

					// Vul bezetting op scherm
					if (File.Exists(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)))
					{
						ProgData.LoadPloegBezetting(ProgData.GekozenKleur, 15);
						foreach (werkdag a in ProgData.ListWerkdagPloeg)
						{
							if (!string.IsNullOrEmpty(a._afwijkingdienst))
							{
								for (int i = 0; i < View.Items.Count; i++) // alle namen/rows
								{
									if (a._naam == View.Items[i].Text) // gevonden naam
									{
										View.Items[i].SubItems[a._dagnummer].Text = a._afwijkingdienst;
									}
								}
							}
						}
					}

					ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur,15);

					// aantal bezetting regel
					int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);
					string[] maxlijst = new string[aantal_dagen_deze_maand + 1];
					//maxlijst[0] = "Aantal";
					ListViewItem item_max = new ListViewItem(maxlijst);
					View.Items.Add(item_max);
					int dag;
					int aantal_mensen;// = ProgData.ListPersoneelKleur.Count;

					List<string> TelNietMeeNamen = new List<string>();
					string locatie = @"telnietmee.ini";
					try
					{
						TelNietMeeNamen = File.ReadAllLines(locatie).ToList();
					}
					catch { }

					for (dag = 1; dag < aantal_dagen_deze_maand + 1; dag++) // aantal dagen
					{
						//string wacht = View.Items[3].SubItems[dag].Text;
						aantal_mensen = ProgData.ListPersoneelKleur.Count;
						for (int i = 4; i < View.Items.Count; i++) // alle namen/rows
						{
							if (!string.IsNullOrEmpty(View.Items[i].SubItems[dag].Text))
								aantal_mensen--;
							if (TelNietMeeNamen.Contains(View.Items[i].SubItems[dag].Text))
								aantal_mensen++;
						}
						if (!string.IsNullOrEmpty(View.Items[3].SubItems[dag].Text))
							View.Items[View.Items.Count - 1].SubItems[dag].Text = aantal_mensen.ToString();
						if (aantal_mensen < InstellingenProg._MinimaalAantalPersonen)
						{
							View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
							View.Items[View.Items.Count - 1].SubItems[dag].BackColor = MinimaalPersonen_;
						}
					}

					// extra diensten
					DateTime dat = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, 1);
					string dir = ProgData.GetDirectoryBezettingMaand(dat);
					ProgData.LoadLooptExtraLijst(dir, ProgData.GekozenKleur);
					if (ProgData.ListLooptExtra.Count > 0)
					{
						// extra diensten regel
						string[] extralijst = new string[aantal_dagen_deze_maand + 1];
						extralijst[0] = "Extra dienst";
						ListViewItem item_extra = new ListViewItem(extralijst);
						View.Items.Add(item_extra);
						foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
						{
							int dagy = ex._datum.Day;
							if (string.IsNullOrEmpty(View.Items[View.Items.Count - 1].SubItems[dagy].Text))
							{
								View.Items[View.Items.Count - 1].SubItems[dagy].Text = "1";
							}
							else
							{
								int inhoud = int.Parse(View.Items[View.Items.Count - 1].SubItems[dagy].Text);
								inhoud++;
								View.Items[View.Items.Count - 1].SubItems[dagy].Text = inhoud.ToString();
							}
						}
					}
				}
			}

			if (waarintijd == 2 || waarintijd == 3)
			{
				// huidig of toekomst

				ProgData.Lees_Namen_lijst();            // lees alle mensen in sectie , personeel_lijst
				ProgData.MaakPloegNamenLijst(ProgData.GekozenKleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
				ProgData.SavePloegNamenLijst(ProgData.GekozenKleur,15);     // save ploegbezetting (de mensen)

				// maak bezettingafwijking.bin voor kleur als die niet bestaat
				// is lijst met werkdagen
				//if (!File.Exists(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)))
				if (!File.Exists(ProgData.Ploeg_Veranderingen_Locatie(ProgData.GekozenKleur)))
				{
					ProgData.MaakLegeBezetting(/*ProgData.Sgekozenjaar(), ProgData.igekozenmaand.ToString(),*/ ProgData.GekozenKleur); // in deze roetine wordt het ook opgeslagen
				}

				CheckEnDealVerhuizing();

				// 1) Haal Ploeg Bezetting
				ProgData.LoadPloegNamenLijst(ProgData.GekozenKleur,15);

				// 2) Zet ploeg en werkplek op scherm
				for (int i = 0; i < ProgData.ListWerkgroepPersoneel.Count; i++)
				{
					// eerst naam werkplek
					string[] werkplek = new string[33];
					werkplek[0] = ProgData.ListWerkgroepPersoneel[i];
					ListViewItem item = new ListViewItem(werkplek);
					View.Items.Add(item);
					View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
					for (int grijzebalk_werkplek = 0; grijzebalk_werkplek < 33; grijzebalk_werkplek++)
					{
						View.Items[View.Items.Count - 1].SubItems[grijzebalk_werkplek].BackColor = Werkplek_;
					}

					// zet in View
					foreach (personeel a in ProgData.ListPersoneelKleur)
					{
						string[] naamlijst = new string[33];
						if (a._werkgroep == ProgData.ListWerkgroepPersoneel[i])
						{
							naamlijst[0] = a._achternaam;
							ListViewItem item_naam = new ListViewItem(naamlijst);
							View.Items.Add(item_naam);
						}
					}
				}

				// Vul bezetting op scherm
				if (File.Exists(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur)))
				{
					ProgData.LoadPloegBezetting(ProgData.GekozenKleur, 15);
					foreach (werkdag a in ProgData.ListWerkdagPloeg)
					{
						if (!string.IsNullOrEmpty(a._afwijkingdienst))
						{
							for (int i = 0; i < View.Items.Count; i++) // alle namen/rows
							{
								if (a._naam == View.Items[i].Text) // gevonden naam
								{
									View.Items[i].SubItems[a._dagnummer].Text = a._afwijkingdienst;
								}
							}
						}
					}
				}

				ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur,15);

				// aantal bezetting regel
				int aantal_dagen_deze_maand = DateTime.DaysInMonth(ProgData.Igekozenjaar, ProgData.igekozenmaand);
				string[] maxlijst = new string[aantal_dagen_deze_maand + 1];
				//maxlijst[0] = "Aantal";
				ListViewItem item_max = new ListViewItem(maxlijst);
				View.Items.Add(item_max);
				int dag;
				int aantal_mensen;// = ProgData.ListPersoneelKleur.Count;

				List<string> TelNietMeeNamen = new List<string>();
				string locatie = @"telnietmee.ini";
				try
				{
					TelNietMeeNamen = File.ReadAllLines(locatie).ToList();
				}
				catch
				{
					MessageBox.Show("telnietmee.ini niet gevonden");
				}

				for (dag = 1; dag < aantal_dagen_deze_maand + 1; dag++) // aantal dagen
				{
					aantal_mensen = ProgData.ListPersoneelKleur.Count;
					//string wacht = View.Items[3].SubItems[dag].Text;
					for (int i = 4; i < View.Items.Count; i++) // alle namen/rows
					{
						if (!string.IsNullOrEmpty(View.Items[i].SubItems[dag].Text))
							aantal_mensen--;
						if (TelNietMeeNamen.Contains(View.Items[i].SubItems[dag].Text))
							aantal_mensen++;
					}
					if (!string.IsNullOrEmpty(View.Items[3].SubItems[dag].Text))
						View.Items[View.Items.Count - 1].SubItems[dag].Text = aantal_mensen.ToString();
					if (aantal_mensen < InstellingenProg._MinimaalAantalPersonen)
					{
						View.Items[View.Items.Count - 1].UseItemStyleForSubItems = false;
						View.Items[View.Items.Count - 1].SubItems[dag].BackColor = MinimaalPersonen_;
					}
				}

				// extra diensten
				DateTime dat = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, 1);
				string dir = ProgData.GetDirectoryBezettingMaand(dat);
				ProgData.LoadLooptExtraLijst(dir, ProgData.GekozenKleur);
				if (ProgData.ListLooptExtra.Count > 0)
				{
					// extra diensten regel
					string[] extralijst = new string[aantal_dagen_deze_maand + 1];
					extralijst[0] = "Extra dienst";
					ListViewItem item_extra = new ListViewItem(extralijst);
					View.Items.Add(item_extra);
					foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
					{
						int dagy = ex._datum.Day;
						if (string.IsNullOrEmpty(View.Items[View.Items.Count - 1].SubItems[dagy].Text))
						{
							View.Items[View.Items.Count - 1].SubItems[dagy].Text = "1";
						}
						else
						{
							int inhoud = int.Parse(View.Items[View.Items.Count - 1].SubItems[dagy].Text);
							inhoud++;
							View.Items[View.Items.Count - 1].SubItems[dagy].Text = inhoud.ToString();
						}
					}
				}
			}
			labelDebug.Text = "";
		}

		private void CheckEnDealVerhuizing()
		{
			ProgData.Lees_Namen_lijst();            // lees alle mensen in sectie , personeel_lijst
													// check of er vorige maand mensen zijn verhuisd

			IEnumerable<personeel> persoon = from a in ProgData.ListPersoneel
											 where (!string.IsNullOrEmpty(a._nieuwkleur))
											 select a;

			foreach (personeel a in persoon)
			{
				// als verhuis datum-maand in verleden is tov huidige maand,
				// aanpassen.
				DateTime overgang = new DateTime(a._verhuisdatum.Year, a._verhuisdatum.Month, 1);
				overgang = overgang.AddMonths(1);
				DateTime eerste_maand = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 1);

				if (overgang <= eerste_maand) // roetine CheckEnDealVerhuizing wordt alleen aangeroepen
											  // bij waarintijd = 2
				{
					string bewaar = ProgData.GekozenKleur;
					if (a._nieuwkleur == null) // geen idee waar dit soms gebeurt
						a._nieuwkleur = "";
					if (!string.IsNullOrEmpty(a._nieuwkleur))
						a._kleur = a._nieuwkleur;
					a._nieuwkleur = "";
					if (a._nieuwkleur == null) // geen idee waar dit soms gebeurt
						a._nieuwkleur = "";
					ProgData.Save_Namen_lijst();
					ProgData.GekozenKleur = a._kleur;
					ProgData.MaakPloegNamenLijst(a._kleur); // bepaal alle mensen in een kleur, kleur_personeel_lijst
					ProgData.SavePloegNamenLijst(a._kleur, 15);         // save ploegbezetting (de mensen)
					ProgData.GekozenKleur = bewaar;
				}
			}
		}

		private static DateTime EasterSunday(int year)
		{
			int day;// = 0;
			int month;// = 0;

			int g = year % 19;
			int c = year / 100;
			int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
			int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

			day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
			month = 3;

			if (day > 31)
			{
				month++;
				day -= 31;
			}

			return new DateTime(year, month, day);
		}


        // Geklikt op view scherm, open invoer form
        private void View_MouseClick(object sender, MouseEventArgs e)
		{
			//Point point = new Point(e.X, e.Y);
			ListViewHitTestInfo info = View.HitTest(e.X, e.Y);
			int row = info.Item.Index;
			int col = info.Item.SubItems.IndexOf(info.SubItem);

			if (ProgData.WaarInTijd() == 1)
			{
				if (col > 0 && row < 4)
				{
					HistoryForm his = new HistoryForm();
					his.comboBoxDag.Text = col.ToString();
					his.ShowDialog();
				}
				else
				{
					MessageBox.Show("In verleden kunt u alleen kijken, niet meer aanpassen!");
				}
			}
			else
			{
				if (((ProgData.RechtenHuidigeGebruiker > 24) && (ProgData.RechtenHuidigeGebruiker < 51) && (ProgData.Huidige_Gebruiker_Werkt_Op_Kleur == ProgData.GekozenKleur))
					|| ProgData.RechtenHuidigeGebruiker > 51)
				{
					try
					{
						string value = info.Item.SubItems[col].Text;
						//MessageBox.Show(string.Format("R{0}:C{1} val '{2}'", row, col, value));

						if (col > 0 && row < 4)
						{
							HistoryForm his = new HistoryForm();
							his.comboBoxDag.Text = col.ToString();
							his.ShowDialog();
						}

						if (col != 0 && View.Items[row].SubItems[0].BackColor != Werkplek_)
						{
							string gekozen_naam = info.Item.SubItems[0].Text;
							string gekozen_datum = col.ToString();

							personeel persoon = ProgData.ListPersoneelKleur.First(a => a._achternaam == gekozen_naam);

							if (e.Button == MouseButtons.Right)
							{
								
								quick.Location = new System.Drawing.Point(e.Location.X + this.Location.X + 180, e.Location.Y + this.Location.Y + 60);
								quick.ShowDialog();
								if (quick.listBox1.SelectedIndex > -1)
								{
									string afwijking = quick.listBox1.SelectedItem.ToString();
									switch (afwijking)
									{
										case "Wis":
											string eerste_2 = "";
											if (value.Length > 2)
												eerste_2 = value.Substring(0, 2);

											if (eerste_2 == "ED" || eerste_2 == "VD" || eerste_2 == "RD")
											{
												MessageBox.Show("Wis een extra/ruil/verschoven dienst aan met linker muisknop");
											}
											else
											{
												ProgData.RegelAfwijking(gekozen_naam, gekozen_datum, "", "Verwijderd", ProgData.Huidige_Gebruiker_Personeel_nummer, ProgData.GekozenKleur);
											}
											break;
										case "Copy":
											quick.listBox1.Items[2] = info.SubItem.Text;
											// extra,ruil of vd niet op deze manier
											if (info.SubItem.Text.Length > 3)
											{
												string test = info.SubItem.Text.Substring(0, 2);
												if (test == "ED" || test == "RD" || test == "VD")
												{
													quick.listBox1.Items[2] = "*****";
													MessageBox.Show("Extra of verschoven diensten kunt u niet zo invoeren");
												}
											}
											break;
										case "*****":
											break;
										default:
											ProgData.RegelAfwijking(gekozen_naam, gekozen_datum, afwijking, "", ProgData.Huidige_Gebruiker_Personeel_nummer, ProgData.GekozenKleur);
											ProgData.NachtErVoorVrij(gekozen_naam, gekozen_datum, afwijking);
											break;
									}
									VulViewScherm();
								}
							}
							else
							{
								DagAfwijkingInvoerForm afw = new DagAfwijkingInvoerForm();
								afw.labelNaam.Text = gekozen_naam;
								afw.labelDatum.Text = gekozen_datum;
								afw.labelMaand.Text = ProgData.Sgekozenmaand();
								afw.labelPersoneelnr.Text = persoon._persnummer.ToString();
								afw.Text = ProgData.Huidige_Gebruiker_Personeel_nummer;
								// voor ed-o ed-m en ed-n
								afw._verzoekdag = new DateTime(ProgData.Igekozenjaar, ProgData.igekozenmaand, col);
								afw.ShowDialog();
								VulViewScherm();
							}
						}
					}
					catch { }
				}
				// geen rechten/ingelogt
				else
				{
					MessageBox.Show("Even inloggen!");
				}
			}
		}

		private void ButtonNu_Click(object sender, EventArgs e)
		{
			//DateTime nu = DateTime.Now;

			ProgData.igekozenmaand = ProgData.ihuidigemaand;
			ProgData.Igekozenjaar = ProgData.ihuidigjaar;

			KleurMaandButton();
			VulViewScherm();
		}

		private void WachtOverzichtToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (InstellingenProg._Wachtoverzicht2Dagen && ProgData.RechtenHuidigeGebruiker < 101)
			{
				OverzichtWachtForm2Dagen owacht2 = new OverzichtWachtForm2Dagen();
				owacht2.ShowDialog();

				ButtonNu_Click(this, null);
			}
			else
			{
				OverzichtWachtForm wacht = new OverzichtWachtForm();
				ProgData.GekozenKleur = comboBoxKleurKeuze.Text;
				wacht.labelKleur.Text = ProgData.GekozenKleur;
				wacht.ShowDialog();
				ButtonNu_Click(this, null);
			}
		}

		private void View_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				ListViewItem item = View.GetItemAt(e.X, e.Y);
				ListViewHitTestInfo info = View.HitTest(e.X, e.Y);

				toolStripStatusLabelInfo.Text = "";
				toolStripStatusRedeAfwijking.Text = "";

				if ((item != null) && (!string.IsNullOrEmpty(info?.SubItem?.Text)))
				{
					int row = info.Item.Index;
					int col = info.Item.SubItems.IndexOf(info.SubItem);
					// extra dienst
					string aant = View.Items[View.Items.Count - 1].Text;
					if (row == View.Items.Count - 1 && aant == "Extra dienst")
					{
						if (info.SubItem.Text == "1") // 1 extra dienst
						{
							foreach (LooptExtraDienst ex in ProgData.ListLooptExtra)
							{
								if (col == ex._datum.Day)
								{
									toolStripStatusLabelInfo.Text = ex._naam;
									if (!string.IsNullOrEmpty(toolStripStatusLabelInfo.Text) && mLastPos != e.Location)
										mTooltip.Show(toolStripStatusLabelInfo.Text, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
								}
							}
						}
						else
						{
							toolStripStatusLabelInfo.Text = "extra dienst nog te doen"; // nog te doen
																						//if (toolStripStatusRedeAfwijking.Text != "" && toolStripStatusRedeAfwijking.Text != " " && mLastPos != e.Location)
																						//    mTooltip.Show(toolStripStatusRedeAfwijking.Text, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
						}
					}
					// rede afwijking
					if (col > 0 && row > 3 && row < View.Items.Count - 1)
					{
						toolStripStatusLabelInfo.Text = info.Item.Text + " " + info.SubItem.Text;
						toolStripStatusRedeAfwijking.Text = GetRedenAfwijking(info.Item.Text, col);
						if (!string.IsNullOrEmpty(toolStripStatusRedeAfwijking.Text) && toolStripStatusRedeAfwijking.Text != " " && mLastPos != e.Location)
						{
							mTooltip.Show(toolStripStatusRedeAfwijking.Text, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
                        }
					}
					// personeel nummer bij naam
					if (col == 0 && row > 3 && row < View.Items.Count - 1)
					{
						string naam = View.Items[row].Text;
						string persnummer = ProgData.Get_Gebruiker_Nummer(naam);
						if (!string.IsNullOrEmpty(persnummer) && mLastPos != e.Location)
							mTooltip.Show(persnummer, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
					}
					// feestdag
					if (col > 0 && row < 5) // feestdag info ?
					{
						if (View.Items[row].SubItems[col].BackColor == Feestdag_)
						{
							int maand = ProgData.igekozenmaand;
							DateTime pasen = EasterSunday((int)numericUpDownJaar.Value);
							DateTime pasen2 = pasen.AddDays(1);
							DateTime hemelsvaart = pasen.AddDays(39);
							DateTime pinksteren = hemelsvaart.AddDays(10); // 1ste pinsterdag
							DateTime pinksteren2 = pinksteren.AddDays(1); // 2ste pinsterdag
							int modulo = (int)numericUpDownJaar.Value % 5; // als vijf mei is 0, dan vrij
							if (maand == 1 && col == 1) toolStripStatusRedeAfwijking.Text = "Nieuwsjaar dag";
							if (maand == 4 && col == 27) toolStripStatusRedeAfwijking.Text = "Konings dag";
							if (maand == 12 && col == 25) toolStripStatusRedeAfwijking.Text = "Eerste Kerstdag";
							if (maand == 12 && col == 26) toolStripStatusRedeAfwijking.Text = "Tweede Kerstdag";
							if (maand == 5 && col == 5 && modulo == 0) toolStripStatusRedeAfwijking.Text = "Bevrijdings dag";
							if (maand == pasen.Month && col == pasen.Day) toolStripStatusRedeAfwijking.Text = "Eerste Paasdag";
							if (maand == pasen2.Month && col == pasen2.Day) toolStripStatusRedeAfwijking.Text = "Tweede Paasdag";
							if (maand == hemelsvaart.Month && col == hemelsvaart.Day) toolStripStatusRedeAfwijking.Text = "Hemelsvaart dag";
							if (maand == pinksteren.Month && col == pinksteren.Day) toolStripStatusRedeAfwijking.Text = "Eerste Pinsterdag";
							if (maand == pinksteren2.Month && col == pinksteren2.Day) toolStripStatusRedeAfwijking.Text = "Tweede Pinsterdag";

							if (!string.IsNullOrEmpty(toolStripStatusRedeAfwijking.Text) && mLastPos != e.Location)
								mTooltip.Show(toolStripStatusRedeAfwijking.Text, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
						}
					}
				}
				if (item != null && (string.IsNullOrEmpty(info?.SubItem?.Text) && checkBoxHoverNaam.Checked))
				{
					try
					{
						int row1 = info.Item.Index;
						mTooltip.Show(View.Items[row1].Text, info.Item.ListView, e.X + 15, e.Y + 15, 1000);
					}
					catch { }
				}
				mLastPos = e.Location;
			}
			catch { }
		}

		private string GetRedenAfwijking(string naam, int dag)
		{
			if (ProgData.ListVeranderingen.Count < 1)
				ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur,15);
			string sdag = dag.ToString(CultureInfo.CurrentCulture);
			try
			{
				if (ProgData.ListVeranderingen.Count > 0)
				{
					veranderingen ver = ProgData.ListVeranderingen.Last(a => (a._naam == naam) && (a._datumafwijking == sdag));
					return ver._rede;
				}
			}
			catch { }
			return "";
		}

		private void KleurLijnenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// zet kleur lijnen
			ZetKleurLijnen kleurLijnen = new ZetKleurLijnen();
			kleurLijnen.ShowDialog();
			VulViewScherm();
		}

		private void ZetLijnen()
		{
			// lijn 1
			if (bool.Parse(ProgData.Lijnen[0]))
			{
				label1.Visible = true;
				label1.Text = ProgData.Lijnen[4];
				panel5.Visible = true;
				panel1.Visible = true;
				int start = int.Parse(ProgData.Lijnen[8]);
				int stop = int.Parse(ProgData.Lijnen[12]);
				panel1.Location = new System.Drawing.Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn);
				panel1.Size = new Size((stop - start + 1) * kolom_breed, 3);
			}
			// lijn 2
			if (bool.Parse(ProgData.Lijnen[1]))
			{
				label2.Visible = true;
				label2.Text = ProgData.Lijnen[5];
				panel6.Visible = true;
				panel2.Visible = true;
				int start = int.Parse(ProgData.Lijnen[9]);
				int stop = int.Parse(ProgData.Lijnen[13]);
				panel2.Location = new System.Drawing.Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (1 * y_as_add_lijn));
				panel2.Size = new Size((stop - start + 1) * kolom_breed, 3);
			}
			// lijn 3
			if (bool.Parse(ProgData.Lijnen[2]))
			{
				label3.Visible = true;
				label3.Text = ProgData.Lijnen[6];
				panel7.Visible = true;
				panel3.Visible = true;
				int start = int.Parse(ProgData.Lijnen[10]);
				int stop = int.Parse(ProgData.Lijnen[14]);
				panel3.Location = new System.Drawing.Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (2 * y_as_add_lijn));
				panel3.Size = new Size((stop - start + 1) * kolom_breed, 3);
			}
			// lijn 4
			if (bool.Parse(ProgData.Lijnen[3]))
			{
				label4.Visible = true;
				label4.Text = ProgData.Lijnen[7];
				panel8.Visible = true;
				panel4.Visible = true;
				int start = int.Parse(ProgData.Lijnen[11]);
				int stop = int.Parse(ProgData.Lijnen[15]);
				panel4.Location = new System.Drawing.Point(View.Location.X + 143 + ((start - 1) * kolom_breed), y_as_eerste_lijn + (3 * y_as_add_lijn));
				panel4.Size = new Size((stop - start + 1) * kolom_breed, 3);
			}
		}

		private void LijnenWeg()
		{
			label1.Visible = false;
			label2.Visible = false;
			label3.Visible = false;
			label4.Visible = false;
			panel1.Visible = false;
			panel2.Visible = false;
			panel3.Visible = false;
			panel4.Visible = false;
			panel5.Visible = false;
			panel6.Visible = false;
			panel7.Visible = false;
			panel8.Visible = false;
		}

		private void RepareerPloegAfwijkingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// test of blauw_bezetting.bin bestaat
			MessageBox.Show($"Ploeg bezetting {0} bestaat niet\nOf is corrupt, Kijken wat ik kan doen", ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur));
			//string Locatie = Path.GetFullPath(ProgData.GetDir() + "\\" + ProgData.GekozenKleur + "_afwijkingen.bin");
			if (File.Exists(ProgData.Ploeg_Veranderingen_Locatie(ProgData.GekozenKleur)))
			{
				MessageBox.Show($"Ploeg veranderingen bestaat wel, repareren!");
				File.Delete(ProgData.Ploeg_Bezetting_Locatie(ProgData.GekozenKleur));
				ProgData.MaakLegeBezetting(/*ProgData.Sgekozenjaar(), ProgData.Sgekozenmaand(),*/ ProgData.GekozenKleur);
				ProgData.LoadVeranderingenPloeg(ProgData.GekozenKleur,15);
				ProgData.LoadPloegBezetting(ProgData.GekozenKleur, 15);
				foreach (veranderingen verander in ProgData.ListVeranderingen)
				{
					werkdag ver = ProgData.ListWerkdagPloeg.First(a => (a._naam == verander._naam) && (a._dagnummer.ToString() == verander._datumafwijking));
					ver._afwijkingdienst = verander._afwijking;
				}
				ProgData.SavePloegBezetting(ProgData.GekozenKleur, 15);
				VulViewScherm();
			}
		}

		private void TimerKill_Tick(object sender, EventArgs e)
		{
			if (File.Exists("kill.ini"))
			{
				// als kill.ini bestaat sluit programma
				if (kill)
				{
					Close();
				}
				else
				{
					kill = true;
					MessageBox.Show("programma wordt gesloten over 30 sec, er is een update, moment");
				}
			}

			if (ProgData.backup_time == DateTime.Now.Minute)
			{
				if (!File.Exists(ProgData.backup_zipnaam))
				{
					timerKill.Enabled = false;
					labelDebug.Text = "Dagelijkse Backup, moment.....";
					ProgData.Backup();
					timerKill.Enabled = true;
					labelDebug.Text = "Dagelijkse Backup gelukt";
				}
			}
		}

		private void RuilOverwerkToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RuilExtraForm rf = new RuilExtraForm();
			if (ProgData.Huidige_Gebruiker_Personeel_nummer != "Admin")
			{
				if (ProgData.Huidige_Gebruiker_Personeel_nummer != "Niemand Ingelogd")
				{
					//int personeel_nr = int.Parse(ProgData.Huidige_Gebruiker_Personeel_nummer);
					//personeel persoon = ProgData.personeel_lijst.First(a => a._persnummer == personeel_nr);
					rf.labelNaam.Text = ProgData.Huidige_Gebruiker_Naam();
				}
				else
				{
					rf.labelNaam.Text = "Niet ingelogd";
				}
			}
			else
			{
				rf.labelNaam.Text = "Admin";
			}
			rf.buttonVraagAan.Enabled = ProgData.RechtenHuidigeGebruiker > 0;
			rf.ShowDialog();
		}

		private void SnipperDagAanvraagToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SnipperAanvraagForm snip = new SnipperAanvraagForm();
			snip.labelNaam.Text = ProgData.Huidige_Gebruiker_Personeel_nummer;
			snip.labelNaamFull.Text = ProgData.Huidige_Gebruiker_Naam();
			snip.ShowDialog();
		}

		private void InstellingenProgrammaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			instellingen_programma.ShowDialog();
			MainFormBezetting2_Shown(this, null);
		}

		private void MainFormBezetting2_FormClosing(object sender, FormClosingEventArgs e)
		{
			Thread.Sleep(300);
			ProgData.CaptureMainScreen();
		}

		private void ImportOudeVeranderDataOudeVersieToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Gaat soms fout als iemand in tussen tijd verhuisd is!");
			MessageBox.Show("Delete de komende 25 maanden in de toekomst, en maak lege");

			DateTime nu = DateTime.Now;

            for (int i = 0; i < 25; i++)
            {
				nu = nu.AddMonths(1);
				string path = Path.GetFullPath($"{nu.Year}\\{nu.Month}"); // maand als nummer
				if (Directory.Exists(path))
                {
                    Directory.Delete(path,true); // delete met inhoud
                }
				_ = Directory.CreateDirectory(path);
				ProgData.Igekozenjaar = nu.Year;
				ProgData.igekozenmaand = nu.Month;
				ProgData.MaakPloegNamenLijst("Blauw");
				ProgData.SavePloegNamenLijst("Blauw",15);
				ProgData.MaakPloegNamenLijst("Rood");
				ProgData.SavePloegNamenLijst("Rood",15);
				ProgData.MaakPloegNamenLijst("Wit");
				ProgData.SavePloegNamenLijst("Wit",15);
				ProgData.MaakPloegNamenLijst("Groen");
				ProgData.SavePloegNamenLijst("Groen",15);
				ProgData.MaakPloegNamenLijst("Geel");
				ProgData.SavePloegNamenLijst("Geel",15);
				ProgData.MaakPloegNamenLijst("DD");
				ProgData.SavePloegNamenLijst("DD",15);
			}

			openFileDialog.FileName = "";
			//openFileDialog.Filter = "(*.Bez)|*.Bez";
			MessageBox.Show("Open oude data bez file. (Wijz...Bez)");
			DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.
			if (result == DialogResult.OK) // Test result.
			{
				MessageBox.Show("Dit gaat tijdje duren, geduld..... (10 min)\nAl ingevulde data wordt overschreven!");
				ProgData.Disable_error_Meldingen = true;
				ProgData.Lees_Namen_lijst();
				OpenDataBase_en_Voer_oude_data_in_Bezetting(openFileDialog.FileName);
				//MessageBox.Show("Klaar met invoer, start programma opnieuw op.");
				ProgData.GekozenKleur = "Blauw";
				ButtonNu_Click(this, null);
				ProgData.Disable_error_Meldingen = false;
				Close();
            }
		}

		private void OpenDataBase_en_Voer_oude_data_in_Bezetting(string file)
		{
			WindowUpdateViewScreen = false;
			using (OleDbConnection connection =
				new OleDbConnection($"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \"{file}\"; Jet OLEDB:Database Password = fcl721"))
			{
				
                bool read;
                int teller = 0;

				OleDbCommand command = new OleDbCommand("select * from Wijzeging", connection);

				connection.Open();
				OleDbDataReader reader = command.ExecuteReader();
				WindowUpdateViewScreen = false;
				DateTime inladen_vanaf_datum = DateTime.Now;
				//inladen_vanaf_datum = inladen_vanaf_datum.AddMonths(-3);
				ProgData.Lees_Namen_lijst();            // lees alle mensen in sectie , personeel_lijst
														// check of er vorige maand mensen zijn verhuisd

				if (reader.Read() == true)
				{
					do
					{
						Application.DoEvents();
						object[] meta = new object[12]; // zodat ze leeg zijn elke keer ivm vorige data
						// inlezen waarden
						_ = reader.GetValues(meta);

                        labelDebug.Text = $"{teller++}";
						labelDebug.Refresh();

						//Console.Write("{0} ", meta[2].ToString()); // pers nummer persoon
						//Console.Write("{0} ", meta[3].ToString()); // naam persoon
						//Console.Write("{0} ", meta[6].ToString()); // datum invoer
						//Console.Write("{0} ", meta[7].ToString()); // datum afwijking
						//Console.Write("{0} ", meta[5].ToString()); // afwijking
						//Console.Write("{0} ", meta[9].ToString()); // personeel nummer invoerder
						//Console.Write("{0} ", meta[11].ToString()); // rede

						string[] datum = new string[12];

						datum = meta[7].ToString().Split('-');
						datum[2] = datum[2].Substring(0, 4);

						DateTime datum_afwijking = new DateTime(int.Parse(datum[2]), int.Parse(datum[1]), int.Parse(datum[0]));

						if ((datum_afwijking > inladen_vanaf_datum) && (ProgData.Bestaat_Gebruiker(meta[2].ToString())))
						{
							try
							{
								string kleur = ProgData.Get_Gebruiker_Kleur(meta[2].ToString());

								// in oude programma is afwijking soms gelijk aan orginele dienst
								// die hoef ik in te voeren
								if (meta[5].ToString() == "O" || meta[5].ToString() == "M" || meta[5].ToString() == "N")
								{
									if (ProgData.MDatum.GetDienst("5PL", datum_afwijking, kleur) == meta[5].ToString())
									{
										kleur = "niet invoeren";
									}
								}



								if (kleur == "Blauw" || kleur == "Geel" || kleur == "Groen" || kleur == "Rood" || kleur == "Wit" || kleur == "DD")
								{
									string naam = meta[3].ToString();
									string invoer_naam = ProgData.Get_Gebruiker_Naam(meta[9].ToString());
									string rede = meta[11].ToString();
									string afwijking = meta[5].ToString().ToUpper();

									//gaat fout als persoon ondertussen op andere kleur zit

									IEnumerable<personeel> persoon = from a in ProgData.ListPersoneel
																		where (a._achternaam == naam)
																		where (!string.IsNullOrEmpty(a._nieuwkleur))
																		select a;

									foreach (personeel a in persoon)
									{
										// als verhuis datum-maand in verleden is tov huidige maand,
										// aanpassen.
										DateTime overgang = new DateTime(a._verhuisdatum.Year, a._verhuisdatum.Month, a._verhuisdatum.Day);

										if (overgang <= datum_afwijking)
										{
											kleur = a._nieuwkleur;
										}
									}
									ProgData.RegelAfwijkingOpDatumEnKleur(datum_afwijking, kleur, naam, datum[0], afwijking, rede, "Import " + invoer_naam);
								}
							}
							catch { }
						}
						read = reader.Read();
					} while (read == true);
				}
				reader.Close();
			}
			WindowUpdateViewScreen = true;
		}

		private void OpenDataBase_en_Voer_Oude_Namen_In(string file)
		{
			using (OleDbConnection connection =
							new OleDbConnection($"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = \"{file}\"; Jet OLEDB:Database Password = fcl721"))
			{
				object[] meta = new object[20];
                bool read;

                OleDbCommand command = new OleDbCommand("select * from Adresen", connection);

				connection.Open();
				OleDbDataReader reader = command.ExecuteReader();

				if (reader.Read() == true)
				{
					//DateTime nu = DateTime.Now;
					do
					{
						System.Windows.Forms.Application.DoEvents();
                        _ = reader.GetValues(meta);

                        //Console.Write("{0} ", meta[2].ToString()); // pers nummer persoon
                        //Console.Write("{0} ", meta[6].ToString()); // datum invoer
                        //Console.Write("{0} ", meta[7].ToString()); // datum afwijking
                        //Console.Write("{0} ", meta[5].ToString()); // afwijking
                        //Console.Write("{0} ", meta[9].ToString()); // personeel nummer invoerder
                        //Console.Write("{0} ", meta[11].ToString()); // rede

                        try
						{
							personeel p = new personeel
							{
								_persnummer = int.Parse(meta[0].ToString()),
								_achternaam = meta[1].ToString(),
								_voornaam = meta[2].ToString(),
								_adres = meta[3].ToString(),
								_postcode = meta[4].ToString(),
								_woonplaats = meta[5].ToString(),
								_telthuis = meta[6].ToString(),
								_tel06prive = meta[7].ToString(),
								_telwerk = meta[8].ToString(),
								_emailwerk = meta[9].ToString(),
								_emailthuis = meta[10].ToString(),
								_adrescodewerk = meta[11].ToString(),
								_funtie = meta[12].ToString(),
								_kleur = meta[13].ToString(),
								_nieuwkleur = "",
								_verhuisdatum = DateTime.Now,
								_tel06werk = meta[14].ToString(),
								_werkgroep = meta[15].ToString(),
								_vuilwerk = meta[16].ToString(),
								_passwoord = "",
								_rechten = 0,
								_reserve1 = "",
								_reserve2 = "",
								_reserve3 = "",
								_reserve4 = "",
								_reserve5 = ""
							};
							ProgData.ListPersoneel.Add(p);
						}
						catch { }
						read = reader.Read();
					} while (read == true);
				}
				reader.Close();
			}
			ProgData.Save_Namen_lijst();
		}

		private void CloseExitStopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Thread.Sleep(300);
			//ProgData.CaptureMainScreen();
			Close();
		}

        private void removeAutoInlogOnderDitWindowsAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string autoinlogfile = $"{directory}\\bezetting2.log";
			if (File.Exists(autoinlogfile))
			{
				File.Delete(autoinlogfile);
			}
		}
    }
}