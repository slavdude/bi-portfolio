using Dal.Archive;
using Dal.Core;

namespace Dal
{
	public interface IDataFactory
	{
		Agency GetNewAgency();
		Client GetNewClient();
		EventPacket GetNewEventPacket();
		GpsData GetNewGpsData();
	}
}