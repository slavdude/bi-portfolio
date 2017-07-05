namespace CellDataUsageMonitor.Service
{
	public class ThresholdInfo
	{
		#region Fields
		private int count = 0;
		private int serialId = 0;
		#endregion

		#region Properties
		public int Count
		{
			get { return this.count; }
			set { this.count = value; }
		}

		public int SerialId
		{
			get { return this.serialId; }
			set { this.serialId = value; }
		}
		#endregion
	}
}