using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Services;
using System.ServiceProcess;

namespace CellDataUsageMonitor.Service
{
	/// <summary>
	/// http://it.toolbox.com/blogs/paytonbyrd/implementing-a-remoting-server-1872
	/// </summary>
	public partial class MonitorService : ServiceBase
	{
		#region Fields
		private TcpChannel channel = null;
		//private Container componentContainer = null;
		#endregion

		#region Constructors
		public MonitorService()
		{
			InitializeComponent();
			//this.componentContainer = new Container();
		}
		#endregion

		#region Methods
		//protected override void OnCustomCommand(int command)
		//{
		//    base.OnCustomCommand(command);
		//}

		protected override void OnStart(string[] args)
		{
			this.channel = new TcpChannel(8000);

			ChannelServices.RegisterChannel(this.channel, true);

			RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemoteMonitor), "RemoteObject.tcp", WellKnownObjectMode.Singleton);
		}

		protected override void OnStop()
		{
			ChannelServices.UnregisterChannel(this.channel);
		}
		#endregion
	}
}