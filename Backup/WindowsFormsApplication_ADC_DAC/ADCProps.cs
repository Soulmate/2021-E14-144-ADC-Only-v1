using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class ADCProps : Form
    {
        public double memoryTime;

        public ADCProps()
        {
            InitializeComponent();

            textBox1.Text = Core.adcReader.memoryTime.ToString();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            double d;
            if (double.TryParse(textBox1.Text, out d))
            {
                memoryTime = d;
                textBox1.Text = memoryTime.ToString();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                textBox1_Leave(this, e);
        }
    }
}
