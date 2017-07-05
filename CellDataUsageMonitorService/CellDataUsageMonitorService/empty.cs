using System; 
using System.Collections; 
using System.ComponentModel; 
using System.Runtime.Remoting; 
using System.Runtime.Remoting.Channels; 
using System.Runtime.Remoting.Channels.Tcp; 
//using PaytonByrd.SimpleRemotingInterface; 

namespace PaytonByrd.SimpleRemotingClient 
{ 
	/// 
	/// http://it.toolbox.com/blogs/paytonbyrd/implementing-a-remoting-client-1913
	/// 
	public class RemotingClientForm// : System.Windows.Forms.Form 
	{
		private string m_strRemotingServer = "localhost"; 
		private int m_intRemotingPort = 8000; 
		//SimpleInterface m_objSimpleCache = null; 
		private System.Timers.Timer m_objTimer; 
		
		// private System.ComponentModel.Container components = null; 
		
		[STAThread] 
		static void Main() 
		{ 
		
		} 
		
		public RemotingClientForm() 
		{ 
		
		} 
		
		#region Windows Form Designer generated code 
		/// 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor. 
		/// 
		private void InitializeComponent() 
		{ 
			m_objTimer = new System.Timers.Timer(); 
		} 
		#endregion 
		
		private void m_objTimer_Elapsed( object sender, System.Timers.ElapsedEventArgs e) 
		{ 
			ConnectCache(); 
		} 
		
		private void ConnectCache() 
		{
			string strURL = String.Format( "tcp://{0}:{1}/SimpleCache.tcp", m_strRemotingServer, m_intRemotingPort); 
			//if(m_objSimpleCache == null) 
			//{ 
			//    m_objSimpleCache = (SimpleInterface) Activator.GetObject( typeof(SimpleInterface), strURL); 
			//} 
		} 
		
		private void cmdSet_Click( object sender, System.EventArgs e) 
		{ 
			ConnectCache(); 
			//if(txtAddKey.Text.Length > 0) 
			//{ 
			//    if(m_objSimpleCache.KeyExists( txtAddKey.Text)) 
			//    { 
			//        m_objSimpleCache[txtAddKey.Text]= txtNewValue.Text; 
			//    } 
			//    else 
			//    { 
			//        m_objSimpleCache.AddToCache( txtAddKey.Text, txtNewValue.Text); 
			//        if(!lstKeys.Items.Contains( txtAddKey.Text)) { lstKeys.Items.Add( txtAddKey.Text); 
			//        } 
			//    } 
			//} 
		} 
		
		private void Find(string pv_strKey) 
		{ 
			//object objValue = null; 
			ConnectCache(); 
			//if(m_objSimpleCache.KeyExists( pv_strKey)) 
			//{ 
			//    objValue = m_objSimpleCache[pv_strKey]; 
			//    if(!lstKeys.Items.Contains(pv_strKey)) 
			//    { 
			//        lstKeys.Items.Add(pv_strKey); 
			//    } 
				
			//    lstKeys.SelectedItem = pv_strKey; 
			//    txtCurrentValue.Text = objValue.ToString(); 
			//} 
		}
		
		private void cmdRemove_Click( object sender, System.EventArgs e) 
		{ 
			ConnectCache(); 
			//if(txtAddKey.Text.Length > 0) 
			//{ 
			//    m_objSimpleCache.RemoveFromCache( txtAddKey.Text); 
			//    if(lstKeys.Items.Contains( txtAddKey.Text)) 
			//    { lstKeys.Items.Remove( txtAddKey.Text); 
			//    } 
			//} 
		} 
	} 
} 