using Business.Utility;
using System;
using System.Collections;
using System.Data;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;

namespace Monitor
{
	/// <summary>
	/// Interaction logic for Charts.xaml
	/// </summary>
	public partial class Charts : Window
	{
		#region Fields
		private bool average = false;
		private Chart chart = null;
		private DataTable data = null;
		private int dataUsageThreshold = -1;
		private bool highest = false;
		private int hitThreshold = -1;
		private bool lowest = false;
		private Enumerators.Period period = Enumerators.Period.Day;
		private bool standardDeviation = false;
		private bool threshold = false;		
		#endregion

		#region Constructors
		public Charts()
		{
			InitializeComponent();

			this.chart = new Chart();
		}
		#endregion

		#region Properties
		public bool Average
		{
			set { this.average = value; }
		}

		public DataTable Data
		{
			set 
			{ 
				this.data = value;
				this.Title = this.chart.Name = this.data.TableName;
			}
		}

		public int DataUsageThreshold
		{
			set { this.dataUsageThreshold = value; }
		}

		public bool Highest
		{
			set { this.highest = value; }
		}

		public int HitThreshold
		{
			set { this.hitThreshold = value; }
		}

		public bool Lowest
		{
			set { this.lowest = value; }
		}

		public Enumerators.Period Period
		{
			set { this.period = value; }
		}

		public bool StandardDeviation
		{
			set { this.standardDeviation = value; }
		}

		public bool Threshold
		{
			set { this.threshold = value; }
		}
		#endregion

		#region Methods
		public void CreateChart()
		{
			//ChartArea area = new ChartArea(this.data.TableName);

			//this.chart.Name = area.Name;

			//this.chart.ChartAreas.Add(area);

			////this.chart.DataBindTable(this.dataTable.AsEnumerable(), "SerialId");

			//Series series = new Series()
			//{
			//    ChartArea = area.Name,
			//    //ChartType = SeriesChartType.Bar,
			//    Name = area.Name
			//};

			//IEnumerable rows = this.data.AsEnumerable();

			////switch (this.graphType)
			////{
			////    case Enumerators.GraphType.Average:
			////        //series.ChartType = SeriesChartType.Bar;
			////        series.Points.DataBind(rows, "SerialId", "Average", "");
			////        break;

			////    case Enumerators.GraphType.Highest:
			////    case Enumerators.GraphType.Lowest:
			////        series.Points.DataBind(rows, "SerialId", "Average", "ReceivedDate");
			////        break;

			////    case Enumerators.GraphType.Threshold:
			////        //series.ChartType = SeriesChartType.Bar;
			////        series.Points.DataBind(rows, "SerialId", "Count", "");
			////        break;
			////}

			//this.chart.Series.Add(series);

			//Legends and titles here.

			this.chartHost.Child = this.chart;
		}

		private void CleanUpResources()
		{
			if (this.chartHost.Child != null)
				this.chartHost.Child.Dispose();

			this.chartHost.Child = null;
		}

		private void CloseClick(object sender, RoutedEventArgs e)
		{
			CleanUpResources();

			Close();
		}

		private void SaveClick(object sender, RoutedEventArgs e)
		{

		}
		#endregion
	}
}