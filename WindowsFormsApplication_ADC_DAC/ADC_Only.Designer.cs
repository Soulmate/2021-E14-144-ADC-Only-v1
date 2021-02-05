
namespace WindowsFormsApplication_ADC_DAC
{
    partial class ADC_Only
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBox_savePath = new System.Windows.Forms.TextBox();
            this.Save = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_ADCStart = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox_Log = new System.Windows.Forms.TextBox();
            this.button_ADCStop = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // textBox_savePath
            // 
            this.textBox_savePath.Location = new System.Drawing.Point(13, 13);
            this.textBox_savePath.Name = "textBox_savePath";
            this.textBox_savePath.Size = new System.Drawing.Size(517, 20);
            this.textBox_savePath.TabIndex = 0;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(536, 12);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(451, 219);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 177);
            this.panel1.TabIndex = 2;
            // 
            // button_ADCStart
            // 
            this.button_ADCStart.Location = new System.Drawing.Point(536, 117);
            this.button_ADCStart.Name = "button_ADCStart";
            this.button_ADCStart.Size = new System.Drawing.Size(75, 23);
            this.button_ADCStart.TabIndex = 1;
            this.button_ADCStart.Text = "Start";
            this.button_ADCStart.UseVisualStyleBackColor = true;
            this.button_ADCStart.Click += new System.EventHandler(this.button_ADCStart_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox_Log
            // 
            this.textBox_Log.Location = new System.Drawing.Point(13, 61);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Log.Size = new System.Drawing.Size(371, 347);
            this.textBox_Log.TabIndex = 3;
            // 
            // button_ADCStop
            // 
            this.button_ADCStop.Location = new System.Drawing.Point(617, 117);
            this.button_ADCStop.Name = "button_ADCStop";
            this.button_ADCStop.Size = new System.Drawing.Size(75, 23);
            this.button_ADCStop.TabIndex = 1;
            this.button_ADCStop.Text = "Stop";
            this.button_ADCStop.UseVisualStyleBackColor = true;
            this.button_ADCStop.Click += new System.EventHandler(this.button_ADCStop_Click);
            // 
            // ADC_Only
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox_Log);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_ADCStop);
            this.Controls.Add(this.button_ADCStart);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.textBox_savePath);
            this.Name = "ADC_Only";
            this.Text = "ADC_Only";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_savePath;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_ADCStart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_Log;
        private System.Windows.Forms.Button button_ADCStop;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}