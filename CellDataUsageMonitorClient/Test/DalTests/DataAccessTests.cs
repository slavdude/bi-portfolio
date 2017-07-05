using Dal;
using NUnit.Framework;

namespace Test.DalTests
{
	[TestFixture]
	public class DataAccessTests
	{
		#region Tests
		[Test]
		public void BuildSqlInStatementInt()
		{
			int[] values = new int[] { 1, 2, 3, 4, 5, 6 };

			string result = DataAccess.BuildSqlInStatement(values, "test");

			Assert.AreEqual("test IN(1, 2, 3, 4, 5, 6) ", result);
		}

		[Test]
		public void BuildSqlInStatementString()
		{
			string[] values = new string[] { "ABC", "DEF", "GHI", "JKL", "MNO'P" };

			string result = DataAccess.BuildSqlInStatement(values, "test");

			Assert.AreEqual("test IN('ABC', 'DEF', 'GHI', 'JKL', 'MNO''P') ", result);
		}
		#endregion
	}
}