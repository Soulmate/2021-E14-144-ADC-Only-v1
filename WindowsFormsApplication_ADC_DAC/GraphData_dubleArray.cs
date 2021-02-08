using System;
using System.Collections.Generic;
using System.Drawing;

using System.IO;

namespace WindowsFormsApplication_ADC_DAC
{
    public class GraphData_dubleArray:GraphData_ED
    {
        //контейнер для данных
        public List<double> dataList;

        private int maxListSizeInternal = 1024*128;
        public int MaxListSize
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
        public GraphData_dubleArray(string name, double x0, double deltaX, IEnumerable<double> dataList):this(name, x0,deltaX)
        {            
            this.Add(dataList);
            Boarders = BoardersFull;
        }
        public GraphData_dubleArray(string name, double x0, double deltaX)
        {
            this.name = name;
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
                TrimLeftToSize(MaxListSize);

                
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
}
