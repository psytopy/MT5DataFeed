using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MT5DealFeed
{
    class DBIO
    {
        static string dbname = "";
        static string instance = "";
        static Logger logger;

        public static bool CheckConfiguration(Logger log)
        {
            logger = log;
            logger.Write(LogLevel.Debug, "Attempting configuration check");
            try
            {
                instance = Helper.GetAppSetting("instance");
                if (String.IsNullOrWhiteSpace(instance)) instance = ".";
                else instance = $".\\{instance}";
                dbname = Helper.GetAppSetting("db");
                if (String.IsNullOrEmpty(dbname)) return false;
                //var mstr = Helper.GetConnectionString("master");
                //if (String.IsNullOrEmpty(mstr))
                Helper.SetConnectionString("master", $"Data Source={instance};Database=master;Trusted_Connection=True;");
                //var cstr = Helper.GetConnectionString(dbname);
                //if (String.IsNullOrEmpty(cstr))
                Helper.SetConnectionString(dbname, $"Data Source={instance};Database={dbname};Trusted_Connection=True;");
                return true;
            }
            catch (Exception e)
            {
                logger.Write(LogLevel.Debug, $"Initial configuration failed. Exception: {e.Message}");
                return false;
            }
        }

        public static bool CheckServerConnectivity(string connectionString)
        {
            logger.Write(LogLevel.Debug, $"Attempting SQL server connectivity check.");
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
                logger.Write(LogLevel.Debug, "SQL server connectivity check passed successfully");
                return true;
            }
            catch (Exception e)
            {
                logger.Write(LogLevel.Debug, $"SQL server connectivity check failed. Exception: {e.Message}");
                return false;
            }
        }

        public static int CheckDatabaseValidity()
        {
            logger.Write(LogLevel.Debug, $"Attempting database validity check.");
            try
            {
                string sql = "SELECT TOP 1 * FROM Deals";
                using (IDbConnection conn = new SqlConnection(Helper.GetConnectionString(dbname)))
                {
                    var data = conn.Query(sql);
                }
                logger.Write(LogLevel.Debug, "Database validity check passed successfully");
                return 0;
            }
            catch (Exception e)
            {
                if (e is SqlException se)
                {

                    if (se.Number == 4060)
                    {
                        logger.Write(LogLevel.Debug, "Database not found");
                        return 1;
                    }
                    logger.Write(LogLevel.Debug, "Database found. Checking Validity");
                    if (se.Number == 208)
                    {
                        logger.Write(LogLevel.Debug, "Database not configured properly");
                        return 2;
                    }
                    logger.Write(LogLevel.Debug, $"Database check error. Exception No. {se.Number} Exception: {e.Message}");
                }

                logger.Write(LogLevel.Debug, $"Database check error. Exception: {e.Message}");
                return 3;
            }
        }

        public static bool CreateDatabase()
        {
            try
            {
                if (String.IsNullOrEmpty(dbname)) return false;
                logger.Write(LogLevel.Debug, $"Attempting SQL Create Database {dbname}");
                string sql = $"CREATE DATABASE {dbname}";
                using (IDbConnection conn = new SqlConnection(Helper.GetConnectionString("master")))
                {
                    conn.Execute(sql);
                }
                logger.Write(LogLevel.Debug, "SQL Create Database successful");
                return true;
            }
            catch (Exception e)
            {
                logger.Write(LogLevel.Debug, $"SQL Create Database failed. {e.Message}");
                return false;
            }
        }

        public static bool SetupDatabase()
        {
            try
            {
                if (String.IsNullOrEmpty(dbname)) return false;
                logger.Write(LogLevel.Debug, $"Attempting SQL Create Table Deals");
                using (IDbConnection conn = new SqlConnection(Helper.GetConnectionString(dbname)))
                {
                    string sql = "CREATE TABLE \"Deals\" (" +
                                 "\"ID\" INT NOT NULL IDENTITY PRIMARY KEY," +
                                 "\"Login\" INT NOT NULL," +
                                 "\"DealNo\" INT NOT NULL," +
                                 "\"Time\" VARCHAR(30) NOT NULL," +
                                 "\"Symbol\" VARCHAR(30) NOT NULL," +
                                 "\"Type\" VARCHAR(10) NOT NULL," +
                                 "\"Volume\" FLOAT NOT NULL," +
                                 "\"ContractSize\" FLOAT NOT NULL," +
                                 "\"Price\" FLOAT NOT NULL," +
                                 "\"Commission\" FLOAT NOT NULL," +
                                 "\"Profit\" FLOAT NOT NULL," +
                                 "\"Comment\" VARCHAR(100) NOT NULL)";
                    conn.Execute(sql);
                }
                logger.Write(LogLevel.Debug, "SQL Create Table successful");
                return true;
            }
            catch (Exception e)
            {
                logger.Write(LogLevel.Debug, $"SQL Create Table failed. {e.Message}");
                return false;
            }
        }

        public static bool WriteData(List<DealData> dealDatas)
        {
            try
            {
                logger.Write(LogLevel.Debug, $"SQL Attempting INSERT INTO Database");
                using (IDbConnection conn = new SqlConnection(Helper.GetConnectionString(dbname)))
                {
                    string sql = $"INSERT INTO Deals(Login, DealNo, Time, Symbol, Type, Volume, ContractSize, Price, Commission, Profit, Comment) VALUES(@Login, @DealNo, @Time, @Symbol, @Type, @Volume, @ContractSize, @Price, @Commission, @Profit, @Comment)";
                    foreach (var data in dealDatas) conn.Execute(sql, data);
                }
                logger.Write(LogLevel.Debug, "SQL INSERT INTO Database successful");
                return true;
            }
            catch (Exception e)
            {
                logger.Write(LogLevel.Debug, $"SQL INSERT INTO Database failed. {e.Message}");
                return false;
            }
        }
    }
}
