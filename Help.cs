using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class Help : Form
    {
        string url = Application.StartupPath + @"\Help\help.html";

        public Help()
        {
            InitializeComponent();
        }

        public void Help_Shown(object sender, EventArgs e)
        {
            this.webBrowser.Navigate(url);  
        }
    }
}
