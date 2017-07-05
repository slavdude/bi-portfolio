using Dal.Archive;
using System;
using System.Collections.Generic;
using System.Data;

namespace Business
{
	/// <summary>
	/// Class for loading agency data into the UI.
	/// </summary>
	public class AgencyData : IDisposable
	{
		#region Fields
		private static Agency agency = null;
		private bool disposed = false;
		private int id = 0;
		private string name = String.Empty;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of AgencyData.
		/// </summary>
		public AgencyData()
		{
		}

		/// <summary>
		/// Creates a new instance of AgencyData.
		/// </summary>
		/// <param name="id">The database ID.</param>
		/// <param name="name">The name of the agency.</param>
		private AgencyData(int id, string name)
		{
			this.id = id;
			this.name = (name + "").Trim();
		}
		#endregion

		#region Finalizers
		~AgencyData()
		{
			Dispose(false);
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!this.disposed)
				{
					if (agency != null)
						agency.Dispose();

					this.disposed = true;
				}
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets the ID of the agency.
		/// </summary>
		public int Id
		{
			get { return this.id; }
		}

		/// <summary>
		/// Gets the name of the agency.
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets all of the available agencies in the database.
		/// </summary>
		/// <returns>AgencyData objects containing information about the agencies.</returns>
		public List<AgencyData> GetAgencies()
		{
			return GetAgencies((DataFactory)null);
		}

		/// <summary>
		/// Gets all of the available agencies in the database.
		/// </summary>
		/// <param name="dataFactory">The data to use for testing.</param>
		/// <returns>AgencyData objects containing information about the agencies.</returns>
		public List<AgencyData> GetAgencies(DataFactory dataFactory)
		{
			List<AgencyData> result = new List<AgencyData>();

			DataFactory df = new DataFactory();

			if (dataFactory != null)
				df = dataFactory;

			agency = df.GetNewAgency();

			using (DataTable table = agency.GetAgencies())
			{
				if (table.Rows.Count > 0)
				{
					foreach (DataRow row in table.Rows)
						result.Add(new AgencyData(Convert.ToInt32(row["agency_id"]), row["agency_name"].ToString()));
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the postal codes for the selected agencies.
		/// </summary>
		/// <param name="ids">The database IDs of the selected agencies.</param>
		/// <returns>The postal codes monitored by the selected agencies.</returns>
		public string[] GetPostalCodes(int[] ids)
		{
			return GetPostalCodes(ids, (DataFactory)null);
		}

		/// <summary>
		/// Gets the postal codes for the selected agencies.
		/// </summary>
		/// <param name="ids">The database IDs of the selected agencies.</param>
		/// <param name="dataFactory">The data to use for testing.</param>
		/// <returns>The postal codes monitored by the selected agencies.</returns>
		public string[] GetPostalCodes(int[] ids, DataFactory dataFactory)
		{
			List<string> result = new List<string>();

			DataFactory df = new DataFactory();

			if (dataFactory != null)
				df = dataFactory;

			agency = df.GetNewAgency();

			using (DataTable data = agency.GetPostalCodesAgencyIds(ids))
			{
				if (data != null)
				{
					if (data.Rows.Count > 0)
					{
						foreach (DataRow row in data.Rows)
							result.Add(row["postal_code"].ToString());
					}
				}
			}

			return result.ToArray();
		}
		#endregion
	}
}