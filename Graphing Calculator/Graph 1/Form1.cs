using System;
using NCalc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Graph_1
{
    public partial class GraphDisplay : System.Windows.Forms.Form
    {
        private Chart chart;
        private TextBox functionInput;
        private Button plotButton;
        private Button zoomInButton;
        private Button zoomOutButton;

        private void Form1_Load(object sender, EventArgs e)
        { 
            string functionText = functionInput.Text;
            Func<double, double> func = ParseFunction(functionText);
            if (func != null)
            {
                PlotFunction(func, -15, 15, 0.01);
            }
            else
            {
                MessageBox.Show("Invalid function format.");
            }
        }

        private void InitializeChart()
        {
            chart = new Chart();
            chart.Dock = DockStyle.Fill;
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.Crossing = 0;
            chartArea.AxisY.Crossing = 0;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Interval = 1;
            chartArea.AxisX.LabelStyle.Interval = 1;
            chartArea.AxisY.LabelStyle.Interval = 1;
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.Maximum = 15;
            chartArea.AxisX.Minimum = -15;
            chartArea.AxisY.Maximum = 10;
            chartArea.AxisY.Minimum = -10;
            chartArea.AxisX.LabelStyle.IsEndLabelVisible = false;
            chartArea.AxisY.LabelStyle.IsEndLabelVisible = false;
            chart.ChartAreas.Add(chartArea);
            this.Controls.Add(chart);
        }

        private void InitializeControls()
        {
            FlowLayoutPanel buttonFrame = new FlowLayoutPanel();
            buttonFrame.Dock = DockStyle.Top;
            buttonFrame.Height = 40;
            buttonFrame.Width = ClientSize.Width;
            buttonFrame.FlowDirection = FlowDirection.LeftToRight;
            buttonFrame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(buttonFrame);

            functionInput = new TextBox();
            functionInput.Location = new Point(10, 40);
            this.Controls.Add(functionInput);

            plotButton = new Button();
            plotButton.Text = "Plot";
            plotButton.Click += PlotButton_Click;
            buttonFrame.Controls.Add(plotButton);

            zoomInButton = new Button();
            zoomInButton.Text = "Zoom In";
            zoomInButton.Click += ZoomInButton_Click;
            buttonFrame.Controls.Add(zoomInButton);

            zoomOutButton = new Button();
            zoomOutButton.Text = "Zoom Out";
            zoomOutButton.Click += ZoomOutButton_Click;
            buttonFrame.Controls.Add(zoomOutButton);
        }

        private void PlotButton_Click(object sender, EventArgs e)
        {
            string functionText = functionInput.Text;
            Func<double, double> func = ParseFunction(functionText);
            if (func != null)
            {
                PlotFunction(func, -15, 15, 0.01);
            }
            else
            {
                MessageBox.Show("Invalid function format.");
            }
        }

        private Func<double, double> ParseFunction(string functionText)
        {
            try
            {
                functionText = System.Text.RegularExpressions.Regex.Replace(functionText, @"(\w+)\^(\w+)", "Pow($1, $2)");

                return x =>
                {
                    var expression = new NCalc.Expression(functionText);
                    expression.Parameters["x"] = x;
                    return Convert.ToDouble(expression.Evaluate());
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing function: {ex.Message}");
                return null;
            }
        }

        private void PlotFunction(Func<double, double> func, double xMin, double xMax, double step)
        {
            Series series = new Series
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.Blue
            };

            Series asymptoteSeries = new Series
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 2,
                Color = Color.Red,
                BorderDashStyle = ChartDashStyle.DashDot
            };

            double yMinLimit = -1e6; 
            double yMaxLimit = 1e6;  

            double? previousY = null;
            bool isAsymptote = false;
            
            for (double x = xMin; x <= xMax; x += step)
            {
                double y = func(x);
                if (double.IsNaN(y) || double.IsInfinity(y))
                {
                    previousY = null;
                    isAsymptote = false;
                    continue;
                }
                if (y < yMinLimit) y = yMinLimit;
                if (y > yMaxLimit) y = yMaxLimit;

                if (previousY.HasValue && Math.Abs(y - previousY.Value) > yMaxLimit / 2)
                {
                    asymptoteSeries.Points.AddXY(x, y);
                    isAsymptote = true;
                }
                else
                {
                    if (!isAsymptote)
                    {
                        series.Points.AddXY(x, y);
                    }
                    else
                    {
                        asymptoteSeries.Points.AddXY(x, y);
                        isAsymptote = false;
                    }
                }

                previousY = y;
            }

            chart.Series.Clear();
            chart.Series.Add(series);
            chart.Series.Add(asymptoteSeries);
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            chart.ChartAreas[0].AxisX.ScaleView.Zoom(chart.ChartAreas[0].AxisX.ScaleView.Position, chart.ChartAreas[0].AxisX.ScaleView.Size / 2);
            chart.ChartAreas[0].AxisY.ScaleView.Zoom(chart.ChartAreas[0].AxisY.ScaleView.Position, chart.ChartAreas[0].AxisY.ScaleView.Size / 2);
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            chart.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chart.ChartAreas[0].AxisY.ScaleView.ZoomReset();
        }

        public GraphDisplay(string functionText)
        {
            InitializeComponent();
            InitializeChart();
            InitializeControls();
            this.Load += new EventHandler(Form1_Load);
            functionInput.Text = functionText;
        }
    }
}
