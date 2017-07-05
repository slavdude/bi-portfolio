using System;
using System.Configuration;

namespace Business
{
	public class Configuration
	{
		#region Fields
		private static Data configurationData = null;
		public static string FileNamePrefix = System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the connection string to the archive database.
		/// </summary>
		public static string ArchiveConnectionString
		{
			get
			{
				LoadCheck();
				return configurationData.ArchiveConnectionString;
			}
		}

		/// <summary>
		/// Gets the name of the configuration assembly.
		/// </summary>
		public static string ConfigurationAssemblyName
		{
			get
			{
				LoadCheck();
				return configurationData.ConfigurationAssemblyName;
			}
		}

		/// <summary>
		/// Gets the name of the configuration file.
		/// </summary>
		public static string ConfigurationFileName
		{
			get
			{
				LoadCheck();
				return configurationData.ConfigurationFileName;
			}
		}

		/// <summary>
		/// Gets the path to the configuration file.
		/// </summary>
		public static string ConfigurationFilePath
		{
			get
			{
				LoadCheck();
				return configurationData.ConfigurationFilePath;
			}
		}

		/// <summary>
		/// Gets the database connection timeout in milliseconds.
		/// </summary>
		public static int ConnectionTimeout
		{
			get
			{
				LoadCheck();
				return configurationData.ConnectionTimeout;
			}
		}

		/// <summary>
		/// Gets the core database connection string.
		/// </summary>
		public static string CoreConnectionString
		{
			get
			{
				LoadCheck();
				return configurationData.CoreConnectionString;
			}
		}

		/// <summary>
		/// Gets a flag to toggle log tracing.
		/// </summary>
		public static bool IsTracing
		{
			get
			{
				LoadCheck();
				return configurationData.IsTracing;
			}
		}

		/// <summary>
		/// Gets the path for log files.
		/// </summary>
		public static string StoragePath
		{
			get
			{
				LoadCheck();
				return configurationData.StoragePath;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Forces a load of the configuration data.
		/// </summary>
		public static void Load()
		{
			LoadConfigurationFromFile();
		}

		/// <summary>
		/// Checks to see if the config file needs to be loaded.
		/// </summary>
		private static void LoadCheck()
		{
			if (configurationData == null)
				LoadConfigurationFromFile();
		}

		/// <summary>
		/// Loads configuration data from the configuration file.
		/// </summary>
		private static void LoadConfigurationFromFile()
		{
			configurationData = (Data)ConfigurationManager.GetSection("Business.Configuration");
		}
		#endregion

		#region Child Class
		public class Data
		{
			#region Fields
			private string archiveConnectionString = String.Empty;
			private string configurationAssemblyName = String.Empty;
			private string configurationFileName = String.Empty;
			private string configurationFilePath = String.Empty;
			private int connectionTimeout = 0;
			private string coreConnectionString = String.Empty;
			private bool isTracing = false;
			private string storagePath = String.Empty;
			#endregion

			#region Properties
			/// <summary>
			/// Gets or sets the connection string to the archive database.
			/// </summary>
			public string ArchiveConnectionString
			{
				get { return this.archiveConnectionString; }
				set { this.archiveConnectionString = value; }
			}

			/// <summary>
			/// Gets or sets the configuration assembly name.
			/// </summary>
			public string ConfigurationAssemblyName
			{
				get { return this.configurationAssemblyName; }
				set { this.configurationAssemblyName = value; }
			}

			/// <summary>
			/// Gets or sets the configuration file name.
			/// </summary>
			public string ConfigurationFileName
			{
				get { return this.configurationFileName; }
				set { this.configurationFileName = value; }
			}

			/// <summary>
			/// Gets or sets the path to the configuration file.
			/// </summary>
			public string ConfigurationFilePath
			{
				get { return this.configurationFilePath; }
				set { this.configurationFilePath = value; }
			}

			/// <summary>
			/// Gets or sets the database connection timeout in milliseconds.
			/// </summary>
			public int ConnectionTimeout
			{
				get { return this.connectionTimeout; }
				set { this.connectionTimeout = value; }
			}

			/// <summary>
			/// Gets or sets the main database connection string.
			/// </summary>
			public string CoreConnectionString
			{
				get { return this.coreConnectionString; }
				set { this.coreConnectionString = value; }
			}

			/// <summary>
			/// Gets or sets a flag toggling trace functionality.
			/// </summary>
			public bool IsTracing
			{
				get { return this.isTracing; }
				set { this.isTracing = value; }
			}

			/// <summary>
			/// Gets or sets the directory for log files.
			/// </summary>
			public string StoragePath
			{
				get { return this.storagePath; }
				set { this.storagePath = value; }
			}
			#endregion
		}
		#endregion
	}
}