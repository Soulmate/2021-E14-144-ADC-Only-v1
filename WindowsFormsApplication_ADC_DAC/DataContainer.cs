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
        public double[,] Date { get => _data; } //данные
        public DateTime t0 = DateTime.Now; //время начала записи. После изменения сделать WriteInfoFile, оно сохраняется в нем
        public readonly double deltaT; //время между отсчетами
        public readonly int channelsQuantity; //число каналов
        public readonly int sizeT; //размер данных по числу отсчетов для каждого канала
        public readonly string filePath; //путь к файлу записи
        public readonly string[] channelNames; //имена каналов               
        double[,] _data; //время, канал
        int currentTimeIndex = 0; //индекс по времени начиная с которого пишутся новые данные
        int timeIndexToWriteToFile = 0; //индекс по времени начиная с которого данные будут сохраняться в файл

        public DataContainer(int channelsQuantity, double deltaT, string filePath, int sizeMB = 1, string[] channelNames = null)
        {
            sizeT = (int)Math.Round((double)sizeMB * 1024 * 1024 / 2 / channelsQuantity);
            _data = new double[sizeT, channelsQuantity];
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
            WriteInfoFile();
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
            if (dataTimeLength > sizeT)
                throw new Exception("AddDataInterleaved недостаточнй размер буффера");

            if (currentTimeIndex + dataTimeLength >= sizeT) //если не хватает места записать в буффер
            {
                Console.WriteLine($"Buffer is full");
                WriteToFile();
                throw new Exception("Buffer is full");                  
            }



            for (int time_i = 0; time_i < dataTimeLength; time_i++)
                for (int ch_i = 0; ch_i < channelsQuantity; ch_i++)
                    _data[currentTimeIndex + time_i, ch_i] = d[time_i * channelsQuantity + ch_i];

            currentTimeIndex += dataTimeLength;
        }

        /// <summary>
        /// Дописать данные в файл
        /// </summary>
        public void WriteToFile()
        {
            using (StreamWriter sr = File.AppendText(filePath)) //todo заменить на бинарные файлы
            {
                for (int time_i = timeIndexToWriteToFile; time_i < currentTimeIndex; time_i++)
                {
                    double time = deltaT * time_i;
                    sr.Write(time.ToString() + "\t");
                    for (int ch_i = 0; ch_i < channelsQuantity; ch_i++)
                    {
                        sr.Write(_data[time_i, ch_i].ToString());
                        if (ch_i < channelsQuantity - 1)
                            sr.Write("\t");
                        else
                            sr.Write("\r\n");
                    }
                }
            }
            timeIndexToWriteToFile = currentTimeIndex;
        }

        public GraphData_dubleArray GetLastChannelData(int ch, double duration)
        {
            if (ch < 0 || ch >= channelsQuantity)
                throw new Exception("ch number error");

            int start_i = (int)(currentTimeIndex - duration / deltaT);
            var gd = new GraphData_dubleArray(channelNames[ch], start_i * deltaT, deltaT);
            var d = Enumerable.Range(start_i, currentTimeIndex)
                .Select(i => _data[i, ch])
                .ToList();
            gd.Add(d);
            return gd;
        }

        public void WriteInfoFile()
        {
            using (var sr = File.CreateText(filePath + ".adc_info"))
            {
                sr.WriteLine(t0.ToString());
                sr.WriteLine($"channelsQuantity {channelsQuantity}");
                sr.WriteLine($"deltaT {deltaT}");
                sr.WriteLine("channelNames:");
                foreach (var chn in channelNames)
                    sr.WriteLine(chn);
            }
        }
    }
}
