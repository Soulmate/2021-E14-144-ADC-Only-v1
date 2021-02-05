using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class Logger : UserControl
    {
        public Logger()
        {
            InitializeComponent();

            dataList = new List<double>();
            //не самый быстрый способ =(
            while(dataList.Count < pointsCount)
                dataList.Add(0);

            pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
            pictureBox1.Resize += new EventHandler(pictureBox1_Resize);
        }

        void pictureBox1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (yMax!=yMin)
            {
                Graphics g = e.Graphics;
                int width = pictureBox1.Width-3;
                int height = pictureBox1.Height-3;

                Point p1, p2;
                p1 = new Point(0, (int)((yMax - dataList[0]) / (yMax - yMin) * height));
                for (int i = 0; i < dataList.Count; i++)
                {
                    p2 = new Point((int)(width * i/(double)pointsCount), (int)((yMax - dataList[i]) / (yMax - yMin) * height));
                    g.DrawLine(Pens.Red,p1,p2);
                    p1 = p2;
                }
            }
        }


        private List<double> dataList;
        public int pointsCount = 500;
        public double yMax, yMin;
        public void Add(double d)
        {
            dataList.Add(d);
            if (dataList.Count > pointsCount)
                dataList.RemoveRange(0, 1);
            pictureBox1.Invalidate();
        }
    }
}
