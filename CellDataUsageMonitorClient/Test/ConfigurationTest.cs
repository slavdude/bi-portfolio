using Business;
using NUnit.Framework;

namespace Test
{
	[TestFixture]
	public class ConfigurationTest
	{
		#region Tests
		[SetUp]
		public void SetUp()
		{
			Configuration.Load();
		}

		[Test]
		public void Load()
		{
			Assert.AreEqual("BI.CellDataUsageMonitor.Client.Test.dll.config", Configuration.ConfigurationFileName, "ConfigurationFileName value is incorrect.");
			Assert.AreEqual("connstring", Configuration.CoreConnectionString, "Core connection string is incorrect.");
			Assert.AreEqual("connstring2", Configuration.ArchiveConnectionString, "Archive connection string is incorrect.");
			Assert.AreEqual(@"test\trace\", Configuration.StoragePath, "StoragePath value is incorrect.");
			Assert.AreEqual(false, Configuration.IsTracing, "IsTracing value is incorrect.");
			Assert.AreEqual(600, Configuration.ConnectionTimeout, "ConnectionTimeout value is incorrect.");
		}
		#endregion
	}
}