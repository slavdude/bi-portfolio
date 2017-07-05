using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Business.Utility
{
	/// <summary>
	/// Class containing static utility methods.
	/// </summary>
	public class Common
	{
		#region Fields
		private static object traceObject = new object();
		#endregion

		#region Methods
		/// <summary>
		/// Formats an Exception into a human-readable message.
		/// </summary>
		/// <param name="e">The Exception to format (reference argument).</param>
		/// <returns>The message.</returns>
		public static string FormatError(ref Exception e)
		{
			string result = "";

			if (e.InnerException != null)
				result = "(" + e.Source + ") " + e.Message + " " + Regex.Replace(e.StackTrace + "", "\r\n", "") + " -- IE (" + e.InnerException.Source + ") " + 
					e.InnerException.Message + " " + Regex.Replace(e.InnerException.StackTrace + "", "\r\n", "");
			else
				result = "(" + e.Source + ") " + e.Message + " " + Regex.Replace(e.StackTrace + "", "\r\n", "");

			return result;
		}

		/// <summary>
		/// Normalizes a file path.
		/// </summary>
		/// <param name="path">The path to normalize.</param>
		/// <returns>The complete path.</returns>
		public static string ResolvePath(string path)
		{
			string result = String.Empty;

			if (!Path.IsPathRooted(path))
				result = Path.GetFullPath(Path.Combine(Configuration.ConfigurationFilePath, path));
			else
				result = Path.GetFullPath(path);

			if (!result.EndsWith("\\"))
				result += "\\";

			if (!Directory.Exists(result))
				Directory.CreateDirectory(result);

			return result;
		}

		/// <summary>
		/// Writes an Exception to the error log.
		/// </summary>
		/// <param name="e">The Exception.</param>
		public static void WriteToErrorLog(Exception e)
		{
			WriteToErrorLog(FormatError(ref e));
		}

		/// <summary>
		/// Writes a message to the error log.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void WriteToErrorLog(string message)
		{
			lock (traceObject)
			{
				message = (message + "").Trim();

				using (LogTrace lt = new LogTrace(Configuration.StoragePath + Configuration.FileNamePrefix + ".Error.log.", 15000000, 5, FileMode.Append))
				{
					lt.CanSplitData = false;

					using (StreamWriter writer = new StreamWriter(lt))
					{
						writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss:fff tt") + " " + message);
					}
				}
			}
		}

		/// <summary>
		/// Writes a message to the trace log.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void WriteToTraceLog(string message)
		{
			lock (traceObject)
			{
				message = (message + "").Trim();

				using (LogTrace lt = new LogTrace(Configuration.StoragePath + Configuration.FileNamePrefix + ".Trace.log.", 15000000, 5, FileMode.Append))
				{
					lt.CanSplitData = false;

					using (StreamWriter writer = new StreamWriter(lt))
					{
						writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss:fff tt") + " " + message);
					}
				}
			}
		}
		#endregion
	}
}