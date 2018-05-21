using NEAT.Utils;
using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NEAT
{
    public partial class ChartForm : Form
    {
        private System.Timers.Timer _refresh;

        public ChartForm()
        {
            InitializeComponent();
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {
            if (_refresh == null)
            {
                _refresh = new System.Timers.Timer();
                _refresh.Interval = 1000;
                _refresh.Elapsed += RefreshChart;
                _refresh.AutoReset = true;
            }
            _refresh.Enabled = false;

            RefreshChart(null, null);         
        }

        private void RefreshChart(object sender, ElapsedEventArgs e)
        {
            chFeedback.Series.Clear();

            Series series = new Series
            {
                Name = "FitnessPerGeneration",
                Color = Color.Red,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line,
            };

            chFeedback.Series.Add(series);

            for(int i = 0; i < Feedback.fitnessPerGeneration.Count; i++)
                series.Points.AddXY(i, Feedback.fitnessPerGeneration[i]);

            chFeedback.Titles.Add("Fitness Per Generation");
        }
    }
}
