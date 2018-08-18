using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WallpaperScheduler
{
    public partial class Form3 : Form
    {
        public String currentLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

        public Form3()
        {
            InitializeComponent();
        }

        public Form3(string num)
        {
            InitializeComponent();
            label2.Text = "For example did you mean " +(Int32.Parse(num.Split(':')[0])+12).ToString()+ " ("+ num.Split(':')[0]+" PM)" +"  instead";
            label3.Text = " of "+ num.Split(':')[0] + " ("+ num.Split(':')[0] + " AM) for "+ num.Split(':')[1]+ " by any chance.";
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                string text = File.ReadAllText(currentLocation + @"\Utilities\generalConfig.cfg");
             
                text = text.Replace("dontShowHoursHelp=1;", "dontShowHoursHelp=0;");
                File.WriteAllText(currentLocation + @"\Utilities\\generalConfig.cfg", text);
            }
            this.DialogResult = DialogResult.OK;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                string text = File.ReadAllText(currentLocation + @"\Utilities\generalConfig.cfg");

                text = text.Replace("dontShowHoursHelp=1;", "dontShowHoursHelp=0;");
                File.WriteAllText(currentLocation + @"\Utilities\\generalConfig.cfg", text);
            }
            this.DialogResult = DialogResult.Cancel;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
