using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();
            textBoxDebug.Text = "";
        }

        public void End()
        {
            buttonClose.Text = "Close (3)";
            this.Refresh();
            Thread.Sleep(1000);
            buttonClose.Text = "Close (2)";
            this.Refresh();
            Thread.Sleep(1000);
            buttonClose.Text = "Close (1)";
            this.Refresh();
            Thread.Sleep(1000);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Size = new Size(0, 0);
        }
    }
}
