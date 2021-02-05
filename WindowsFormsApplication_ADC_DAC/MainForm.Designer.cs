namespace WindowsFormsApplication_ADC_DAC
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.button3 = new System.Windows.Forms.Button();
            this.panelGenerator = new System.Windows.Forms.Panel();
            this.panelGeneratorBottom = new System.Windows.Forms.Panel();
            this.statusStripGenerator = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelGenerator = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripGenerator = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoad = new System.Windows.Forms.ToolStripButton();
            this.panelADC = new System.Windows.Forms.Panel();
            this.statusStripADC = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelADC = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripADC = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButtonStart = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItemStartADCWithGenerator = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonStopADC = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSaveADC = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSaveAsADC = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.timerInfoUpdate = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialogGenerator = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogGenerator = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogADC = new System.Windows.Forms.SaveFileDialog();
            this.toolStripButtonPropADC = new System.Windows.Forms.ToolStripButton();
            this.paramsPulse = new WindowsFormsApplication_ADC_DAC.Params();
            this.paramsContinuous = new WindowsFormsApplication_ADC_DAC.Params();
            this.logger1 = new WindowsFormsApplication_ADC_DAC.Logger();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonADCFourier = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panelGeneratorBottom.SuspendLayout();
            this.statusStripGenerator.SuspendLayout();
            this.toolStripGenerator.SuspendLayout();
            this.statusStripADC.SuspendLayout();
            this.toolStripADC.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel1.Controls.Add(this.statusStripGenerator);
            this.splitContainer1.Panel1.Controls.Add(this.toolStripGenerator);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelADC);
            this.splitContainer1.Panel2.Controls.Add(this.statusStripADC);
            this.splitContainer1.Panel2.Controls.Add(this.toolStripADC);
            this.splitContainer1.Size = new System.Drawing.Size(1071, 631);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.button1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelGenerator, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelGeneratorBottom, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1071, 303);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(3, 236);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(194, 64);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(194, 227);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(186, 201);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pulsed";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.paramsPulse, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button2, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(180, 195);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(3, 173);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(174, 19);
            this.button2.TabIndex = 2;
            this.button2.Text = "Submit";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(186, 201);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Continuous";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.paramsContinuous, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.button3, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(180, 195);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // button3
            // 
            this.button3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button3.Location = new System.Drawing.Point(3, 173);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(174, 19);
            this.button3.TabIndex = 2;
            this.button3.Text = "Submit";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // panelGenerator
            // 
            this.panelGenerator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGenerator.Location = new System.Drawing.Point(203, 3);
            this.panelGenerator.Name = "panelGenerator";
            this.panelGenerator.Size = new System.Drawing.Size(865, 227);
            this.panelGenerator.TabIndex = 2;
            // 
            // panelGeneratorBottom
            // 
            this.panelGeneratorBottom.Controls.Add(this.logger1);
            this.panelGeneratorBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGeneratorBottom.Location = new System.Drawing.Point(203, 236);
            this.panelGeneratorBottom.Name = "panelGeneratorBottom";
            this.panelGeneratorBottom.Size = new System.Drawing.Size(865, 64);
            this.panelGeneratorBottom.TabIndex = 3;
            // 
            // statusStripGenerator
            // 
            this.statusStripGenerator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelGenerator});
            this.statusStripGenerator.Location = new System.Drawing.Point(0, 328);
            this.statusStripGenerator.Name = "statusStripGenerator";
            this.statusStripGenerator.Size = new System.Drawing.Size(1071, 22);
            this.statusStripGenerator.TabIndex = 1;
            this.statusStripGenerator.Text = "statusStrip1";
            // 
            // toolStripStatusLabelGenerator
            // 
            this.toolStripStatusLabelGenerator.Name = "toolStripStatusLabelGenerator";
            this.toolStripStatusLabelGenerator.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabelGenerator.Text = "Status";
            // 
            // toolStripGenerator
            // 
            this.toolStripGenerator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripButtonSaveAs,
            this.toolStripButtonLoad});
            this.toolStripGenerator.Location = new System.Drawing.Point(0, 0);
            this.toolStripGenerator.Name = "toolStripGenerator";
            this.toolStripGenerator.Size = new System.Drawing.Size(1071, 25);
            this.toolStripGenerator.TabIndex = 0;
            this.toolStripGenerator.Text = "toolStrip2";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonSaveAs
            // 
            this.toolStripButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveAs.Image")));
            this.toolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
            this.toolStripButtonSaveAs.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonSaveAs.Text = "Save As";
            this.toolStripButtonSaveAs.Click += new System.EventHandler(this.toolStripButtonSaveAs_Click);
            // 
            // toolStripButtonLoad
            // 
            this.toolStripButtonLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLoad.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLoad.Image")));
            this.toolStripButtonLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoad.Name = "toolStripButtonLoad";
            this.toolStripButtonLoad.Size = new System.Drawing.Size(37, 22);
            this.toolStripButtonLoad.Text = "Load";
            this.toolStripButtonLoad.Click += new System.EventHandler(this.toolStripButtonLoad_Click);
            // 
            // panelADC
            // 
            this.panelADC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelADC.Location = new System.Drawing.Point(0, 25);
            this.panelADC.Name = "panelADC";
            this.panelADC.Size = new System.Drawing.Size(1071, 230);
            this.panelADC.TabIndex = 2;
            // 
            // statusStripADC
            // 
            this.statusStripADC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelADC});
            this.statusStripADC.Location = new System.Drawing.Point(0, 255);
            this.statusStripADC.Name = "statusStripADC";
            this.statusStripADC.Size = new System.Drawing.Size(1071, 22);
            this.statusStripADC.TabIndex = 1;
            this.statusStripADC.Text = "statusStrip2";
            // 
            // toolStripStatusLabelADC
            // 
            this.toolStripStatusLabelADC.Name = "toolStripStatusLabelADC";
            this.toolStripStatusLabelADC.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabelADC.Text = "Status";
            // 
            // toolStripADC
            // 
            this.toolStripADC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonStart,
            this.toolStripButtonStopADC,
            this.toolStripSeparator1,
            this.toolStripButtonSaveADC,
            this.toolStripButtonSaveAsADC,
            this.toolStripSeparator2,
            this.toolStripButtonPropADC,
            this.toolStripSeparator3,
            this.toolStripButtonADCFourier});
            this.toolStripADC.Location = new System.Drawing.Point(0, 0);
            this.toolStripADC.Name = "toolStripADC";
            this.toolStripADC.Size = new System.Drawing.Size(1071, 25);
            this.toolStripADC.TabIndex = 0;
            this.toolStripADC.Text = "toolStrip1";
            // 
            // toolStripSplitButtonStart
            // 
            this.toolStripSplitButtonStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButtonStart.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemStartADCWithGenerator});
            this.toolStripSplitButtonStart.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonStart.Image")));
            this.toolStripSplitButtonStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonStart.Name = "toolStripSplitButtonStart";
            this.toolStripSplitButtonStart.Size = new System.Drawing.Size(47, 22);
            this.toolStripSplitButtonStart.Text = "Start";
            this.toolStripSplitButtonStart.ButtonClick += new System.EventHandler(this.toolStripSplitButtonStart_ButtonClick);
            // 
            // toolStripMenuItemStartADCWithGenerator
            // 
            this.toolStripMenuItemStartADCWithGenerator.Checked = true;
            this.toolStripMenuItemStartADCWithGenerator.CheckOnClick = true;
            this.toolStripMenuItemStartADCWithGenerator.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemStartADCWithGenerator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItemStartADCWithGenerator.Name = "toolStripMenuItemStartADCWithGenerator";
            this.toolStripMenuItemStartADCWithGenerator.Size = new System.Drawing.Size(178, 22);
            this.toolStripMenuItemStartADCWithGenerator.Text = "Start with generator";
            // 
            // toolStripButtonStopADC
            // 
            this.toolStripButtonStopADC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonStopADC.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStopADC.Image")));
            this.toolStripButtonStopADC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStopADC.Name = "toolStripButtonStopADC";
            this.toolStripButtonStopADC.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonStopADC.Text = "Stop";
            this.toolStripButtonStopADC.Click += new System.EventHandler(this.toolStripButtonStopADC_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonSaveADC
            // 
            this.toolStripButtonSaveADC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveADC.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveADC.Image")));
            this.toolStripButtonSaveADC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveADC.Name = "toolStripButtonSaveADC";
            this.toolStripButtonSaveADC.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonSaveADC.Text = "Save";
            this.toolStripButtonSaveADC.Click += new System.EventHandler(this.toolStripButtonSaveADC_Click);
            // 
            // toolStripButtonSaveAsADC
            // 
            this.toolStripButtonSaveAsADC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSaveAsADC.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSaveAsADC.Image")));
            this.toolStripButtonSaveAsADC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSaveAsADC.Name = "toolStripButtonSaveAsADC";
            this.toolStripButtonSaveAsADC.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonSaveAsADC.Text = "Save As";
            this.toolStripButtonSaveAsADC.Click += new System.EventHandler(this.toolStripButtonSaveAsADC_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // timerInfoUpdate
            // 
            this.timerInfoUpdate.Enabled = true;
            this.timerInfoUpdate.Interval = 50;
            this.timerInfoUpdate.Tick += new System.EventHandler(this.timerInfoUpdate_Tick);
            // 
            // saveFileDialogGenerator
            // 
            this.saveFileDialogGenerator.Filter = "Settings files|*.set";
            // 
            // openFileDialogGenerator
            // 
            this.openFileDialogGenerator.Filter = "Settings files|*.set";
            // 
            // saveFileDialogADC
            // 
            this.saveFileDialogADC.Filter = "Data files|*.dat";
            // 
            // toolStripButtonPropADC
            // 
            this.toolStripButtonPropADC.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonPropADC.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPropADC.Image")));
            this.toolStripButtonPropADC.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPropADC.Name = "toolStripButtonPropADC";
            this.toolStripButtonPropADC.Size = new System.Drawing.Size(64, 22);
            this.toolStripButtonPropADC.Text = "Properties";
            this.toolStripButtonPropADC.Click += new System.EventHandler(this.toolStripButtonPropADC_Click);
            // 
            // paramsPulse
            // 
            this.paramsPulse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramsPulse.Location = new System.Drawing.Point(3, 3);
            this.paramsPulse.Name = "paramsPulse";
            this.paramsPulse.Size = new System.Drawing.Size(174, 164);
            this.paramsPulse.TabIndex = 1;
            // 
            // paramsContinuous
            // 
            this.paramsContinuous.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramsContinuous.Location = new System.Drawing.Point(3, 3);
            this.paramsContinuous.Name = "paramsContinuous";
            this.paramsContinuous.Size = new System.Drawing.Size(174, 164);
            this.paramsContinuous.TabIndex = 1;
            // 
            // logger1
            // 
            this.logger1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logger1.Location = new System.Drawing.Point(0, 0);
            this.logger1.Name = "logger1";
            this.logger1.Size = new System.Drawing.Size(865, 64);
            this.logger1.TabIndex = 0;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonADCFourier
            // 
            this.toolStripButtonADCFourier.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonADCFourier.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonADCFourier.Image")));
            this.toolStripButtonADCFourier.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonADCFourier.Name = "toolStripButtonADCFourier";
            this.toolStripButtonADCFourier.Size = new System.Drawing.Size(63, 22);
            this.toolStripButtonADCFourier.Text = "Show FTT";
            this.toolStripButtonADCFourier.Click += new System.EventHandler(this.toolStripButtonADCFourier_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1071, 631);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.panelGeneratorBottom.ResumeLayout(false);
            this.statusStripGenerator.ResumeLayout(false);
            this.statusStripGenerator.PerformLayout();
            this.toolStripGenerator.ResumeLayout(false);
            this.toolStripGenerator.PerformLayout();
            this.statusStripADC.ResumeLayout(false);
            this.statusStripADC.PerformLayout();
            this.toolStripADC.ResumeLayout(false);
            this.toolStripADC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStripADC;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveADC;
        private System.Windows.Forms.StatusStrip statusStripGenerator;
        private System.Windows.Forms.ToolStrip toolStripGenerator;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Params paramsPulse;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Params paramsContinuous;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGenerator;
        private System.Windows.Forms.StatusStrip statusStripADC;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelADC;
        private System.Windows.Forms.Panel panelGenerator;
        private System.Windows.Forms.Panel panelGeneratorBottom;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAs;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoad;
        private System.Windows.Forms.Panel panelADC;
        private Logger logger1;
        private System.Windows.Forms.Timer timerInfoUpdate;
        private System.Windows.Forms.SaveFileDialog saveFileDialogGenerator;
        private System.Windows.Forms.OpenFileDialog openFileDialogGenerator;
        private System.Windows.Forms.ToolStripButton toolStripButtonStopADC;
        private System.Windows.Forms.SaveFileDialog saveFileDialogADC;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonStart;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartADCWithGenerator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSaveAsADC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonPropADC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonADCFourier;
    }
}