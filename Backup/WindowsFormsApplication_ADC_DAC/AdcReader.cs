using System;
using System.Collections.Generic;
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
                graphData.maxListSize = (int)(memoryTimeInternal / deltaT);
            }
        }
        
        public GrapherControl grapherControl;
        public GraphData_dubleArray graphData;
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
            grapherControl = new GrapherControl();            
            deltaT = Core.module.deltaT;
            graphData = new GraphData_dubleArray("Input", System.Drawing.Color.Red, 0, deltaT);
            graphData.maxListSize = (int)(memoryTime / deltaT);
            graphData.Boarders.Width = 10;
            grapherControl.Add(graphData);
            grapherControl.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void Start()
        {
            //если цикл чтения работает, то остановим его
            if (readingThread!= null && readingThread.IsAlive)            
                Stop();          
            graphData.Clear();
            graphData.x0 = 0;

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
                lock (graphData)
                {
                    graphData.Add(Core.module.ReadOutVoltageArray());
                    graphData.Boarders.Height = graphData.BoardersFull.Height;
                    grapherControl.UpdateGraph();
                }
                Thread.Sleep((int)(1000 / updateRate));
            }
        }

    }
}
