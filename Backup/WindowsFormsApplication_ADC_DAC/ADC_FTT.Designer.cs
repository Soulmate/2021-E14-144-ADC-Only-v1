namespace WindowsFormsApplication_ADC_DAC
{
    partial class ADC_FTT
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
            this.grapher1 = new WindowsFormsApplication_ADC_DAC.Grapher();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // grapher1
            // 
            this.grapher1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grapher1.Location = new System.Drawing.Point(0, 0);
            this.grapher1.Name = "grapher1";
            this.grapher1.Size = new System.Drawing.Size(614, 373);
            this.grapher1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ADC_FTT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 373);
            this.Controls.Add(this.grapher1);
            this.Name = "ADC_FTT";
            this.Text = "ADC_FTT";
            this.ResumeLayout(false);

        }

        #endregion

        private Grapher grapher1;
        private System.Windows.Forms.Timer timer1;

    }
}