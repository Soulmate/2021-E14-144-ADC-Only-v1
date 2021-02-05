using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    class Automation
    {        
        public bool autoStart = false;
        public bool autoEnd = false;
        public string savePath = null;
        public DateTime timeStart;
        public double duration = 0;

        public void Start() //вызывается при старте
        {            
            if (autoStart)
            {
                timeStart = DateTime.Now;
                Core.adcReader.Start();
                //todo старт записи
            }
        }
        public void RunAutomationLoop() //вызывается регулярно
        {
            if (autoEnd && (DateTime.Now - timeStart).TotalSeconds > duration) //TODO переделать на точное время
            {
                Core.adcReader.Stop();
                //TODO может что-то еще завершить

                if (savePath != null)
                {
                    Core.adcReader.graphData1.SaveToFile(savePath + " chan1.dat");
                    Core.adcReader.graphData2.SaveToFile(savePath + " chan2.dat");
                    Core.adcReader.graphData3.SaveToFile(savePath + " chan3.dat");
                    Core.adcReader.graphData4.SaveToFile(savePath + " chan4.dat");  //TODO в один файл
                }
                Application.Exit();
            }
        }
    }
}
