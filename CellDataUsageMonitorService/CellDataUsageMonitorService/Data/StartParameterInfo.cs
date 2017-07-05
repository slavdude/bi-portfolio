namespace CellDataUsageMonitor.Service.Data
{
	public class StartParameterInfo
	{
		#region Fields
		private int count = 0;
		private int periodType = -1;
		private int[] serialIds = null;
		#endregion

		#region Properties
		public int Count
		{
			get { return this.count; }
			set { this.count = value; }
		}

		public int PeriodType
		{
			get { return this.periodType; }
			set { this.periodType = value; }
		}

		public int[] SerialIds
		{
			get { return this.serialIds; }
			set { this.serialIds = value; }
		}
		#endregion

	}
}