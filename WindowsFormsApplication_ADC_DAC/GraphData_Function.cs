using System;
using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApplication_ADC_DAC
{
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
