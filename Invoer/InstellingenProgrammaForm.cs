using Bezetting2.Data;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Bezetting2.Invoer
{
    public partial class InstellingenProgrammaForm : Form
    {
        public InstellingenProgrammaForm()
        {
            InitializeComponent();
        }

        private void InstellingenProgrammaForm_Shown(object sender, EventArgs e)
        {
            checkBoxGebruikRuilExtra.Checked = InstellingenProg._GebruikExtraRuil;
            checkBoxGebruikSnipper.Checked = InstellingenProg._GebruikSnipper;
            textBoxMinAantalPersonen.Text = InstellingenProg._MinimaalAantalPersonen.ToString(CultureInfo.CurrentCulture);
            comboBoxRooster.Text = InstellingenProg._Rooster;
            checkBoxTelVakAlsVK.Checked = InstellingenProg._TelVakAlsVK;
            checkBoxWachtoverzichtAls2Dagen.Checked = InstellingenProg._Wachtoverzicht2Dagen;
            textBoxLocatieKalender.Text = InstellingenProg._LocatieKalender;
            checkBoxGebruikWachtOverzicht.Checked = InstellingenProg._GebruikWachtOverzicht;
            checkBoxSorteerOpWerkgroep.Checked = InstellingenProg._SorteerOokWerkplek;
        }

        private void CheckBoxGebruikRuilExtra_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._GebruikExtraRuil = checkBoxGebruikRuilExtra.Checked;
        }

        private void CheckBoxGebruikSnipper_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._GebruikSnipper = checkBoxGebruikSnipper.Checked;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            InstellingenProg._MinimaalAantalPersonen = int.Parse(textBoxMinAantalPersonen.Text);
            InstellingenProg.SaveProgrammaData();
        }

        private void ComboBoxRooster_SelectedIndexChanged(object sender, EventArgs e)
        {
            InstellingenProg._Rooster = comboBoxRooster.Text;
        }

        private void CheckBoxTelVakAlsVK_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._TelVakAlsVK = checkBoxTelVakAlsVK.Checked;
        }

        private void checkBoxWachtoverzichtAls2Dagen_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._Wachtoverzicht2Dagen = checkBoxWachtoverzichtAls2Dagen.Checked;
        }

        private void textBoxLocatieKalender_TextChanged(object sender, EventArgs e)
        {
            InstellingenProg._LocatieKalender = textBoxLocatieKalender.Text;
        }

        private void checkBoxGebruikWachtOverzicht_CheckedChanged(object sender, EventArgs e)
        {
             InstellingenProg._GebruikWachtOverzicht = checkBoxGebruikWachtOverzicht.Checked;
        }

        //checkBoxSorteerOpWerkgroep
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._SorteerOokWerkplek = checkBoxSorteerOpWerkgroep.Checked;
        }
    }
}