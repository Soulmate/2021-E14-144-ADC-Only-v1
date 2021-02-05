using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Reflection;

namespace WindowsFormsApplication_ADC_DAC
{
    public partial class Params : UserControl
    {
        public Params()
        {
            InitializeComponent();
        }

        public override string Text
        {
            set
            {
                groupBox1.Text = value;
            }
        }

        private List<Param> paramList = new List<Param>();

        public void Associate(object Settings)
        {
            Type type = Settings.GetType();
            FieldInfo[] fiArray = type.GetFields();            
            MethodInfo mi = type.GetMethod("RaiseValueChangedEvent");
            EventInfo ei = type.GetEvent("ChangedEvent");

            paramList.Clear();
            panel1.Controls.Clear();

            foreach (FieldInfo fi in fiArray)
            {
                if (fi.FieldType == typeof(Double))
                    Add(new ParamDouble(Settings, fi, mi, ei));                
                if (fi.FieldType == typeof(Boolean))                    
                    Add(new ParamBool(Settings, fi, mi, ei));
                if (fi.FieldType == typeof(double[]))
                    Add(new ParamDoubleArray(Settings, fi, mi, ei));
                if (fi.FieldType == typeof(KeyValuePair<double,double>[] ))
                    Add(new ParamDoubleDoubleArray(Settings, fi, mi, ei));
            }
        } 

        private void Add(Param p)
        {
            paramList.Add(p);

            int y = 0;
            if (panel1.Controls.Count >0 )            
                y = 2 + panel1.Controls[panel1.Controls.Count - 1].Location.Y + panel1.Controls[panel1.Controls.Count - 1].Height;

            Label l = new Label();
            l.Text = p.name;
            l.Location = new Point(0, y);
            l.TextAlign = ContentAlignment.MiddleLeft;
            panel1.Controls.Add(l);

            if (p is ParamDoubleDoubleArray)
            {
                p.control.Location = new Point(0, y + l.Height);
                p.control.Width = l.Width + 50;
            }
            else
            {
                p.control.Location = new Point(l.Width, y);
                p.control.Width = 50;
            }
            panel1.Controls.Add(p.control);
        }


        
        abstract class Param
        {
            public abstract Control control
            {get;}
            public string name;
            public object target;
        }

        class ParamDouble:Param
        {            
            public double value;
            public TextBox tb;

            public override Control control
            {
                get { return (Control)tb; }
            }

            bool internalIsCorrect;
            bool isCorrect
            {
                get { return internalIsCorrect; }
                set
                {
                    internalIsCorrect = value;
                    if (value)
                        tb.ForeColor = Color.Black;
                    else
                        tb.ForeColor = Color.Red;
                }
            }
            
            MethodInfo mi;
            FieldInfo fi;
            

            public ParamDouble(object target, FieldInfo fi, MethodInfo mi, EventInfo ei)
            {
                name = fi.Name;
                this.mi = mi;
                this.fi = fi;
                this.target = target;
                value = (double)fi.GetValue(target);
                tb = new TextBox();
                tb.Text = value.ToString();
                
                tb.Leave += new EventHandler(tb_Leave);
                tb.KeyDown += new KeyEventHandler(tb_KeyDown);

                ei.AddEventHandler(target, new EventHandler(ValueUpdate));
            }

            void tb_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                    tb_Leave(sender, e);
            }

            private void tb_Leave(object sender, EventArgs e)
            {
                double tmpValue;
                if (double.TryParse(tb.Text, out tmpValue))
                {
                    if (tmpValue != value)
                    {
                        value = tmpValue;
                        fi.SetValue(target, value);
                        isCorrect = true;

                        mi.Invoke(target, new object[] { name });
                    }
                }
                else
                    isCorrect = false;
            }

            private void ValueUpdate(object sender, EventArgs e)
            {
                value = (double)fi.GetValue(target);
                tb.Text = value.ToString();
            }
        }

        class ParamBool:Param
        {            
            public bool value;
            public CheckBox cb;
            public override Control control
            {
                get { return (Control)cb; }
            }
                        
            MethodInfo mi;
            FieldInfo fi;

            public ParamBool(object target, FieldInfo fi, MethodInfo mi, EventInfo ei)
            {
                name = fi.Name;
                this.mi = mi;
                this.fi = fi;
                this.target = target;
                value = (bool)fi.GetValue(target);
                cb = new CheckBox();
                cb.Checked = value;

                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);

                ei.AddEventHandler(target, new EventHandler(ValueUpdate));
            }

            void cb_CheckedChanged(object sender, EventArgs e)
            {
                value = cb.Checked;
                fi.SetValue(target, value);
                
                mi.Invoke(target, new object[] { name });
            }

