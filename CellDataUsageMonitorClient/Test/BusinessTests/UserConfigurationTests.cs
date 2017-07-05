using Business;
using NUnit.Framework;
using System.IO;

namespace Test.BusinessTests
{
	[TestFixture]
	public class UserConfigurationTests
	{
		#region Fields
		private string path = Configuration.ConfigurationFilePath + "\\DataFiles\\";
		#endregion

		#region Setup/Teardown
		[TestFixtureSetUp]
		public void SetUp()
		{
			if (!Directory.Exists(this.path))
				Directory.CreateDirectory(this.path);
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			if (File.Exists(this.path + "SerializeTest.xml"))
				File.Delete(this.path + "SerializeTest.xml");
		}
		#endregion

		#region Tests
		[Test]
		public void Deserialize()
		{
			string file = this.path + "deserialize.xml";

			UserConfiguration result = new UserConfiguration();

			result = result.Deserialize(file);

			Assert.AreEqual(33, result.AfltHits.Daily);
			Assert.AreEqual(456, result.AfltHits.Weekly);
			Assert.AreEqual("AgencyABC", result.Agencies);
			Assert.AreEqual(3455, result.DataUsage.Daily);
			Assert.AreEqual(10234, result.DataUsage.Weekly);
			Assert.AreEqual("me@email.com", result.Emails);
			Assert.AreEqual("02134", result.PostalCodes);
			Assert.AreEqual("12", result.SerialIds);
		}

		[Test]
		public void DeserializeNoThreshold()
		{
			string file = this.path + "deserializenothreshold.xml";

			UserConfiguration result = new UserConfiguration();

			result = result.Deserialize(file);

			Assert.AreEqual(-1, result.AfltHits.Daily);
			Assert.AreEqual(-1, result.AfltHits.Weekly);
			Assert.AreEqual("AgencyABC", result.Agencies);
			Assert.AreEqual(-1, result.DataUsage.Daily);
			Assert.AreEqual(-1, result.DataUsage.Weekly);
			Assert.AreEqual("me@email.com", result.Emails);
			Assert.AreEqual("02134", result.PostalCodes);
			Assert.AreEqual("12", result.SerialIds);
		}

		[Test]
		public void Serialize()
		{
			string file = this.path + "SerializeTest.xml";

			UserConfiguration uc = new UserConfiguration()
			{
				AfltHits = new Threshold() { Daily = 12, Weekly = 24 },
				Agencies = "Agency1,Agency2",
				DataUsage = new Threshold() { Daily = 100, Weekly = 700 },
				Emails = "test1@bi.com;test2@bi.com",
				PostalCodes = "80301,80303",
				SerialIds = "1,2,4,6-8"
			};

			bool result = uc.Serialize(file);

			Assert.IsTrue(result);

			Assert.IsTrue(File.Exists(file));

			string text = "";

			using (TextReader tr = File.OpenText(file))
			{
				text = tr.ReadToEnd();
				tr.Close();
			}

			Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<UserConfiguration xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=" +
				"\"http://www.w3.org/2001/XMLSchema\">\r\n  <AfltHits>\r\n    <Daily>12</Daily>\r\n    <Weekly>24</Weekly>\r\n  </AfltHits>\r\n  <Agencies>Agency1,Agency2</Agencies>\r\n  " +
				"<DataUsage>\r\n    <Daily>100</Daily>\r\n    <Weekly>700</Weekly>\r\n  </DataUsage>\r\n  <Emails>test1@bi.com;test2@bi.com</Emails>\r\n  <PostalCodes>80301,80303</PostalCodes>" +
				"\r\n  <SerialIds>1,2,4,6-8</SerialIds>\r\n</UserConfiguration>", text);
		}
		#endregion
	}
}