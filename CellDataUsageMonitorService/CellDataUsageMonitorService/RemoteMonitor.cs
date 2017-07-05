using CellDataUsageMonitor.Service.Data;
using System;
using System.Collections;
using System.Net.Mail;
using System.Threading;
using System.Timers;

namespace CellDataUsageMonitor.Service
{
	/// <summary>
	/// http://it.toolbox.com/blogs/paytonbyrd/implementing-a-remoting-server-1872
	/// </summary>
	public class RemoteMonitor : Proxy
	{
		#region Fields
		private string[] agencies = null;
		private ArchiveData archiveData = null;
		private Hashtable cache = new Hashtable();
		private CoreData coreData = null;
		private int dataUsageDay = -1;
		private int dataUsageWeek = -1;
		private string[] emails = null;
		private int hitsDay = -1;
		private int hitsWeek = -1;
		private string[] postalCodes = null;
		private bool running = false;
		private int[] serialIds = null;
		private System.Timers.Timer timer = null;
		#endregion

		#region Constructors
		public RemoteMonitor()
		{
			Configuration.Load();
			this.timer = new System.Timers.Timer();

			this.timer.Elapsed += new ElapsedEventHandler(TimerElapsed);

			this.coreData = new CoreData(Configuration.CoreConnectionString, Configuration.ConnectionTimeout);

			this.archiveData = new ArchiveData(Configuration.ArchiveConnectionString, Configuration.ConnectionTimeout);
		}
		#endregion

		#region Properties
		public override int CacheSize
		{
			get { return this.cache.Count; }
		}

		public override object this[object key]
		{
			get
			{
				return this.cache[key];
			}
			set
			{
				this.cache[key] = value;
			}
		}
		#endregion

		#region Methods
		public override void AddToCache(object key, object value)
		{
			if (!this.cache.ContainsKey(key))
				this.cache.Add(key, value);
			else throw new ApplicationException("Duplicate key.");
		}

		public override void Dispose()
		{
			if (this.timer != null)
			{
				this.timer.Elapsed -= new ElapsedEventHandler(TimerElapsed);
				this.timer.Dispose();
			}

			this.timer = null;

			if (this.coreData != null)
				this.coreData.Dispose();

			this.coreData = null;

			if (this.archiveData != null)
				this.archiveData.Dispose();

			this.archiveData = null;

			if (this.cache != null)
				this.cache.Clear();

			this.cache = null;
		}

		public override bool KeyExists(object key)
		{
			return this.cache.ContainsKey(key);
		}

		public override bool MonitorRunning()
		{
			return this.running;
		}

		public override object RemoveFromCache(object key)
		{
			if (this.cache.ContainsKey(key))
				this.cache.Remove(key);

			return null;
		}

		private void RunDataChecks()
		{
			this.timer.Enabled = false;

			//Do checks here.  If item over threshold, send email.

			if (this.dataUsageDay > -1)
				CheckDataUsageThreshold(0);

			if (this.dataUsageWeek > -1)
				CheckDataUsageThreshold(1);

			if (this.hitsDay > -1)
				CheckHitThreshold(0);

			if (this.hitsWeek > -1)
				CheckHitThreshold(1);

			//Since the ARC database takes a long time to process, we may have to spin the call to it off as an async thread.

			this.timer.Enabled = true;
		}

		private ThresholdInfo[] CheckHitThreshold(int periodType)
		{
			string additionalText = "AFLT hits";

			ThresholdInfo[] data = null;

			if (periodType == 0)
			{
				Thread thread = new Thread(new ParameterizedThreadStart(this.archiveData.ExceedThreshold));

				thread.Start(new StartParameterInfo() { SerialIds = this.serialIds, PeriodType = periodType, Count = this.hitsDay });

				thread.Join();
				//this.archiveData.ExceedThreshold(this.serialIds, this.hitsDay, periodType);

				data = this.archiveData.Data;

				SendEmails(data, new ThresholdInfo[0], "daily", additionalText);
			}
			else
			{
				Thread thread = new Thread(new ParameterizedThreadStart(this.archiveData.ExceedThreshold));

				thread.Start(new StartParameterInfo() { SerialIds = this.serialIds, PeriodType = periodType, Count = this.hitsWeek });

				thread.Join();

				//this.archiveData.ExceedThreshold(this.serialIds, this.hitsWeek, periodType);

				data = this.archiveData.Data;

				SendEmails(new ThresholdInfo[0], data, "weekly", additionalText);
			}

			throw new NotImplementedException();
		}

