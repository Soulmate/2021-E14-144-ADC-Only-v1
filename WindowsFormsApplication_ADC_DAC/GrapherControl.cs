using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace WindowsFormsApplication_ADC_DAC
{
    public partial class GrapherControl : UserControl
    {
        public GrapherControl()
        {
            InitializeComponent();            
        }

        private void grapher1_Updating(object sender, EventArgs e)
        {
            RectangleF F = grapher1.gdSelected.BoardersFull;
            RectangleF G = grapher1.gdSelected.Boarders;

            float xPosition = hScrollBar1.Value / (float)(hScrollBar1.Maximum - hScrollBar1.Minimum);
            //float yPosition = vScrollBar1.Value / (float)(vScrollBar1.Maximum - vScrollBar1.Minimum);
            float yPosition = 0;
            foreach (GraphData_ED gd in grapher1.gdList)
            {
                gd.Boarders.X = F.X + xPosition * (F.Width - G.Width);
                gd.Boarders.Y = F.Y + yPosition * (F.Height - G.Height);
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            grapher1.UpdateGraph();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            grapher1.UpdateGraph();
        }

        public void Add(GraphData_ED gd)
        {
            grapher1.Add(gd);
        }

        public void UpdateGraph()
        {
            grapher1.UpdateGraph();
        }
    }
}
