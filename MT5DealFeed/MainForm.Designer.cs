﻿
namespace MT5DealFeed
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBox_server = new System.Windows.Forms.TextBox();
            this.label_server = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_logout = new System.Windows.Forms.Button();
            this.button_login = new System.Windows.Forms.Button();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.textBox_login = new System.Windows.Forms.TextBox();
            this.label_password = new System.Windows.Forms.Label();
            this.label_login = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_stat_db_stat = new System.Windows.Forms.Label();
            this.label_db_stat = new System.Windows.Forms.Label();
            this.label_stat_conn_stat = new System.Windows.Forms.Label();
            this.label_conn_stat = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_db_confirm = new System.Windows.Forms.Button();
            this.textBox_db_name = new System.Windows.Forms.TextBox();
            this.label_db_name = new System.Windows.Forms.Label();
            this.label_activity = new System.Windows.Forms.Label();
            this.label_activity_status = new System.Windows.Forms.Label();
            this.label_deal = new System.Windows.Forms.Label();
            this.label_deal_no = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_server
            // 
            this.textBox_server.Location = new System.Drawing.Point(99, 37);
            this.textBox_server.Name = "textBox_server";
            this.textBox_server.Size = new System.Drawing.Size(358, 20);
            this.textBox_server.TabIndex = 0;
            // 
            // label_server
            // 
            this.label_server.AutoSize = true;
            this.label_server.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_server.Location = new System.Drawing.Point(17, 37);
            this.label_server.Name = "label_server";
            this.label_server.Size = new System.Drawing.Size(55, 20);
            this.label_server.TabIndex = 1;
            this.label_server.Text = "Server";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_logout);
            this.groupBox1.Controls.Add(this.button_login);
            this.groupBox1.Controls.Add(this.textBox_password);
            this.groupBox1.Controls.Add(this.textBox_login);
            this.groupBox1.Controls.Add(this.label_password);
            this.groupBox1.Controls.Add(this.label_login);
            this.groupBox1.Controls.Add(this.label_server);
            this.groupBox1.Controls.Add(this.textBox_server);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(473, 273);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login";
            // 
            // button_logout
            // 
            this.button_logout.Enabled = false;
            this.button_logout.Location = new System.Drawing.Point(382, 209);
            this.button_logout.Name = "button_logout";
            this.button_logout.Size = new System.Drawing.Size(75, 23);
            this.button_logout.TabIndex = 7;
            this.button_logout.Text = "Logout";
            this.button_logout.UseVisualStyleBackColor = true;
            this.button_logout.Click += new System.EventHandler(this.button_logout_Click);
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(236, 209);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(75, 23);
            this.button_login.TabIndex = 6;
            this.button_login.Text = "Login";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(99, 124);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.Size = new System.Drawing.Size(358, 20);
            this.textBox_password.TabIndex = 5;
            this.textBox_password.UseSystemPasswordChar = true;
            // 
            // textBox_login
            // 
            this.textBox_login.Location = new System.Drawing.Point(99, 81);
            this.textBox_login.Name = "textBox_login";
            this.textBox_login.Size = new System.Drawing.Size(358, 20);
            this.textBox_login.TabIndex = 4;
            // 
            // label_password
            // 
            this.label_password.AutoSize = true;
            this.label_password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_password.Location = new System.Drawing.Point(17, 124);
            this.label_password.Name = "label_password";
            this.label_password.Size = new System.Drawing.Size(78, 20);
            this.label_password.TabIndex = 3;
            this.label_password.Text = "Password";
            // 
            // label_login
            // 
            this.label_login.AutoSize = true;
            this.label_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_login.Location = new System.Drawing.Point(17, 81);
            this.label_login.Name = "label_login";
            this.label_login.Size = new System.Drawing.Size(48, 20);
            this.label_login.TabIndex = 2;
            this.label_login.Text = "Login";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_deal_no);
            this.groupBox2.Controls.Add(this.label_deal);
            this.groupBox2.Controls.Add(this.label_activity_status);
            this.groupBox2.Controls.Add(this.label_activity);
            this.groupBox2.Controls.Add(this.label_stat_db_stat);
            this.groupBox2.Controls.Add(this.label_db_stat);
            this.groupBox2.Controls.Add(this.label_stat_conn_stat);
            this.groupBox2.Controls.Add(this.label_conn_stat);
            this.groupBox2.Location = new System.Drawing.Point(491, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 273);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // label_stat_db_stat
            // 
            this.label_stat_db_stat.AutoSize = true;
            this.label_stat_db_stat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_stat_db_stat.Location = new System.Drawing.Point(149, 85);
            this.label_stat_db_stat.Name = "label_stat_db_stat";
            this.label_stat_db_stat.Size = new System.Drawing.Size(70, 16);
            this.label_stat_db_stat.TabIndex = 5;
            this.label_stat_db_stat.Text = "Not Found";
            // 
            // label_db_stat
            // 
            this.label_db_stat.AutoSize = true;
            this.label_db_stat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_db_stat.Location = new System.Drawing.Point(12, 85);
            this.label_db_stat.Name = "label_db_stat";
            this.label_db_stat.Size = new System.Drawing.Size(117, 16);
            this.label_db_stat.TabIndex = 4;
            this.label_db_stat.Text = "Database Status : ";
            // 
            // label_stat_conn_stat
            // 
            this.label_stat_conn_stat.AutoSize = true;
            this.label_stat_conn_stat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_stat_conn_stat.Location = new System.Drawing.Point(149, 41);
            this.label_stat_conn_stat.Name = "label_stat_conn_stat";
            this.label_stat_conn_stat.Size = new System.Drawing.Size(91, 16);
            this.label_stat_conn_stat.TabIndex = 3;
            this.label_stat_conn_stat.Text = "Disconnected";
            // 
            // label_conn_stat
            // 
            this.label_conn_stat.AutoSize = true;
            this.label_conn_stat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_conn_stat.Location = new System.Drawing.Point(12, 41);
            this.label_conn_stat.Name = "label_conn_stat";
            this.label_conn_stat.Size = new System.Drawing.Size(124, 16);
            this.label_conn_stat.TabIndex = 2;
            this.label_conn_stat.Text = "Connection Status : ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_db_confirm);
            this.groupBox3.Controls.Add(this.textBox_db_name);
            this.groupBox3.Controls.Add(this.label_db_name);
            this.groupBox3.Location = new System.Drawing.Point(12, 291);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(776, 147);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Database";
            // 
            // button_db_confirm
            // 
            this.button_db_confirm.Location = new System.Drawing.Point(680, 106);
            this.button_db_confirm.Name = "button_db_confirm";
            this.button_db_confirm.Size = new System.Drawing.Size(75, 23);
            this.button_db_confirm.TabIndex = 7;
            this.button_db_confirm.Text = "Confirm";
            this.button_db_confirm.UseVisualStyleBackColor = true;
            this.button_db_confirm.Click += new System.EventHandler(this.button_db_confirm_Click);
            // 
            // textBox_db_name
            // 
            this.textBox_db_name.Location = new System.Drawing.Point(169, 47);
            this.textBox_db_name.Name = "textBox_db_name";
            this.textBox_db_name.Size = new System.Drawing.Size(586, 20);
            this.textBox_db_name.TabIndex = 3;
            // 
            // label_db_name
            // 
            this.label_db_name.AutoSize = true;
            this.label_db_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_db_name.Location = new System.Drawing.Point(17, 45);
            this.label_db_name.Name = "label_db_name";
            this.label_db_name.Size = new System.Drawing.Size(125, 20);
            this.label_db_name.TabIndex = 2;
            this.label_db_name.Text = "Database Name";
            // 
            // label_activity
            // 
            this.label_activity.AutoSize = true;
            this.label_activity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_activity.Location = new System.Drawing.Point(12, 128);
            this.label_activity.Name = "label_activity";
            this.label_activity.Size = new System.Drawing.Size(87, 16);
            this.label_activity.TabIndex = 6;
            this.label_activity.Text = "Last Activity : ";
            // 
            // label_activity_status
            // 
            this.label_activity_status.AutoSize = true;
            this.label_activity_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_activity_status.Location = new System.Drawing.Point(105, 128);
            this.label_activity_status.Name = "label_activity_status";
            this.label_activity_status.Size = new System.Drawing.Size(71, 16);
            this.label_activity_status.TabIndex = 7;
            this.label_activity_status.Text = "No Activity";
            // 
            // label_deal
            // 
            this.label_deal.AutoSize = true;
            this.label_deal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_deal.Location = new System.Drawing.Point(12, 172);
            this.label_deal.Name = "label_deal";
            this.label_deal.Size = new System.Drawing.Size(128, 16);
            this.label_deal.TabIndex = 8;
            this.label_deal.Text = "No. of Deal Added : ";
            // 
            // label_deal_no
            // 
            this.label_deal_no.AutoSize = true;
            this.label_deal_no.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_deal_no.Location = new System.Drawing.Point(146, 172);
            this.label_deal_no.Name = "label_deal_no";
            this.label_deal_no.Size = new System.Drawing.Size(15, 16);
            this.label_deal_no.TabIndex = 9;
            this.label_deal_no.Text = "0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "MetaTrader Deal Recorder";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_server;
        private System.Windows.Forms.Label label_server;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_logout;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.TextBox textBox_login;
        private System.Windows.Forms.Label label_password;
        private System.Windows.Forms.Label label_login;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_conn_stat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_stat_conn_stat;
        private System.Windows.Forms.Label label_stat_db_stat;
        private System.Windows.Forms.Label label_db_stat;
        private System.Windows.Forms.Button button_db_confirm;
        private System.Windows.Forms.TextBox textBox_db_name;
        private System.Windows.Forms.Label label_db_name;
        private System.Windows.Forms.Label label_activity;
        private System.Windows.Forms.Label label_activity_status;
        private System.Windows.Forms.Label label_deal_no;
        private System.Windows.Forms.Label label_deal;
    }
}

