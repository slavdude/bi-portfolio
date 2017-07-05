using Business;
using Business.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Monitor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields
		private List<AgencyData> agencyDataList = null;
		private string[] agencyList = null;
		private string emailDefaultText = "Enter the email addresses of users who should receive alerts, separated by semicolons.";
		private string emailList = String.Empty;
		private bool numericInput = false;
		private string[] postalCodeList = null;
		private string serialIdList = String.Empty;
		private string serviceLabelText = "Cell Usage Data Service is ";
		private bool serviceRunning = false;
		private List<Window> windows = new List<Window>();
		#endregion

		#region Constructors
		public MainWindow()
		{
			InitializeComponent();		
		}
		#endregion

		#region Methods
		/// <summary>
		/// Handles the Agencies.SelectionChanged event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AgenciesSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			LoadPostalCodes(true);
		}

		private List<Window> Calculate(Statistics stats, Enumerators.Period pd, Enumerators.StatisticsType type, bool? average, bool? high, bool? low,
			bool? stdDev, bool? threshold)
		{
			List<Window> result = new List<Window>();

			//DataTable highTable = null;
			//DataTable lowTable = null;
			//DataTable thresholdTable = null;
			//double standardDeviation = 0;

			//Enumerators.GraphType graphType = Enumerators.GraphType.Average;

			string title = "AFLT Hits";

			if (type == Enumerators.StatisticsType.DataUsage)
				title = "Cell Data Usage";

			//if (average.HasValue && average.Value)
			//    averageTable = stats.GetAverage(type, pd);

			DataTable data = stats.GetData(type);

			if (data != null)
			{
				data.TableName = title;

				Charts charts = new Charts();



				result.Add(charts);

				//if (data.Rows.Count > 1)
				//    result.Add(new Charts());
				//else
				//    result.Add(new Lists(data));
			}

			//if (high.HasValue && high.Value)
			//    highTable = stats.GetHighest(type);

			//if (highTable != null)
			//{
			//    highTable.TableName = title + "Highest";

			//    graphType = Enumerators.GraphType.Highest;

			//    if (highTable.Rows.Count > 1)
			//        result.Add(new Charts(highTable, graphType));
			//    else
			//        result.Add(new Lists(highTable));
			//}

			//if (low.HasValue && low.Value)
			//    lowTable = stats.GetLowest(type);

			//if (lowTable != null)
			//{
			//    lowTable.TableName = title + "Lowest";

			//    graphType = Enumerators.GraphType.Lowest;

			//    if (lowTable.Rows.Count > 1)
			//        result.Add(new Charts(lowTable, graphType));
			//    else
			//        result.Add(new Lists(lowTable));
			//}

			//Figure out where to put this.

			//if (stdDev.HasValue && stdDev.Value)
			//    standardDeviation = stats.GetStandardDeviation(type, pd);

			//if (threshold.HasValue && threshold.Value)
			//    thresholdTable = stats.GetDevicesOverThreshold(type, pd);

			//if (thresholdTable != null)
			//{
			//    thresholdTable.TableName = title + "Threshold";

			//    graphType = Enumerators.GraphType.Threshold;

			//    if (thresholdTable.Rows.Count > 1)
			//        result.Add(new Charts(thresholdTable, graphType));
			//    else
			//        result.Add(new Lists(thresholdTable));
			//}

			return result;
		}

		/// <summary>
		/// Handles CheckBox.Checked events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Checked(object sender, RoutedEventArgs e)
		{
			string name = ((CheckBox)sender).Name;

			if ((name == "dataUsage") || (name == "afltHits"))
				EnableDisablePeriodList();
			else
			{
				if (name == "exceedThreshold")
					this.threshold.IsEnabled = true;

				EnableDisableGenerateButton();
			}
		}

		private bool CheckServiceStatus()
		{
			return false;
		}

		/// <summary>
		/// Handles the Close.Click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CloseClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private string[] ConvertToStringArray(ListBox listBox)
		{
			string[] result = null;

			if (listBox.SelectedItems.Count > 0)
			{
				result = new string[listBox.SelectedItems.Count];

				for (int i = 0; i < listBox.SelectedItems.Count; i++)
					result[i] = ((ListBoxItem)listBox.SelectedItems[i]).Content.ToString();
			}

			return result;
		}

		/// <summary>
		/// Handles the EmailExpander.Collapsed event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EmailExpanderCollapsed(object sender, RoutedEventArgs e)
		{
			this.dataUsageGroup.Visibility = System.Windows.Visibility.Visible;
		}

		/// <summary>
		/// Handles the EmailExpander.Expanded event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EmailExpanderExpanded(object sender, RoutedEventArgs e)
		{
			this.dataUsageGroup.Visibility = System.Windows.Visibility.Hidden;
		}

		/// <summary>
		/// Handles the Emails.KeyDown event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EmailsKeyDown(object sender, KeyEventArgs e)
		{
			ResetText();
		}

		/// <summary>
		/// Handles the Emails.MouseDown event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EmailsMouseDown(object sender, MouseButtonEventArgs e)
		{
			ResetText();
		}

		/// <summary>
		/// Enables or disables the Generate Statistics button based on the user's selections.
		/// </summary>
		private void EnableDisableGenerateButton()
		{
			this.generateStatistics.IsEnabled = false;

			if (this.average.IsChecked.HasValue && this.average.IsChecked.Value)
				this.generateStatistics.IsEnabled = true;
			else if (this.highest.IsChecked.HasValue && this.highest.IsChecked.Value)
				this.generateStatistics.IsEnabled = true;
			else if (this.lowest.IsChecked.HasValue && this.lowest.IsChecked.Value)
				this.generateStatistics.IsEnabled = true;
			else if (this.standardDeviation.IsChecked.HasValue && this.standardDeviation.IsChecked.Value)
				this.generateStatistics.IsEnabled = true;

			if (this.threshold.IsEnabled)
				this.generateStatistics.IsEnabled = this.numericInput;
		}

		/// <summary>
		/// Enables or disables the Period dropdown based on the user's selections.
		/// </summary>
		private void EnableDisablePeriodList()
		{
			this.period.IsEnabled = ((this.dataUsage.IsChecked.HasValue && this.dataUsage.IsChecked.Value) ||
				(this.afltHits.IsChecked.HasValue && this.afltHits.IsChecked.Value));
		}

		/// <summary>
		/// Generates the charts or lists for the statistical options selected by the user.
		/// </summary>
		private void GenerateStatistics()
		{
			this.agencyList = ConvertToStringArray(this.agencies);

			this.postalCodeList = ConvertToStringArray(this.postalCodes);

			Statistics stats = new Statistics()
			{
				Agencies = this.agencyList,
				PostalCodes = this.postalCodeList,
				SerialIds = this.serialIds.Text,
				StartDate = Convert.ToDateTime(this.startDate.Text),
				EndDate = Convert.ToDateTime(this.endDate.Text),
				AfltHitThreshold = Convert.ToInt32(this.hits.Text.Length > 0 ? this.hits.Text : "-1"),
				DataUsageThreshold = Convert.ToInt32(this.usage.Text.Length > 0 ? this.usage.Text : "-1")
			};

			Enumerators.Period pd = (Enumerators.Period)Enum.Parse(typeof(Enumerators.Period), ((ListBoxItem)this.period.SelectedItem).Content.ToString());

			try
			{
				if (this.afltHits.IsChecked.HasValue && this.afltHits.IsChecked.Value)
					this.windows.AddRange(Calculate(stats, pd, Enumerators.StatisticsType.AfltHit, this.average.IsChecked,
						this.highest.IsChecked, this.lowest.IsChecked, this.standardDeviation.IsChecked, this.exceedThreshold.IsChecked));

				if (this.dataUsage.IsChecked.HasValue && this.dataUsage.IsChecked.Value)
					this.windows.AddRange(Calculate(stats, pd, Enumerators.StatisticsType.DataUsage, this.average.IsChecked,
						this.highest.IsChecked, this.lowest.IsChecked, this.standardDeviation.IsChecked, this.exceedThreshold.IsChecked));

				foreach (Window w in this.windows)
					w.Show();
			}
			finally
			{
				stats.Dispose();
			}
		}

		/// <summary>
		/// Handles the GenerateStatistics.Click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GenerateStatisticsClick(object sender, RoutedEventArgs e)
		{
			GenerateStatistics();
		}

		/// <summary>
		/// Checks to see whether the input string represents an integer value.
		/// </summary>
		/// <param name="input">The string to check.</param>
		/// <returns>True if the string represents an integer; otherwise false.</returns>
		private bool IsNumeric(string input)
		{
			input = (input + "").Trim();

			Regex regex = new Regex(@"^\d+$");

			return regex.Match(input).Success;
		}

		/// <summary>
		/// Loads the Agencies listbox.
		/// </summary>
		private void LoadAgencies()
		{
			this.agencies.Items.Clear();

			using (AgencyData agencies = new AgencyData())
			{
				this.agencyDataList = agencies.GetAgencies();

				if (this.agencyDataList != null)
				{
					foreach (AgencyData ad in this.agencyDataList)
						this.agencies.Items.Add(new ListBoxItem() { Content = ad.Name });
				}
			}
		}

		/// <summary>
		/// Handles the LoadConfiguration.Click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoadConfigurationClick(object sender, RoutedEventArgs e)
		{
			LoadConfigurationFile(Configuration.ConfigurationFilePath + "userdata.xml");
		}

		/// <summary>
		/// Loads configuration data from the provided file.
		/// </summary>
		/// <param name="fileName">The name of the file to load.</param>
		private void LoadConfigurationFile(string fileName)
		{
			fileName = (fileName + "").Trim();

			UserConfiguration uc = new UserConfiguration();

			uc = uc.Deserialize(fileName);

			this.serialIdList = uc.SerialIds;
			this.emailList = uc.Emails;
			this.agencyList = (uc.Agencies != null ? (uc.Agencies.Length > 0 ? uc.Agencies.Split(new char[] { ',' }) : null) : null);
			this.postalCodeList = (uc.PostalCodes != null ? (uc.PostalCodes.Length > 0 ? uc.PostalCodes.Split(new char[] { ',' }) : null) : null);
			this.usagePerDay.Text = (uc.DataUsage.Daily.Value == -1 ? "" : uc.DataUsage.Daily.Value.ToString());
			this.usagePerWeek.Text = (uc.DataUsage.Weekly.Value == -1 ? "" : uc.DataUsage.Weekly.Value.ToString());
			this.hitsPerDay.Text = (uc.AfltHits.Daily.Value == -1 ? "" : uc.AfltHits.Daily.Value.ToString());
			this.hitsPerWeek.Text = (uc.AfltHits.Weekly.Value == -1 ? "" : uc.AfltHits.Weekly.Value.ToString());

			if (this.emailList != null)
			{
				if (this.emailList.Length > 0)
				{
					ResetText();
					this.emails.Text = this.emailList;
				}
			}

			this.serialIds.Text = this.serialIdList;

			ListBoxItem[] lbi = null;
			List<ListBoxItem> items = null;

			if (this.agencyList != null)
			{
				lbi = new ListBoxItem[this.agencies.Items.Count];

				this.agencies.Items.CopyTo(lbi, 0);

				items = lbi.ToList();

				for (int i = 0; i < this.agencyList.Length; i++)
				{
					ListBoxItem item = null;

					try
					{
						item = items.Where(x => x.Content.ToString() == this.agencyList[i]).First();
					}
					catch (InvalidOperationException)
					{
						//NOP
					}

					if (item != null)
						this.agencies.SelectedItems.Add(item);
				}
			}

			if (this.postalCodeList != null)
			{
				lbi = new ListBoxItem[this.postalCodes.Items.Count];

				this.postalCodes.Items.CopyTo(lbi, 0);

				items = lbi.ToList();

				for (int i = 0; i < this.postalCodeList.Length; i++)
				{
					ListBoxItem item = null;

					try
					{
						item = items.Where(x => x.Content.ToString() == this.postalCodeList[i]).First();
					}
					catch (InvalidOperationException)
					{
					}

					if (item != null)
						this.postalCodes.SelectedItems.Add(item);
				}
			}
		}

		/// <summary>
		/// Loads the PostalCodes listbox.
		/// </summary>
		/// <param name="selectionChanged">Flag indicating the source of the call--false if initial load; true if Agencies is changed.</param>
		private void LoadPostalCodes(bool selectionChanged)
		{
			this.postalCodes.Items.Clear();

			using (AgencyData agency = new AgencyData())
			{
				List<int> ids = new List<int>();

				List<ListBoxItem> items = new List<ListBoxItem>();

				if (selectionChanged)
				{
					foreach (ListBoxItem lbi in this.agencies.SelectedItems)
						items.Add(lbi);
				}
				else
				{
					foreach (ListBoxItem lbi in this.agencies.Items)
						items.Add(lbi);
				}

				foreach (ListBoxItem lbi in items)
					ids.Add(this.agencyDataList.Where(x => x.Name == lbi.Content.ToString()).First().Id);

				this.postalCodeList = agency.GetPostalCodes(ids.ToArray());
			}

			if (this.postalCodeList.Length > 0)
			{
				foreach (string s in this.postalCodeList)
					this.postalCodes.Items.Add(new ListBoxItem() { Content = s });
			}
		}

		/// <summary>
		/// Loads the user interface.
		/// </summary>
		private void LoadUI()
		{
			this.serviceRunning = CheckServiceStatus();

			if (this.serviceRunning)
			{
				this.runStatus.Content = this.serviceLabelText + "Running";
				this.runStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 128, 0));
				this.start.Content = "Stop Service";
			}
			else
			{
				this.runStatus.Content = this.serviceLabelText + "Stopped";
				this.runStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
				this.start.Content = "Start Service";
			}

			DateTime now = DateTime.Now;
			this.startDate.Text = this.endDate.Text = new DateTime(now.Year, now.Month, now.Day).ToString();
			this.emails.Text = this.emailDefaultText;
			this.emails.Foreground = new SolidColorBrush(Colors.DarkGray);
			this.emails.FontStyle = FontStyles.Italic;

			LoadAgencies();

			if (File.Exists(Configuration.ConfigurationFilePath + "userdata.xml"))
				LoadConfigurationFile(Configuration.ConfigurationFilePath + "userdata.xml");

			if (this.agencyList != null)
			{
				if (this.agencyList.Length > 0)
					SetSelectedAgencies();
			}

			if (this.agencies.SelectedItems.Count > 0)
				LoadPostalCodes(false);

			if (this.emailList.Length > 0)
			{
				this.emails.Foreground = new SolidColorBrush(Colors.Black);
				this.emails.FontStyle = FontStyles.Normal;

				this.emails.Text = this.emailList;
			}
		}

		/// <summary>
		/// Handles the Period.SelectionChanged event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PeriodSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.measures.IsEnabled = false;

			if (this.period.SelectedIndex == -1) return;

			this.measures.IsEnabled = true;
		}

		/// <summary>
		/// Sets the text style of the Emails text block.
		/// </summary>
		private void ResetText()
		{
			if (this.emails.Text == this.emailDefaultText)
			{
				this.emails.Foreground = new SolidColorBrush(Colors.Black);
				this.emails.FontStyle = FontStyles.Normal;
				this.emails.Text = "";
			}
		}

		private void SetSelectedAgencies()
		{
			this.agencies.SelectedItems.Clear();

			ListBoxItem[] items = new ListBoxItem[this.agencies.Items.Count];

			this.agencies.Items.CopyTo(items, 0);

			foreach (string s in this.agencyList)
				this.agencies.SelectedItems.Add(items.Where(x => x.Content.ToString() == s).First());

			items = null;
		}

		/// <summary>
		/// Handles the Start.Click event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StartClick(object sender, RoutedEventArgs e)
		{
			if (this.start.Content.ToString() == "Start Service")
			{
				if (MessageBox.Show("Do you want to save the current alert configuration?", "Save Configuration", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					SaveConfigurationFile(Configuration.ConfigurationFilePath + "userdata.xml");

				this.start.Content = "Stop Service";
				StartService();
			}
			else
			{
				this.start.Content = "Start Service";
				StopService();
			}
		}

		private void StopService()
		{
			MessageBox.Show("Implement");
		}

		private void StartService()
		{
			MessageBox.Show("Implement");
		}

		/// <summary>
		/// Saves an alert configuration to the provided file.
		/// </summary>
		/// <param name="fileName">The name of the file to save.</param>
		private void SaveConfigurationFile(string fileName)
		{
			fileName = (fileName + "").Trim();

			UserConfiguration uc = new UserConfiguration();

			if (this.emails.Text != this.emailDefaultText)
				uc.Emails = this.emailList = this.emails.Text;

			uc.DataUsage = new Threshold();

			if (this.usagePerDay.Text.Trim().Length > 0)
				uc.DataUsage.Daily = Convert.ToInt32(this.usagePerDay.Text);
			if (this.usagePerWeek.Text.Trim().Length > 0)
				uc.DataUsage.Weekly = Convert.ToInt32(this.usagePerWeek.Text);

			uc.AfltHits = new Threshold();

			if (this.hitsPerDay.Text.Trim().Length > 0)
				uc.AfltHits.Daily = Convert.ToInt32(this.hitsPerDay.Text);
			if (this.hitsPerWeek.Text.Trim().Length > 0)
				uc.AfltHits.Weekly = Convert.ToInt32(this.hitsPerWeek.Text);

			uc.SerialIds = this.serialIds.Text;

			if (this.agencies.SelectedItems.Count > 0)
			{
				this.agencyList = ConvertToStringArray(this.agencies);

				foreach (string s in this.agencyList)
					uc.Agencies += s + ",";

				uc.Agencies = uc.Agencies.Remove(uc.Agencies.Length - 1);
			}

			if (this.postalCodes.SelectedItems.Count > 0)
			{
				this.postalCodeList = ConvertToStringArray(this.postalCodes);

				foreach (string s in this.postalCodeList)
					uc.PostalCodes += s + ",";

				uc.PostalCodes = uc.PostalCodes.Remove(uc.PostalCodes.Length - 1);
			}

			if (uc.Serialize(fileName))
				MessageBox.Show("Alert configuration saved successfully.");
		}

		/// <summary>
		/// Handles TextBox.TextChanged events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox tb = sender as TextBox;

			string name = tb.Name;

			this.numericInput = false;

			if (name == "usage")
			{
				if (this.usage.Text.Trim().Length == 0)
					return;
			}
			else
			{
				if (this.hits.Text.Trim().Length == 0)
					return;
			}

			this.numericInput = IsNumeric(tb.Text);

			EnableDisableGenerateButton();
		}

		/// <summary>
		/// Handles CheckBox.Unchecked events.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Unchecked(object sender, RoutedEventArgs e)
		{
			string name = ((CheckBox)sender).Name;

			if ((name == "dataUsage") || (name == "afltHits"))
				EnableDisablePeriodList();
			else
			{
				if (name == "exceedThreshold")
				{
					this.usage.Text = "";
					this.hits.Text = "";

					this.threshold.IsEnabled = false;
				}

				EnableDisableGenerateButton();
			}
		}

		/// <summary>
		/// Handles the Window.Initialized event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WindowInitialized(object sender, EventArgs e)
		{
			LoadUI();
		}
		#endregion
	}
}