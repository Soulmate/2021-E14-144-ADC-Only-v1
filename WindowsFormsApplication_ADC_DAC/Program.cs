using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace WindowsFormsApplication_ADC_DAC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //парсинг входных парамеров
            if (args.Length == 0)
                Console.WriteLine("Запуск без параметров");
            else if (args.Length == 2) // savePath, duration
            {

                double duration;
                bool test1 = double.TryParse(args[1], out duration);                

                if (!test1)
                {
                    Console.WriteLine("Параметры не верные");
                    return;
                }                
                Core.automation.autoStart = true;
                Core.automation.autoEnd = true;
                Core.automation.savePath = args[0];
                Core.automation.duration = duration;
            }
            else
            {
                Console.WriteLine("Неправильное число параметров");
                return;
            }



            Application.Run(new ADC_Only());


            Core.adcReader.Stop();
            //Core.generator.Stop();
        }
    }
}