		private ThresholdInfo[] CheckDataUsageThreshold(int periodType)
		{
			string additionalText = "bytes";

			ThresholdInfo[] data = null;

			if (periodType == 0)
			{
				//Thread thread = new Thread(new ParameterizedThreadStart(this.coreData.ExceedThreshold));

				//thread.Start(new StartParameterInfo() { Count = this.dataUsageDay, PeriodType = periodType, SerialIds = this.serialIds });

				this.coreData.ExceedThreshold(new StartParameterInfo() { Count = this.dataUsageDay, PeriodType = periodType, SerialIds = this.serialIds });

				data = this.coreData.Data;

				SendEmails(data, new ThresholdInfo[0], "daily usage", additionalText);
			}
			else
			{
				this.coreData.ExceedThreshold(new StartParameterInfo() { Count = this.dataUsageDay, PeriodType = periodType, SerialIds = this.serialIds });

				data = this.coreData.Data;

				SendEmails(new ThresholdInfo[0], data, "weekly usage", additionalText);
			}

			throw new NotImplementedException();
		}

		private void SendEmails(ThresholdInfo[] daily, ThresholdInfo[] weekly, string typeText, string additionalText)
		{
			string text = "";
			
			string startText = "\n\nThese serial numbers exceeded the {0} threshold of {1} {2}:\n\n";

			typeText = (typeText + "").Trim();
			additionalText = (additionalText + "").Trim();

			if (daily.Length > 0)
			{
				if (additionalText.Contains("bytes"))
					text += String.Format(startText, typeText, this.dataUsageDay.ToString(), additionalText);
				else
					text += String.Format(startText, typeText, this.hitsDay.ToString(), additionalText);

				foreach (ThresholdInfo ti in daily)
					text += ti.SerialId.ToString() + "\t" + ti.Count.ToString() + "\n";
			}

			if (weekly.Length > 0)
			{
				if (additionalText.Contains("bytes"))
					text += String.Format(startText, "weekly usage", this.dataUsageWeek.ToString(), "bytes");
				else
					text += String.Format(startText, typeText, this.hitsWeek.ToString(), additionalText);

				foreach (ThresholdInfo ti in weekly)
					text += ti.SerialId.ToString() + "\t" + ti.Count.ToString() + "\n";
			}

			string toEmails = "";

			foreach (string s in this.emails)
				toEmails += s + ";";

			using (MailMessage mm = new MailMessage(Configuration.FromEmail, toEmails))
			{
				mm.Subject = "Threshold Notification - " + (daily.Length > 0 ? "Daily" : "Weekly");
				mm.IsBodyHtml = true;
				mm.Body = text;

				using (SmtpClient c = new SmtpClient())
				{
					c.Host = Configuration.SmtpServer;
					c.Send(mm);
				}
			}
		}

		public override void StartMonitor(int[] serialIds, string[] agencies, string[] postalCodes, string[] emails, int dataUsageDay, int dataUsageWeek, int hitsDay, int hitsWeek)
		{
			this.serialIds = serialIds;
			this.agencies = agencies;
			this.postalCodes = postalCodes;
			this.emails = emails;
			this.hitsDay = hitsDay;
			this.hitsWeek = hitsWeek;
			this.dataUsageDay = dataUsageDay;
			this.dataUsageWeek = dataUsageWeek;
			this.timer.Enabled = true;
			this.running = true;
			throw new NotImplementedException();
		}

		public override void StopMonitor()
		{
			this.serialIds = null;
			this.agencies = null;
			this.postalCodes = null;
			this.emails = null;
			this.hitsDay = -1;
			this.hitsWeek = -1;
			this.dataUsageDay = -1;
			this.dataUsageWeek = -1;

			this.timer.Enabled = false;
			this.running = false;
			throw new NotImplementedException();
		}

		private void TimerElapsed(object sender, ElapsedEventArgs e)
		{
			RunDataChecks();
		}
		#endregion
	}
}