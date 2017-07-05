using Dal.Archive;
using Dal.Core;

namespace Business
{
	public class DataFactory : Dal.IDataFactory
	{
		#region Methods
		public virtual Agency GetNewAgency()
		{
			return Agency.NewAgency(Configuration.ArchiveConnectionString, Configuration.ConnectionTimeout);
		}

		public virtual Client GetNewClient()
		{
			return Client.NewClient(Configuration.ArchiveConnectionString, Configuration.ConnectionTimeout);
		}

		public virtual EventPacket GetNewEventPacket()
		{
			return EventPacket.NewEventPacket(Configuration.CoreConnectionString, Configuration.ConnectionTimeout);
		}

		public virtual GpsData GetNewGpsData()
		{
			return GpsData.NewGpsData(Configuration.ArchiveConnectionString, Configuration.ConnectionTimeout);
		}
		#endregion
	}
}