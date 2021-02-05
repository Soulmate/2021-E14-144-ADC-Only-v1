using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class ADC_FTT : Form
    {
        public ADC_FTT()
        {
            InitializeComponent();
            FTT.ClassRealFtt rftt;
            lock (Core.adcReader.graphData)
            {
                rftt = new FTT.ClassRealFtt(Core.adcReader.graphData.dataList.ToArray(), 1024 * 128, Core.adcReader.graphData.deltaX);
            }
            GraphData_dubleArray gd = new GraphData_dubleArray("Signal FTT", Color.Blue, 0, rftt.deltaF);
            gd.Clear();
            if (rftt.result != null)
                gd.Add(rftt.result);
            gd.yMin = 0;
            gd.Boarders = gd.BoardersFull;
            gd.Boarders.Width = 10;
            grapher1.Add(gd);
            grapher1.UpdateGraph();
        }

        private void UpdateFTT()
        {
            FTT.ClassRealFtt rftt;
            lock (Core.adcReader.graphData)
            {
                rftt = new FTT.ClassRealFtt(Core.adcReader.graphData.dataList.ToArray(), 1024 * 128, Core.adcReader.graphData.deltaX);
            }
            GraphData_dubleArray gd = (GraphData_dubleArray)grapher1.gdList[0];
            gd.Clear();
            if (rftt.result != null)
                gd.Add(rftt.result);
            gd.yMin = 0;
            gd.Boarders = gd.BoardersFull;
            gd.Boarders.Width = 10;
            grapher1.UpdateGraph();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateFTT();
        }
    }
}
