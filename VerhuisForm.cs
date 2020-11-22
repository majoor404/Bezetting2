using System;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class VerhuisForm : Form
    {
        public VerhuisForm()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxNieuwRooster.Text))
                Close();
        }
    }
}