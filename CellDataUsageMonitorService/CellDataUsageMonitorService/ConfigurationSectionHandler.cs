using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace CellDataUsageMonitor.Service
{
	public class ConfigurationSectionHandler : IConfigurationSectionHandler
	{
		#region Methods
		public object Create(object parent, object configContext, XmlNode section)
		{
			Configuration.Data result = null;
			XmlElement xml = (XmlElement)section;

			result = new Configuration.Data();

			System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			result.ConfigurationAssemblyName = Path.GetFileName(((ExeContext)(config.EvaluationContext.HostingContext)).ExePath);
			result.ConfigurationFilePath = Path.GetDirectoryName(config.FilePath) + "\\";
			result.ConfigurationFileName = Path.GetFileName(config.FilePath);
			result.CoreConnectionString = xml.SelectSingleNode("CoreConnectionString").InnerText.Trim();
			result.ArchiveConnectionString = xml.SelectSingleNode("ArchiveConnectionString").InnerText.Trim();
			result.IsTracing = Convert.ToBoolean(xml.SelectSingleNode("IsTracing").InnerText.Trim().Length > 0 ?
				xml.SelectSingleNode("IsTracing").InnerText.Trim() : "False");
			result.StoragePath = xml.SelectSingleNode("StoragePath").InnerText.Trim();
			result.ConnectionTimeout = Convert.ToInt32(xml.SelectSingleNode("ConnectionTimeout").InnerText.Trim());
			result.SmtpServer = xml.SelectSingleNode("SmtpServer").InnerText.Trim();
			result.FromEmail = xml.SelectSingleNode("FromEmail").InnerText.Trim();

			return result;
		}
		#endregion
	}
}