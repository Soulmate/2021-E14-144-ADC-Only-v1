using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Core.adcReader.grapher.Dock = DockStyle.Fill;
            panel1.Controls.Add(Core.adcReader.grapherControl);
            //panel2.Controls.Add(Core.generator.grapher);
            params1.Associate(Core.generator.settings);

            StopCheckTimer.Interval = 100;
            StopCheckTimer.Tick += new EventHandler(StopCheckTimer_Tick);
            StopCheckTimer.Start();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            Core.adcReader.graphData.SaveToFile("data.txt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Core.generator.settings.SaveToFile("Settings.xml");
        }

        Timer StopCheckTimer = new Timer();
        private void button3_Click(object sender, EventArgs e)
        {
            if (Core.generator.stopFlag)
            {
                button3.Text = "S T O P";
                Core.generator.Start();
            }
            else
            {
                button3.Text = "S T A R T";
                Core.generator.Stop();
            }
        }
        void StopCheckTimer_Tick(object sender, EventArgs e)
        {
            if (Core.generator.stopFlag)
            {
                button3.Text = "S T A R T";
            }
            else
            {
                button3.Text = "S T O P";
            }
        }
    }
}
