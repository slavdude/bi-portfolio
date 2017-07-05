using System;
using System.Data;

namespace Dal.Core
{
	/// <summary>
	/// Data-access class for the EventPacket table in the IPLP database.
	/// </summary>
	public class EventPacket : IDisposable
	{
		#region Fields
		private DataAccess dataAccess = null;
		private bool disposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of EventPacket.
		/// </summary>
		public EventPacket()
		{
		}

		/// <summary>
		/// Creates a new instance of EventPacket.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The command timeout.</param>
		private EventPacket(string connectionString, int timeout)
		{
			this.dataAccess = new DataAccess((connectionString + "").Trim(), timeout);
		}
		#endregion

		#region Finalizers
		~EventPacket()
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
		/// Gets event packet data for the provided serial numbers in the provided date range.
		/// </summary>
		/// <param name="serialIds">The serial numbers.</param>
		/// <param name="start">The start date.</param>
		/// <param name="end">The end date.</param>
		/// <returns>The event packet ID, device ID, received date, and packet size of each packet.</returns>
		public virtual DataTable GetEventPackets(int[] serialIds, DateTime start, DateTime end)
		{
			DataTable result = null;

			string sql = "SELECT DISTINCT [EventPacketId], [SerialId], [ReceivedDate], [PacketSize] FROM [dbo].[EventPacket] WHERE " + DataAccess.BuildSqlInStatement(serialIds, 
				"[SerialId]") + " AND [ReceivedDate] BETWEEN '" + start.ToString() + "' AND '" + end.ToString() + "' ORDER BY [SerialId], [ReceivedDate]";

			result = this.dataAccess.GetDataTable(sql, CommandType.Text);

			return result;
		}

		/// <summary>
		/// Gets packet data information for the specified date range.
		/// </summary>
		/// <param name="start">The start date.</param>
		/// <param name="end">The end date.</param>
		/// <returns>The packets.</returns>
		public virtual DataTable GetPacketsByDateRange(DateTime start, DateTime end)
		{
			DataTable result = null;

			string sql = "SELECT DISTINCT [EventPacketId], [SerialId], [ReceivedDate], [PacketSize] FROM [dbo].[EventPacket] WHERE [ReceivedDate] BETWEEN '" + start.ToString() + "' " +
					"AND '" + end.ToString() + "' ORDER BY [SerialId], [ReceivedDate]";
			
			result = this.dataAccess.GetDataTable(sql, CommandType.Text);

			return result;
		}

		/// <summary>
		/// Creates a new instance of EventPacket.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The command timeout.</param>
		/// <returns></returns>
		public static EventPacket NewEventPacket(string connectionString, int timeout)
		{
			return new EventPacket((connectionString + "").Trim(), timeout);
		}
		#endregion
	}
}