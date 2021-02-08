using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace WindowsFormsApplication_ADC_DAC
{
    class AdcReader
    {
        public readonly int channelsQuantity = 4;
        public readonly double adcRate_kHz = 1; //(кГц) частота работы АЦП
        public readonly double adcRange = 10000; //(mВ) Максимальное входное напряжение АЦП
        public readonly int dataStep = 32; //число отсчетов, вычитываемое за один раз
        public readonly string saveFile;

        public double deltaT => Core.module.deltaT; // (с) время между отсчетами
        public double updateRate = 25; //Гц   частота вычитываения данных из кольцевого буфера, можно менять походу
        public DataContainer dataContainer;
        public Thread readingThread;
        public bool stopFlag = true;



        public AdcReader(
            int channelsQuantity = 4,
            double adcRate_kHz = 1,
            string saveFilePath = "tampData.dat")
        {
            this.channelsQuantity = channelsQuantity;
            this.adcRate_kHz = adcRate_kHz;
            this.saveFile = saveFilePath;


            try
            {
                Core.module = new LusbApi_Wrapper.Module(false, channelsQuantity, adcRate_kHz, adcRange, dataStep);
            }
            catch
            {
                Console.WriteLine("Type 'e' to enter emulation mode");
                if (Console.ReadLine() == "e")
                    Core.module = new LusbApi_Wrapper.Module(true, channelsQuantity, adcRate_kHz, adcRange, dataStep);
                else
                    throw new ArgumentException("Module initialization failure");
            }
            


            dataContainer = new DataContainer(channelsQuantity, deltaT, saveFilePath);

            //for (int i = 0; i < channelsQuantity; i++)
            //{
            //    GraphData_dubleArray graphData = new GraphData_dubleArray("Ch " + i, Grapher.GetNextColor(), 0, deltaT); ;
            //    graphData.MaxListSize = (int)(MemoryTime / deltaT);
            //    graphData.Boarders.Width = 10;
            //    graphData_arr.Add(graphData);
            //    grapherControl1.Add(graphData);
            //}
            //grapherControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void Start()
        {
            //если цикл чтения работает, то остановим его
            if (readingThread != null && readingThread.IsAlive)
                Stop();
            //foreach (var gd in graphData_arr)
            //{
            //    gd.Clear();
            //    gd.x0 = 0;
            //}
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
                dataContainer.AddDataInterleaved(d);

                //for (int ch_i = 0; ch_i< channelsQuantity; ch_i++)
                //{
                //    double[] ch_data = d.Where((x, i) => i % channelsQuantity == ch_i).ToArray();
                //    var gd = graphData_arr[ch_i];
                //    lock (gd)
                //    {
                //        gd.Add(ch_data);
                //        gd.Boarders.Height = gd.BoardersFull.Height;
                //    }
                //}
                //grapherControl1.UpdateGraph();

                Thread.Sleep((int)(1000 / updateRate));
            }
        }

    }
}
