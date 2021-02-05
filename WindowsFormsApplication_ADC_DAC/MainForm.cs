using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class MainForm : Form
    {        
        public MainForm()
        {
            InitializeComponent();

            logger1.yMax = 5;
            logger1.yMin = -5;

            paramsPulse.Associate(Core.generator.settings.settingsPulsed);
            paramsContinuous.Associate(Core.generator.settings.settingsContinuous);

            panelGenerator.Controls.Add(Core.generator.grapherPulse);

            panelADC.Controls.Add(Core.adcReader.grapherControl1);
        }

        private void timerInfoUpdate_Tick(object sender, EventArgs e)
        {
            logger1.Add(Core.module.outputVoltage);
            if (Core.generator.stopFlag)
            {
                button1.Text = "S T A R T";
            }
            else
            {
                button1.Text = "S T O P";
            }
            toolStripStatusLabelGenerator.Text = String.Format("time {0:f2} s, output {1:F2} V", Core.generator.timeFromStart, Core.module.outputVoltage);
            toolStripStatusLabelADC.Text = String.Format("{0} data points", Core.adcReader.graphData1.PointsCount);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Core.generator.stopFlag)
            {
                button1.Text = "S T O P";
                Core.generator.Start();

                if (toolStripMenuItemStartADCWithGenerator.Checked)
                    Core.adcReader.Start();
            }
            else
            {
                button1.Text = "S T A R T";
                Core.generator.Stop();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Core.generator.mode = (Generator.Mode)tabControl1.SelectedIndex;
            if (tabControl1.SelectedIndex == 0)
            {
                panelGenerator.Controls.Clear();
                panelGenerator.Controls.Add(Core.generator.grapherPulse);                
                Core.generator.settingsPulsed_ChangedEvent(this, new EventArgs());
            }
            else
            {
                panelGenerator.Controls.Clear();
                panelGenerator.Controls.Add(Core.generator.grapherContinuous);
                Core.generator.settingsContinuous_ChangedEvent(this, new EventArgs());                
            }
        }

        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            if (saveFileDialogGenerator.ShowDialog() == DialogResult.OK)
                Core.generator.settings.SaveToFile(saveFileDialogGenerator.FileName);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialogGenerator.FileName == "" && saveFileDialogGenerator.ShowDialog() == DialogResult.OK)                
                Core.generator.settings.SaveToFile(saveFileDialogGenerator.FileName);     

        }

        private void toolStripButtonLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialogGenerator.ShowDialog() == DialogResult.OK)
            {                
                Core.generator.settings = Core.generator.settings.LoadFromFile(openFileDialogGenerator.FileName);
                paramsContinuous.Associate(Core.generator.settings.settingsContinuous);
                paramsPulse.Associate(Core.generator.settings.settingsPulsed);
                Core.generator.settings.settingsPulsed.ChangedEvent += new EventHandler(Core.generator.settingsPulsed_ChangedEvent);
                Core.generator.settings.settingsContinuous.ChangedEvent += new EventHandler(Core.generator.settingsContinuous_ChangedEvent);
                Core.generator.settingsContinuous_ChangedEvent(this, new EventArgs());
                Core.generator.settingsPulsed_ChangedEvent(this, new EventArgs());
            }
        }

        private void toolStripButtonSaveADC_Click(object sender, EventArgs e)
        {
            if (Core.adcReader.graphData1 != null)
            {
                if (saveFileDialogADC.FileName != "")
                    Core.adcReader.graphData1.SaveToFile(saveFileDialogADC.FileName);
                else
                    if (saveFileDialogADC.ShowDialog() == DialogResult.OK)
                        Core.adcReader.graphData1.SaveToFile(saveFileDialogADC.FileName);
            }
        }

        private void toolStripButtonStopADC_Click(object sender, EventArgs e)
        {
            Core.adcReader.Stop();
        }

        private void toolStripSplitButtonStart_ButtonClick(object sender, EventArgs e)
        {
            Core.adcReader.Start();
        }

        private void toolStripButtonSaveAsADC_Click(object sender, EventArgs e)
        {
            if (Core.adcReader.graphData1 != null)
            {
                if (saveFileDialogADC.ShowDialog() == DialogResult.OK)
                    Core.adcReader.graphData1.SaveToFile(saveFileDialogADC.FileName);
            }
        }

        private void toolStripButtonPropADC_Click(object sender, EventArgs e)
        {
            ADCProps adcProps = new ADCProps();
            adcProps.StartPosition = FormStartPosition.CenterParent;
            if (adcProps.ShowDialog() == DialogResult.OK)
                Core.adcReader.memoryTime = adcProps.memoryTime;
        }

        private void toolStripButtonADCFourier_Click(object sender, EventArgs e)
        {
            (new ADC_FTT()).Show();
        }
    }
}
