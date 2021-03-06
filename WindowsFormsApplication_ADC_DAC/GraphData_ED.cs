﻿using System.Drawing;

namespace WindowsFormsApplication_ADC_DAC
{
    //эквидистантные данные для построения
    public abstract class GraphData_ED
    {
        public string name = "Unnamed";

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
}
