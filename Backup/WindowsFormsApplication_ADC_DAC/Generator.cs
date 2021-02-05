using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Xml.Serialization;
using System.IO;

namespace WindowsFormsApplication_ADC_DAC
{
    public class Generator
    {
        public Generator()
        {
            settings = new Settings();
            settings.settingsPulsed.ChangedEvent += new EventHandler(settingsPulsed_ChangedEvent);
            settings.settingsContinuous.ChangedEvent += new EventHandler(settingsContinuous_ChangedEvent);

            grapherPulse.Add(new GraphData_Function("Signal", System.Drawing.Color.Red, 0, settings.settingsPulsed.durationTime, new GraphData_Function.GraphFunc(Signal)));
            grapherPulse.Add(new GraphData_Function("Frequency", System.Drawing.Color.Blue, 0, settings.settingsPulsed.durationTime, new GraphData_Function.GraphFunc(Frequency)));
            grapherPulse.Add(new GraphData_Function("Amplitude", System.Drawing.Color.Black, 0, settings.settingsPulsed.durationTime, new GraphData_Function.GraphFunc(Amplitude)));

            grapherContinuous.Add(new GraphData_Function("Signal", System.Drawing.Color.Red, 0, 1/settings.settingsContinuous.frequency0, new GraphData_Function.GraphFunc(Signal)));

            grapherContinuous.Dock = grapherPulse.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        public void settingsPulsed_ChangedEvent(object sender, EventArgs e)
        {
            ((GraphData_Function)(grapherPulse.gdList[0])).xMax = settings.settingsPulsed.durationTime;
            ((GraphData_Function)(grapherPulse.gdList[1])).xMax = settings.settingsPulsed.durationTime;
            ((GraphData_Function)(grapherPulse.gdList[2])).xMax = settings.settingsPulsed.durationTime;    
            grapherPulse.UpdateFuncGraphs();
            foreach (GraphData_Function gd in grapherPulse.gdList)
                gd.Boarders = gd.BoardersFull;
            grapherPulse.UpdateGraph();
        }
        public void settingsContinuous_ChangedEvent(object sender, EventArgs e)
        {
            ((GraphData_Function)(grapherContinuous.gdList[0])).xMax = 1 / settings.settingsContinuous.frequency0;            
            grapherContinuous.UpdateFuncGraphs();
            foreach (GraphData_Function gd in grapherContinuous.gdList)
                gd.Boarders = gd.BoardersFull;
            grapherContinuous.UpdateGraph();
        }
        public Settings settings;

        public Grapher grapherPulse = new Grapher();
        public Grapher grapherContinuous = new Grapher();

        public bool stopFlag = true;
        public void Start()
        {
            if (stopFlag)
            {
                stopFlag = false;
                startTime = DateTime.Now;

                tempDateTime = DateTime.Now;
                iFreq = 0;

                Thread generationThread = new Thread(new ThreadStart(Generate));
                generationThread.Name = "GeneratingLoop";
                //generationThread.IsBackground = true;
                generationThread.Start();
            }
        }
        public void Stop()
        {
            stopFlag = true;
            Thread.Sleep(10);
            Core.module.outputVoltage = 0;
        }
                
        private double time; //sec
        public double timeFromStart
        {
            get { return time; }
        }
        private DateTime startTime;

        private DateTime tempDateTime;
        private int iFreq = 0;
        private double voltageUpdareFrequency;
                

        private void Generate()
        {
            while (!stopFlag)
            {
                time = (DateTime.Now - startTime).TotalSeconds;

                if (mode == Mode.Pulse && settings.settingsPulsed.repeatTime==0 && time > settings.settingsPulsed.durationTime)
                {
                    stopFlag = true;
                    Core.module.outputVoltage = 0;
                }
                else
                    Core.module.outputVoltage = Signal(time);
                
                if ((iFreq++)%30 == 0)
                {
                    voltageUpdareFrequency = 30 / (DateTime.Now - tempDateTime).TotalSeconds;
                    tempDateTime = DateTime.Now;
                }                
            }
        }


        

        //можно вынести в отдельный файл

        public double Signal(double time)
        {
            if (mode == Mode.Pulse)
            {
                double t = time;
                if (settings.settingsPulsed.repeatTime != 0)
                    t %= settings.settingsPulsed.repeatTime;
                if (!settings.settingsPulsed.useManualSignal)
                {
                    if (t > settings.settingsPulsed.durationTime)
                        return 0;

                    double frequency = Frequency(t);
                    if (deltaPhase(t, frequency) > 0)
                        return Amplitude(t, frequency) * Math.Sin(2 * Math.PI * frequency * t);
                    else
                        return 0;
                }
                else
                {
                    //добавить генерацию из заданного вручную сигнала
                    return 0;
                }
            }
            if (mode == Mode.Continuous)
            {
                if (!settings.settingsContinuous.useManualSignal)
                {
                    double result = 0;
                    for (int k = 0; k < settings.settingsContinuous.As.Length; k++)
                        result += settings.settingsContinuous.As[k] * Math.Sin(2 * Math.PI * settings.settingsContinuous.frequency0 * k * time);
                    for (int k = 0; k < settings.settingsContinuous.Ac.Length; k++)
                        result += settings.settingsContinuous.Ac[k] * Math.Cos(2 * Math.PI * settings.settingsContinuous.frequency0 * k * time);
                    return result;
                }
                else
                {
                    //добавить генерацию из заданного вручную сигнала
                    return 0;
                }
            }
            return 0;
        }
        public double Frequency(double time)
        {
            double fE = settings.settingsPulsed.frequencyE;
            double fB = settings.settingsPulsed.frequencyB;

            if (settings.settingsPulsed.binomialOmega)
                return settings.settingsPulsed.frequencyE + (settings.settingsPulsed.frequencyAlpha + settings.settingsPulsed.frequencyBeta * time) * time;
            else
                return time / settings.settingsPulsed.durationTime * (fB - fE) + fE;
        }
        public double Amplitude(double time, double f)
        {
            double aE = settings.settingsPulsed.amplitudeE;
            double aB = settings.settingsPulsed.amplitudeB;

            if (settings.settingsPulsed.autoAmlitude)
                return aE * settings.settingsPulsed.frequencyE * settings.settingsPulsed.frequencyE /f / f ;
            else
                return time / settings.settingsPulsed.durationTime * (aB - aE) + aE;
        }
        public double Amplitude(double time)
        {
            return Amplitude(time, Frequency(time));
        }

        //Возвращает изменение фазы, произошедшее с передыдущего вызова этой функции.        
        public double deltaPhase(double time, double frequency)
        {
            double currentPhase = 2*3.14*time*frequency;
            double result = currentPhase - phase;
            phase = currentPhase;
            return result;
        }
        double phase = 0;

        public enum Mode { Pulse, Continuous };
        public Mode mode = Mode.Pulse;

        [Serializable]
        public class Settings
        {
            public SettingsPulsed settingsPulsed = new SettingsPulsed();
            public SettingsContinuous settingsContinuous = new SettingsContinuous();

            public void SaveToFile(string path)
            {
                XmlSerializer xmlFormat = new XmlSerializer(this.GetType(), new Type[] { typeof(SettingsPulsed), typeof(SettingsContinuous) });
                Stream fStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                xmlFormat.Serialize(fStream, this);
                fStream.Close();
            }
            public Settings LoadFromFile(string path)
            {
                XmlSerializer xmlFormat = new XmlSerializer(this.GetType(), new Type[] { typeof(SettingsPulsed), typeof(SettingsContinuous) });

                Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                Settings loadedSettings = (Settings)xmlFormat.Deserialize(fStream);
                fStream.Close();

                return loadedSettings;
            }
        }

        [Serializable]
        public class SettingsPulsed
        {
            public double durationTime = 3;
            public double repeatTime = 0;

            public double frequencyE = 3;
            public double frequencyB = 1;

            public double amplitudeE = 3;
            public double amplitudeB = 0;
            public bool autoAmlitude = true;

            public bool binomialOmega = false;
            public double frequencyAlpha = 0;
            public double frequencyBeta = 0;

            public bool useManualSignal = false;
            public KeyValuePair<double, double>[] manualSignal = new KeyValuePair<double, double>[] { };

            public event EventHandler ChangedEvent;
            public void RaiseValueChangedEvent(object sender)
            {
                if (ChangedEvent != null)
                    ChangedEvent(sender, new EventArgs());
            }            
        }

        [Serializable]
        public class SettingsContinuous
        {
            public double frequency0 = 1;
            //коэффициенты перед синусами в фурье
            public double[] As = new double[]{0,1};
            //коэффициенты перед косинусами в фурье
            public double[] Ac = new double[] {};

            public bool useManualSignal = false;
            public KeyValuePair<double, double>[] manualSignal = new KeyValuePair<double, double>[] { };

            public event EventHandler ChangedEvent;
            public void RaiseValueChangedEvent(object sender)
            {
                if (ChangedEvent != null)
                    ChangedEvent(sender, new EventArgs());
            }            
        }
    }
}
