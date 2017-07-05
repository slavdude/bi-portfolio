using System;
using System.Data;

namespace Dal.Archive
{
	/// <summary>
	/// Data-access class for GpsData.
	/// </summary>
	public class GpsData : IDisposable
	{
		#region Fields
		private DataAccess dataAccess = null;
		private bool disposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of GpsData.
		/// </summary>
		public GpsData()
		{
		}

		/// <summary>
		/// Creates a new instance of GpsData.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The database command timeout.</param>
		private GpsData(string connectionString, int timeout)
		{
			this.dataAccess = new DataAccess((connectionString + "").Trim(), timeout);
		}
		#endregion

		#region Finalizers
		~GpsData()
		{
			Dispose(false);
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Cleans up resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Cleans up resources.
		/// </summary>
		/// <param name="disposing">Flag indicating whether to dispose of internal resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!this.disposed)
				{
					if (this.dataAccess != null)
						this.dataAccess.Dispose();

					this.disposed = true;
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets the number of hits for specified devices during a specified period of time.
		/// </summary>
		/// <param name="ids">The device IDs.</param>
		/// <param name="start">The starting date.</param>
		/// <param name="end">The ending date.</param>
		/// <returns>The hits meeting the specified criteria.</returns>
		public virtual DataTable GetHits(int[] ids, DateTime start, DateTime end)
		{
			DataTable result = null;

			string sql = "SELECT lSerial_ID, CAST(receive_date_local AS date) AS Received, COUNT(tiFix_Type) AS Hits, receive_date_gmt FROM GPSData WITH (NoLock) " +
					"WHERE receive_date_local BETWEEN '" + start.ToString() + "' AND '" + end.ToString() + "' AND tiFix_Type = 23 " +
					"AND " + DataAccess.BuildSqlInStatement(ids, "lSerial_ID") +
					"GROUP BY lSerial_ID, receive_date_local, receive_date_gmt";

			result = this.dataAccess.GetDataTable(sql, CommandType.Text);

			return result;
		}

		/// <summary>
		/// Gets the number of hits per device during a specified period of time.
		/// </summary>
		/// <param name="start">The starting date.</param>
		/// <param name="end">The ending date.</param>
		/// <returns>The hits meeting the specified criteria.</returns>
		public virtual DataTable GetHitsByDateRange(DateTime start, DateTime end)
		{
			DataTable result = null;

			string sql = "SELECT lSerial_ID, CAST(receive_date_local AS date) AS Received, COUNT(tiFix_Type) AS Hits, receive_date_gmt FROM GPSData WITH (NoLock) " +
					"WHERE receive_date_local BETWEEN '" + start.ToString() + "' AND '" + end.ToString() + "' AND tiFix_Type = 23 " +
					"GROUP BY lSerial_ID, receive_date_local, receive_date_gmt";

			result = this.dataAccess.GetDataTable(sql, CommandType.Text);

			return result;
		}

		/// <summary>
		/// Creates a new instance of GpsData.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The database command timeout.</param>
		/// <returns></returns>
		public static GpsData NewGpsData(string connectionString, int timeout)
		{
			return new GpsData((connectionString + "").Trim(), timeout);
		}
		#endregion
	}
}