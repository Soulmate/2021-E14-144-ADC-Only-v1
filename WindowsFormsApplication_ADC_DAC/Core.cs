using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsApplication_ADC_DAC
{
    static class Core
    {
        public static LusbApi_Wrapper.Module module;
        public static AdcReader adcReader;
        public static Automation automation;

        static Core()
        {   
            adcReader = new AdcReader(); //тут инициализируется устройство            
            automation = new Automation();
            automation.Start();            
        }
    }
}
