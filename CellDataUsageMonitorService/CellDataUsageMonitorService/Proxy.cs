using System;

namespace CellDataUsageMonitor.Service
{
	/// <summary>
	/// http://it.toolbox.com/blogs/paytonbyrd/implementing-a-remoting-server-1872
	/// </summary>
	public abstract class Proxy : MarshalByRefObject
	{
		#region Properties
		public abstract int CacheSize { get; }
		public abstract object this[object key] { get; set; }
		#endregion

		#region Methods
		public abstract void AddToCache(object key, object value);
		public abstract void Dispose();
		public abstract bool KeyExists(object key);
		public abstract bool MonitorRunning();
		public abstract object RemoveFromCache(object key);
		public abstract void StartMonitor(int[] serialIds, string[] agencies, string[] postalCodes, string[] emails, int dataUsageDay, int dataUsageWeek, int hitsDay, int hitsWeek);
		public abstract void StopMonitor();
		#endregion
	}
}