            private void ValueUpdate(object sender, EventArgs e)
            {
                value = (bool)fi.GetValue(target);
                cb.Checked = value;
            }
        }

        class ParamDoubleArray : Param
        {
            public double[] value;
            public TextBox tb;
            private string tmpText;

            public override Control control
            {
                get { return (Control)tb; }
            }

            bool internalIsCorrect;
            bool isCorrect
            {
                get { return internalIsCorrect; }
                set
                {
                    internalIsCorrect = value;
                    if (value)
                        tb.ForeColor = Color.Black;
                    else
                        tb.ForeColor = Color.Red;
                }
            }

            MethodInfo mi;
            FieldInfo fi;

            public ParamDoubleArray(object target, FieldInfo fi, MethodInfo mi, EventInfo ei)
            {
                name = fi.Name;
                this.mi = mi;
                this.fi = fi;
                this.target = target;
                value = (double[])fi.GetValue(target);
                tb = new TextBox();
                tb.Multiline = true;
                tb.Height = 60;
                tb.ScrollBars = ScrollBars.Vertical;
                tb.Text = "";
                if (value != null)
                    foreach (double d in value)
                        tb.Text += d.ToString() + "\r\n";

                tb.Leave += new EventHandler(tb_Leave);
                tb.KeyDown += new KeyEventHandler(tb_KeyDown);

                ei.AddEventHandler(target, new EventHandler(ValueUpdate));
            }

            void tb_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter && e.Control == true)
                    tb_Leave(sender, e);
            }

            private void tb_Leave(object sender, EventArgs e)
            {
                if (tmpText != tb.Text)
                {
                    List<double> tmpValue = new List<double>();
                    foreach (string s in tb.Lines)
                    {
                        if (s != "")
                        {
                            double d;
                            if (double.TryParse(s.Replace(" ", "").Replace("\t", ""), out d))
                                tmpValue.Add(d);
                            else
                            {
                                isCorrect = false;
                                return;
                            }
                        }
                        value = tmpValue.ToArray();
                    }

                    tb.Text = "";
                    if (value != null)
                        foreach (double d in value)
                            tb.Text += d.ToString() + "\r\n";

                    tmpText = tb.Text;

                    fi.SetValue(target, value);
                    isCorrect = true;

                    mi.Invoke(target, new object[] { name });
                }
            }

            private void ValueUpdate(object sender, EventArgs e)
            {
                value = (double[])fi.GetValue(target);
                tb.Text = "";
                if (value != null)
                foreach (double d in value)
                    tb.Text += d.ToString() + "\r\n";
            }
        }


        class ParamDoubleDoubleArray : Param
        {
            public KeyValuePair<double, double>[] value;
            public TextBox tb;
            private string tmpText;

            public override Control control
            {
                get { return (Control)tb; }
            }

            bool internalIsCorrect;
            bool isCorrect
            {
                get { return internalIsCorrect; }
                set
                {
                    internalIsCorrect = value;
                    if (value)
                        tb.ForeColor = Color.Black;
                    else
                        tb.ForeColor = Color.Red;
                }
            }

            MethodInfo mi;
            FieldInfo fi;

            public ParamDoubleDoubleArray(object target, FieldInfo fi, MethodInfo mi, EventInfo ei)
            {
                name = fi.Name;
                this.mi = mi;
                this.fi = fi;
                this.target = target;
                value = (KeyValuePair<double, double>[])fi.GetValue(target);
                tb = new TextBox();
                tb.Multiline = true;
                tb.Height = 150;
                tb.ScrollBars = ScrollBars.Vertical;
                tb.Text = "";
                if (value != null)
                foreach (KeyValuePair<double, double> dd in value)
                    tb.Text += dd.Key.ToString() + "\t" + dd.Value.ToString() + "\r\n";

                tb.Leave += new EventHandler(tb_Leave);
                tb.KeyDown += new KeyEventHandler(tb_KeyDown);

                ei.AddEventHandler(target, new EventHandler(ValueUpdate));
            }

            void tb_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter && e.Control == true)
                    tb_Leave(sender, e);
            }

            private void tb_Leave(object sender, EventArgs e)
            {
                if (tmpText != tb.Text)
                {
                    List<KeyValuePair<double, double>> tmpValue = new List<KeyValuePair<double, double>>();
                    foreach (string s in tb.Lines)
                    {
                        if (s != "")
                        {
                            string[] splited = s.Split(new String[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
                            if (splited.Length >= 2)
                            {
                                double x, y;
                                if (double.TryParse(splited[0], out x) && double.TryParse(splited[1], out y))
                                    tmpValue.Add(new KeyValuePair<double, double>(x, y));
                                else
                                {
                                    isCorrect = false;
                                    return;
                                }
                            }
                        }
                        value = tmpValue.ToArray();
                    }

                    tb.Text = "";
                    if (value != null)
                    foreach (KeyValuePair<double, double> dd in value)
                        tb.Text += dd.Key.ToString() + "\t" + dd.Value.ToString() + "\r\n";

                    tmpText = tb.Text;

                    fi.SetValue(target, value);
                    isCorrect = true;

                    mi.Invoke(target, new object[] { name });
                }
            }

            private void ValueUpdate(object sender, EventArgs e)
            {
                value = (KeyValuePair<double, double>[])fi.GetValue(target);
                tb.Text = "";
                if (value != null)
                foreach (KeyValuePair<double, double> dd in value)
                    tb.Text += dd.Key.ToString() + "\t" + dd.Value.ToString() + "\r\n";
            }
        }
    }
}
