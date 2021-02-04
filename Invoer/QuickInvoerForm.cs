using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezetting2.Invoer
{
    public partial class QuickInvoerForm : Form
    {
        private bool Exit;
        public QuickInvoerForm()
        {
            InitializeComponent();
        }

        private void QuickInvoerForm_Shown(object sender, EventArgs e)
        {
            Exit = false;
            List<string> PopUpNamen = new List<string>();
            string locatie = @"BezData\\popupmenu.ini";
            try
            {
                PopUpNamen = File.ReadAllLines(locatie).ToList();
                PopUpNamen.RemoveAt(0); // de help text;
                listBox1.DataSource = PopUpNamen;
            }
            catch { }
            Exit = true;
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Exit)
                Close();
        }
    }
}
