using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Linq;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class Grapher : UserControl
    {
        static readonly uint[] BrightPastel = new uint[] { 0xFF418CF0, 0xFFFCB441, 0xFFDF3A02, 0xFF056492, 0xFFBFBFBF, 0xFF1A3B69, 0xFFFFE382, 0xFF129CDD, 0xFFCA6B4B, 0xFF005CDB, 0xFFF3D288, 0xFF506381, 0xFFF1B9A8, 0xFFE0830A, 0xFF7893BE }; //https://stackoverflow.com/questions/14204827/ms-chart-for-net-predefined-palettes-color-list
        static int pallette_index = 0;
        public static Color GetNextColor()
        {
            pallette_index++;
            if (pallette_index >= BrightPastel.Length)
                pallette_index = 0;
            return Color.FromArgb((int)BrightPastel[pallette_index]);            
        }
       

        public Grapher()
        {            
            InitializeComponent();
            
            drawBox = new Rectangle(0, 0, pictureBox1.Width-3, pictureBox1.Height-3);

            pictureBox1.Paint += new PaintEventHandler(Draw);
            pictureBox1.Resize += new EventHandler(pictureBox1_Resize);
        }

        void pictureBox1_Resize(object sender, EventArgs e)
        {
            drawBox = new Rectangle(0,0,pictureBox1.Width-3,pictureBox1.Height-3);
            pictureBox1.Invalidate();
        }

        void Draw(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            foreach (GraphData_ED gd in gdList)
            {
                lock (gd)
                {
                    int iStart = Math.Max(0, gd.iByX(gd.Boarders.X));
                    int iEnd = Math.Min(gd.PointsCount - 1, gd.iByX(gd.Boarders.X + gd.Boarders.Width));
                    if (iEnd - iStart >= 2 && gd.Boarders.Height != 0 && gd.Boarders.Width != 0)
                    {
                        int deltaI = Math.Max(1, (iEnd - iStart) / pictureBox1.Width);
                        Point p1 = GetPoint(gd.X(iStart), gd.Y(iStart), gd.Boarders);
                        for (int i = iStart + 1; i <= iEnd; i += deltaI)
                        {
                            Point p2 = GetPoint(gd.X(i), gd.Y(i), gd.Boarders);
                            g.DrawLine(new Pen(gd.color), p1, p2);
                            p1 = p2;
                        }
                    }
                    string s = String.Format("{0}\nx: {1} ~ {2}\ny: {3} ~ {4}", gd.name, gd.Boarders.X, gd.Boarders.X + gd.Boarders.Width, gd.Boarders.Y, gd.Boarders.Y + gd.Boarders.Height);
                    g.DrawString(s, new Font("Arial", 8), new SolidBrush(gd.color), drawBox.X + drawBox.Width - 150, drawBox.Y + 20 + 40 * gdList.IndexOf(gd));
                }
            }
            if (gdSelected != null)
            {
                lock (gdSelected)
                {
                    double[] xMarks = GetXMarksArray(gdSelected, 5);
                    double[] yMarks = GetYMarksArray(gdSelected, 3);
                    foreach (double x in xMarks)
                    {
                        int xPx = GetPoint(x, 0, gdSelected.Boarders).X;
                        g.DrawLine(new Pen(gdSelected.color), xPx, drawBox.Y + drawBox.Height, xPx, drawBox.Y + drawBox.Height - 3);
                        g.DrawString(x.ToString(), new Font("Arial", 10), new SolidBrush(gdSelected.color), xPx - 10, drawBox.Y + drawBox.Height - 20);
                    }
                    foreach (double y in yMarks)
                    {
                        int yPx = GetPoint(0, y, gdSelected.Boarders).Y;
                        g.DrawLine(new Pen(gdSelected.color), drawBox.X, yPx, drawBox.X + 3, yPx);
                        g.DrawString(y.ToString(), new Font("Arial", 10), new SolidBrush(gdSelected.color), drawBox.X + 5, yPx - 7);
                    }
                }
            }   
        }

        private double[] GetXMarksArray(GraphData_ED gd, int minNumberOfPoints)
        {
            List<double> res = new List<double>();
            double x0 = gd.Boarders.X;
            double x1 = gd.Boarders.X + gd.Boarders.Width;

            double lg = Math.Log10((x1-x0)/(double)minNumberOfPoints);
            int pow = (int)lg;
            if (lg < 0)
                pow--;
            double delta1 = Math.Pow(10,(int)pow);

            lg = Math.Log10((x1 - x0) / (double)minNumberOfPoints * 2.0);
            pow = (int)lg;
            if (lg < 0)
                pow--;
            double delta2 = Math.Pow(10, (int)pow)/2.0;

            double delta = Math.Max(delta1, delta2);

            double tmp = (x0 / delta);
            double tmp2 = (int)tmp;
            if (tmp < 0)
                tmp2--;
            double x = tmp2 * delta;
            while (x + delta < x1)
                res.Add(x += delta);
            return res.ToArray();
        }

        private double[] GetYMarksArray(GraphData_ED gd, int minNumberOfPoints)
        {
            List<double> res = new List<double>();
            double y0 = gd.Boarders.Y;
            double y1 = gd.Boarders.Y + gd.Boarders.Height;

            double lg = Math.Log10((y1 - y0) / (double)minNumberOfPoints);
            int pow = (int)lg;
            if (lg < 0)
                pow--;
            double delta1 = Math.Pow(10, (int)pow);

            lg = Math.Log10((y1 - y0) / (double)minNumberOfPoints * 2.0);
            pow = (int)lg;
            if (lg < 0)
                pow--;
            double delta2 = Math.Pow(10, (int)pow) / 2.0;

            double delta = Math.Max(delta1, delta2);

            double tmp = (y0 / delta);
            double tmp2 = (int)tmp;
            if (tmp<0)
                tmp2--;
            double y = tmp2 * delta;            
            while (y + delta < y1)
                res.Add(y += delta);
            return res.ToArray();
        }


        public void UpdateGraph()
        {
            if (Updating != null)
                Updating(this, new EventArgs());

            pictureBox1.Invalidate();
        }

        public void UpdateFuncGraphs()
        {
            foreach (GraphData_ED gdf in gdList)
                if (gdf is GraphData_Function)
                    ((GraphData_Function)gdf).Update();
        }

        public List<GraphData_ED> gdList = new List<GraphData_ED>();
        public GraphData_ED gdSelected;
        public Rectangle drawBox;

        public Point GetPoint(double x, double y, RectangleF dataBoarders)
        {
            Point result = new Point();
            result.X = (int)(drawBox.X + drawBox.Width * (x - dataBoarders.X) / dataBoarders.Width);
            result.Y = (int)(drawBox.Y + drawBox.Height * (1 - (y - dataBoarders.Y) / dataBoarders.Height));
            return result;
        }

        public void Add(GraphData_ED gd)
        {
            gdList.Add(gd);
            if (gdList.Count == 1)
                gdSelected = gd;
        }

        public void Clear()
        {
            gdList.Clear();       
            gdSelected = null;
        }

        public event EventHandler Updating;
    }
}
