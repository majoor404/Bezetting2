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
    public partial class VulAanvraagInForm : Form
    {
        public VulAanvraagInForm()
        {
            InitializeComponent();

            labelNaam.Text = "";
            labelDatum.Text = "";
            labelDienst.Text = "";
            labelKleur.Text = "";
            textBoxAfwijking.Text = "";
            textBoxRede.Text = "";
        }
    }
}
