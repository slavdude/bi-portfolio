using Business;
using Dal.Archive;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;

namespace Test.BusinessTests
{
	[TestFixture]
	public class AgencyDataTests
	{
		#region Fields
		private AgencyData agencyData = null;
		#endregion

		#region Setup/Teardown
		[TestFixtureSetUp]
		public void SetUp()
		{
			this.agencyData = new AgencyData();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			this.agencyData.Dispose();
		}
		#endregion

		#region Tests
		[Test]
		public void GetAgencies()
		{
			List<AgencyData> result = this.agencyData.GetAgencies(new DataFactoryMock());

			Assert.AreEqual(3, result.Count);

			Assert.AreEqual("Agency1", result[0].Name);
			Assert.AreEqual(1, result[0].Id);

			Assert.AreEqual("Agency2", result[1].Name);
			Assert.AreEqual(2, result[1].Id);

			Assert.AreEqual("Agency3", result[2].Name);
			Assert.AreEqual(3, result[2].Id);
		}

		[Test]
		public void GetPostalCodes()
		{
			string[] result = this.agencyData.GetPostalCodes(new int[] { 1, 2, 3 }, new DataFactoryMock());

			Assert.AreEqual(3, result.Length);

			Assert.AreEqual("12345", result[0]);
			Assert.AreEqual("12346", result[1]);
			Assert.AreEqual("12344", result[2]);
		}
		#endregion

		#region Child Classes
		public class DataFactoryMock : DataFactory
		{
			public override Agency GetNewAgency()
			{
				return new AgencyMock();
			}
		}

		public class AgencyMock : Agency
		{
			public override DataTable GetAgencies()
			{
				DataTable result = new DataTable("agcy");

				result.Columns.Add("agency_id", typeof(int));
				result.Columns.Add("agency_name", typeof(string));

				DataRow row = result.NewRow();
				row["agency_id"] = 1;
				row["agency_name"] = "Agency1";
				result.Rows.Add(row);

				row = result.NewRow();
				row["agency_id"] = 2;
				row["agency_name"] = "Agency2";
				result.Rows.Add(row);

				row = result.NewRow();
				row["agency_id"] = 3;
				row["agency_name"] = "Agency3";
				result.Rows.Add(row);

				return result;
			}

			public override DataTable GetPostalCodesAgencyIds(int[] ids)
			{
				if (ids.Length == 0) return null;

				DataTable result = new DataTable("pc");

				result.Columns.Add("postal_code", typeof(string));

				DataRow row = result.NewRow();
				row["postal_code"] = "12345";
				result.Rows.Add(row);

				row = result.NewRow();
				row["postal_code"] = "12346";
				result.Rows.Add(row);

				row = result.NewRow();
				row["postal_code"] = "12344";
				result.Rows.Add(row);

				return result;
			}
		}
		#endregion
	}
}