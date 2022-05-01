using Microsoft.Win32;
using System;
using System.Configuration;

namespace MT5DealFeed
{
    class Helper
    {
        static string provider = "System.Data.SqlClient";
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
        }

        public static void SetConnectionString(string name, string connectionString)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.ConnectionStrings.ConnectionStrings[name] != null) config.ConnectionStrings.ConnectionStrings[name].ConnectionString = connectionString;
            else config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(name, connectionString, provider));
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static void RemoveConnectionString(string name)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.ConnectionStrings.ConnectionStrings[name] != null)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(name);
            }
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? "";
        }

        public static void SetAppSetting(string key, string value)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] != null) config.AppSettings.Settings[key].Value = value;
            else config.AppSettings.Settings.Add(key, value);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static bool CheckInstallation()
        {
            try
            {
                string regKey = "SOFTWARE\\Microsoft\\MTDataFeed";
                var key = Registry.LocalMachine.OpenSubKey(regKey, RegistryKeyPermissionCheck.ReadSubTree);
                if (key == null) return false;
                var installed = key.GetValue("Installed");
                if (installed == null) return false;
                return (int)installed == 1;
            }
            catch
            {
                return false;
            }
        }
    }
}
