using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class Grapher : UserControl
    {
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

    //эквидистантные данные для построения
    public abstract class GraphData_ED
    {
        //цвет отрисовки
        public Color color = Color.Red;
        public string name = "Name";

        //границы отображаемных данных
        public RectangleF Boarders;
        //границы данных
        public abstract RectangleF BoardersFull
        {
            get;
        }

        public abstract int PointsCount
        {
            get;
            set;
        }

        public abstract double X(int i);
        public abstract double Y(int i);
        //получить индекс по значению Х
        public abstract int iByX(double x);        
    }

    public class GraphData_dubleArray:GraphData_ED
    {
        //контейнер для данных
        public List<double> dataList;

        private int maxListSizeInternal = 1024*128;
        public int maxListSize
        {
            get { return maxListSizeInternal; }
            set
            {
                if (value > 0)
                {
                    maxListSizeInternal = value;
                    TrimLeftToSize(maxListSizeInternal);
                }
            }
        }
                
        //шаг по икс, минимальное значение икса, границы икрек
        public double deltaX, x0, yMin, yMax;
        
        //конструкторы
        public GraphData_dubleArray(string name, Color color, double x0, double deltaX, IEnumerable<double> dataList):this(name, color, x0,deltaX)
        {            
            this.Add(dataList);
            Boarders = BoardersFull;
        }
        public GraphData_dubleArray(string name, Color color, double x0, double deltaX)
        {
            this.name = name;
            this.color = color;
            this.deltaX = deltaX;
            this.x0 = x0;
            this.dataList = new List<double>();            
        }
        
        //Добавление массива новых элементов
        public void Add(IEnumerable<double> yList)
        {
            lock (this)
            {
                //если до этого ymax и ymin не определены
                if (PointsCount == 0)
                {
                    yMax = double.MinValue;
                    yMin = double.MaxValue;
                }

                dataList.AddRange(yList);
                TrimLeftToSize(maxListSize);

                
                foreach (double y in yList)
                {
                    yMax = Math.Max(yMax, y);
                    yMin = Math.Min(yMin, y);
                }
            }
        }
        public void Clear()
        {
            dataList.Clear();
            yMax = yMin = 0;
        }

        //обрезать слева до размера
        public void TrimLeftToSize(int size)
        {
            lock (this)
            {
                if (PointsCount > size)
                {
                    x0 += (PointsCount - size) * deltaX;

                    dataList.RemoveRange(0, PointsCount - size);                    

                    yMax = double.MinValue;
                    yMin = double.MaxValue;                    
                    foreach (double y in dataList)
                    {
                        yMax = Math.Max(yMax, y);
                        yMin = Math.Min(yMin, y);
                    }
                    if (yMax == double.MinValue)
                        yMax = 0;
                    if (yMin == double.MaxValue)
                        yMin = 0;
                }
            }
        }
                
        //сохранение в файл
        public void SaveToFile(string path)
        {
            lock (this)
            {
                StreamWriter sr = File.CreateText(path);
                for (int i = 0; i < PointsCount; i++)
                    sr.WriteLine("{0}\t{1}", X(i), Y(i));
                sr.Close();
            }
        }
        //загрузка из файла
        public void LoadFromFile(string path)
        {                
                List<double> tempDataList = new List<double>();

                double x0 = 0;
                double prevX = 0;
                double deltaX = 0;

                StreamReader sr = File.OpenText(path);
                string input = null;
                int i=0;
                while ((input = sr.ReadLine()) != null)
                {                     
                    string[] splited = input.Split(new string[] { "/t" },StringSplitOptions.RemoveEmptyEntries);
                    if (splited.Length >= 2)
                    {
                        double x;
                        double y;
                        if (double.TryParse(splited[0],out x) && double.TryParse(splited[1],out y))
                        {
                            
                            if (i>1)
                                {
                                    if (Math.Abs((x - prevX) - deltaX) < deltaX / 100.0)
                                        tempDataList.Add(y);
                                    else
                                        throw new ApplicationException("not equiqistance file");
                                }                            
                            if (i == 0)
                                x0 = x;
                            if (i>0)
                                deltaX = x-prevX;
                            prevX = x;
                            i++;
                         }
                    }
                    if (deltaX != 0)
                    {
                        //перепишем данные
                        this.x0 = x0;
                        this.deltaX = deltaX;
                        this.dataList = new List<double>();
                        this.Add(tempDataList);
                        this.Boarders = this.BoardersFull;
                    }
                }
                sr.Close();            
        }
        
        //границы данных
        public override RectangleF BoardersFull
        {
            get { return new RectangleF((float)x0, (float)yMin, (float)(X(PointsCount - 1) - x0), (float)(yMax - yMin)); }
        }
        public override int PointsCount
        {
            get { return dataList.Count; }
            set { }
        }
        public override double X(int i)
        {
            return x0 + i * deltaX;
        }
        public override double Y(int i)
        {
            if (i >= 0 && i < PointsCount)
                return dataList[i];
            else
                return 0;
        }        
        public override int iByX(double x)
        {
            return (int)((x - x0) / deltaX);
        }
    }
    
    public class GraphData_Function: GraphData_ED
    {
        //контейнер для данных
        private List<double> dataList;

        //делегат для функции
        public delegate double GraphFunc(double x);
        private GraphFunc function;
        
        //шаг по икс, минимальное значение икса, границы икрек
        public double xMax, xMin, yMin, yMax;

        public int points = 100;




        public GraphData_Function(string name, Color color, double xMin, double xMax, GraphFunc function)
        {
            this.name = name;
            this.color = color;
            this.xMax = xMax;
            this.xMin = xMin;
            this.function = function;
            Update();
            Boarders = BoardersFull;
        }

        public void Update()
        {
            if (points > 0)
            {
                dataList = new List<double>(points);

                yMax = double.MinValue;
                yMin = double.MaxValue;

                for (int i = 0; i < points; i++)
                {
                    double y = function(X(i));
                    yMax = Math.Max(yMax, y);
                    yMin = Math.Min(yMin, y);
                    dataList.Add(y);
                }
            }            
        }

        public override RectangleF BoardersFull
        {
            get { return new RectangleF((float)xMin, (float)yMin, (float)xMax, (float)(yMax - yMin)); }
        }        
        public override int PointsCount
        {
            get
            {
                return points;
            }
            set
            {
                if (value != points)
                {
                    points = value;
                    Update();
                }
            }
        }
        public override double X(int i)
        {
            return (i / (double)points) * (xMax - xMin) + xMin;
        }
        public override double Y(int i)
        {
            if (i >= 0 && i < dataList.Count)
                return dataList[i];
            else
                return 0;
        }
        public override int iByX(double x)
        {
 	        return (int)((x - xMin) / ((xMax - xMin)/(double)points));
        }
    }
}
