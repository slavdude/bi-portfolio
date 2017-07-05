using System;
using System.Data;

namespace Dal.Archive
{
	/// <summary>
	/// Data-access class for Agency table.
	/// </summary>
	public class Agency : IDisposable
	{
		#region Fields
		private DataAccess dataAccess = null;
		private bool disposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of Agency.
		/// </summary>
		public Agency()
		{
		}

		/// <summary>
		/// Creates a new instance of Agency.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The database command timeout.</param>
		private Agency(string connectionString, int timeout)
		{
			this.dataAccess = new DataAccess((connectionString + "").Trim(), timeout);
		}
		#endregion

		#region Finalizers
		~Agency()
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
		/// Gets all the agencies in the database.
		/// </summary>
		/// <returns>The agency IDs and names.</returns>
		public virtual DataTable GetAgencies()
		{
			DataTable result = null;

			string sql = "SELECT DISTINCT agency_id, agency_name FROM agency WITH (NoLock) " +
				"ORDER BY agency_name";

			result = this.dataAccess.GetDataTable(sql, CommandType.Text);

			return result;
		}

		/// <summary>
		/// Gets the postal codes belonging to the supplied agencies.
		/// </summary>
		/// <param name="ids">The agency IDs.</param>
		/// <returns>The postal codes.</returns>
		public virtual DataTable GetPostalCodesAgencyIds(int[] ids)
		{
			DataTable result = null;

			string sql = "SELECT DISTINCT a.postal_code, ag.agency_id FROM address a WITH (NoLock) INNER JOIN client c WITH (NoLock) ON a.entity_id = " +
				"c.client_id INNER JOIN tbldevice d WITH (NoLock) ON d.client_id = c.client_id INNER JOIN agency ag WITH (NoLock) ON ag.agency_id = d.agency_id " +
				"WHERE a.postal_code IS NOT NULL AND RTRIM(ltrim(a.postal_code)) <> '' AND " + DataAccess.BuildSqlInStatement(ids, "ag.agency_id") +
				"ORDER BY a.postal_code";

			result = this.dataAccess.GetDataTable(sql, CommandType.Text);		

			return result;
		}

		/// <summary>
		/// Creates a new instance of Agency.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The database command timeout.</param>
		/// <returns></returns>
		public static Agency NewAgency(string connectionString, int timeout)
		{
			return new Agency((connectionString + "").Trim(), timeout);
		}
		#endregion
	}
}