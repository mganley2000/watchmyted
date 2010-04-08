using System;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.IO;
using System.IO.Ports;

namespace Energy.Library
{
    /// <summary>
    /// This controller class handles all the access to the SQLite database
    /// Stores and Reads configuration, rates, readings, etc.
    /// To dump sqlite database from a command line use 
    /// sqlite WMYTED.s3db .dump > output.sql
    /// </summary>
    public class DatabaseController
    {
        private const string DATABASE_SQLITE = "Database.SQLite";
        private const string DATABASE_VALID_CHECK = "SELECT VeNumber FROM Version";

        #region "Init"

        public static bool IsDatabaseValid()
        {
            bool initValid = false;
            string version = string.Empty;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = DATABASE_VALID_CHECK;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["VeNumber"] != DBNull.Value)
                    {
                        version = reader["VeNumber"].ToString();
                        initValid = true;
                    }
                    else
                    {
                        initValid = false;
                    }

                }

                conn.Close();
            }
            catch
            {
                // do not throw; a database has been created
                initValid = false;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (initValid);
        }

        /// <summary>
        /// Only use when a new database needs to be created
        /// </summary>
        public static int InitializeDatabase()
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            int rowCount;
            StringBuilder sb = new StringBuilder();

            try
            {
                StreamReader sr = new StreamReader(@"dbInit.txt");
                sb.Append(sr.ReadToEnd());
                sr.Close();

                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }
        #endregion


        #region "Meters"

        public static bool DoesMeterIDExist(int meterid)
        {
            bool exists = false;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader;
            const string sql = "SELECT MeID FROM Meter WHERE MeID = {0}";

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(sql, meterid);

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    exists = true;
                }
                else
                {
                    exists = false;
                }
                reader.Close();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return (exists);
        }

        public static int InsertMeter(Meter meter)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            int rowCount;
            StringBuilder sb = new StringBuilder();

            sb.Append("INSERT INTO Meter (MeID, MeMtID, MeName, MeDescription) ");
            sb.Append("VALUES ");
            sb.AppendFormat("( {0}, {1}, '{2}', '{3}' ) ", meter.ID, (int)meter.Type, meter.Name, meter.Description);

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int UpdateMeter(Meter meter)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            int rowCount;
            StringBuilder sb = new StringBuilder();

            sb.Append("UPDATE Meter SET ");
            sb.Append("MeName = @name,");
            sb.Append("MeDescription = @desc");
            sb.Append("WHERE MeID = @meterID ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.Parameters.AddWithValue("@name", meter.Name);
                cmd.Parameters.AddWithValue("@desc", meter.Description);
                cmd.Parameters.AddWithValue("@meterID", meter.ID);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }


        public static MeterCollection GetMeters()
        {
            MeterCollection meters = new MeterCollection();
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();
            Meter meter;

            sb.Append("SELECT MeID, MeMtID, MeName, MeDescription FROM Meter ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    meter = new Meter();
                    if (reader["MeID"] != DBNull.Value) { meter.ID = (int)((long)reader["MeID"]); }
                    if (reader["MeMtID"] != DBNull.Value) { meter.Type = (Enums.MeterType)((long)reader["MeMtID"]); }
                    if (reader["MeName"] != DBNull.Value) { meter.Name = reader["MeName"].ToString(); }
                    if (reader["MeDescription"] != DBNull.Value) { meter.Description = reader["MeDescription"].ToString(); }
                    meters.Add(meter);
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (meters);
        }

        public static Meter GetMeter(int meterid)
        {
            Meter meter = null;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT MeID, MeMtID, MeName, MeDescription FROM Meter WHERE MeID = @meterid ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.Parameters.AddWithValue("@meterid", meterid);

                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    meter = new Meter();
                    if (reader["MeID"] != DBNull.Value) { meter.ID = (int)((long)reader["MeID"]); }
                    if (reader["MeMtID"] != DBNull.Value) { meter.Type = (Enums.MeterType)((long)reader["MeMtID"]); }
                    if (reader["MeName"] != DBNull.Value) { meter.Name = reader["MeName"].ToString(); }
                    if (reader["MeDescription"] != DBNull.Value) { meter.Description = reader["MeDescription"].ToString(); }
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (meter);
        }

        #endregion

        #region "Configuration"

        public static Configuration GetConfiguration()
        {
            Configuration config = new Configuration();
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT CnName, CnDescription, CnCOMPort, CnBaud, ");
            sb.Append("CnParity, CnDataBits, CnStopBits, CnSelectedRate, ");
            sb.Append("CnTEDUrl1, CnTEDUrl2, CnTEDUrl3, CnGAEReadingsServiceUrl, CnGAEGmail, CnGAEPassword, CnTEDMTU0Ind, ");
            sb.Append("CnGAESiteURL, CnGAEAppName ");
            sb.Append("FROM Configuration WHERE CnName = 'default' ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["CnName"] != DBNull.Value) { config.Name = reader["CnName"].ToString(); }
                    if (reader["CnDescription"] != DBNull.Value) { config.Description = reader["CnDescription"].ToString(); }
                    if (reader["CnCOMPort"] != DBNull.Value) { config.COMPort = reader["CnCOMPort"].ToString(); }
                    if (reader["CnBaud"] != DBNull.Value) { config.Baud = (int)((long)reader["CnBaud"]); }
                    if (reader["CnParity"] != DBNull.Value) { config.Parity = (Parity)((long)reader["CnParity"]); }
                    if (reader["CnDataBits"] != DBNull.Value) { config.DataBits = (int)((long)reader["CnDataBits"]); }
                    if (reader["CnStopBits"] != DBNull.Value) { config.StopBits = (int)((long)reader["CnStopBits"]); }
                    if (reader["CnSelectedRate"] != DBNull.Value)
                    {
                        config.RateID = (int)((long)reader["CnSelectedRate"]);
                    }
                    else
                    {
                        config.RateID = 0;
                    }
                    if (reader["CnTEDMTU0Ind"] != DBNull.Value) { config.TEDMTU0Enabled = (bool)reader["CnTEDMTU0Ind"]; }
                    if (reader["CnTEDUrl1"] != DBNull.Value) { config.TEDUrl1 = reader["CnTEDUrl1"].ToString(); }
                    if (reader["CnTEDUrl2"] != DBNull.Value) { config.TEDUrl2 = reader["CnTEDUrl2"].ToString(); }
                    if (reader["CnTEDUrl3"] != DBNull.Value) { config.TEDUrl3 = reader["CnTEDUrl3"].ToString(); }
                    if (reader["CnGAEReadingsServiceUrl"] != DBNull.Value) { config.GAEReadingsServiceUrl = reader["CnGAEReadingsServiceUrl"].ToString(); }
                    if (reader["CnGAEGmail"] != DBNull.Value) { config.GAEGmail = reader["CnGAEGmail"].ToString(); }
                    if (reader["CnGAEPassword"] != DBNull.Value) { config.GAEPassword = reader["CnGAEPassword"].ToString(); }
                    if (reader["CnGAESiteURL"] != DBNull.Value) { config.GAESiteUrl = reader["CnGAESiteURL"].ToString(); }
                    if (reader["CnGAEAppName"] != DBNull.Value) { config.GAEAppName = reader["CnGAEAppName"].ToString(); }

                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (config);
        }

        public static int UpdateGAEConfiguration(string sid, Configuration config)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            StringBuilder sb = new StringBuilder();
            int rowCount;

            sb.Append("UPDATE Configuration SET ");
            sb.Append("CnGAEReadingsServiceUrl = @CnGAEReadingsServiceUrl,");
            sb.Append("CnGAEGmail = @CnGAEGmail,");
            sb.Append("CnGAEPassword = @CnGAEPassword,");
            sb.Append("CnGAESiteUrl = @CnGAESiteUrl,");
            sb.Append("CnGAEAppName = @CnGAEAppName ");
            sb.Append("WHERE CnName = @sid ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.Parameters.AddWithValue("@CnGAEReadingsServiceUrl", config.GAEReadingsServiceUrl);
                cmd.Parameters.AddWithValue("@CnGAEGmail", config.GAEGmail);
                cmd.Parameters.AddWithValue("@CnGAEPassword", config.GAEPassword);
                cmd.Parameters.AddWithValue("@CnGAESiteUrl", config.GAESiteUrl);
                cmd.Parameters.AddWithValue("@CnGAEAppName", config.GAEAppName);
                cmd.Parameters.AddWithValue("@sid", sid);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int UpdateTEDConfiguration(string sid, Configuration config)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            StringBuilder sb = new StringBuilder();
            int rowCount;

            sb.Append("UPDATE Configuration SET ");
            sb.Append("CnTEDMTU0Ind = @tedMtu0Enabled, ");
            sb.Append("CnTEDUrl1 = @tedURL1,");
            sb.Append("CnTEDUrl2 = @tedURL2,");
            sb.Append("CnTEDUrl3 = @tedURL3 ");
            sb.Append("WHERE CnName = @sid ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
          
                cmd.Parameters.AddWithValue("@tedMtu0Enabled", config.TEDMTU0Enabled);
                cmd.Parameters.AddWithValue("@tedURL1", config.TEDUrl1);
                cmd.Parameters.AddWithValue("@tedURL2", config.TEDUrl2);
                cmd.Parameters.AddWithValue("@tedURL3", config.TEDUrl3);
                cmd.Parameters.AddWithValue("@sid", sid);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int UpdatePortConfiguration(string sid, Configuration config)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            int rowCount;
            StringBuilder sb = new StringBuilder();

            sb.Append("UPDATE Configuration SET ");
            sb.AppendFormat("CnCOMPort = '{0}',", config.COMPort);
            sb.AppendFormat("CnBaud = {0},", config.Baud);
            sb.AppendFormat("CnParity = {0},", (int)config.Parity);
            sb.AppendFormat("CnDataBits = {0},", config.DataBits);
            sb.AppendFormat("CnStopBits = {0} ", config.StopBits);
            sb.AppendFormat("WHERE CnName = '{0}' ", sid);

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int UpdateSelectedRateConfiguration(string sid, int rateID)
        {
            Configuration config = new Configuration();
            config.RateID = rateID;
            return (UpdateSelectedRateConfiguration(sid, config));
        }

        public static int UpdateSelectedRateConfiguration(string sid, Configuration config)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            int rowCount;
            StringBuilder sb = new StringBuilder();

            sb.Append("UPDATE Configuration SET CnSelectedRate = @rateID ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.Parameters.AddWithValue("@rateID", config.RateID);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        #endregion

        #region "Readings"

        public static int InsertReading(int meterid, Enums.Interval interval, Enums.UnitOfMeasure uom, DateTime timestamp, double energyConsumption)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            int rowCount;
            StringBuilder sb = new StringBuilder();
            string formatDate;

            formatDate = timestamp.ToString("yyyy/MM/dd HH:mm:ss");

            sb.Append("INSERT INTO Reading (JaMeID, JaInID, JaUnID, JaDatestamp, JaRead) ");
            sb.Append("VALUES ");
            sb.AppendFormat("( @meterID, @interval, @uom, @timestamp, @energyConsumption )");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.Parameters.AddWithValue("@meterID", meterid);
                cmd.Parameters.AddWithValue("@interval", (int)interval);
                cmd.Parameters.AddWithValue("@uom", (int)uom);
                cmd.Parameters.AddWithValue("@timestamp", formatDate);
                cmd.Parameters.AddWithValue("@energyConsumption", energyConsumption);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static ReadingCollection GetReadings(int meterid, Enums.Interval interval, Enums.UnitOfMeasure uom, string startDate, string endDate)
        {
            ReadingCollection readings = new ReadingCollection();
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();
            Reading r;

            sb.Append("SELECT JaInID, JaUnID, JaDatestamp, JaRead ");
            sb.Append("FROM Reading WHERE JaMeID = @meterID AND JaInID = @interval AND JaUnID = @uom ");
            sb.Append("AND JaDatestamp >= @startDate AND JaDatestamp <= @endDate");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.Parameters.AddWithValue("@meterID", meterid);
                cmd.Parameters.AddWithValue("@interval", (int)interval);
                cmd.Parameters.AddWithValue("@uom", (int)uom);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    r = new Reading();
                    if (reader["JaInID"] != DBNull.Value) { r.DataInterval = (Enums.Interval)((long)reader["JaInID"]); }
                    if (reader["JaUnID"] != DBNull.Value) { r.Units = (Enums.UnitOfMeasure)((long)reader["JaUnID"]); }
                    if (reader["JaDatestamp"] != DBNull.Value) { r.Datestamp = DateTime.Parse(reader["JaDatestamp"].ToString()); }
                    if (reader["JaRead"] != DBNull.Value) { r.Quantity = (double)(reader["JaRead"]); }
                    readings.Add(r);
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (readings);
        }

        public static int DeleteReading(int meterid, Enums.Interval interval, Enums.UnitOfMeasure uom, DateTime timestamp)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            int rowCount;
            StringBuilder sb = new StringBuilder();
            string formatDate;

            formatDate = timestamp.ToString("yyyy/MM/dd HH:mm:ss");

            sb.Append("DELETE FROM Reading WHERE JaMeID = @meterID AND JaInID = @interval AND JaUnID = @uom AND JaDatestamp = @timestamp ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.Parameters.AddWithValue("@meterID", meterid);
                cmd.Parameters.AddWithValue("@interval", (int)interval);
                cmd.Parameters.AddWithValue("@uom", (int)uom);
                cmd.Parameters.AddWithValue("@timestamp", formatDate);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static bool DoesReadingExist(int meterid, Enums.Interval interval, Enums.UnitOfMeasure uom, DateTime timestamp)
        {
            bool exists = false;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();
            string formatDate;

            formatDate = timestamp.ToString("yyyy/MM/dd HH:mm:ss");

            sb.Append("SELECT JaMeID FROM Reading WHERE JaMeID = @meterID AND JaInID = @interval AND JaUnID = @uom AND JaDatestamp = @timestamp");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.Parameters.AddWithValue("@meterID", meterid);
                cmd.Parameters.AddWithValue("@interval", (int)interval);
                cmd.Parameters.AddWithValue("@uom", (int)uom);
                cmd.Parameters.AddWithValue("@timestamp", formatDate);

                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    exists = true;
                }
                else
                {
                    exists = false;
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (exists);
        }

        #endregion

        #region "Rate Information"

        public static bool DoesRateExist(int rateid)
        {
            bool exists = false;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT RaID FROM Rate WHERE RaID = @rateid");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.Parameters.AddWithValue("@rateid", rateid);

                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    exists = true;
                }
                else
                {
                    exists = false;
                }

                conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (exists);
        }

        public static int GetTopRateID()
        {
            int rateid = -1;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();

            sb.Append("select RaID from Rate Order By RaID limit 1");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["RaID"] != DBNull.Value) { rateid = (int)((long)reader["RaID"]); }
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (rateid);
        }

        public static Rate GetRate(int rateid)
        {
            Rate rate = null;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM Rate WHERE RaID = @rateid ");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.Parameters.AddWithValue("@rateid", rateid);

                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rate = new Rate();
                    if (reader["RaID"] != DBNull.Value) { rate.ID = (int)((long)reader["RaID"]); }
                    if (reader["RaName"] != DBNull.Value) { rate.Name = (string)reader["RaName"]; }
                    if (reader["RaDescription"] != DBNull.Value) { rate.Description = (string)reader["RaDescription"]; }
                    if (reader["RaSeasonalInd"] != DBNull.Value) { rate.IsSeasonal = (bool)reader["RaSeasonalInd"]; }
                    if (reader["RaBasicChargesInd"] != DBNull.Value) { rate.HasBasicCharges = (bool)reader["RaBasicChargesInd"]; }
                    if (reader["RaTimeOfUseChargesInd"] != DBNull.Value) { rate.HasTimeOfUseCharges = (bool)reader["RaTimeOfUseChargesInd"]; }
                    if (reader["RaCurrentInd"] != DBNull.Value) { rate.IsCurrentlySelectedRate = (bool)reader["RaCurrentInd"]; }
                    if (reader["RaBasicCharge"] != DBNull.Value) { rate.BasicCharge = (double)reader["RaBasicCharge"]; }
                    if (reader["RaSummerBasicCharge"] != DBNull.Value) { rate.SummerBasicCharge = (double)reader["RaSummerBasicCharge"]; }
                    if (reader["RaWinterBasicCharge"] != DBNull.Value) { rate.WinterBasicCharge = (double)reader["RaWinterBasicCharge"]; }

                    // weekday charges
                    if (reader["RaPeakCharge"] != DBNull.Value) { rate.PeakCharge = (double)reader["RaPeakCharge"]; }
                    if (reader["RaOffPeakCharge"] != DBNull.Value) { rate.OffPeakCharge = (double)reader["RaOffPeakCharge"]; }
                    if (reader["RaPeakStartHour"] != DBNull.Value) { rate.PeakStartHour = (int)((long)reader["RaPeakStartHour"]); }
                    if (reader["RaPeakEndHour"] != DBNull.Value) { rate.PeakEndHour = (int)((long)reader["RaPeakEndHour"]); }

                    if (reader["RaSummerPeakCharge"] != DBNull.Value) { rate.SummerPeakCharge = (double)reader["RaSummerPeakCharge"]; }
                    if (reader["RaSummerOffPeakCharge"] != DBNull.Value) { rate.SummerOffPeakCharge = (double)reader["RaSummerOffPeakCharge"]; }
                    if (reader["RaSummerPeakStartHour"] != DBNull.Value) { rate.SummerPeakStartHour = (int)((long)reader["RaSummerPeakStartHour"]); }
                    if (reader["RaSummerPeakEndHour"] != DBNull.Value) { rate.SummerPeakEndHour = (int)((long)reader["RaSummerPeakEndHour"]); }

                    if (reader["RaWinterPeakCharge"] != DBNull.Value) { rate.WinterPeakCharge = (double)reader["RaWinterPeakCharge"]; }
                    if (reader["RaWinterOffPeakCharge"] != DBNull.Value) { rate.WinterOffPeakCharge = (double)reader["RaWinterOffPeakCharge"]; }
                    if (reader["RaWinterPeakStartHour"] != DBNull.Value) { rate.WinterPeakStartHour = (int)((long)reader["RaWinterPeakStartHour"]); }
                    if (reader["RaWinterPeakEndHour"] != DBNull.Value) { rate.WinterPeakEndHour = (int)((long)reader["RaWinterPeakEndHour"]); }

                    //weekend charges
                    if (reader["RaPeakWeekendCharge"] != DBNull.Value) { rate.PeakWeekendCharge = (double)reader["RaPeakWeekendCharge"]; }
                    if (reader["RaOffPeakWeekendCharge"] != DBNull.Value) { rate.OffPeakWeekendCharge = (double)reader["RaOffPeakWeekendCharge"]; }
                    if (reader["RaSummerPeakWeekendCharge"] != DBNull.Value) { rate.SummerPeakWeekendCharge = (double)reader["RaSummerPeakWeekendCharge"]; }
                    if (reader["RaSummerOffPeakWeekendCharge"] != DBNull.Value) { rate.SummerOffPeakWeekendCharge = (double)reader["RaSummerOffPeakWeekendCharge"]; }
                    if (reader["RaWinterPeakWeekendCharge"] != DBNull.Value) { rate.WinterPeakWeekendCharge = (double)reader["RaWinterPeakWeekendCharge"]; }
                    if (reader["RaWinterOffPeakWeekendCharge"] != DBNull.Value) { rate.WinterOffPeakWeekendCharge = (double)reader["RaWinterOffPeakWeekendCharge"]; }

                    if (reader["RaPeakWeekendStartHour"] != DBNull.Value) { rate.PeakWeekendStartHour = (int)((long)reader["RaPeakWeekendStartHour"]); }
                    if (reader["RaPeakWeekendEndHour"] != DBNull.Value) { rate.PeakWeekendEndHour = (int)((long)reader["RaPeakWeekendEndHour"]); }
                    if (reader["RaSummerPeakWeekendStartHour"] != DBNull.Value) { rate.SummerPeakWeekendStartHour = (int)((long)reader["RaSummerPeakWeekendStartHour"]); }
                    if (reader["RaSummerPeakWeekendEndHour"] != DBNull.Value) { rate.SummerPeakWeekendEndHour = (int)((long)reader["RaSummerPeakWeekendEndHour"]); }
                    if (reader["RaWinterPeakWeekendStartHour"] != DBNull.Value) { rate.WinterPeakWeekendStartHour = (int)((long)reader["RaWinterPeakWeekendStartHour"]); }
                    if (reader["RaWinterPeakWeekendEndHour"] != DBNull.Value) { rate.WinterPeakWeekendEndHour = (int)((long)reader["RaWinterPeakWeekendEndHour"]); }

                    if (reader["RaSummerStartDate"] != DBNull.Value) { rate.SummerStartMonth = (string)reader["RaSummerStartDate"]; }
                    if (reader["RaSummerEndDate"] != DBNull.Value) { rate.SummerEndMonth = (string)reader["RaSummerEndDate"]; }
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (rate);
        }

        public static int InsertRate()
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            StringBuilder sb = new StringBuilder();
            int rowCount;

            sb.Append("INSERT INTO Rate (RaName, RaDescription, ");
            sb.Append("RaSeasonalInd, RaBasicChargesInd, RaTimeOfUseChargesInd, RaCurrentInd, RaSummerStartDate, RaSummerEndDate ) ");
            sb.Append("VALUES ");
            sb.Append("( @RaName, @RaDescription, ");
            sb.Append("@RaSeasonalInd, @RaBasicChargesInd, @RaTimeOfUseChargesInd, @RaCurrentInd, @RaSummerStartDate, @RaSummerEndDate )");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.Parameters.AddWithValue("@RaName", "default");
                cmd.Parameters.AddWithValue("@RaDescription", "default");
                cmd.Parameters.AddWithValue("@RaSeasonalInd", false);
                cmd.Parameters.AddWithValue("@RaBasicChargesInd", true);
                cmd.Parameters.AddWithValue("@RaTimeOfUseChargesInd", false);
                cmd.Parameters.AddWithValue("@RaCurrentInd", true);
                cmd.Parameters.AddWithValue("@RaSummerStartDate", "June 1");
                cmd.Parameters.AddWithValue("@RaSummerEndDate", "October 31");

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int InsertRate(Rate rate)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            StringBuilder sb = new StringBuilder();
            int rowCount;

            sb.Append("INSERT INTO Rate (RaName, RaDescription, ");
            sb.Append("RaSeasonalInd, RaBasicChargesInd, RaTimeOfUseChargesInd, RaCurrentInd, ");
            sb.Append("RaBasicCharge, RaSummerBasicCharge, RaWinterBasicCharge, ");
            sb.Append("RaPeakCharge, RaOffPeakCharge, RaPeakStartHour, RaPeakEndHour, ");
            sb.Append("RaPeakWeekendCharge, RaOffPeakWeekendCharge, RaPeakWeekendStartHour, RaPeakWeekendEndHour, ");
            sb.Append("RaSummerPeakCharge, RaSummerOffPeakCharge, RaSummerPeakStartHour, RaSummerPeakEndHour, ");
            sb.Append("RaWinterPeakCharge, RaWinterOffPeakCharge, RaWinterPeakStartHour, RaWinterPeakEndHour, ");
            sb.Append("RaSummerPeakWeekendCharge, RaSummerOffPeakWeekendCharge, RaSummerPeakWeekendStartHour, RaSummerPeakWeekendEndHour, ");
            sb.Append("RaWinterPeakWeekendCharge, RaWinterOffPeakWeekendCharge, RaWinterPeakWeekendStartHour, RaWinterPeakWeekendEndHour, ");
            sb.Append("RaSummerStartDate, RaSummerEndDate) ");
            sb.Append("VALUES ");
            sb.Append("( @RaName, @RaDescription, ");
            sb.Append("@RaSeasonalInd, @RaBasicChargesInd, @RaTimeOfUseChargesInd, @RaCurrentInd, ");
            sb.Append("@RaBasicCharge, @RaSummerBasicCharge, @RaWinterBasicCharge, ");
            sb.Append("@RaPeakCharge, @RaOffPeakCharge, @RaPeakStartHour, @RaPeakEndHour, ");
            sb.Append("@RaPeakWeekendCharge, @RaOffPeakWeekendCharge, @RaPeakWeekendStartHour, @RaPeakWeekendEndHour, ");
            sb.Append("@RaSummerPeakCharge, @RaSummerOffPeakCharge, @RaSummerPeakStartHour, @RaSummerPeakEndHour, ");
            sb.Append("@RaWinterPeakCharge, @RaWinterOffPeakCharge, @RaWinterPeakStartHour, @RaWinterPeakEndHour, ");
            sb.Append("@RaSummerPeakWeekendCharge, @RaSummerOffPeakWeekendCharge, @RaSummerPeakWeekendStartHour, @RaSummerPeakWeekendEndHour, ");
            sb.Append("@RaWinterPeakWeekendCharge, @RaWinterOffPeakWeekendCharge, @RaWinterPeakWeekendStartHour, @RaWinterPeakWeekendEndHour, ");
            sb.Append("@RaSummerStartDate, @RaSummerEndDate)");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.Parameters.AddWithValue("@RaName", rate.Name);
                cmd.Parameters.AddWithValue("@RaDescription", rate.Description);
                cmd.Parameters.AddWithValue("@RaSeasonalInd", rate.IsSeasonal);
                cmd.Parameters.AddWithValue("@RaBasicChargesInd", rate.HasBasicCharges);
                cmd.Parameters.AddWithValue("@RaTimeOfUseChargesInd", rate.HasTimeOfUseCharges);
                cmd.Parameters.AddWithValue("@RaCurrentInd", rate.IsCurrentlySelectedRate);
                cmd.Parameters.AddWithValue("@RaBasicCharge", rate.BasicCharge);
                cmd.Parameters.AddWithValue("@RaSummerBasicCharge", rate.SummerBasicCharge);
                cmd.Parameters.AddWithValue("@RaWinterBasicCharge", rate.WinterBasicCharge);
                cmd.Parameters.AddWithValue("@RaPeakCharge", rate.PeakCharge);
                cmd.Parameters.AddWithValue("@RaOffPeakCharge", rate.OffPeakCharge);
                cmd.Parameters.AddWithValue("@RaPeakStartHour", rate.PeakStartHour);
                cmd.Parameters.AddWithValue("@RaPeakEndHour", rate.PeakEndHour);
                cmd.Parameters.AddWithValue("@RaPeakWeekendCharge", rate.PeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaOffPeakWeekendCharge", rate.OffPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaPeakWeekendStartHour", rate.PeakWeekendStartHour);
                cmd.Parameters.AddWithValue("@RaPeakWeekendEndHour", rate.PeakWeekendEndHour);
                cmd.Parameters.AddWithValue("@RaSummerPeakCharge", rate.SummerPeakCharge);
                cmd.Parameters.AddWithValue("@RaSummerOffPeakCharge", rate.SummerOffPeakCharge);
                cmd.Parameters.AddWithValue("@RaSummerPeakStartHour", rate.SummerPeakStartHour);
                cmd.Parameters.AddWithValue("@RaSummerPeakEndHour", rate.SummerPeakEndHour);
                cmd.Parameters.AddWithValue("@RaWinterPeakCharge", rate.WinterPeakCharge);
                cmd.Parameters.AddWithValue("@RaWinterOffPeakCharge", rate.WinterOffPeakCharge);
                cmd.Parameters.AddWithValue("@RaWinterPeakStartHour", rate.WinterPeakStartHour);
                cmd.Parameters.AddWithValue("@RaWinterPeakEndHour", rate.WinterPeakEndHour);
                cmd.Parameters.AddWithValue("@RaSummerPeakWeekendCharge", rate.SummerPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaSummerOffPeakWeekendCharge", rate.SummerOffPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaSummerPeakWeekendStartHour", rate.SummerPeakWeekendStartHour);
                cmd.Parameters.AddWithValue("@RaSummerPeakWeekendEndHour", rate.SummerPeakWeekendEndHour);
                cmd.Parameters.AddWithValue("@RaWinterPeakWeekendCharge", rate.WinterPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaWinterOffPeakWeekendCharge", rate.WinterOffPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaWinterPeakWeekendStartHour", rate.WinterPeakWeekendStartHour);
                cmd.Parameters.AddWithValue("@RaWinterPeakWeekendEndHour", rate.WinterPeakWeekendEndHour);
                cmd.Parameters.AddWithValue("@RaSummerStartDate", rate.SummerStartMonth);
                cmd.Parameters.AddWithValue("@RaSummerEndDate", rate.SummerEndMonth);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int UpdateRate(Rate rate)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            StringBuilder sb = new StringBuilder();
            int rowCount;

            sb.Append("UPDATE Rate SET ");
            sb.Append("RaName = @RaName, ");
            sb.Append("RaDescription = @RaDescription, ");
            sb.Append("RaSeasonalInd = @RaSeasonalInd, ");
            sb.Append("RaBasicChargesInd = @RaBasicChargesInd, ");
            sb.Append("RaTimeOfUseChargesInd = @RaTimeOfUseChargesInd, ");
            sb.Append("RaCurrentInd = @RaCurrentInd, ");
            sb.Append("RaBasicCharge = @RaBasicCharge, ");
            sb.Append("RaSummerBasicCharge = @RaSummerBasicCharge, ");
            sb.Append("RaWinterBasicCharge = @RaWinterBasicCharge, ");
            sb.Append("RaPeakCharge = @RaPeakCharge, ");
            sb.Append("RaOffPeakCharge = @RaOffPeakCharge, ");
            sb.Append("RaPeakWeekendCharge = @RaPeakWeekendCharge, ");
            sb.Append("RaOffPeakWeekendCharge = @RaOffPeakWeekendCharge, ");
            sb.Append("RaPeakStartHour = @RaPeakStartHour, ");
            sb.Append("RaPeakEndHour = @RaPeakEndHour, ");
            sb.Append("RaPeakWeekendStartHour = @RaPeakWeekendStartHour, ");
            sb.Append("RaPeakWeekendEndHour = @RaPeakWeekendEndHour, ");
            sb.Append("RaSummerPeakCharge = @RaSummerPeakCharge, ");
            sb.Append("RaSummerOffPeakCharge = @RaSummerOffPeakCharge, ");
            sb.Append("RaSummerPeakStartHour = @RaSummerPeakStartHour, ");
            sb.Append("RaSummerPeakEndHour = @RaSummerPeakEndHour, ");
            sb.Append("RaWinterPeakCharge = @RaWinterPeakCharge, ");
            sb.Append("RaWinterOffPeakCharge = @RaWinterOffPeakCharge, ");
            sb.Append("RaWinterPeakStartHour = @RaWinterPeakStartHour, ");
            sb.Append("RaWinterPeakEndHour = @RaWinterPeakEndHour, ");
            sb.Append("RaSummerPeakWeekendCharge = @RaSummerPeakWeekendCharge, ");
            sb.Append("RaSummerOffPeakWeekendCharge = @RaSummerOffPeakWeekendCharge, ");
            sb.Append("RaSummerPeakWeekendStartHour = @RaSummerPeakWeekendStartHour, ");
            sb.Append("RaSummerPeakWeekendEndHour = @RaSummerPeakWeekendEndHour, ");
            sb.Append("RaWinterPeakWeekendCharge = @RaWinterPeakWeekendCharge, ");
            sb.Append("RaWinterOffPeakWeekendCharge = @RaWinterOffPeakWeekendCharge, ");
            sb.Append("RaWinterPeakWeekendStartHour = @RaWinterPeakWeekendStartHour, ");
            sb.Append("RaWinterPeakWeekendEndHour = @RaWinterPeakWeekendEndHour, ");
            sb.Append("RaSummerStartDate = @RaSummerStartDate, ");
            sb.Append("RaSummerEndDate = @RaSummerEndDate ");
            sb.Append("WHERE RaID = @rateid");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);

                cmd.Parameters.AddWithValue("@rateid", rate.ID);
                cmd.Parameters.AddWithValue("@RaName", rate.Name);
                cmd.Parameters.AddWithValue("@RaDescription", rate.Description);
                cmd.Parameters.AddWithValue("@RaSeasonalInd", rate.IsSeasonal);
                cmd.Parameters.AddWithValue("@RaBasicChargesInd", rate.HasBasicCharges);
                cmd.Parameters.AddWithValue("@RaTimeOfUseChargesInd", rate.HasTimeOfUseCharges);
                cmd.Parameters.AddWithValue("@RaCurrentInd", rate.IsCurrentlySelectedRate);
                cmd.Parameters.AddWithValue("@RaBasicCharge", rate.BasicCharge);
                cmd.Parameters.AddWithValue("@RaSummerBasicCharge", rate.SummerBasicCharge);
                cmd.Parameters.AddWithValue("@RaWinterBasicCharge", rate.WinterBasicCharge);
                cmd.Parameters.AddWithValue("@RaPeakCharge", rate.PeakCharge);
                cmd.Parameters.AddWithValue("@RaOffPeakCharge", rate.OffPeakCharge);
                cmd.Parameters.AddWithValue("@RaPeakWeekendCharge", rate.PeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaOffPeakWeekendCharge", rate.OffPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaPeakStartHour", rate.PeakStartHour);
                cmd.Parameters.AddWithValue("@RaPeakEndHour", rate.PeakEndHour);
                cmd.Parameters.AddWithValue("@RaPeakWeekendStartHour", rate.PeakWeekendStartHour);
                cmd.Parameters.AddWithValue("@RaPeakWeekendEndHour", rate.PeakWeekendEndHour);
                cmd.Parameters.AddWithValue("@RaSummerPeakCharge", rate.SummerPeakCharge);
                cmd.Parameters.AddWithValue("@RaSummerOffPeakCharge", rate.SummerOffPeakCharge);
                cmd.Parameters.AddWithValue("@RaSummerPeakStartHour", rate.SummerPeakStartHour);
                cmd.Parameters.AddWithValue("@RaSummerPeakEndHour", rate.SummerPeakEndHour);
                cmd.Parameters.AddWithValue("@RaWinterPeakCharge", rate.WinterPeakCharge);
                cmd.Parameters.AddWithValue("@RaWinterOffPeakCharge", rate.WinterOffPeakCharge);
                cmd.Parameters.AddWithValue("@RaWinterPeakStartHour", rate.WinterPeakStartHour);
                cmd.Parameters.AddWithValue("@RaWinterPeakEndHour", rate.WinterPeakEndHour);
                cmd.Parameters.AddWithValue("@RaSummerPeakWeekendCharge", rate.SummerPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaSummerOffPeakWeekendCharge", rate.SummerOffPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaSummerPeakWeekendStartHour", rate.SummerPeakWeekendStartHour);
                cmd.Parameters.AddWithValue("@RaSummerPeakWeekendEndHour", rate.SummerPeakWeekendEndHour);
                cmd.Parameters.AddWithValue("@RaWinterPeakWeekendCharge", rate.WinterPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaWinterOffPeakWeekendCharge", rate.WinterOffPeakWeekendCharge);
                cmd.Parameters.AddWithValue("@RaWinterPeakWeekendStartHour", rate.WinterPeakWeekendStartHour);
                cmd.Parameters.AddWithValue("@RaWinterPeakWeekendEndHour", rate.WinterPeakWeekendEndHour);
                cmd.Parameters.AddWithValue("@RaSummerStartDate", rate.SummerStartMonth);
                cmd.Parameters.AddWithValue("@RaSummerEndDate", rate.SummerEndMonth);

                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int UpdateRateAsOnlyCurrentRate(int rateID)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            int rowCount;

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = "UPDATE Rate SET RaCurrentInd = 0";
                rowCount = cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE Rate SET RaCurrentInd = 1 WHERE RaID = " + rateID.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int DeleteRate(Rate rate)
        {
            SQLiteConnection conn;
            SQLiteCommand cmd;
            StringBuilder sb = new StringBuilder();
            int rowCount;

            sb.Append("DELETE FROM Rate WHERE RaID = @rateid");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.Parameters.AddWithValue("@rateid", rate.ID);
                cmd.CommandText = sb.ToString();
                rowCount = cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {
                throw;
            }

            return (rowCount);
        }

        public static int GetRateCount()
        {
            int count = 0;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();

            sb.Append("Select count(RaID) As Count FROM Rate");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["Count"] != DBNull.Value) { count = (int)((long)reader["Count"]); }
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (count);
        }

        #endregion


        #region "Dropdown Selectors"

        public static DataTable GetSelectChart()
        {
            DataTable list;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            int id = 0;
            string name = string.Empty;

            list = new DataTable();
            list.Columns.Add(new DataColumn("Display", typeof(string)));
            list.Columns.Add(new DataColumn("Id", typeof(int)));

            sb.Append("SELECT ScID, ScName FROM SelectChart ORDER BY ScSortOrder");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["ScID"] != DBNull.Value) { id = (int)((long)reader["ScID"]); }
                    if (reader["ScName"] != DBNull.Value) { name = reader["ScName"].ToString(); }

                    list.Rows.Add(list.NewRow());
                    list.Rows[i][0] = name;
                    list.Rows[i][1] = id;
                    i++;
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (list);
        }

        public static DataTable GetSelectDate()
        {
            DataTable list;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            int id = 0;
            string name = string.Empty;

            list = new DataTable();
            list.Columns.Add(new DataColumn("Display", typeof(string)));
            list.Columns.Add(new DataColumn("Id", typeof(int)));

            sb.Append("SELECT SdID, SdName FROM SelectDate ORDER BY SdSortOrder");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["SdID"] != DBNull.Value) { id = (int)((long)reader["SdID"]); }
                    if (reader["SdName"] != DBNull.Value) { name = reader["SdName"].ToString(); }

                    list.Rows.Add(list.NewRow());
                    list.Rows[i][0] = name;
                    list.Rows[i][1] = id;
                    i++;
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (list);
        }

        public static DataTable GetSelectTime()
        {
            DataTable list;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            int id = 0;
            string name = string.Empty;

            list = new DataTable();
            list.Columns.Add(new DataColumn("Display", typeof(string)));
            list.Columns.Add(new DataColumn("Id", typeof(int)));

            sb.Append("SELECT StID, StName FROM SelectTime ORDER BY StSortOrder");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["StID"] != DBNull.Value) { id = (int)((long)reader["StID"]); }
                    if (reader["StName"] != DBNull.Value) { name = reader["StName"].ToString(); }

                    list.Rows.Add(list.NewRow());
                    list.Rows[i][0] = name;
                    list.Rows[i][1] = id;
                    i++;
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (list);
        }

        public static DataTable GetRateList()
        {
            DataTable list;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();
            int i = 0;
            int id = 0;
            string name = string.Empty;

            list = new DataTable();
            list.Columns.Add(new DataColumn("Display", typeof(string)));
            list.Columns.Add(new DataColumn("Id", typeof(int)));

            sb.Append("SELECT RaID, RaName FROM Rate ORDER BY RaID");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["RaID"] != DBNull.Value) { id = (int)((long)reader["RaID"]); }
                    if (reader["RaName"] != DBNull.Value) { name = reader["RaName"].ToString(); }

                    list.Rows.Add(list.NewRow());
                    list.Rows[i][0] = name;
                    list.Rows[i][1] = id;
                    i++;
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (list);
        }

        #endregion


        #region "Helpers"

        private static SQLiteConnection GetConnection()
        {
            SQLiteConnection conn;
            string connString;

            connString = ConfigurationManager.AppSettings[DATABASE_SQLITE].ToString();
            conn = new SQLiteConnection(connString);

            return (conn);
        }

        public static int GetLastInsertedID(string tableName)
        {
            int seq = -1;
            SQLiteConnection conn;
            SQLiteCommand cmd;
            SQLiteDataReader reader = null;
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM sqlite_sequence WHERE name = @name");

            try
            {
                conn = GetConnection();
                conn.Open();

                cmd = new SQLiteCommand(conn);
                cmd.Parameters.AddWithValue("@name", tableName);
                cmd.CommandText = sb.ToString();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (reader["seq"] != DBNull.Value) { seq = (int)((long)reader["seq"]); }
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }

            return (seq);
        }

        #endregion


    }
}
