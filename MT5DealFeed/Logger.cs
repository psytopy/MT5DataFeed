using System;
using System.IO;

namespace MT5DealFeed
{
    class Logger
    {
        string logfile = "";
        int level = 0;

        public delegate void ServerStatusEventHandler(MTServerStatus status);
        public delegate void DealAddedEventHandler(long dealno);
        public event ServerStatusEventHandler ServerStatusChanged;
        public event DealAddedEventHandler DealAdded;

        public Logger(string filename)
        {
            logfile = filename;
        }

        public void SetLevel(LogLevel level)
        {
            this.level = (int)level;
        }

        public void Write(LogLevel level, string data)
        {
            try
            {
                if ((int)level <= this.level) File.AppendAllText(logfile, $"[{level}]\t{DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss\.fff")}\t{data} {Environment.NewLine}");
            }
            catch
            {
                return;
            }
        }

        public void NotifyServerStatus(MTServerStatus status)
        {
            ServerStatusChanged?.Invoke(status);
        }

        public void NotifyDealAdded(long dealno)
        {
            DealAdded?.Invoke(dealno);
        }
    }

    enum MTServerStatus
    {
        Connected,
        Disconnected
    }

    enum LogLevel
    {
        Critical,
        Error,
        Info,
        Debug,
        Trace
    }
}
