using System;
using System.Drawing;
using System.Windows.Forms;

using static System.Math;

namespace goldenMethod
{
    public partial class Form1 : Form
    {
        private const int CS_DROPSHADOW = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Panel1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public Form1()
        {
            InitializeComponent();
            chart1.Series[1].Color = Color.CornflowerBlue;
            chart1.Series[1].BorderWidth = 3;
            chart1.Series[2].Color = Color.CornflowerBlue;
            chart1.Series[2].BorderWidth = 3;
            chart1.Series[3].Color = Color.Black;
            chart1.Series[4].Color = Color.Black;
            chart1.Series[0].Color = Color.Red;
            chart1.Series[0].BorderWidth = 3;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private double function(double x)
        {
            double y = Double.NaN;
            string expression = comboBox1.Text;
            expression = expression.Replace(String.Format("x"), x.ToString().Replace(String.Format(","), String.Format(".")));
            expression = expression.Replace(String.Format("e"), "2.71828182846");
            //textBox4.AppendText(Round(x, 2) + "; " + Round(Convert.ToDouble(JsMath.Eval(expression)), 2) + "\r\n");
            try
            {
                y = Convert.ToDouble(JsMath.Eval(expression));
            }
            catch
            {
                
            }
            return y;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int counter = 0, counter2 = 0;
                double a, b, ao, bo, eps, xmax, xmin, x1, x2, y1, y2, F = (1 + Sqrt(5)) / 2;
                Double.TryParse(textBox1.Text, out a);
                Double.TryParse(textBox2.Text, out b);
                Double.TryParse(textBox3.Text, out eps);
                ao = a;
                bo = b;

                while (Math.Abs(b - a) >= eps)
                {
                    counter++;
                    x1 = b - (b - a) / F;
                    x2 = a + (b - a) / F;
                    y1 = function(x1);
                    y2 = function(x2);
                    if (Double.IsNaN(y1) || Double.IsNaN(y2))
                    {
                        MessageBox.Show("Некорректная функция", "Ошибка функции");
                        break;
                    }
                    if (checkBox1.Checked == true)
                    {
                        textBox4.AppendText("\r\nИТЕРАЦИЯ №" + counter + " (поиск max)\r\n");
                        textBox4.AppendText("x1=" + Round(x1, 4) + "; ");
                        textBox4.AppendText("x2=" + Round(x2, 4) + "\r\n");
                        textBox4.AppendText("y1=" + Round(y1, 4) + "; ");
                        textBox4.AppendText("y2=" + Round(y2, 4) + "\r\n");
                    }
                    if (y1 <= y2)
                    {
                        a = x1;
                        if (checkBox1.Checked == true)
                            textBox4.AppendText("y1≤y2 → a=" + Round(a, 4) + "; b=" + Round(b, 4) + "\r\n");
                    }
                    else
                    {
                        b = x2;
                        if (checkBox1.Checked == true)
                            textBox4.AppendText("y1>y2 → a=" + Round(a, 4) + "; b=" + Round(b, 4) + "\r\n");
                    }
                    if (checkBox1.Checked == true)
                    {
                        textBox4.AppendText("Погрешность: " + (b - a) + "<" + eps + "\r\n");
                        textBox4.AppendText("===========================\r\n");
                    }
                }
                xmax = (a + b) / 2;

                a = ao;
                b = bo;
                while (Math.Abs(b - a) >= eps)
                {
                    counter2++;
                    x1 = b - (b - a) / F;
                    x2 = a + (b - a) / F;
                    y1 = function(x1);
                    y2 = function(x2);
                    if (checkBox1.Checked == true)
                    {
                        textBox4.AppendText("\r\nИТЕРАЦИЯ №" + counter2 + " (поиск min)\r\n");
                        textBox4.AppendText("x1=" + Round(x1, 4) + "; ");
                        textBox4.AppendText("x2=" + Round(x2, 4) + "\r\n");
                        textBox4.AppendText("y1=" + Round(y1, 4) + "; ");
                        textBox4.AppendText("y2=" + Round(y2, 4) + "\r\n");
                    }
                    if (y1 > y2)
                    {
                        a = x1;
                        if (checkBox1.Checked == true)
                            textBox4.AppendText("y1>y2 → a=" + Round(a, 4) + "; b=" + Round(b, 4) + "\r\n");
                    }
                    else
                    {
                        b = x2;
                        if (checkBox1.Checked == true)
                            textBox4.AppendText("y1≤y2 → a=" + Round(a, 4) + "; b=" + Round(b, 4) + "\r\n");
                    }
                    if (checkBox1.Checked == true)
                    {
                        textBox4.AppendText("Погрешность: " + (b - a) + "<" + eps + "\r\n");
                        textBox4.AppendText("===========================\r\n");
                    }
                }
                xmin = (a + b) / 2;

                textBox4.AppendText("\r\n\r\nРЕЗУЛЬТАТ\r\nЗа " + counter + " и " + counter2 + " итераций (соответственно) с точностью e=" + eps + " найден максимум функции f(x)=" + comboBox1.Text + " на отрезке от a=" + Round(ao, 4) + " до b=" + Round(bo, 4) + " в точке:\r\nx=" + xmax + "\r\ny(x)=" + function(xmax) + "\r\nи минимум - в точке: \r\nx=" + xmin + "\r\ny(x)=" + function(xmin) + "\r\n");

                double XAmin = -6, XAmax = 6, YAmin = function(xmin) - Abs(function(xmin) / 2 + 1), YAmax = function(xmax) + Abs(function(xmax) / 2 + 1);
                if (Convert.ToDouble(textBox7.Text) < Convert.ToDouble(textBox6.Text))
                {
                    XAmin = Convert.ToDouble(textBox7.Text);
                    XAmax = Convert.ToDouble(textBox6.Text);
                }
                else
                    MessageBox.Show("Некорректная область определения", "Ошибка графика");

                chart1.ChartAreas[0].AxisX.Minimum = XAmin;
                chart1.ChartAreas[0].AxisX.Maximum = XAmax;
                chart1.ChartAreas[0].AxisY.Minimum = YAmin;
                chart1.ChartAreas[0].AxisY.Maximum = YAmax;
                chart1.Series[0].Points.Clear();
                for (double px = XAmin; px <= XAmax; px += 0.1)
                {
                    double py = function(px);
                    if (Double.IsNegativeInfinity(py) == true)
                        py = YAmin;
                    if (Double.IsPositiveInfinity(py) == true)
                        py = YAmax;
                    chart1.Series[0].Points.AddXY(px, py);
                }
                chart1.Series[1].Points.Clear();
                chart1.Series[1].Points.AddXY(ao, YAmin * 1.1);
                chart1.Series[1].Points.AddXY(ao, function(xmin) + (function(xmax) - function(xmin)) / 2);
                chart1.Series[1].Points.AddXY(ao, YAmax * 1.1);
                chart1.Series[2].Points.Clear();
                chart1.Series[2].Points.AddXY(bo, YAmin * 1.1);
                chart1.Series[2].Points.AddXY(bo, function(xmin) + (function(xmax) - function(xmin)) / 2);
                chart1.Series[2].Points.AddXY(bo, YAmax * 1.1);
                chart1.Series[3].Points.Clear();
                chart1.Series[3].Points.AddXY(XAmin, function(xmax));
                chart1.Series[3].Points.AddXY(xmax, function(xmax));
                chart1.Series[3].Points.AddXY(XAmax, function(xmax));
                chart1.Series[4].Points.Clear();
                chart1.Series[4].Points.AddXY(XAmin, function(xmin));
                chart1.Series[4].Points.AddXY(xmax, function(xmin));
                chart1.Series[4].Points.AddXY(XAmax, function(xmin));
                chart1.Series[5].Points.Clear();
                chart1.Series[5].Points.AddXY(xmax, function(xmax));
                chart1.Series[6].Points.Clear();
                chart1.Series[6].Points.AddXY(xmin, function(xmin));
            }
            catch
            {
                MessageBox.Show("Некорректный ввод", "Ошибка ввода");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(textBox4.Text);
        }

        private void Chart1_Click(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
