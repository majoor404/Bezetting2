using Bezetting2.Data;
using System;
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
            textBoxMinAantalPersonen.Text = InstellingenProg._MinimaalAantalPersonen.ToString();
            comboBoxRooster.Text = InstellingenProg._Rooster;
            checkBoxTelVakAlsVK.Checked = InstellingenProg._TelVakAlsVK;
            checkBoxTelAalsVK.Checked = InstellingenProg._TelAalsVK;
        }

        private void checkBoxGebruikRuilExtra_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._GebruikExtraRuil = checkBoxGebruikRuilExtra.Checked;
        }



        private void checkBoxGebruikSnipper_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._GebruikSnipper = checkBoxGebruikSnipper.Checked;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            InstellingenProg._MinimaalAantalPersonen = int.Parse(textBoxMinAantalPersonen.Text);
            InstellingenProg.SaveProgrammaData();
        }

        private void comboBoxRooster_SelectedIndexChanged(object sender, EventArgs e)
        {
            InstellingenProg._Rooster = comboBoxRooster.Text;
        }

        private void checkBoxTelVakAlsVK_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._TelVakAlsVK = checkBoxTelVakAlsVK.Checked;
        }

      
        
        private void checkBoxTelAalsVK_CheckedChanged(object sender, EventArgs e)
        {
            InstellingenProg._TelAalsVK = checkBoxTelAalsVK.Checked;
        }
    }
}