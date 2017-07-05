using System;
using System.IO;
using System.Xml.Serialization;

namespace Business
{
	#region UserConfiguration Class
	/// <summary>
	/// Class for storing and retrieving user-defined alerts.
	/// </summary>
	[XmlRoot]
	public class UserConfiguration
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of UserConfiguration.
		/// </summary>
		public UserConfiguration()
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// The number of AFLT hits per day and week.
		/// </summary>
		public Threshold AfltHits;
		/// <summary>
		/// The agencies of interest.
		/// </summary>
		public string Agencies;
		/// <summary>
		/// The data usage (in bytes) per day and week.
		/// </summary>
		public Threshold DataUsage;
		/// <summary>
		/// The emails of users to be notified when thresholds have been crossed.
		/// </summary>
		public string Emails;
		/// <summary>
		/// The postal codes of interest.
		/// </summary>
		public string PostalCodes;
		/// <summary>
		/// The device IDs of interest.
		/// </summary>
		public string SerialIds;
		#endregion

		#region Methods
		/// <summary>
		/// Gets an alert configuration from a file.
		/// </summary>
		/// <param name="fileName">The name of the file to load.</param>
		/// <returns>The alert data.</returns>
		public UserConfiguration Deserialize(string fileName)
		{
			UserConfiguration result = null;

			fileName = (fileName + "").Trim();

			XmlSerializer xs = new XmlSerializer(typeof(UserConfiguration));

			try
			{
				using (TextReader tr = File.OpenText(fileName))
				{
					result = (UserConfiguration)xs.Deserialize(tr);
				}
			}
			finally
			{
				xs = null;
			}

			return result;
		}

		/// <summary>
		/// Saves an alert configuration to a file.
		/// </summary>
		/// <param name="fileName">The name of the file to save to.</param>
		/// <returns>True if successful; otherwise false.</returns>
		public bool Serialize(string fileName)
		{
			bool result = false;

			fileName = (fileName + "").Trim();

			XmlSerializer xs = new XmlSerializer(typeof(UserConfiguration));

			if (!this.AfltHits.Daily.HasValue)
				this.AfltHits.Daily = -1;

			if (!this.AfltHits.Weekly.HasValue)
				this.AfltHits.Weekly = -1;

			if (!this.DataUsage.Daily.HasValue)
				this.DataUsage.Daily = -1;

			if (!this.DataUsage.Weekly.HasValue)
				this.DataUsage.Weekly = -1;

			try
			{
				using (TextWriter tw = File.CreateText(fileName))
				{
					xs.Serialize(tw, this);
					tw.Close();
				}

				result = true;
			}
			finally
			{
				xs = null;
			}

			return result;
		}
		#endregion
	}
	#endregion

	#region Threshold Child Class
	/// <summary>
	/// Class to define daily and weekly thresholds for user notification.
	/// </summary>
	public class Threshold
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of Threshold.
		/// </summary>
		public Threshold()
		{
		}
		#endregion

		#region Properties
		/// <summary>
		/// The number of items per day.
		/// </summary>
		public int? Daily;

		/// <summary>
		/// The number of items per week.
		/// </summary>
		public int? Weekly;
		#endregion
	}
	#endregion
}