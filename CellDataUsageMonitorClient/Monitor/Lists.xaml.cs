using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Windows;

namespace Monitor
{
	/// <summary>
	/// Interaction logic for Lists.xaml
	/// </summary>
	public partial class Lists : Window
	{
		#region Fields
		private DataTable table = null;
		#endregion

		#region Constructors
		public Lists()
		{
			InitializeComponent();
		}

		public Lists(DataTable data)
		{
			this.table = data;
			this.Title = this.table.TableName;

			InitializeComponent();
		}
		#endregion

		#region Methods
		private void CloseClick(object sender, RoutedEventArgs e)
		{
			this.table.Dispose();

			this.Close();
		}	

		private void SaveClick(object sender, RoutedEventArgs e)
		{
			ShowSaveDialog();
		}

		private void SaveData(string fileName)
		{
			fileName = (fileName + "").Trim();

			bool result = false;

			try
			{
				using (StreamWriter writer = File.CreateText(fileName))
				{
					string data = "";

					foreach (DataColumn c in this.table.Columns)
						data += c.Caption + ",";

					data = data.Remove(data.Length - 1);

					writer.WriteLine(data);

					foreach (DataRow row in this.table.Rows)
					{
						object[] values = row.ItemArray;

						foreach (object o in values)
							data += o.ToString() + ",";

						data = data.Remove(data.Length - 1);

						writer.WriteLine(data);

						data = "";
					}

					writer.Close();
				}

				result = true;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
			finally
			{
				MessageBox.Show(result ? "Data saved." : "Failed saving data.");
			}
		}

		private void ShowSaveDialog()
		{
			SaveFileDialog sfd = new SaveFileDialog();

			try
			{
				sfd.DefaultExt = ".csv";
				sfd.Filter = "Comma-delimited files (*.csv)|*.csv";
				sfd.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
				sfd.Title = "Select Location";
				sfd.OverwritePrompt = true;
				sfd.FileName = "LimitData";

				bool? result = sfd.ShowDialog();

				if (result.HasValue)
				{
					if (result.Value)
						SaveData(sfd.FileName);
				}
			}
			finally
			{
				sfd = null;
			}
		}
		
		private void WindowInitialized(object sender, EventArgs e)
		{			
			this.grid.ItemsSource = this.table.AsEnumerable();
			this.grid.IsReadOnly = true;
		}
		#endregion
	}
}