using System;
using System.Globalization;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MT5DealFeed
{
    public partial class MainForm : Form
    {
        Logger logger;
        bool userlogout = false, isretrying = true;
        int dealaddednum = 0, historydealaddednum = 0;
        int retries = 10;
        int retrygap = 10;
        public MainForm()
        {
            InitializeComponent();
            logger = new Logger("applog.log");
            logger.ServerStatusChanged += ServerStatusChanged;
            logger.DealAdded += DealAdded;
            logger.SetLevel(LogLevel.Debug);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Enabled = false;
            BeginInvoke((MethodInvoker)InitLoading);
        }

        private async void button_login_Click(object sender, EventArgs e)
        {
            ulong id = 0;
            if (!UInt64.TryParse(textBox_login.Text, out id))
            {
                MessageBox.Show("Invalid Login!");
                return;
            }
            if (!MTDataFeed.Start())
            {
                MessageBox.Show("Cannot initialize API");
                return;
            }
            label_stat_conn_stat.Text = "Connecting";
            label_stat_conn_stat.ForeColor = Color.FromArgb(Int32.Parse("92970C", NumberStyles.HexNumber));
            button_login.Enabled = false;
            button_db_confirm.Enabled = false;
            button_logout.Enabled = true;
            textBox_login.Enabled = false;
            textBox_password.Enabled = false;
            textBox_server.Enabled = false;
            var task = Task.Run(() => { return MTDataFeed.Login(textBox_server.Text, id, textBox_password.Text); });
            await task;
            if (!task.Result)
            {
                label_stat_conn_stat.Text = "Disconnected";
                label_stat_conn_stat.ForeColor = Color.Red;
                button_login.Enabled = true;
                button_db_confirm.Enabled = true;
                button_logout.Enabled = false;
                textBox_login.Enabled = true;
                textBox_password.Enabled = true;
                textBox_server.Enabled = true;
                MessageBox.Show("Cannot login to the server");
                return;
            }
            userlogout = false;
            dealaddednum = 0;
            label_stat_conn_stat.Text = "Connected";
            label_stat_conn_stat.ForeColor = Color.Green;
            label_deal_no.Text = dealaddednum.ToString();
            button_gethistory.Enabled = true;
        }

        private void button_logout_Click(object sender, EventArgs e)
        {
            userlogout = true;
            MTDataFeed.Logout();
            MTDataFeed.Stop();
            label_stat_conn_stat.Text = "Disconnected";
            label_stat_conn_stat.ForeColor = Color.Red;
            label_activity_status.Text = $"Disconnected on {DateTime.Now.ToString("HH:mm:ss.fff")}";
            label_activity_status.ForeColor = Color.Red;
            button_logout.Enabled = false;
            button_login.Enabled = true;
            button_db_confirm.Enabled = true;
            button_gethistory.Enabled = false;
            textBox_login.Enabled = true;
            textBox_password.Enabled = true;
            textBox_server.Enabled = true;
        }

        private void button_db_confirm_Click(object sender, EventArgs e)
        {
            if (button_db_confirm.Text == "Edit")
            {
                textBox_db_name.Enabled = true;
                button_db_confirm.Text = "Confirm";

                DisableLoginControls();

                label_stat_db_stat.Text = "Not Configured";
                label_stat_db_stat.ForeColor = Color.Red;
            }
            else if (button_db_confirm.Text == "Confirm")
            {
                Helper.SetAppSetting("db", textBox_db_name.Text);
                logger.Write(LogLevel.Info, "Database configuration updated");
                DBIO.CheckConfiguration(logger);
                logger.Write(LogLevel.Info, "Checking database validity");
                var res = DBIO.CheckDatabaseValidity();
                if (res != 0)
                {
                    if (res == 1)
                    {
                        logger.Write(LogLevel.Info, "Database does not exist. Attempting to create");
                        if (!DBIO.CreateDatabase())
                        {
                            logger.Write(LogLevel.Error, "Could not create database");
                            RollBackConfig();
                            DisabledMode();
                            textBox_db_name.Text = "";
                            MessageBox.Show("Could not setup the database.");
                            return;
                        }
                        logger.Write(LogLevel.Info, "Database creation successful");
                        res = 2;
                        Thread.Sleep(5000);
                    }
                    if (res == 2)
                    {
                        logger.Write(LogLevel.Debug, "Database found. Attempting configuration of db");
                        if (!DBIO.SetupDatabase())
                        {
                            logger.Write(LogLevel.Error, "Cannot create database tables");
                            RollBackConfig();
                            DisabledMode();
                            textBox_db_name.Text = "";
                            MessageBox.Show("Could not setup the database.");
                            return;
                        }
                    }
                    if (res == 3)
                    {
                        logger.Write(LogLevel.Error, "Unknown error occured during database configuration");
                        RollBackConfig();
                        DisabledMode();
                        textBox_db_name.Text = "";
                        MessageBox.Show("Could not setup the database.");
                        return;
                    }
                    logger.Write(LogLevel.Debug, "Database successfully configured");
                }

                logger.Write(LogLevel.Info, "Database is valid");
                textBox_db_name.Enabled = false;
                button_db_confirm.Text = "Edit";

                EnableLoginControls();

                label_stat_db_stat.Text = "Configured";
                label_stat_db_stat.ForeColor = Color.Green;
            }
        }

        #region Added By Avia To Get History
        private void button_gethistory_Click(object sender, EventArgs e)
        {
            historydealaddednum = 0;
            label_history_deal_no.Text = historydealaddednum.ToString();
            DateTime from = datebox_form.Value;
            DateTime to = datebox_to.Value;
            ulong id = 0;
            if (!UInt64.TryParse(textBox_login.Text, out id))
            {
                MessageBox.Show("Invalid Login!");
                return;
            }
            if (from >= to)
            {
                MessageBox.Show("From date must be less than To date!");
                return;
            }
            Task.Run(() => { MTDataFeed.GetHistory(id, from, to); }).ContinueWith((x)=> { button_gethistory.Invoke((MethodInvoker)delegate { MessageBox.Show("Fetching Complete!"); }); });
        }
        #endregion

        private void DisableLoginControls()
        {
            button_login.Enabled = false;
            button_logout.Enabled = false;
            button_gethistory.Enabled = false;
        }

        private void EnableLoginControls()
        {
            button_login.Enabled = true;
            button_logout.Enabled = false;
            button_gethistory.Enabled = false;
        }

        private void DisabledMode()
        {
            DisableLoginControls();
            label_stat_db_stat.Text = "Not Configured";
            label_stat_db_stat.ForeColor = Color.Red;

            label_stat_conn_stat.Text = "Disconnected";
            label_stat_conn_stat.ForeColor = Color.Red;

            textBox_db_name.Enabled = true;
            button_db_confirm.Text = "Confirm";
        }

        private void RollBackConfig()
        {
            logger.Write(LogLevel.Info, "Rolling back configurations");
            var dbname = Helper.GetAppSetting("db");
            Helper.SetAppSetting("db", "");
            Helper.RemoveConnectionString(dbname);
            logger.Write(LogLevel.Info, "Rollback Complete");
        }

        private void InitLoading()
        {
            MTDataFeed.InitLogger(logger);
            DisabledMode();
            logger.Write(LogLevel.Info, "App Startup");
            logger.Write(LogLevel.Info, "Database Server Instance \"SQLEXPRESS\"");
            if (!DBIO.CheckConfiguration(logger))
            {
                logger.Write(LogLevel.Info, "Database configuration not found");
                DisabledMode();
            }
            else
            {
                logger.Write(LogLevel.Info, "Database configuration found");
                logger.Write(LogLevel.Info, "Database Validity Check");
                var res = DBIO.CheckDatabaseValidity();
                if (res != 0)
                {
                    if (res == 1)
                    {
                        logger.Write(LogLevel.Info, "Database does not exist. Attempting to create");
                        if (!DBIO.CreateDatabase())
                        {
                            logger.Write(LogLevel.Error, "Could not create database");
                            RollBackConfig();
                            DisabledMode();
                            textBox_db_name.Text = "";
                            Enabled = true;
                            return;
                        }
                        else
                        {
                            logger.Write(LogLevel.Debug, "Database creation successful.");
                            res = 2;
                            Thread.Sleep(5000);
                        }
                    }
                    if (res == 2)
                    {
                        logger.Write(LogLevel.Debug, "Database found. Attempting configuration of db");
                        if (!DBIO.SetupDatabase())
                        {
                            logger.Write(LogLevel.Error, "Error configuring db");
                            RollBackConfig();
                            DisabledMode();
                            textBox_db_name.Text = "";
                            Enabled = true;
                            return;
                        }
                    }
                    if (res == 3)
                    {
                        logger.Write(LogLevel.Debug, "Unknown error occured during database configuration");
                        RollBackConfig();
                        DisabledMode();
                        textBox_db_name.Text = "";
                        Enabled = true;
                        return;
                    }
                    logger.Write(LogLevel.Debug, "Database configuration successful");

                    EnableLoginControls();
                    label_stat_db_stat.Text = "Configured";
                    label_stat_db_stat.ForeColor = Color.Green;

                    textBox_db_name.Enabled = false;
                    textBox_db_name.Text = Helper.GetAppSetting("db");
                    button_db_confirm.Text = "Edit";
                }
                else
                {
                    EnableLoginControls();
                    label_stat_db_stat.Text = "Configured";
                    label_stat_db_stat.ForeColor = Color.Green;

                    textBox_db_name.Enabled = false;
                    textBox_db_name.Text = Helper.GetAppSetting("db");
                    button_db_confirm.Text = "Edit";
                }
                Enabled = true;
            }
            Enabled = true;
        }

        private async void ServerStatusChanged(MTServerStatus status)
        {
            if (status == MTServerStatus.Connected)
            {
                isretrying = false;
                label_stat_conn_stat.Text = "Connected";
                label_stat_conn_stat.ForeColor = Color.Green;
                label_activity_status.Text = $"Connected on {DateTime.Now.ToString("HH:mm:ss.fff")}";
                label_activity_status.ForeColor = Color.Green;
                button_login.Enabled = false;
                button_logout.Enabled = true;
                button_gethistory.Enabled = true;
                textBox_login.Enabled = false;
                textBox_password.Enabled = false;
                textBox_server.Enabled = false;
            }
            if (status == MTServerStatus.Disconnected)
            {
                if (userlogout)
                {
                    label_stat_conn_stat.Text = "Disconnected";
                    label_stat_conn_stat.ForeColor = Color.Red;
                    label_activity_status.Text = $"Disconnected on {DateTime.Now.ToString("HH:mm:ss.fff")}";
                    label_activity_status.ForeColor = Color.Red;
                    //MessageBox.Show($"Metatrader Server Disconnected! Timestamp: {DateTime.Now}");
                    button_logout.Enabled = false;
                    button_login.Enabled = true;
                    button_gethistory.Enabled = false;
                    textBox_login.Enabled = true;
                    textBox_password.Enabled = true;
                    textBox_server.Enabled = true;
                    return;
                }
                if (isretrying) return;

                isretrying = true;
                var disconnectionTime = DateTime.Now.ToString("HH:mm:ss.fff");
                label_stat_conn_stat.Text = "Connecting";
                label_stat_conn_stat.ForeColor = Color.FromArgb(Int32.Parse("92970C", NumberStyles.HexNumber));
                label_activity_status.Text = $"Disconnected on {disconnectionTime}";
                label_activity_status.ForeColor = Color.Red;
                int n = 0;
                bool loginres = false;
                do
                {
                    await Task.Delay(retrygap * 1000);
                    logger.Write(LogLevel.Error, "Attempting to reconnect...");
                    label_activity_status.Text = $"Attempting reconnect ({n + 1})";
                    label_activity_status.ForeColor = Color.FromArgb(Int32.Parse("92970C", NumberStyles.HexNumber));
                    var task = Task.Run(() => { return MTDataFeed.Login(textBox_server.Text, UInt32.Parse(textBox_login.Text), textBox_password.Text); });
                    await task;
                    loginres = task.Result;
                    n++;
                }
                while (!loginres && !userlogout && n < retries);

                if (!loginres)
                {
                    label_stat_conn_stat.Text = "Disconnected";
                    label_stat_conn_stat.ForeColor = Color.Red;
                    label_activity_status.Text = $"Disconnected on {disconnectionTime}";
                    label_activity_status.ForeColor = Color.Red;
                    button_logout.Enabled = false;
                    button_login.Enabled = true;
                    button_gethistory.Enabled = false;
                    textBox_login.Enabled = true;
                    textBox_password.Enabled = true;
                    textBox_server.Enabled = true;
                }
            }
        }

        private void DealAdded(long dealno, bool history)
        {
            if (history)
            {
                historydealaddednum++;
                label_history_deal_no.Text = historydealaddednum.ToString();
            }
            else
            {
                dealaddednum++;
                label_deal_no.Text = dealaddednum.ToString();
            }   
        }
    }
}
