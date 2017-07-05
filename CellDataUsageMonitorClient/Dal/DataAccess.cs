using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dal
{
	/// <summary>
	/// Class for interacting with a database.
	/// </summary>
	public class DataAccess : IDisposable
	{
		#region Fields
		private SqlConnection connection = null;
		private string connectionString = String.Empty;
		private int commandTimeout = 0;
		private bool disposed = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of DataAccess.
		/// </summary>
		/// <param name="connectionString">The database connection string.</param>
		/// <param name="timeout">The database connection timeout.</param>
		public DataAccess(string connectionString, int timeout)
		{
			this.connectionString = (connectionString + "").Trim();
			this.commandTimeout = timeout;
			this.connection = new SqlConnection(this.connectionString);
		}
		#endregion

		#region Finalizers
		~DataAccess()
		{
			Dispose(false);
			GC.SuppressFinalize(this);
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Cleans up resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Cleans up resources.
		/// </summary>
		/// <param name="disposing">Flag indicating whether to dispose of internal resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!this.disposed)
				{
					if (this.connection.State != ConnectionState.Closed)
						this.connection.Close();

					this.connection.Dispose();

					this.connection = null;
				}

				this.disposed = true;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Generates a string from integer values for SQL IN statements.
		/// </summary>
		/// <param name="values">The values to use.</param>
		/// <param name="name">The name of the column in the table.</param>
		/// <returns>The completed string.</returns>
		public static string BuildSqlInStatement(int[] values, string name)
		{
			string result = "";

			name = (name + "").Trim();

			result = name + " IN(";

			for (int i = 0; i < values.Length; i++)
			{
				result += values[i].ToString();

				if (i < values.Length - 1)
					result += ", ";
			}

			result += ") ";

			return result;
		}

		/// <summary>
		/// Generates a string from string values for SQL IN statements, escaping single quotes in the string values.
		/// </summary>
		/// <param name="values">The values to use.</param>
		/// <param name="name">The name of the column in the table.</param>
		/// <returns>The completed string.</returns>
		public static string BuildSqlInStatement(string[] values, string name)
		{
			string result = "";

			name = (name + "").Trim();

			result = name + " IN(";

			for (int i = 0; i < values.Length; i++)
			{
				result += "'" + values[i].Replace("'", "''") + "'";	//Escape the single quotes in the string.

				if (i < values.Length - 1)
					result += ", ";
			}

			result += ") ";

			return result;
		}

		/// <summary>
		/// Gets a SqlDataReader.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <param name="commandType">The type of command to run.</param>
		/// <returns>The result set of the query.</returns>
		private SqlDataReader GetDataReader(string sql, CommandType commandType)
		{
			SqlDataReader result = null;

			sql = (sql + "").Trim();

			if (connection.State == ConnectionState.Closed)
			{
				connection.ConnectionString = connectionString;
				connection.Open();
			}

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandText = sql;
				command.CommandType = commandType;
				command.CommandTimeout = this.commandTimeout;

				result = command.ExecuteReader();
			}

			return result;
		}

		/// <summary>
		/// Gets a SqlDataReader.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <param name="commandType">The type of command to run.</param>
		/// <param name="parameters">The parameters to use in the query.</param>
		/// <returns>The result set of the query.</returns>
		private SqlDataReader GetDataReader(string sql, CommandType commandType, List<SqlParameter> parameters)
		{
			SqlDataReader result = null;

			sql = (sql + "").Trim();

			if (connection.State == ConnectionState.Closed)
			{
				connection.ConnectionString = connectionString;
				connection.Open();
			}

			using (SqlCommand command = connection.CreateCommand())
			{
				command.CommandText = sql;
				command.CommandType = commandType;
				command.CommandTimeout = this.commandTimeout;

				command.Parameters.AddRange(parameters.ToArray());

				result = command.ExecuteReader();
			}

			return result;
		}

		/// <summary>
		/// Gets a DataTable.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <param name="commandType">The type of command to run.</param>
		/// <returns>The result set of the query.</returns>
		public DataTable GetDataTable(string sql, CommandType commandType)
		{
			DataTable result = null;

			sql = (sql + "").Trim();

			try
			{
				using (SqlDataReader reader = GetDataReader(sql, commandType))
				{
					if (reader.HasRows)
					{
						result = new DataTable();

						result.Load(reader);
					}

					reader.Close();
				}
			}
			finally
			{
				if (this.connection.State != ConnectionState.Closed)
					this.connection.Close();
			}

			return result;
		}

		/// <summary>
		/// Gets a DataTable.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <param name="commandType">The type of command to run.</param>
		/// <param name="parameters">The parameters to use in the query.</param>
		/// <returns>The result set of the query.</returns>
		public DataTable GetDataTable(string sql, CommandType commandType, List<SqlParameter> parameters)
		{
			DataTable result = null;

			sql = (sql + "").Trim();

			try
			{
				using (SqlDataReader reader = GetDataReader(sql, commandType, parameters))
				{
					if (reader.HasRows)
					{
						result = new DataTable();

						result.Load(reader);
					}

					reader.Close();
				}
			}
			finally
			{
				if (this.connection.State != ConnectionState.Closed)
					this.connection.Close();
			}

			return result;
		}

		/// <summary>
		/// Gets a scalar value from the query.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <param name="commandType">The type of command to run.</param>
		/// <returns>The result set of the query.</returns>
		public object GetScalar(string sql, CommandType commandType)
		{
			object result;

			sql = (sql + "").Trim();

			try
			{
				if (connection.State == ConnectionState.Closed)
				{
					connection.ConnectionString = connectionString;
					connection.Open();
				}

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = sql;
					command.CommandType = commandType;
					command.CommandTimeout = this.commandTimeout;

					result = command.ExecuteScalar();
				}
			}
			finally
			{
				this.connection.Close();
			}

			return result;
		}

		/// <summary>
		/// Gets a scalar value from the query.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <param name="commandType">The type of command to run.</param>
		/// <param name="parameters">The parameters to use in the query.</param>
		/// <returns>The result set of the query.</returns>
		public object GetScalar(string sql, CommandType commandType, List<SqlParameter> parameters)
		{
			object result;

			sql = (sql + "").Trim();

			try
			{
				if (connection.State == ConnectionState.Closed)
				{
					connection.ConnectionString = connectionString;
					connection.Open();
				}

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = sql;
					command.CommandType = commandType;
					command.CommandTimeout = this.commandTimeout;

					command.Parameters.AddRange(parameters.ToArray());

					result = command.ExecuteScalar();
				}
			}
			finally
			{
				this.connection.Close();
			}

			return result;
		}

		/// <summary>
		/// Processes a query that returns no results.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <param name="commandType">The type of command to run.</param>
		/// <returns>The number of rows affected.</returns>
		public int ProcessCommand(string sql, CommandType commandType)
		{
			int result = 0;

			sql = (sql + "").Trim();

			try
			{
				if (connection.State == ConnectionState.Closed)
				{
					connection.ConnectionString = connectionString;
					connection.Open();
				}

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = sql;
					command.CommandType = commandType;
					command.CommandTimeout = this.commandTimeout;

					result = command.ExecuteNonQuery();
				}
			}
			finally
			{
				this.connection.Close();
			}

			return result;
		}

		/// <summary>
		/// Processes a query that returns no results.
		/// </summary>
		/// <param name="sql">The query to run.</param>
		/// <param name="commandType">The type of command to run.</param>
		/// <param name="parameters">The parameters to use in the query.</param>
		/// <returns>The number of rows affected.</returns>
		public int ProcessCommand(string sql, CommandType commandType, List<SqlParameter> parameters)
		{
			int result = 0;

			sql = (sql + "").Trim();

			try
			{
				if (connection.State == ConnectionState.Closed)
				{
					connection.ConnectionString = connectionString;
					connection.Open();
				}

				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = sql;
					command.CommandType = commandType;
					command.CommandTimeout = this.commandTimeout;

					command.Parameters.AddRange(parameters.ToArray());

					result = command.ExecuteNonQuery();
				}
			}
			finally
			{
				this.connection.Close();
			}

			return result;
		}
		#endregion
	}
}