using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApplication_ADC_DAC
{
    static class Core
    {
        public static LusbApi_Wrapper.Module module;
        public static Generator generator;
        public static AdcReader adcReader;

        static Core()
        {
            
            generator = new Generator();
            adcReader = new AdcReader();

            //generator.Start();
        }
    }
}
