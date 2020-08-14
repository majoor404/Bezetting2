using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bezetting2
{
    public partial class InlogForm : Form
    {
        private List<rechten> rechten_lijst = new List<rechten>();
        
        public InlogForm()
        {
            InitializeComponent();
        }

        private void InlogForm_Shown(object sender, EventArgs e)
        {
            textBoxNum.Text = Environment.UserName;
            textBoxPass.Text = "";
            textBoxChangePasswoord.Text = "";

            textBoxNum.Focus();
        }

        private void buttonOke_Click(object sender, EventArgs e)
        {
            //check passwoord
            if (textBoxNum.Text == "Admin" && textBoxPass.Text == "konijn")
            {
                ProgData.Huidige_Gebruiker = "Admin";
                ProgData. RechtenHuidigeGebruiker = 99;
            }
            else
            {
                Lees_passwoorden_lijst();
                foreach (rechten a in rechten_lijst)
                {
                    if ("a" + a._persnummer == textBoxNum.Text)
                    {
                        if (unscramble(a._passwoord) == textBoxPass.Text)
                        {
                            // juiste inlog
                            ProgData.Huidige_Gebruiker = a._persnummer;
                            ProgData.RechtenHuidigeGebruiker = a._rechten;
                        }
                    }
                }
            }

        }

        private void Lees_passwoorden_lijst()
        {
            rechten_lijst.Clear();
            try
            {
                using (Stream stream = File.Open("rechten.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();

                    rechten_lijst = (List<rechten>)bin.Deserialize(stream);
                }
            }
            catch (IOException)
            {
            }
        }

        private string unscramble(string woord)
        {
            string ret = "";
            if (woord.Length < 1)
                return ret;
            for (int h = 0; h < woord.Length; h++)
            {
                int xx22 = 0;
                xx22 = (int)woord[h];
                xx22 -= 2;
                xx22 -= woord.Length - h - 1;
                char character = (char)xx22;
                ret = ret + character.ToString();
            }
            return ret;
        }
    }
}
