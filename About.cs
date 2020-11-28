using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Shown(object sender, EventArgs e)
        {
            DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
            label1.Text = buildDate.ToString();

            textBox2.Clear();
            string[] lines = File.ReadAllLines("versie.ini");
            textBox2.Text = String.Join(Environment.NewLine, lines);
            textBox2.SelectionStart = textBox2.TextLength;
            textBox2.ScrollToCaret();
        }
    }
}
