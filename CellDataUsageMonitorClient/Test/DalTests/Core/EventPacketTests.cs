using Dal.Core;
using NUnit.Framework;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Test.DalTests.Core
{
	[TestFixture]
	public class EventPacketTests
	{
		#region Fields
		private string connectionStringStub = "Data Source=" + Dns.GetHostName().ToUpper() + @"\sstwokeight;Integrated Security=true;";
		private string databaseName = String.Empty;
		private EventPacket packet = null;
		private string path = String.Empty;
		#endregion

		#region Setup/Teardown
		[TestFixtureSetUp]
		public void SetUp()
		{
			Trace.WriteLine("Loading.");

			Business.Configuration.Load();

			Trace.WriteLine("Setting up database.");

			this.databaseName = Guid.NewGuid().ToString();

			DatabaseHelper.CreateDatabase(this.databaseName, "Core");

			this.path = Business.Configuration.ConfigurationFilePath + "DataFiles\\";

			string script = DatabaseHelper.LoadSqlFile(this.path + "coretestdata.sql");

			DatabaseHelper.ExecuteSqlScript(script, this.databaseName);

			this.packet = EventPacket.NewEventPacket(this.connectionStringStub + "Initial Catalog=" + this.databaseName, 600);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			this.packet.Dispose();

			File.Delete(this.path + "coretestdata.txt.err.log");
			File.Delete(this.path + "coretestdata.txt.err.log.Error.Txt");

			DatabaseHelper.DropDatabase(this.databaseName);
		}
		#endregion

		#region Tests
		[Test]
		public void GetEventPackets()
		{
			DataTable result = this.packet.GetEventPackets(new int[] { 100005, 1100366, 1000025 }, DateTime.Parse("2009-05-12 4:40:00 PM"), DateTime.Parse("2009-05-12 4:54:00 PM"));

			Assert.AreEqual(3, result.Rows.Count);

			Assert.AreEqual(25, result.Rows[0]["EventPacketId"]);
			Assert.AreEqual(1000025, result.Rows[0]["SerialId"]);
			Assert.AreEqual("5/12/2009 4:41:54 PM", result.Rows[0]["ReceivedDate"].ToString());
			Assert.AreEqual(15, result.Rows[0]["PacketSize"]);

			Assert.AreEqual(26, result.Rows[1]["EventPacketId"]);
			Assert.AreEqual(1100366, result.Rows[1]["SerialId"]);
			Assert.AreEqual("5/12/2009 4:52:54 PM", result.Rows[1]["ReceivedDate"].ToString());
			Assert.AreEqual(58, result.Rows[1]["PacketSize"]);

			Assert.AreEqual(27, result.Rows[2]["EventPacketId"]);
			Assert.AreEqual(1100366, result.Rows[2]["SerialId"]);
			Assert.AreEqual("5/12/2009 4:53:28 PM", result.Rows[2]["ReceivedDate"].ToString());
			Assert.AreEqual(82, result.Rows[2]["PacketSize"]);
		}

		[Test]
		public void GetPacketsByDateRange()
		{
			DataTable result = this.packet.GetPacketsByDateRange(DateTime.Parse("2009-05-12 12:00:00 AM"), DateTime.Parse("2009-05-12 11:59:59 PM"));

			Assert.AreEqual(49, result.Rows.Count);
		}
		#endregion
	}
}