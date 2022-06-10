using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MT5DealFeed
{
    public partial class MainForm : Form
    {
        Logger logger;
        int dealaddednum = 0;
        int retries = 5;
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

        private void button_login_Click(object sender, EventArgs e)
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
            if (!MTDataFeed.Login(textBox_server.Text, id, textBox_password.Text))
            {
                MessageBox.Show("Cannot login to the server");
                return;
            }
            dealaddednum = 0;
            label_stat_conn_stat.Text = "Connected";
            label_stat_conn_stat.ForeColor = Color.Green;
            button_login.Enabled = false;
            button_db_confirm.Enabled = false;
            button_logout.Enabled = true;
        }

        private void button_logout_Click(object sender, EventArgs e)
        {
            MTDataFeed.Logout();
            MTDataFeed.Stop();
            label_stat_conn_stat.Text = "Disconnected";
            label_stat_conn_stat.ForeColor = Color.Red;
            label_activity_status.Text = $"Disconnected on {DateTime.Now.ToString("HH:mm:ss.fff")}";
            label_activity_status.ForeColor = Color.Red;
            button_logout.Enabled = false;
            button_login.Enabled = true;
            button_db_confirm.Enabled = true;
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

        private void DisableLoginControls()
        {
            button_login.Enabled = false;
            button_logout.Enabled = false;
        }

        private void EnableLoginControls()
        {
            button_login.Enabled = true;
            button_logout.Enabled = true;
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

        private void ServerStatusChanged(MTServerStatus status)
        {
            if (status == MTServerStatus.Connected)
            {
                label_stat_conn_stat.Text = "Connected";
                label_stat_conn_stat.ForeColor = Color.Green;
                label_activity_status.Text = $"Connected on {DateTime.Now.ToString("HH:mm:ss.fff")}";
                label_activity_status.ForeColor = Color.Green;
                button_login.Enabled = false;
                button_logout.Enabled = true;
            }
            if (status == MTServerStatus.Disconnected)
            {
                label_stat_conn_stat.Text = "Disconnected";
                label_stat_conn_stat.ForeColor = Color.Red;
                label_activity_status.Text = $"Disconnected on {DateTime.Now.ToString("HH:mm:ss.fff")}";
                label_activity_status.ForeColor = Color.Red;
                //MessageBox.Show($"Metatrader Server Disconnected! Timestamp: {DateTime.Now}");
                button_logout.Enabled = false;
                button_login.Enabled = true;
                MTDataFeed.Login(textBox_server.Text, UInt32.Parse(textBox_login.Text), textBox_password.Text);
            }
        }

        private void DealAdded(long dealno)
        {
            dealaddednum++;
            label_deal_no.Text = dealaddednum.ToString();
        }
    }
}
