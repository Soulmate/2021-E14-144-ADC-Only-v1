using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class ADC_Only : Form
    {
        public ADC_Only()
        {
            InitializeComponent();

            panel1.Controls.Add(Core.adcReader.grapherControl1);

            //if (Core.automation.autoStartSequence)
            //    this.WindowState = FormWindowState.Minimized;
        }

       

        private void timer1_Tick(object sender, EventArgs e)
        {
            Core.automation.RunAutomationLoop();

            textBox_Log.Text = "";
            foreach (var gd in Core.adcReader.graphData_arr)
            {
                List<double> d = gd.dataList;
                if (d.Count > 1)
                    textBox_Log.Text += $"{gd.name}: {d.Last()} V\r\n";
                else
                    textBox_Log.Text += $"{gd.name}: NAN V\r\n";
            }               
        }

        private void button_ADCStart_Click(object sender, EventArgs e)
        {
            Core.adcReader.Start();
        }

        private void button_ADCStop_Click(object sender, EventArgs e)
        {
            Core.adcReader.Stop();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string savePath = textBox_savePath.Text;
            if (savePath != "")
                saveFileDialog1.FileName = savePath;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                savePath = saveFileDialog1.FileName;
                textBox_savePath.Text = savePath;

                //TODO!!! сохранять время от времени, иначе невозможно писать длинные записи
                if (savePath != null)
                {
                    foreach (var gd in Core.adcReader.graphData_arr)
                        gd.SaveToFile(savePath + " " + gd.name + ".dat");
                }
            }
        }
    }
}
