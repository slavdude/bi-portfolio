using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CellDataUsageMonitor.Service.Data
{
	public class ArchiveData : IDisposable, IData
	{
		#region Fields
		private ThresholdInfo[] data = null;
		private DataAccess dataAccess = null;
		private bool disposed = false;
		#endregion

		#region Constructors
		public ArchiveData(string connectionString, int timeout)
		{
			this.dataAccess = new DataAccess((connectionString + "").Trim(), timeout);
		}
		#endregion

		#region Finalizers
		~ArchiveData()
		{
			Dispose(false);
		}
		#endregion

		#region IDisposable Implementation
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!this.disposed)
				{
					if (this.dataAccess != null)
						this.dataAccess.Dispose();

					this.dataAccess = null;
					this.disposed = true;
				}
			}
		}
		#endregion

		#region Properties
		public ThresholdInfo[] Data
		{
			get { return this.data; }
			set { this.data = value; }
		}
		#endregion


		public void ExceedThreshold(object info)
		{
			StartParameterInfo start = (StartParameterInfo)info;
			throw new NotImplementedException();
		}
	}
}
