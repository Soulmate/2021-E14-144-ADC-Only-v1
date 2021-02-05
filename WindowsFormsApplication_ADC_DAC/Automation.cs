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


                //TODO!!! сохранять время от времени, иначе невозможно писать длинные записи
                if (savePath != null)
                {
                    foreach(var gd in Core.adcReader.graphData_arr)
                        gd.SaveToFile(savePath + " " + gd.name + ".dat");
                }
                Application.Exit();
            }
        }
    }
}
