using MetaQuotes.MT5CommonAPI;
using MetaQuotes.MT5ManagerAPI;
using System;
using System.Collections.Generic;


namespace MT5DealFeed
{
    class MTDataFeed
    {
        static CManager manager = null;
        static Logger logger = null;

        public static void InitLogger(Logger log)
        {
            logger = log;
        }

        public static bool Start()
        {
            try
            {
                manager = new CManager();
                return manager.Start(logger);
            }
            catch
            {
                return false;
            }
        }

        public static bool Login(string server, ulong login, string password)
        {
            try
            {
                return manager.Login(server, login, password);
            }
            catch
            {
                return false;
            }
        }

        public static void Logout()
        {
            try
            {
                manager.Logout();
            }
            catch
            {
                return;
            }
        }

        public static void Stop()
        {
            try
            {
                manager.Stop();
            }
            catch
            {
                return;
            }
        }
    }

    class CManager : CIMTManagerSink
    {
        CIMTManagerAPI m_manager = null;
        CDeal cdealObj = null;
        Logger logger = null;

        public bool Start(Logger log)
        {
            try
            {
                logger = log;
                cdealObj = new CDeal();
                string message = string.Empty;
                MTRetCode res = MTRetCode.MT_RET_OK_NONE;
                if ((res = SMTManagerAPIFactory.Initialize(null)) != MTRetCode.MT_RET_OK)
                {
                    message = string.Format("Loading manager API failed ({0})", res);
                    logger.Write(LogLevel.Debug, message);
                    return false;
                }
                m_manager = SMTManagerAPIFactory.CreateManager(SMTManagerAPIFactory.ManagerAPIVersion, out res);
                if ((res != MTRetCode.MT_RET_OK) || (m_manager == null))
                {
                    SMTManagerAPIFactory.Shutdown();
                    message = string.Format("Creating manager interface failed ({0})", (res == MTRetCode.MT_RET_OK ? "Managed API is null" : res.ToString()));
                    logger.Write(LogLevel.Debug, message);
                    return false;
                }
                if ((res = RegisterSink()) != MTRetCode.MT_RET_OK)
                {
                    Stop();
                    message = string.Format("Manager sink native link failed ({0})", res);
                    logger.Write(LogLevel.Debug, message);
                    return false;
                }
                if ((res = m_manager.Subscribe(this)) != MTRetCode.MT_RET_OK)
                {
                    Stop();
                    message = string.Format("Manager event subscribtion failed ({0})", res);
                    logger.Write(LogLevel.Debug, message);
                    return false;
                }
                cdealObj.Initialize(logger);
                return true;
            }
            catch(Exception e)
            {
                logger.Write(LogLevel.Debug, $"Exception Occured at MTDataFeed.Start(): {e.Message}");
                return false;
            }
        }

        public bool Login(string server, ulong login, string password)
        {
            try
            {
                MTRetCode res = m_manager.Connect(server, login, password, null, CIMTManagerAPI.EnPumpModes.PUMP_MODE_FULL, 30000);
                if (res != MTRetCode.MT_RET_OK)
                {
                    logger.Write(LogLevel.Debug, $"Connection failed ({res})");
                    return false;
                }
                if ((res = m_manager.DealSubscribe(cdealObj)) != MTRetCode.MT_RET_OK)
                {
                    logger.Write(LogLevel.Debug, $"Real-time data subscribe failed ({res})");
                    return false;
                }
                logger.Write(LogLevel.Debug, "Connection Successful");
                return true;
            }
            catch(Exception e)
            {
                logger.Write(LogLevel.Debug, $"Exception Occured at MTDataFeed.Login(): {e.Message}");
                return false;
            }
        }

        public void Logout()
        {
            if (m_manager != null)
            {
                m_manager.Unsubscribe(this);
                m_manager.DealUnsubscribe(cdealObj);
                m_manager.Disconnect();
            }
        }

        public void Stop()
        {

            if (m_manager != null)
            {
                m_manager.Dispose();
                m_manager = null;
            }
            SMTManagerAPIFactory.Shutdown();
        }

        public override void OnConnect()
        {
            logger.Write(LogLevel.Debug, "Connection established to server");
            logger.NotifyServerStatus(MTServerStatus.Connected);
        }

        public override void OnDisconnect()
        {
            logger.Write(LogLevel.Debug, "Connection lost to server");
            logger.NotifyServerStatus(MTServerStatus.Disconnected);
        }
    }

    class CDeal : CIMTDealSink
    {
        Logger logger = null;
        public MTRetCode Initialize(Logger log)
        {
            logger = log;
            return RegisterSink();
        }

        public override void OnDealAdd(CIMTDeal deal)
        {
            if (deal != null)
            {
                DealData dealData = new DealData();
                dealData.Login = (long)deal.Login();
                dealData.DealNo = (long)deal.Deal();
                dealData.Time = DateTimeOffset.FromUnixTimeSeconds(deal.Time()).DateTime.ToString("yyyy'-'mm'-'dd hh:mm:ss.fff");
                dealData.Symbol = deal.Symbol();
                dealData.Type = ((DealType)deal.Action()).ToString();
                dealData.Volume = deal.Volume() / 10000;
                dealData.Price = deal.Price();
                dealData.ContractSize = deal.ContractSize();
                dealData.Commission = deal.Commission();
                dealData.Profit = deal.Profit();
                dealData.Comment = deal.Comment();
                DBIO.WriteData(new List<DealData>() { dealData });
                logger.Write(LogLevel.Debug, $"Deal added ID: {dealData.DealNo}");
                logger.NotifyDealAdded(dealData.DealNo);
            }
        }
    }
}
