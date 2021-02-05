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

            List<double> arr1 = Core.adcReader.graphData1.dataList;
            List<double> arr2 = Core.adcReader.graphData2.dataList;

            textBox_Log.Text = "";
            if (arr1.Count > 1)
                textBox_Log.Text += $"1 ch: {arr1.Last()} V\r\n";
            else
                textBox_Log.Text += $"1 ch: NAN V\r\n";
            if (arr2.Count > 1)
                textBox_Log.Text += $"2 ch: {arr2.Last()} V\r\n";
            else
                textBox_Log.Text += $"2 ch: NAN V\r\n";
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
                Core.adcReader.graphData1.SaveToFile(savePath + " chan1.dat");
                Core.adcReader.graphData2.SaveToFile(savePath + " chan2.dat");
                Core.adcReader.graphData3.SaveToFile(savePath + " chan3.dat");
                Core.adcReader.graphData4.SaveToFile(savePath + " chan4.dat");  //TODO в один файл
            }
        }
    }
}
