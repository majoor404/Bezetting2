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
    public partial class OverzichtWachtForm2Dagen : Form
    {
        public OverzichtWachtForm2Dagen()
        {
            InitializeComponent();
            
        }

        private void OverzichtWachtForm2_Shown(object sender, EventArgs e)
        {
            listBox2.Items.Add("nummer 1");
            listBox2.Items.Add("nummer 2");
            listBox2.Items.Add("nummer 3");
            listBox2.Items.Add("nummer 4");
            listBox1.Items.Add("nummer 1");
            listBox1.Items.Add("nummer 2");
            listBox1.Items.Add("nummer 3");
            listBox1.Items.Add("nummer 4");
        }

        private void listBox29_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                ListBox Sender = (ListBox)sender;
                string afw = Sender.Items[e.Index].ToString();
                if (afw.Length > 3) afw = afw.Substring(0, 3);
                if (afw.Length > 2 && afw.Substring(0, 2) == "EV")
                    afw = "EV";

                switch (afw)
                {
                    case "Z":
                    case "VK":
                    case "8OI":
                    case "VRI":
                    case "VAK":
                    case "VF":
                    case "OPL":
                    case "OI8":
                    case "EV":
                        e.Graphics.FillRectangle(Brushes.Lavender, e.Bounds);
                        break;
                    case "ED-":
                    case "VD-":
                    case "RD-":
                        e.Graphics.FillRectangle(Brushes.LightSalmon, e.Bounds);
                        break;
                    default:
                        e.DrawBackground();
                        break;
                }

                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(Sender.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
                }
            }
        }
    }
}
