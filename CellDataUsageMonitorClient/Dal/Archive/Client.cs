using System;
using System.Data;

namespace Dal.Archive
{
	/// <summary>
	/// Data-access class for Client table.
	/// </summary>
	public class Client : IDisposable
	{
		#region Fields
		private DataAccess dataAccess = null;
		private bool disposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of Client.
		/// </summary>
		public Client()
		{
		}

		/// <summary>
		/// Creates a new instance of Client.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The database command timeout.</param>
		private Client(string connectionString, int timeout)
		{
			this.dataAccess = new DataAccess((connectionString + "").Trim(), timeout);
		}
		#endregion

		#region Finalizers
		~Client()
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
		/// Gets the clients associated with the provided devices.
		/// </summary>
		/// <param name="serialIds">The device IDs.</param>
		/// <returns>The clients to whom the devices are assigned.</returns>
		public virtual DataTable GetClientsByDevice(int[] serialIds)
		{
			DataTable result = null;

			string sql = "SELECT DISTINCT c.client_id, ISNULL(c.first_name, '<blank>') AS first_name, ISNULL(LTRIM(RTRIM(c.middle_initial)), '') AS middle_initial, c.last_name " +
				"FROM client c WITH (NoLock) INNER JOIN tbldevice d WITH (NoLock) ON d.client_id = c.client_id WHERE " + 
				DataAccess.BuildSqlInStatement(serialIds, "d.seid") +
				"ORDER BY c.client_id";

			result = this.dataAccess.GetDataTable(sql, CommandType.Text);

			return result;
		}

		/// <summary>
		/// Creates a new instance of Client.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The database command timeout.</param>
		/// <returns></returns>
		public static Client NewClient(string connectionString, int timeout)
		{
			return new Client((connectionString + "").Trim(), timeout);
		}
		#endregion
	}
}