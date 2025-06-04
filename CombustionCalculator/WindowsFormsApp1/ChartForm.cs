using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class ChartForm : Form
    {
        public ChartForm(double Qn, double t0, double t0b, double ta, double tab, double VL, double iT0, double iT0b, double iTa, double iTab)
        {
            InitializeComponent();
            PlotFullChart(chart1, Qn);
            
            // Добавляем точки с рассчитанными температурами
            AddCalculatedPoints(t0, t0b, ta, tab, VL, iT0, iT0b, iTa, iTab);
            
            chart1.ChartAreas[0].AxisX.Title = "Температура, °C";
            chart1.ChartAreas[0].AxisY.Title = "Энтальпия, кДж/м³";
            chart1.Legends[0].Docking = Docking.Right;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
        }

        private void AddCalculatedPoints(double t0, double t0b, double ta, double tab, double VL, double iT0, double iT0b, double iTa, double iTab)
        {
            // Создаем серию для точек при α=1
            var seriesAlpha1 = new Series("Расчетные точки (α=1)")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 10,
                MarkerColor = Color.Red
            };

            // Создаем серию для точек при расчетном α
            var seriesAlphaN = new Series($"Расчетные точки (VL={VL:F1}%)")
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Diamond,
                MarkerSize = 10,
                MarkerColor = Color.Blue
            };

            // Добавляем точки для α=1
            seriesAlpha1.Points.AddXY(t0, iT0);
            seriesAlpha1.Points[0].Label = $"t₀={t0:F1}°C";
            
            seriesAlpha1.Points.AddXY(t0b, iT0b);
            seriesAlpha1.Points[1].Label = $"t₀б={t0b:F1}°C";

            // Добавляем точки для расчетного α
            seriesAlphaN.Points.AddXY(ta, iTa);
            seriesAlphaN.Points[0].Label = $"tₐ={ta:F1}°C";
            
            seriesAlphaN.Points.AddXY(tab, iTab);
            seriesAlphaN.Points[1].Label = $"tₐб={tab:F1}°C";

            // Добавляем серии на график
            chart1.Series.Add(seriesAlpha1);
            chart1.Series.Add(seriesAlphaN);
        }

        private void PlotFullChart(Chart chart, double Qn)
        {
            chart.Series.Clear();

            var mainTable = TemperatureTable.GetMainTable(Qn);
            var bTable = TemperatureTable.GetBTable(Qn);

            PlotTable(chart, mainTable, isDashed: false, "Main");
            PlotTable(chart, bTable, isDashed: true, "B");
        }

        private void PlotTable(
            Chart chart,
            Dictionary<int, (double[] Temperatures, double[] Enthalpies)> table,
            bool isDashed,
            string labelSuffix
        )
        {
            foreach (var kv in table)
            {
                int vl = kv.Key;
                var (temps, enthals) = kv.Value;

                var series = new Series($"{labelSuffix}: VL={vl}%")
                {
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2
                };

                // Разные стили линий для лучшей различимости
                if (isDashed)
                {
                    switch (vl)
                    {
                        case 0:
                            series.BorderDashStyle = ChartDashStyle.Dash;
                            break;
                        case 20:
                            series.BorderDashStyle = ChartDashStyle.DashDot;
                            break;
                        case 40:
                            series.BorderDashStyle = ChartDashStyle.DashDotDot;
                            break;
                        case 60:
                            series.BorderDashStyle = ChartDashStyle.Dot;
                            series.BorderWidth = 3;
                            break;
                        case 80:
                            series.BorderDashStyle = ChartDashStyle.DashDot;
                            series.BorderWidth = 3;
                            break;
                        case 100:
                            series.BorderDashStyle = ChartDashStyle.Dash;
                            series.BorderWidth = 3;
                            break;
                    }
                }
                else
                {
                    series.BorderDashStyle = ChartDashStyle.Solid;
                }

                for (int i = 0; i < temps.Length; i++)
                    series.Points.AddXY(temps[i], enthals[i]);

                chart.Series.Add(series);
            }
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {

        }
    }
}
