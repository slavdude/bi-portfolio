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
	public class AgencyTests
	{
		#region Fields
		private Agency agency = null;
		private string connectionStringStub = "Data Source=" + Dns.GetHostName().ToUpper() + @"\sstwokeight;Integrated Security=true;";
		private string databaseName = String.Empty;
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

			this.agency = Agency.NewAgency(this.connectionStringStub + "Initial Catalog=" + this.databaseName, 600);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			this.agency.Dispose();

			File.Delete(this.path + "archivetestdata.txt.err.log");
			File.Delete(this.path + "archivetestdata.txt.err.log.Error.Txt");

			DatabaseHelper.DropDatabase(this.databaseName);
		}
		#endregion

		#region Tests
		[Test]
		public void GetAgencies()
		{
			DataTable result = this.agency.GetAgencies();

			Assert.AreEqual(1, result.Rows.Count);

			Assert.AreEqual(179, result.Rows[0]["agency_id"]);
			Assert.AreEqual("zy_Testing Agency", result.Rows[0]["agency_name"]);
		}

		[Test]
		public void GetPostalCodesAgencyIds()
		{
			DataTable result = this.agency.GetPostalCodesAgencyIds(new int[] { 179, 180 });

			Assert.AreEqual(1, result.Rows.Count);

			Assert.AreEqual("80301", result.Rows[0]["postal_code"]);
			Assert.AreEqual(179, result.Rows[0]["agency_id"]);
		}
		#endregion
	}
}