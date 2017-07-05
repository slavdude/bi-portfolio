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
	public class ClientTests
	{
		#region Fields
		private Client client = null;
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

			this.client = Client.NewClient(this.connectionStringStub + "Initial Catalog=" + this.databaseName, 600);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			this.client.Dispose();

			File.Delete(this.path + "archivetestdata.txt.err.log");
			File.Delete(this.path + "archivetestdata.txt.err.log.Error.Txt");

			DatabaseHelper.DropDatabase(this.databaseName);
		}
		#endregion

		#region Tests
		[Test]
		public void GetClientsByDevice()
		{
			DataTable result = this.client.GetClientsByDevice(new int[] { 1100302, 1100325 });

			Assert.AreEqual(2, result.Rows.Count);

			Assert.AreEqual(45397, result.Rows[0]["client_id"]);
			Assert.AreEqual("Beta - Battery Test", result.Rows[0]["first_name"]);
			Assert.IsNullOrEmpty(result.Rows[0]["middle_initial"].ToString());
			Assert.AreEqual("1100302", result.Rows[0]["last_name"]);

			Assert.AreEqual(45421, result.Rows[1]["client_id"]);
			Assert.AreEqual("Beta - At Sendum", result.Rows[1]["first_name"]);
			Assert.IsNullOrEmpty(result.Rows[1]["middle_initial"].ToString());
			Assert.AreEqual("1100325", result.Rows[1]["last_name"]);
		}
		#endregion
	}
}