using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace WindowsFormsApplication_ADC_DAC
{
    class AdcReader
    {
        public int channelsQuantity = 4;
        public double adcRate = 1; //(кГц) частота работы АЦП
        public double adcRange = 10000; //(mВ) Максимальное входное напряжение АЦП
        public int dataStep = 32; //число отсчетов, вычитываемое за один раз
        public double deltaT; // (с) время между отсчетами
        public double updateRate = 25; //Гц   частота вычитываения данных из кольцевого буфера   

        private double memoryTimeInternal = 60; //максимальное время записи
        public double MemoryTime
        {
            get { return memoryTimeInternal; }
            set
            {
                memoryTimeInternal = value;
                foreach (var gd in graphData_arr)
                    gd.MaxListSize = (int)(memoryTimeInternal / deltaT);
            }
        }

        public GrapherControl grapherControl1;
        public List<GraphData_dubleArray> graphData_arr = new List<GraphData_dubleArray>();
        public Thread readingThread;
        public bool stopFlag = true;


        public AdcReader()
        {
            try
            {
                Core.module = new LusbApi_Wrapper.Module(false, channelsQuantity, adcRate, adcRange, dataStep);
            }
            catch
            {
                Console.WriteLine("Type 'e' to enter emulation mode");
                if (Console.ReadLine() == "e")
                    Core.module = new LusbApi_Wrapper.Module(true, channelsQuantity, adcRate, adcRange, dataStep);
                else
                    throw new ArgumentException("Module initialization failure");
            }
            grapherControl1 = new GrapherControl();
            deltaT = Core.module.deltaT;
            for (int i = 0; i < channelsQuantity; i++)
            {
                GraphData_dubleArray graphData = new GraphData_dubleArray("Ch " + i, Grapher.GetNextColor(), 0, deltaT); ;
                graphData.MaxListSize = (int)(MemoryTime / deltaT);
                graphData.Boarders.Width = 10;
                graphData_arr.Add(graphData);
                grapherControl1.Add(graphData);
            }
            grapherControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void Start()
        {
            //если цикл чтения работает, то остановим его
            if (readingThread != null && readingThread.IsAlive)
                Stop();
            foreach (var gd in graphData_arr)
            {
                gd.Clear();
                gd.x0 = 0;
            }
            stopFlag = false;
            readingThread = new Thread(new ThreadStart(ReadingLoop));
            readingThread.Name = "ReadingLoop";
            //generationThread.IsBackground = true;
            readingThread.Start();
        }
        public void Stop()
        {
            stopFlag = true;
            //подождем удвоенное время цикла чтения
            Thread.Sleep((int)(1000 / updateRate) * 2);
        }

        private void ReadingLoop()
        {
            while (!stopFlag)
            {
                double[] d = Core.module.ReadOutVoltageArray();
                double[][] d_split = new double[channelsQuantity][];
                for (int ch_i = 0; ch_i< channelsQuantity; ch_i++)
                {
                    double[] ch_data = d.Where((x, i) => i % channelsQuantity == ch_i).ToArray();
                    var gd = graphData_arr[ch_i];
                    lock (gd)
                    {
                        gd.Add(ch_data);
                        gd.Boarders.Height = gd.BoardersFull.Height;
                    }
                }
                grapherControl1.UpdateGraph();

                Thread.Sleep((int)(1000 / updateRate));
            }
        }

    }
}
