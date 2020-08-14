using Bezetting2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
