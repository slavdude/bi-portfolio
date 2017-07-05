using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace Test
{
	/// <summary>
	/// Helper methods for database testing.
	/// </summary>
	public class DatabaseHelper
	{
		#region Fields
		private static string localDatabase = "Data Source=" + Dns.GetHostName().ToUpper() + @"\sstwokeight;Initial Catalog=master;Integrated Security=True";
		#endregion

		#region Methods
		/// <summary>
		/// Creates a temporary database.
		/// </summary>
		public static void CreateDatabase(string dbName, string instance)
		{
			dbName = (dbName + "").Trim();

			instance = (instance + "").Trim();

			string script = LoadSqlFile(Business.Configuration.ConfigurationFilePath + "\\DBScripts\\" + instance + "CreateDatabase.sql");

			ExecuteSqlScript(script, dbName);
		}

		/// <summary>
		/// Drops the temporary database.
		/// </summary>
		/// <param name="databaseName">The name of the database to drop.</param>
		public static void DropDatabase(string databaseName)
		{
			databaseName += "";
			string script = LoadSqlFile(Business.Configuration.ConfigurationFilePath + "DBScripts\\DropDatabase.sql").Replace("##DatabaseName##", databaseName);
			ExecuteSqlScript(script, databaseName.Trim());
		}

		/// <summary>
		/// Executes a SQL script that returns no rows.
		/// </summary>
		/// <param name="script">The script to execute.</param>
		/// <returns>Any error text that gets generated.</returns>
		public static string ExecuteSqlScript(string script, string dbName)
		{
			string result = String.Empty;
			SqlConnection connection = new SqlConnection(localDatabase);
			Server server = new Server(new ServerConnection(connection));

			dbName += "";

			script = script.Replace("##DatabaseName##", dbName.Trim());

			try
			{
				server.ConnectionContext.ExecuteNonQuery(script);
				server.ConnectionContext.Disconnect();
			}
			catch (Exception e)
			{
				result = script + "\n" + e.Message + "\nData:\n" + e.InnerException;
				System.Console.WriteLine(result);
			}
			return result;
		}

		/// <summary>
		/// Replaces text in a file.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <param name="findText">The text to find.</param>
		/// <param name="replaceText">The text to substitute for findText.</param>
		public static void FileReplaceText(string fileName, string findText, string replaceText)
		{
			string text = string.Empty;

			using (StreamReader reader = new StreamReader(fileName))
			{
				text = reader.ReadToEnd();
				reader.Close();
			}

			text = text.Replace(findText, replaceText);

			using (StreamWriter writer = new StreamWriter(fileName, false))
			{
				writer.Write(text);
				writer.Close();
			}
		}

		/// <summary>
		/// Gets the configuration.
		/// </summary>
		public static void Initialize()
		{
			Business.Configuration.Load();
		}

		/// <summary>
		/// Bulk-inserts the selected data file.
		/// </summary>
		/// <param name="fileName">The name of the file to load.</param>
		/// <param name="tableName">The table to load it to.</param>
		/// <returns>The number of rows loaded.</returns>
		public static string LoadDataFile(string fileName, string tableName, string databaseName)
		{
			string loadScript = Business.Configuration.ConfigurationFilePath + "DBScripts\\BulkInsert.sql";

			fileName += "";
			tableName += "";
			databaseName += "";
			string script = LoadSqlFile(loadScript);

			script = script.Replace("##DatabaseName##", databaseName.Trim());
			script = script.Replace("##TableName##", "dbo." + tableName.Trim());
			script = script.Replace("##FileName##", fileName.Trim());

			return ExecuteSqlScript(script, databaseName.Trim());
		}

		/// <summary>
		/// Reads a SQL query file into memory.
		/// </summary>
		/// <param name="fileName">The name of the file to read.</param>
		/// <returns>The text within the file.</returns>
		public static string LoadSqlFile(string fileName)
		{
			string result = string.Empty;

			fileName += "";

			using (StreamReader input = new StreamReader(new FileStream(fileName.Trim(), FileMode.Open, FileAccess.Read, FileShare.Read, 8, FileOptions.Asynchronous)))
			{
				result = input.ReadToEnd();
			}

			return result;
		}
		#endregion
	}
}