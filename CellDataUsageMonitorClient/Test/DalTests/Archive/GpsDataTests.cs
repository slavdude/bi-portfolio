using Dal.Archive;
using NUnit.Framework;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Test.DalTests.Archive
{
	[TestFixture]
	public class GpsDataTests
	{
		#region Fields
		private string connectionStringStub = "Data Source=" + Dns.GetHostName().ToUpper() + @"\sstwokeight;Integrated Security=true;";
		private string databaseName = String.Empty;
		private GpsData gpsData = null;
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

			DatabaseHelper.CreateDatabase(this.databaseName, "Archive");

			this.path = Business.Configuration.ConfigurationFilePath + "DataFiles\\";

			string script = DatabaseHelper.LoadSqlFile(this.path + "archivetestdata.sql");

			DatabaseHelper.ExecuteSqlScript(script, this.databaseName);

			this.gpsData = GpsData.NewGpsData(this.connectionStringStub + "Initial Catalog=" + this.databaseName, 600);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			this.gpsData.Dispose();

			File.Delete(this.path + "archivetestdata.txt.err.log");
			File.Delete(this.path + "archivetestdata.txt.err.log.Error.Txt");

			DatabaseHelper.DropDatabase(this.databaseName);
		}
		#endregion

		#region Tests
		[Test]
		public void GetHits()
		{
			DataTable result = this.gpsData.GetHits(new int[] { 1100302 }, DateTime.Parse("2008-12-19"), DateTime.Parse("2008-12-30"));

			Assert.AreEqual(3, result.Rows.Count);

			Assert.AreEqual(1100302, result.Rows[0]["lSerial_ID"]);
			Assert.AreEqual("12/19/2008", Convert.ToDateTime(result.Rows[0]["Received"]).ToShortDateString());
			Assert.AreEqual(1, result.Rows[0]["Hits"]);

			Assert.AreEqual(1100302, result.Rows[1]["lSerial_ID"]);
			Assert.AreEqual("12/19/2008", Convert.ToDateTime(result.Rows[1]["Received"]).ToShortDateString());
			Assert.AreEqual(3, result.Rows[1]["Hits"]);

			Assert.AreEqual(1100302, result.Rows[2]["lSerial_ID"]);
			Assert.AreEqual("12/19/2008", Convert.ToDateTime(result.Rows[2]["Received"]).ToShortDateString());
			Assert.AreEqual(1, result.Rows[2]["Hits"]);
		}

		[Test]
		public void GetHitsByDateRange()
		{
			DataTable result = this.gpsData.GetHitsByDateRange(DateTime.Parse("2008-12-19"), DateTime.Parse("2008-12-30"));

			Assert.AreEqual(3, result.Rows.Count);

			Assert.AreEqual(1100302, result.Rows[0]["lSerial_ID"]);
			Assert.AreEqual("12/19/2008", Convert.ToDateTime(result.Rows[0]["Received"]).ToShortDateString());
			Assert.AreEqual(1, result.Rows[0]["Hits"]);

			Assert.AreEqual(1100302, result.Rows[1]["lSerial_ID"]);
			Assert.AreEqual("12/19/2008", Convert.ToDateTime(result.Rows[1]["Received"]).ToShortDateString());
			Assert.AreEqual(3, result.Rows[1]["Hits"]);

			Assert.AreEqual(1100302, result.Rows[2]["lSerial_ID"]);
			Assert.AreEqual("12/19/2008", Convert.ToDateTime(result.Rows[2]["Received"]).ToShortDateString());
			Assert.AreEqual(1, result.Rows[2]["Hits"]);
		}
		#endregion
	}
}