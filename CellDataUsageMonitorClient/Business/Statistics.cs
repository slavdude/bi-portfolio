using Business.Utility;
using Dal.Archive;
using Dal.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Business
{
	/// <summary>
	/// Class for calculation and display of AFLT hits and cell data usage.
	/// </summary>
	public class Statistics : IDisposable
	{
		#region Fields
		private bool afltHits = false;
		private int afltHitThreshold = -1;
		private Agency agency = null;
		private string[] agencies = null;
		private bool average = false;
		private Client client = null;
		private bool dataUsage = false;
		private int dataUsageThreshold = -1;
		private bool disposed = false;
		private DateTime endDate = DateTime.Now;
		private bool exceedsThreshold = false;
		private EventPacket eventPacket = null;
		private GpsData gpsData = null;
		private bool highest = false;
		private bool lowest = false;
		private Enumerators.Period period = Enumerators.Period.Day;
		private string[] postalCodes = null;
		private string serialIds = String.Empty;
		private bool standardDeviation = false;
		private DateTime startDate = DateTime.Now;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of Statistics.
		/// </summary>
		public Statistics()
		{
		}

		/// <summary>
		/// Creates a new instance of Statistics.
		/// </summary>
		/// <param name="agencies">The list of agencies available.</param>
		/// <param name="postalCodes">The postal codes for each agency.</param>
		/// <param name="serialIds">The available device IDs.</param>
		public Statistics(string[] agencies, string[] postalCodes, string serialIds)
		{
			this.agencies = agencies;
			this.postalCodes = postalCodes;
			this.serialIds = serialIds;
		}
		#endregion

		#region Finalizers
		~Statistics()
		{
			Dispose(false);
		}
		#endregion

		#region IDisposable Members
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!this.disposed)
				{
					if (this.agency != null)
						this.agency.Dispose();
					this.agency = null;

					if (this.client != null)
						this.client.Dispose();
					this.client = null;

					if (this.eventPacket != null)
						this.eventPacket.Dispose();
					this.eventPacket = null;

					if (this.gpsData != null)
						this.gpsData.Dispose();
					this.gpsData = null;

					this.disposed = true;
				}
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a flag indicating whether to track AFLT hits.
		/// </summary>
		public bool AfltHits
		{
			get { return this.afltHits; }
			set { this.afltHits = value; }
		}

		/// <summary>
		/// Gets or sets the AFLT hit threshold.
		/// </summary>
		public int AfltHitThreshold
		{
			get { return this.afltHitThreshold; }
			set { this.afltHitThreshold = value; }
		}

		/// <summary>
		/// Gets or sets the available agencies.
		/// </summary>
		public string[] Agencies
		{
			get { return this.agencies; }
			set { this.agencies = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether to track averages.
		/// </summary>
		public bool Average
		{
			get { return this.average; }
			set { this.average = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether to track cell data usage.
		/// </summary>
		public bool DataUsage
		{
			get { return this.dataUsage; }
			set { this.dataUsage = value; }
		}

		/// <summary>
		/// Gets or sets the cell data usage threshold.
		/// </summary>
		public int DataUsageThreshold
		{
			get { return this.dataUsageThreshold; }
			set { this.dataUsageThreshold = value; }
		}

		/// <summary>
		/// Gets or sets the ending date of the period of interest.
		/// </summary>
		public DateTime EndDate
		{
			get { return this.endDate; }
			set { this.endDate = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether to track items exceeding a specified threshold.
		/// </summary>
		public bool ExceedsThreshold
		{
			get { return this.exceedsThreshold; }
			set { this.exceedsThreshold = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether to track the item with the highest data usage and/or AFLT hits.
		/// </summary>
		public bool Highest
		{
			get { return this.highest; }
			set { this.highest = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether to track the item with the lowest data usage and/or AFLT hits.
		/// </summary>
		public bool Lowest
		{
			get { return this.lowest; }
			set { this.lowest = value; }
		}

		/// <summary>
		/// Gets or sets the type of period of interest (day, week, month).
		/// </summary>
		public Enumerators.Period Period
		{
			get { return this.period; }
			set { this.period = value; }
		}

		/// <summary>
		/// Gets or sets the available postal codes.
		/// </summary>
		public string[] PostalCodes
		{
			get { return this.postalCodes; }
			set { this.postalCodes = value; }
		}

		/// <summary>
		/// Gets or sets the serial numbers of the devices of interest.
		/// </summary>
		public string SerialIds
		{
			get { return this.serialIds; }
			set { this.serialIds = value; }
		}

		/// <summary>
		/// Gets or sets a flag indicating whether to track standard deviations.
		/// </summary>
		public bool StandardDeviation
		{
			get { return this.standardDeviation; }
			set { this.standardDeviation = value; }
		}

		/// <summary>
		/// Gets or sets the starting date of the period of interest.
		/// </summary>
		public DateTime StartDate
		{
			get { return this.startDate; }
			set { this.startDate = value; }
		}
		#endregion

		#region Methods
		///// <summary>
		///// Calculates a standard deviation for a range of values.
		///// </summary>
		///// <param name="values">The values to use.</param>
		///// <returns>The standard deviation, to 5 decimal places.</returns>
		//public static double CalculateStandardDeviation(List<double> values)
		//{
		//    if (values.Count == 1) return double.NaN;

		//    double average = values.Average();

		//    double derivation = 0;

		//    for (int i = 0; i < values.Count; i++)
		//    {
		//        double value = values[i] - average;

		//        derivation += (value * value);
		//    }
			
		//    return Math.Round(Math.Sqrt(derivation / (values.Count - 1)), 5);
		//}

		/// <summary>
		/// Expands a numeric range, given the upper and lower bounds as a string.
		/// </summary>
		/// <param name="data">The range to expand.</param>
		/// <returns>The members of the range as a string array.</returns>
		public string[] ExpandRange(string data)
		{
			List<string> result = new List<string>();

			string[] bounds = data.Split(("-").ToCharArray(), 2);

			int start = Convert.ToInt32(bounds[0]);
			int end = Convert.ToInt32(bounds[1]);

			for (int i = 0; i < end - start; i++)
				result.Add((start + i).ToString());

			result.Add(end.ToString());

			return result.ToArray();
		}

		///// <summary>
		///// Gets the average number of items per period.
		///// </summary>
		///// <param name="type">The type of average to get.</param>
		///// <param name="period">The period to calculate.</param>
		///// <returns>The averages meeting the specified criteria.</returns>
		//public DataTable GetAverage(Enumerators.StatisticsType type, Enumerators.Period period)
		//{
		//    return GetAverage(type, period, (DataFactory)null);
		//}

		///// <summary>
		///// Gets the average number of items per period.
		///// </summary>
		///// <param name="type">The type of average to get.</param>
		///// <param name="period">The period to calculate.</param>
		///// <param name="dataFactory">The data to use for testing.</param>
		///// <returns>The averages meeting the specified criteria.</returns>
		//public DataTable GetAverage(Enumerators.StatisticsType type, Enumerators.Period period, DataFactory dataFactory)
		//{
		//    DataTable result = null;

		//    DataFactory df = new DataFactory();

		//    if (dataFactory != null)
		//        df = dataFactory;

		//    DataTable temp = null;

		//    List<string> list = ParseSerialIds();

		//    int[] ids = new int[list.Count];

		//    for (int i = 0; i < list.Count; i++)
		//        ids[i] = Convert.ToInt32(list[i]);

		//    if (type == Enumerators.StatisticsType.DataUsage)
		//    {
		//        this.eventPacket = df.GetNewEventPacket();
		//        temp = this.eventPacket.GetEventPackets(ids, this.startDate, this.endDate);
		//    }
		//    else
		//    {
		//        this.gpsData = df.GetNewGpsData();
		//        temp = this.gpsData.GetHits(ids, this.startDate, this.endDate);
		//    }

		//    if (temp != null)
		//    {
		//        result = new DataTable();

		//        result.Columns.Add("SerialId", typeof(int));
		//        result.Columns.Add("Average", typeof(double));

		//        foreach (int i in ids)
		//        {
		//            DataRow[] rows = null;

		//            if (type == Enumerators.StatisticsType.DataUsage)
		//                rows = temp.Select("SerialId = " + i.ToString());
		//            else
		//                rows = temp.Select("lSerial_ID = " + i.ToString());

		//            double average = 0;

		//            try
		//            {
		//                if (type == Enumerators.StatisticsType.DataUsage)
		//                    average = rows.Average(x => x.Field<int>("PacketSize"));
		//                else
		//                    average = rows.Average(x => x.Field<int>("Hits"));

		//                switch (period)
		//                {
		//                    case Enumerators.Period.Week:
		//                        average /= 7;
		//                        break;

		//                    case Enumerators.Period.Month:
		//                        average /= 30;
		//                        break;
		//                }

		//                DataRow row = result.NewRow();

		//                row["SerialId"] = i;
		//                row["Average"] = Math.Round(average, 5);

		//                result.Rows.Add(row);
		//            }
		//            catch (InvalidOperationException)
		//            {
		//                //NOP
		//            }
		//        }
		//    }

		//    return result;
		//}

		///// <summary>
		///// Gets the devices that exceed a specified threshold for a period.
		///// </summary>
		///// <param name="type">The type of threshold to get.</param>
		///// <param name="period">The period to calculate.</param>
		///// <returns>The devices meeting the specified criteria.</returns>
		//public DataTable GetDevicesOverThreshold(Enumerators.StatisticsType type, Enumerators.Period period)
		//{
		//    return GetDevicesOverThreshold(type, period, (DataFactory)null);
		//}

		///// <summary>
		///// Gets the devices that exceed a specified threshold for a period.
		///// </summary>
		///// <param name="type">The type of threshold to get.</param>
		///// <param name="period">The period to calculate.</param>
		///// <param name="dataFactory">The data to use for testing.</param>
		///// <returns>The devices meeting the specified criteria.</returns>
		//public DataTable GetDevicesOverThreshold(Enumerators.StatisticsType type, Enumerators.Period period, DataFactory dataFactory)
		//{
		//    DataTable result = null;

		//    DataFactory df = new DataFactory();

		//    if (dataFactory != null)
		//        df = dataFactory;

		//    DataTable temp = null;

		//    if (type == Enumerators.StatisticsType.DataUsage)
		//    {
		//        this.eventPacket = df.GetNewEventPacket();
		//        temp = this.eventPacket.GetPacketsByDateRange(this.startDate, this.endDate);
		//    }
		//    else
		//    {
		//        this.gpsData = df.GetNewGpsData();
		//        temp = this.gpsData.GetHitsByDateRange(this.startDate, this.endDate);
		//    }

		//    if (temp != null)
		//    {
		//        result = new DataTable();

		//        DataRow[] rows = new DataRow[temp.Rows.Count];

		//        temp.Rows.CopyTo(rows, 0);

		//        List<DataRow> filter = null;

		//        List<int> ids = null;

		//        try
		//        {
		//            if (period != Enumerators.Period.Day)
		//            {
		//                if (type == Enumerators.StatisticsType.DataUsage)
		//                    ids = (from r in rows
		//                           select r.Field<int>("SerialId")).Distinct().ToList();
		//                else
		//                    ids = (from r in rows
		//                           select r.Field<int>("lSerial_ID")).Distinct().ToList();
		//            }

		//            switch (period)
		//            {
		//                case Enumerators.Period.Day:
		//                    result.Columns.Add("SerialId", typeof(int));
		//                    result.Columns.Add("ReceivedDate", typeof(DateTime));
		//                    result.Columns.Add("Count", typeof(int));

		//                    try
		//                    {
		//                        if (type == Enumerators.StatisticsType.DataUsage && this.dataUsageThreshold > -1)
		//                            filter = rows.Where(x => x.Field<int>("PacketSize") >= this.dataUsageThreshold).ToList<DataRow>();
		//                        else if (this.afltHitThreshold > -1)
		//                            filter = rows.Where(x => x.Field<int>("Hits") >= this.afltHitThreshold).ToList<DataRow>();
		//                    }
		//                    catch (InvalidOperationException)
		//                    {
		//                        //NOP
		//                    }
		//                    break;

		//                case Enumerators.Period.Week:
		//                    result.Columns.Add("SerialId", typeof(int));
		//                    result.Columns.Add("WeeklyCount", typeof(int));

		//                    foreach (int id in ids)
		//                    {
		//                        int sum = 0;

		//                        try
		//                        {
		//                            if (type == Enumerators.StatisticsType.DataUsage)
		//                            {
		//                                sum = (from f in rows
		//                                       where f.Field<int>("SerialId") == id
		//                                       select f.Field<int>("PacketSize")).Sum();

		//                                if (sum >= this.dataUsageThreshold)
		//                                {
		//                                    DataRow row = result.NewRow();
		//                                    row["SerialId"] = id;
		//                                    row["WeeklyCount"] = sum;

		//                                    if (filter == null)
		//                                        filter = new List<DataRow>();

		//                                    filter.Add(row);
		//                                }
		//                            }
		//                            else
		//                            {
		//                                sum = (from f in rows
		//                                       where f.Field<int>("lSerial_ID") == id
		//                                       select f.Field<int>("Hits")).Sum();

		//                                if (sum >= this.afltHitThreshold)
		//                                {
		//                                    DataRow row = result.NewRow();
		//                                    row["SerialId"] = id;
		//                                    row["WeeklyCount"] = sum;

		//                                    if (filter == null)
		//                                        filter = new List<DataRow>();

		//                                    filter.Add(row);
		//                                }
		//                            }
		//                        }
		//                        catch (InvalidOperationException)
		//                        {
		//                            //NOP
		//                        }
		//                    }

		//                    break;

		//                case Enumerators.Period.Month:
		//                    result.Columns.Add("SerialId", typeof(int));
		//                    result.Columns.Add("MonthlyCount", typeof(int));

		//                    foreach (int id in ids)
		//                    {
		//                        int sum = 0;

		//                        try
		//                        {
		//                            if (type == Enumerators.StatisticsType.DataUsage)
		//                            {
		//                                sum = (from f in rows
		//                                       where f.Field<int>("SerialId") == id
		//                                       select f.Field<int>("PacketSize")).Sum();

		//                                if (sum >= this.dataUsageThreshold)
		//                                {
		//                                    DataRow row = result.NewRow();
		//                                    row["SerialId"] = id;
		//                                    row["MonthlyCount"] = sum;

		//                                    if (filter == null)
		//                                        filter = new List<DataRow>();

		//                                    filter.Add(row);
		//                                }
		//                            }
		//                            else
		//                            {
		//                                sum = (from f in rows
		//                                       where f.Field<int>("lSerial_ID") == id
		//                                       select f.Field<int>("Hits")).Sum();

		//                                if (sum >= this.afltHitThreshold)
		//                                {
		//                                    DataRow row = result.NewRow();
		//                                    row["SerialId"] = id;
		//                                    row["MonthlyCount"] = sum;

		//                                    if (filter == null)
		//                                        filter = new List<DataRow>();

		//                                    filter.Add(row);
		//                                }
		//                            }
		//                        }
		//                        catch (InvalidOperationException)
		//                        {
		//                            //NOP
		//                        }
		//                    }
		//                    break;
		//            }
		//        }
		//        catch (InvalidOperationException)
		//        {
		//            //NOP
		//        }

		//        result.Rows.Clear();

		//        if (filter != null)
		//        {
		//            foreach (DataRow dr in filter)
		//            {
		//                result.NewRow();

		//                switch (period)
		//                {
		//                    case Enumerators.Period.Day:
		//                        if (type == Enumerators.StatisticsType.DataUsage)
		//                            result.Rows.Add(dr["SerialId"], dr["ReceivedDate"], dr["PacketSize"]);
		//                        else
		//                            result.Rows.Add(dr["lSerial_ID"], dr["Received"], dr["Hits"]);
		//                        break;

		//                    case Enumerators.Period.Month:
		//                        result.Rows.Add(dr["SerialId"], dr["MonthlyCount"]);
		//                        break;

		//                    case Enumerators.Period.Week:
		//                        result.Rows.Add(dr["SerialId"], dr["WeeklyCount"]);
		//                        break;
		//                }
		//            }
		//        }
		//    }

		//    return result;
		//}

		/// <summary>
		/// Gets the data usage or AFLT hits for the period.
		/// </summary>
		/// <param name="type">The type of count to get.</param>
		/// <returns>The devices meeting the specified criteria.</returns>
		public DataTable GetData(Enumerators.StatisticsType type)
		{
			return GetData(type, (DataFactory)null);
		}

		/// <summary>
		/// Gets the data usage or AFLT hits for the period.
		/// </summary>
		/// <param name="type">The type of count to get.</param>
		/// <param name="dataFactory">The data to use for testing.</param>
		/// <returns>The devices meeting the specified criteria.</returns>
		public DataTable GetData(Enumerators.StatisticsType type, DataFactory dataFactory)
		{
			DataTable result = null;

			DataFactory df = new DataFactory();

			if (dataFactory != null)
				df = dataFactory;

			DataTable temp = null;

			try
			{
				if (type == Enumerators.StatisticsType.DataUsage)
				{
					this.eventPacket = df.GetNewEventPacket();
					temp = this.eventPacket.GetPacketsByDateRange(this.startDate, this.endDate);
				}
				else
				{
					this.gpsData = df.GetNewGpsData();
					temp = this.gpsData.GetHitsByDateRange(this.startDate, this.endDate);
				}

				if (temp != null)
				{
					result = new DataTable();

					result.Columns.Add("SerialId", typeof(int));
					result.Columns.Add("ReceivedDate", typeof(DateTime));
					result.Columns.Add("Count", typeof(int));

					DataRow[] rows = new DataRow[temp.Rows.Count];

					temp.Rows.CopyTo(rows, 0);

					int max = 0;
					List<DataRow> filter = new List<DataRow>();

					if (type == Enumerators.StatisticsType.DataUsage)
					{
						max = (from r in rows
							   select r.Field<int>("PacketSize")).Max();

						filter = (from r in rows
								  where r.Field<int>("PacketSize") == max
								  select r).ToList();
					}
					else
					{
						max = (from r in rows
							   select r.Field<int>("Hits")).Max();

						filter = (from r in rows
								  where r.Field<int>("Hits") == max
								  select r).ToList();
					}

					foreach (DataRow row in filter)
					{
						result.NewRow();

						if (type == Enumerators.StatisticsType.DataUsage)
							result.Rows.Add(row["SerialId"], row["ReceivedDate"], row["PacketSize"]);
						else
							result.Rows.Add(row["lSerial_ID"], row["Received"], row["Hits"]);
					}
				}
			}
			finally
			{
				temp.Dispose();
			}

			return result;
		}

		///// <summary>
		///// Gets the devices with the lowest data usage or AFLT hits for the period.
		///// </summary>
		///// <param name="type">The type of count to get.</param>
		///// <param name="period">The period to calculate.</param>
		///// <returns>The devices meeting the specified criteria.</returns>
		//public DataTable GetLowest(Enumerators.StatisticsType type)
		//{
		//    return GetLowest(type, (DataFactory)null);
		//}

		///// <summary>
		///// Gets the devices with the lowest data usage or AFLT hits for the period.
		///// </summary>
		///// <param name="type">The type of count to get.</param>
		///// <param name="period">The period to calculate.</param>
		///// <param name="dataFactory">The data to use for testing.</param>
		///// <returns>The devices meeting the specified criteria.</returns>
		//public DataTable GetLowest(Enumerators.StatisticsType type, DataFactory dataFactory)
		//{
		//    DataTable result = null;

		//    DataFactory df = new DataFactory();

		//    if (dataFactory != null)
		//        df = dataFactory;

		//    DataTable temp = null;

		//    try
		//    {
		//        if (type == Enumerators.StatisticsType.DataUsage)
		//        {
		//            this.eventPacket = df.GetNewEventPacket();
		//            temp = this.eventPacket.GetPacketsByDateRange(this.startDate, this.endDate);
		//        }
		//        else
		//        {
		//            this.gpsData = df.GetNewGpsData();
		//            temp = this.gpsData.GetHitsByDateRange(this.startDate, this.endDate);
		//        }

		//        if (temp != null)
		//        {
		//            result = new DataTable();

		//            result.Columns.Add("SerialId", typeof(int));
		//            result.Columns.Add("ReceivedDate", typeof(DateTime));
		//            result.Columns.Add("Count", typeof(int));

		//            DataRow[] rows = new DataRow[temp.Rows.Count];

		//            temp.Rows.CopyTo(rows, 0);

		//            int min = 0;
		//            List<DataRow> filter = new List<DataRow>();

		//            if (type == Enumerators.StatisticsType.DataUsage)
		//            {
		//                min = (from r in rows
		//                       select r.Field<int>("PacketSize")).Min();

		//                filter = (from r in rows
		//                          where r.Field<int>("PacketSize") == min
		//                          select r).ToList();
		//            }
		//            else
		//            {
		//                min = (from r in rows
		//                       select r.Field<int>("Hits")).Min();

		//                filter = (from r in rows
		//                          where r.Field<int>("Hits") == min
		//                          select r).ToList();
		//            }

		//            foreach (DataRow row in filter)
		//            {
		//                result.NewRow();

		//                if (type == Enumerators.StatisticsType.DataUsage)
		//                    result.Rows.Add(row["SerialId"], row["ReceivedDate"], row["PacketSize"]);
		//                else
		//                    result.Rows.Add(row["lSerial_ID"], row["Received"], row["Hits"]);
		//            }
		//        }
		//    }
		//    finally
		//    {
		//        temp.Dispose();
		//    }

		//    return result;
		//}

		///// <summary>
		///// Gets the standard deviation for the specified period.
		///// </summary>
		///// <param name="type">The type of standard deviation to get.</param>
		///// <param name="period">The period to calculate.</param>
		///// <returns>The standard deviation.</returns>
		//public double GetStandardDeviation(Enumerators.StatisticsType type, Enumerators.Period period)
		//{
		//    return GetStandardDeviation(type, period, (DataFactory)null);
		//}

		///// <summary>
		///// Gets the standard deviation for the specified period.
		///// </summary>
		///// <param name="type">The type of standard deviation to get.</param>
		///// <param name="period">The period to calculate.</param>
		///// <returns>The standard deviation.</returns>
		//public double GetStandardDeviation(Enumerators.StatisticsType type, Enumerators.Period period, DataFactory dataFactory)
		//{
		//    double result = 0;

		//    DataFactory df = new DataFactory();

		//    if (dataFactory != null)
		//        df = dataFactory;

		//    DataTable temp = null;

		//    try
		//    {
		//        if (type == Enumerators.StatisticsType.DataUsage)
		//        {
		//            this.eventPacket = df.GetNewEventPacket();
		//            temp = this.eventPacket.GetPacketsByDateRange(this.startDate, this.endDate);
		//        }
		//        else
		//        {
		//            this.gpsData = df.GetNewGpsData();
		//            temp = this.gpsData.GetHitsByDateRange(this.startDate, this.endDate);
		//        }

		//        if(temp.Rows.Count > 0)
		//        {
		//            DataRow[] rows = new DataRow[temp.Rows.Count];

		//            temp.Rows.CopyTo(rows, 0);

		//            List<DateTime> dates = new List<DateTime>();

		//            if (type == Enumerators.StatisticsType.DataUsage)
		//                dates = (from r in rows
		//                         select r.Field<DateTime>("ReceivedDate")).Distinct().ToList();
		//            else
		//                dates = (from r in rows
		//                         select r.Field<DateTime>("Received")).Distinct().ToList();

		//            List<double> averages = new List<double>();

		//            foreach (DateTime d in dates)
		//            {
		//                double average = 0;

		//                if (type == Enumerators.StatisticsType.DataUsage)
		//                    average = (from r in rows
		//                               where r.Field<DateTime>("ReceivedDate") == d
		//                               select r.Field<int>("PacketSize")).Average();
		//                else
		//                    average = (from r in rows
		//                               where r.Field<DateTime>("Received") == d
		//                               select r.Field<int>("Hits")).Average();

		//                averages.Add(average);
		//            }

		//            result = Statistics.CalculateStandardDeviation(averages);

		//            if (period == Enumerators.Period.Month)
		//                result /= 30d;
		//            else if (period == Enumerators.Period.Week)
		//                result /= 7d;
		//        }
		//    }
		//    finally
		//    {
		//        temp.Dispose();
		//    }

		//    return Math.Round(result, 5);
		//}

		/// <summary>
		/// Separates the input serial IDs and expands their range if necessary.
		/// </summary>
		/// <returns>The serial IDs as a list.</returns>
		private List<string> ParseSerialIds()
		{
			List<string> result = new List<string>();

			this.serialIds = (this.serialIds + "").Trim();

			if (this.serialIds.Contains(","))
			{
				string[] temp = this.serialIds.Split(new char[] { ',' });

				foreach (string s in temp)
				{
					if (s.Contains("-"))
						result.AddRange(ExpandRange(s));
					else
						result.Add(s);
				}
			}
			else if (this.serialIds.Contains("-"))
				result.AddRange(ExpandRange(this.serialIds));
			else
				result.Add(this.serialIds);

			return result;
		}
		#endregion
	}
}