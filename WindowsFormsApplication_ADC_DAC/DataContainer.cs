using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace WindowsFormsApplication_ADC_DAC
{/// <summary>
/// класс для хранения эквидистантных многоканальных данных double
/// </summary>
    class DataContainer
    {
        public double[,] Buffer { get => buffer; }
        public readonly double deltaT; //время между отсчетами
        public readonly int channelsQuantity;
        public readonly int bufferTimeLenght; //размер буфера по числу отсчетов для каждого канала
        public readonly string filePath;
        public readonly string[] channelNames;        

        double[,] buffer; //время, канал
        int bufferCurrentTimeIndex = 0; //индекс по времени куда писать


        public DataContainer(int channelsQuantity, double deltaT, string filePath, int bufferSizeMB = 1, string[] channelNames = null)
        {
            bufferTimeLenght = (int)Math.Round((double)bufferSizeMB * 1024 * 1024 / 2 / channelsQuantity);
            buffer = new double[bufferTimeLenght, channelsQuantity];
            this.filePath = filePath;
            this.deltaT = deltaT;

            if (channelNames == null)
            {
                channelNames = new string[channelsQuantity];
                for (int ch_i = 0; ch_i < channelsQuantity; ch_i++)
                    channelNames[ch_i] = @"Ch {ch_i}";
            }
            else if (channelNames.Length != channelsQuantity)
                throw new Exception("DataContainer channelNames count error");

            //write info file:
            using (var sr = File.CreateText(filePath + ".adc_info"))
            {
                sr.WriteLine(DateTime.Now.ToString());
                sr.WriteLine($"channelsQuantity {channelsQuantity}");
                sr.WriteLine($"deltaT {deltaT}");                
                sr.WriteLine("channelNames:");
                foreach( var chn in channelNames)
                    sr.WriteLine(chn);
            }
        }

        /// <summary>
        /// Добавить данные в виде ch1 ch2 ch3 ch4 ch1 ch2 ch3 ch4 ch1 ...
        /// </summary>
        /// <param name="d"></param>
        public void AddDataInterleaved(double[] d)
        {
            if (d.Length % channelsQuantity != 0)
                throw new Exception("AddDataInterleaved data size error");
            int dataTimeLength = d.Length / channelsQuantity;
            if (dataTimeLength > bufferTimeLenght)
                throw new Exception("AddDataInterleaved недостаточнй размер буффера");

            if (bufferCurrentTimeIndex + dataTimeLength >= bufferTimeLenght) //если не хватает места записать в буффер
            {
                FlushBufferToFile();
                Console.WriteLine($"Buffer is full, saved to {filePath}");
            }



            for (int time_i = 0; time_i < dataTimeLength; time_i++)
                for (int ch_i = 0; ch_i < channelsQuantity; ch_i++)
                    buffer[bufferCurrentTimeIndex + time_i, ch_i] = d[time_i * channelsQuantity + ch_i];

            bufferCurrentTimeIndex += dataTimeLength;
        }

        /// <summary>
        /// Скинуть данные в файл и очистить буффер
        /// </summary>
        public void FlushBufferToFile()
        {
            using (StreamWriter sr = File.AppendText(filePath)) //todo заменить на бинарные файлы
            {
                for (int time_i = 0; time_i < bufferCurrentTimeIndex; time_i++)
                {
                    double time = deltaT * time_i;
                    sr.Write(time.ToString() + "\t");
                    for (int ch_i = 0; ch_i < channelsQuantity; ch_i++)
                    {
                        sr.Write(buffer[time_i, ch_i].ToString());
                        if (ch_i < channelsQuantity - 1)
                            sr.Write("\t");
                        else
                            sr.Write("\r\n");
                    }
                }
            }
            bufferCurrentTimeIndex = 0; //сбрасываем как если ничего не записано
        }

        public GraphData_dubleArray GetLastChannelData(int ch, double duration)
        {
            if (ch < 0 || ch >= channelsQuantity)
                throw new Exception("ch number error");
            //var gd = new GraphData_dubleArray(channelNames[ch], 
        }
    }
}
