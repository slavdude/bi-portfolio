using Business;
using Business.Utility;
using Dal.Archive;
using Dal.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace Test.BusinessTests
{
	[TestFixture]
	public class StatisticsTests
	{
		#region Fields
		private Statistics statistics = null;
		#endregion

		#region Setup/Teardown
		[TestFixtureSetUp]
		public void SetUp()
		{
			this.statistics = new Statistics();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			this.statistics.Dispose();
		}
		#endregion

		#region Tests
		//[Test]
		//public void CalculateStandardDeviation()
		//{
		//    List<double> list = new List<double>();

		//    list.Add(2);
		//    list.Add(4);
		//    list.Add(5);
		//    list.Add(6);
		//    list.Add(10);

		//    double result = Statistics.CalculateStandardDeviation(list);

		//    Assert.AreEqual(2.96648, result);

		//    list.Clear();

		//    list.Add(2);

		//    result = Statistics.CalculateStandardDeviation(list);

		//    Assert.IsTrue(double.IsNaN(result));
		//}

		[Test]
		public void ExpandRange()
		{
			Statistics stats = new Statistics();

			string[] result = stats.ExpandRange("1-10");

			Assert.AreEqual(10, result.Length);

			Assert.AreEqual("1", result[0]);
			Assert.AreEqual("2", result[1]);
			Assert.AreEqual("3", result[2]);
			Assert.AreEqual("4", result[3]);
			Assert.AreEqual("5", result[4]);
			Assert.AreEqual("6", result[5]);
			Assert.AreEqual("7", result[6]);
			Assert.AreEqual("8", result[7]);
			Assert.AreEqual("9", result[8]);
			Assert.AreEqual("10", result[9]);
		}

		//[Test]
		//public void GetAverageDataUsageDay()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.SerialIds = "1,2,3";

		//    DataTable result = this.statistics.GetAverage(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Day, new DataFactoryMock());

		//    Assert.IsNotNull(result);

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(136, result.Rows[0]["Average"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(544, result.Rows[1]["Average"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetAverageDataUsageMonth()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.SerialIds = "1,2,3";

		//    DataTable result = this.statistics.GetAverage(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Month, new DataFactoryMock());

		//    Assert.IsNotNull(result);

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(4.53333, result.Rows[0]["Average"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(18.13333, result.Rows[1]["Average"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetAverageDataUsageWeek()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.SerialIds = "1,2,3";

		//    DataTable result = this.statistics.GetAverage(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Week, new DataFactoryMock());

		//    Assert.IsNotNull(result);

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(19.42857, result.Rows[0]["Average"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(77.71429, result.Rows[1]["Average"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetAverageHitsDay()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.SerialIds = "1,2,3";

		//    DataTable result = this.statistics.GetAverage(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Day, new DataFactoryMock());

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(1.5, result.Rows[0]["Average"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(1, result.Rows[1]["Average"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetAverageHitsMonth()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.SerialIds = "1,2,3";

		//    DataTable result = this.statistics.GetAverage(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Day, new DataFactoryMock());

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(1.5, result.Rows[0]["Average"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(1, result.Rows[1]["Average"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetAverageHitsWeek()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.SerialIds = "1,2,3";

		//    DataTable result = this.statistics.GetAverage(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Day, new DataFactoryMock());

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(1.5, result.Rows[0]["Average"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(1, result.Rows[1]["Average"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetDevicesOverDataUsageThresholdDay()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.DataUsageThreshold = 28;

		//    DataTable result = this.statistics.GetDevicesOverThreshold(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Day, new DataFactoryMock());

		//    Assert.AreEqual(4, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual("1/1/2011 12:34:00 PM", result.Rows[0]["ReceivedDate"].ToString());
		//    Assert.AreEqual(28, result.Rows[0]["Count"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual("2/2/2011 3:23:00 AM", result.Rows[1]["ReceivedDate"].ToString());
		//    Assert.AreEqual(64, result.Rows[1]["Count"]);

		//    Assert.AreEqual(1, result.Rows[2]["SerialId"]);
		//    Assert.AreEqual("10/24/2011 8:56:00 PM", result.Rows[2]["ReceivedDate"].ToString());
		//    Assert.AreEqual(244, result.Rows[2]["Count"]);

		//    Assert.AreEqual(2, result.Rows[3]["SerialId"]);
		//    Assert.AreEqual("12/12/2011 9:02:00 PM", result.Rows[3]["ReceivedDate"].ToString());
		//    Assert.AreEqual(1024, result.Rows[3]["Count"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetDevicesOverDataUsageThresholdMonth()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.DataUsageThreshold = 1050;

		//    DataTable result = this.statistics.GetDevicesOverThreshold(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Month, new DataFactoryMock());

		//    Assert.AreEqual(1, result.Rows.Count);

		//    Assert.AreEqual(2, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(1088, result.Rows[0]["MonthlyCount"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetDevicesOverDataUsageThresholdWeek()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.DataUsageThreshold = 256;

		//    DataTable result = this.statistics.GetDevicesOverThreshold(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Week, new DataFactoryMock());

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(272, result.Rows[0]["WeeklyCount"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(1088, result.Rows[1]["WeeklyCount"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetDevicesOverHitThresholdDay()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.AfltHitThreshold = 2;

		//    DataTable result = this.statistics.GetDevicesOverThreshold(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Day, new DataFactoryMock());

		//    Assert.AreEqual(1, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual("10/24/2011 8:56:00 PM", result.Rows[0]["ReceivedDate"].ToString());
		//    Assert.AreEqual(2, result.Rows[0]["Count"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetDevicesOverHitThresholdMonth()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.AfltHitThreshold = 2;

		//    DataTable result = this.statistics.GetDevicesOverThreshold(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Month, new DataFactoryMock());

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(3, result.Rows[0]["MonthlyCount"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(2, result.Rows[1]["MonthlyCount"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetDevicesOverHitThresholdWeek()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");
		//    this.statistics.AfltHitThreshold = 2;

		//    DataTable result = this.statistics.GetDevicesOverThreshold(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Week, new DataFactoryMock());

		//    Assert.AreEqual(2, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual(3, result.Rows[0]["WeeklyCount"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual(2, result.Rows[1]["WeeklyCount"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetHighestDataUsage()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    DataTable result = this.statistics.GetHighest(Enumerators.StatisticsType.DataUsage, new DataFactoryMock());

		//    Assert.AreEqual(1, result.Rows.Count);

		//    Assert.AreEqual(2, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual("12/12/2011 9:02:00 PM", result.Rows[0]["ReceivedDate"].ToString());
		//    Assert.AreEqual(1024, result.Rows[0]["Count"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetHighestHits()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    DataTable result = this.statistics.GetHighest(Enumerators.StatisticsType.AfltHit, new DataFactoryMock());

		//    Assert.AreEqual(1, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual("10/24/2011 8:56:00 PM", result.Rows[0]["ReceivedDate"].ToString());
		//    Assert.AreEqual(2, result.Rows[0]["Count"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetLowestDataUsage()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    DataTable result = this.statistics.GetLowest(Enumerators.StatisticsType.DataUsage, new DataFactoryMock());

		//    Assert.AreEqual(1, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual("1/1/2011 12:34:00 PM", result.Rows[0]["ReceivedDate"].ToString());
		//    Assert.AreEqual(28, result.Rows[0]["Count"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetLowestHits()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    DataTable result = this.statistics.GetLowest(Enumerators.StatisticsType.AfltHit, new DataFactoryMock());

		//    Assert.AreEqual(3, result.Rows.Count);

		//    Assert.AreEqual(1, result.Rows[0]["SerialId"]);
		//    Assert.AreEqual("1/1/2011 12:34:00 PM", result.Rows[0]["ReceivedDate"].ToString());
		//    Assert.AreEqual(1, result.Rows[0]["Count"]);

		//    Assert.AreEqual(2, result.Rows[1]["SerialId"]);
		//    Assert.AreEqual("2/2/2011 3:23:00 AM", result.Rows[1]["ReceivedDate"].ToString());
		//    Assert.AreEqual(1, result.Rows[1]["Count"]);

		//    Assert.AreEqual(2, result.Rows[2]["SerialId"]);
		//    Assert.AreEqual("12/12/2011 9:02:00 PM", result.Rows[2]["ReceivedDate"].ToString());
		//    Assert.AreEqual(1, result.Rows[2]["Count"]);

		//    result.Dispose();
		//}

		//[Test]
		//public void GetStandardDeviationDataUsageDay()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    double result = this.statistics.GetStandardDeviation(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Day, new DataFactoryMock());

		//    Assert.AreEqual(465.68659, result);
		//}

		//[Test]
		//public void GetStandardDeviationDataUsageMonth()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    double result = this.statistics.GetStandardDeviation(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Month, new DataFactoryMock());

		//    Assert.AreEqual(15.52289, result);
		//}

		//[Test]
		//public void GetStandardDeviationDataUsageWeek()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    double result = this.statistics.GetStandardDeviation(Enumerators.StatisticsType.DataUsage, Enumerators.Period.Week, new DataFactoryMock());

		//    Assert.AreEqual(66.52666, result);
		//}

		//[Test]
		//public void GetStandardDeviationHitsDay()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    double result = this.statistics.GetStandardDeviation(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Day, new DataFactoryMock());

		//    Assert.AreEqual(0.5, result);
		//}

		//[Test]
		//public void GetStandardDeviationHitsMonth()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    double result = this.statistics.GetStandardDeviation(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Month, new DataFactoryMock());

		//    Assert.AreEqual(0.0167, Math.Round(result, 4));
		//}

		//[Test]
		//public void GetStandardDeviationHitsWeek()
		//{
		//    this.statistics.StartDate = DateTime.Parse("1/1/2011");
		//    this.statistics.EndDate = DateTime.Parse("12/15/2011");

		//    double result = this.statistics.GetStandardDeviation(Enumerators.StatisticsType.AfltHit, Enumerators.Period.Week, new DataFactoryMock());

		//    Assert.AreEqual(0.0714, Math.Round(result, 4));
		//}
		#endregion

		#region Child Classes
		public class DataFactoryMock : DataFactory
		{
			public override Client GetNewClient()
			{
				return new ClientMock();
			}

			public override EventPacket GetNewEventPacket()
			{
				return new EventPacketMock();
			}

			public override GpsData GetNewGpsData()
			{
				return new GpsDataMock();
			}
		}

		public class ClientMock : Client
		{
			public override DataTable GetClientsByDevice(int[] serialIds)
			{
				if (serialIds.Length == 0) return null;

				DataTable result = new DataTable("c");

				result.Columns.Add("client_id", typeof(int));
				result.Columns.Add("first_name", typeof(string));
				result.Columns.Add("middle_initial", typeof(string));
				result.Columns.Add("last_name", typeof(string));

				DataRow row = result.NewRow();
				row["client_id"] = 1;
				row["first_name"] = "Bartholomew";
				row["middle_initial"] = "J";
				row["last_name"] = "Simpson";
				result.Rows.Add(row);

				row = result.NewRow();
				row["client_id"] = 2;
				row["first_name"] = "Fred";
				row["middle_initial"] = "";
				row["last_name"] = "Flintstone";
				result.Rows.Add(row);

				row = result.NewRow();
				row["client_id"] = 3;
				row["first_name"] = "John";
				row["middle_initial"] = "";
				row["last_name"] = "Doe";
				result.Rows.Add(row);

				return result;
			}
		}

		public class EventPacketMock : EventPacket
		{
			public override DataTable GetEventPackets(int[] serialIds, DateTime start, DateTime end)
			{
				if (serialIds.Length == 0) return null;

				if (start < DateTime.Parse("1/1/2011 12:00:00 AM")) return null;

				if (end > DateTime.Parse("12/15/2011 11:59:59 PM")) return null;

				DataTable result = new DataTable("ep");

				result.Columns.Add("EventPacketId", typeof(int));
				result.Columns.Add("SerialId", typeof(int));
				result.Columns.Add("ReceivedDate", typeof(DateTime));
				result.Columns.Add("PacketSize", typeof(int));

				DataRow row = result.NewRow();
				row["EventPacketId"] = 1;
				row["SerialId"] = 1;
				row["ReceivedDate"] = DateTime.Parse("1/1/2011 12:34 PM");
				row["PacketSize"] = 28;
				result.Rows.Add(row);

				row = result.NewRow();
				row["EventPacketId"] = 2;
				row["SerialId"] = 2;
				row["ReceivedDate"] = DateTime.Parse("2/2/2011 3:23 AM");
				row["PacketSize"] = 64;
				result.Rows.Add(row);

				row = result.NewRow();
				row["EventPacketId"] = 3;
				row["SerialId"] = 1;
				row["ReceivedDate"] = DateTime.Parse("2/2/2011 6:58 AM");
				row["PacketSize"] = 16;

				row = result.NewRow();
				row["EventPacketId"] = 4;
				row["SerialId"] = 1;
				row["ReceivedDate"] = DateTime.Parse("10/24/2011 8:56 PM");
				row["PacketSize"] = 244;
				result.Rows.Add(row);

				row = result.NewRow();
				row["EventPacketId"] = 5;
				row["SerialId"] = 2;
				row["ReceivedDate"] = DateTime.Parse("12/12/2011 9:02 PM");
				row["PacketSize"] = 1024;
				result.Rows.Add(row);

				return result;
			}

			public override DataTable GetPacketsByDateRange(DateTime start, DateTime end)
			{
				if (start < DateTime.Parse("1/1/2011 12:00:00 AM")) return null;

				if (end > DateTime.Parse("12/15/2011 11:59:59 PM")) return null;

				DataTable result = new DataTable("ep");

				result.Columns.Add("EventPacketId", typeof(int));
				result.Columns.Add("SerialId", typeof(int));
				result.Columns.Add("ReceivedDate", typeof(DateTime));
				result.Columns.Add("PacketSize", typeof(int));

				DataRow row = result.NewRow();
				row["EventPacketId"] = 1;
				row["SerialId"] = 1;
				row["ReceivedDate"] = DateTime.Parse("1/1/2011 12:34 PM");
				row["PacketSize"] = 28;
				result.Rows.Add(row);

				row = result.NewRow();
				row["EventPacketId"] = 2;
				row["SerialId"] = 2;
				row["ReceivedDate"] = DateTime.Parse("2/2/2011 3:23 AM");
				row["PacketSize"] = 64;
				result.Rows.Add(row);

				row = result.NewRow();
				row["EventPacketId"] = 4;
				row["SerialId"] = 1;
				row["ReceivedDate"] = DateTime.Parse("10/24/2011 8:56 PM");
				row["PacketSize"] = 244;
				result.Rows.Add(row);

				row = result.NewRow();
				row["EventPacketId"] = 5;
				row["SerialId"] = 2;
				row["ReceivedDate"] = DateTime.Parse("12/12/2011 9:02 PM");
				row["PacketSize"] = 1024;
				result.Rows.Add(row);

				return result;
			}
		}

		public class GpsDataMock : GpsData
		{
			public override DataTable GetHits(int[] ids, DateTime start, DateTime end)
			{
				if (ids.Length == 0) return null;

				if (start < DateTime.Parse("1/1/2011 12:00:00 AM")) return null;

				if (end > DateTime.Parse("12/15/2011 11:59:59 PM")) return null;

				DataTable result = new DataTable("hits");

				result.Columns.Add("lSerial_ID", typeof(int));
				result.Columns.Add("Received", typeof(DateTime));
				result.Columns.Add("Hits", typeof(int));

				DataRow row = result.NewRow();

				row["lSerial_ID"] = 1;
				row["Received"] = DateTime.Parse("1/1/2011 12:34 PM");
				row["Hits"] = 1;
				result.Rows.Add(row);

				row = result.NewRow();
				row["lSerial_ID"] = 2;
				row["Received"] = DateTime.Parse("2/2/2011 3:23 AM");
				row["Hits"] = 1;
				result.Rows.Add(row);

				row = result.NewRow();
				row["lSerial_ID"] = 1;
				row["Received"] = DateTime.Parse("10/24/2011 8:56 PM");
				row["Hits"] = 2;
				result.Rows.Add(row);

				row = result.NewRow();
				row["lSerial_ID"] = 2;
				row["Received"] = DateTime.Parse("12/12/2011 9:02 PM");
				row["Hits"] = 1;
				result.Rows.Add(row);

				return result;
			}

			public override DataTable GetHitsByDateRange(DateTime start, DateTime end)
			{
				if (start < DateTime.Parse("1/1/2011 12:00:00 AM")) return null;

				if (end > DateTime.Parse("12/15/2011 11:59:59 PM")) return null;

				DataTable result = new DataTable("gps");

				result.Columns.Add("lSerial_ID", typeof(int));
				result.Columns.Add("Received", typeof(DateTime));
				result.Columns.Add("Hits", typeof(int));

				DataRow row = result.NewRow();

				row["lSerial_ID"] = 1;
				row["Received"] = DateTime.Parse("1/1/2011 12:34 PM");
				row["Hits"] = 1;
				result.Rows.Add(row);

				row = result.NewRow();
				row["lSerial_ID"] = 2;
				row["Received"] = DateTime.Parse("2/2/2011 3:23 AM");
				row["Hits"] = 1;
				result.Rows.Add(row);

				row = result.NewRow();
				row["lSerial_ID"] = 1;
				row["Received"] = DateTime.Parse("10/24/2011 8:56 PM");
				row["Hits"] = 2;
				result.Rows.Add(row);

				row = result.NewRow();
				row["lSerial_ID"] = 2;
				row["Received"] = DateTime.Parse("12/12/2011 9:02 PM");
				row["Hits"] = 1;
				result.Rows.Add(row);

				return result;
			}
		}
		#endregion
	}
}