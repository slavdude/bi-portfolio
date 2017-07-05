namespace Business.Utility
{
	/// <summary>
	/// Enumerators used by the Data Usage Monitor Client.
	/// </summary>
	public class Enumerators
	{
		/// <summary>
		/// Periods for statistical measurements.
		/// </summary>
		public enum Period : int
		{
			Day = 0,
			Week = 1,
			Month = 2
		}

		/// <summary>
		/// The types of statistical information requested.
		/// </summary>
		public enum StatisticsType : int
		{
			AfltHit = 0,
			DataUsage = 1
		}
	}
}