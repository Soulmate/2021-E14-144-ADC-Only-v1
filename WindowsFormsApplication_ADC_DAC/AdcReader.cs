using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace WindowsFormsApplication_ADC_DAC
{
    class AdcReader
    {
        public double adcRate = 1; //(кГц) частота работы АЦП
        public double adcRange = 10000; //(mВ) Максимальное входное напряжение АЦП
        public int dataStep = 32; //число отсчетов, вычитываемое за один раз
        public double deltaT; // (с) время между отсчетами
        public double updateRate = 25; //Гц   частота вычитываения данных из кольцевого буфера   

        private double memoryTimeInternal = 60; //максимальное время записи
        public double memoryTime
        {
            get { return memoryTimeInternal; }
            set
            {
                memoryTimeInternal = value;
                graphData1.maxListSize = (int)(memoryTimeInternal / deltaT);
                graphData2.maxListSize = (int)(memoryTimeInternal / deltaT);
                graphData3.maxListSize = (int)(memoryTimeInternal / deltaT);
                graphData4.maxListSize = (int)(memoryTimeInternal / deltaT);
            }
        }
        
        public GrapherControl grapherControl1;
        public GraphData_dubleArray graphData1;
        public GraphData_dubleArray graphData2;
        public GraphData_dubleArray graphData3;
        public GraphData_dubleArray graphData4;
        public Thread readingThread;
        public bool stopFlag = true;


        public AdcReader()
        {
            try
            {
                Core.module = new LusbApi_Wrapper.Module(false, adcRate, adcRange, dataStep);
            }
            catch
            {                
                Console.WriteLine("Type 'e' to enter emulation mode");
                if (Console.ReadLine() == "e")
                    Core.module = new LusbApi_Wrapper.Module(true, adcRate, adcRange, dataStep);
                else
                    throw new ArgumentException("Module initialization failure");
            }
            grapherControl1 = new GrapherControl();
            deltaT = Core.module.deltaT;
            graphData1 = new GraphData_dubleArray("1 ch", System.Drawing.Color.Red, 0, deltaT);
            graphData2 = new GraphData_dubleArray("2 ch", System.Drawing.Color.Green, 0, deltaT);
            graphData3 = new GraphData_dubleArray("3 ch", System.Drawing.Color.Blue, 0, deltaT);
            graphData4 = new GraphData_dubleArray("4 ch", System.Drawing.Color.Black, 0, deltaT);
            graphData1.maxListSize = (int)(memoryTime / deltaT);
            graphData2.maxListSize = (int)(memoryTime / deltaT);
            graphData3.maxListSize = (int)(memoryTime / deltaT);
            graphData4.maxListSize = (int)(memoryTime / deltaT);
            graphData1.Boarders.Width = 10;
            graphData2.Boarders.Width = 10;
            graphData3.Boarders.Width = 10;
            graphData4.Boarders.Width = 10;
            grapherControl1.Add(graphData1);
            grapherControl1.Add(graphData2);
            grapherControl1.Add(graphData3);
            grapherControl1.Add(graphData4);
            grapherControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void Start()
        {
            //если цикл чтения работает, то остановим его
            if (readingThread!= null && readingThread.IsAlive)            
                Stop();          
            graphData1.Clear();
            graphData2.Clear();
            graphData3.Clear();
            graphData4.Clear();
            graphData1.x0 = 0;
            graphData2.x0 = 0;
            graphData3.x0 = 0;
            graphData4.x0 = 0;

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
                lock (graphData1)
                {
                    lock (graphData2)
                    {
                        lock (graphData3)
                        {
                            lock (graphData4)
                            {
                                List<double> d = Core.module.ReadOutVoltageArray().ToList();
                                int chan_count = 4; //число каналов
                                List<double> d1 = d.Where((x, i) => i % chan_count == 0).ToList();
                                List<double> d2 = d.Where((x, i) => i % chan_count == 1).ToList();
                                List<double> d3 = d.Where((x, i) => i % chan_count == 2).ToList();
                                List<double> d4 = d.Where((x, i) => i % chan_count == 3).ToList();

                                graphData1.Add(d1);
                                graphData2.Add(d2);
                                graphData3.Add(d3);
                                graphData4.Add(d4);
                                graphData1.Boarders.Height = graphData1.BoardersFull.Height;
                                graphData2.Boarders.Height = graphData2.BoardersFull.Height;
                                graphData3.Boarders.Height = 5; // graphData3.BoardersFull.Height;
                                graphData4.Boarders.Height = 5; // graphData4.BoardersFull.Height;
                                grapherControl1.UpdateGraph();
                            }
                        }
                    }
                }
                Thread.Sleep((int)(1000 / updateRate));
            }
        }

    }
}
