namespace CellDataUsageMonitor.Service.Data
{
	public interface IData
	{
		ThresholdInfo[] Data { get; set; }
		void ExceedThreshold(object info);
	}
}