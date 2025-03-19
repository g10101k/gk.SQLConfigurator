/*
 *  "gk.SQLConfigurator", Excel add-in that allows you to fill / edit / delete SQL table data.
 *
 *  Copyright (C) 2015-2019  Igor Tyulyakov aka g10101k, g101k. Contacts: <g101k@mail.ru>
 *  
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 */
using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.IO;
using gk.SQLConfigurator.Providers;
using gk.SQLConfigurator.Config;
using System.Collections;
using System.Collections.Generic;
using gk.SQLConfigurator.Enums;


namespace gk.SQLConfigurator
{
    public partial class frmDbConnect : Form
    {
        public frmDbConnect()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            txtServer.Text          = ApplicationConfig.Instance.ConnectionCfg.Server;
            txtUser.Text            = ApplicationConfig.Instance.ConnectionCfg.Username;
            txtPassword.Text        = ApplicationConfig.Instance.ConnectionCfg.Password;
            chkdWindowsAuth.Checked = ApplicationConfig.Instance.ConnectionCfg.WindowsAuth;
            database.Text           = ApplicationConfig.Instance.ConnectionCfg.Database;

            if (ApplicationConfig.Instance.ConnectionCfg.DBType == DatabaseType.SqlServer)
            {
                txtPort.Enabled = false;
            }

            if (ApplicationConfig.Instance.ConnectionCfg.DBType == DatabaseType.PostgreSql)
            {
                txtUser.Enabled         = true;
                txtPassword.Enabled     = true;
                chkdWindowsAuth.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            ApplicationConfig.Instance.ConnectionCfg.Server      = txtServer.Text;
            ApplicationConfig.Instance.ConnectionCfg.Username    = txtUser.Text;
            ApplicationConfig.Instance.ConnectionCfg.Password    = txtPassword.Text;
            ApplicationConfig.Instance.ConnectionCfg.WindowsAuth = chkdWindowsAuth.Checked;
            ApplicationConfig.Instance.ConnectionCfg.Database    = database.Text;
            
            if (!string.IsNullOrEmpty(txtPort.Text))
                ApplicationConfig.Instance.ConnectionCfg.Port = int.Parse(txtPort.Text);

            ApplicationConfig.Instance.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void database_DropDown(object sender, EventArgs e)
        {
            try
            {
                database.Items.Clear();

                ConnectionConfig cfg = new ConnectionConfig
                {
                    DBType = ApplicationConfig.Instance.ConnectionCfg.DBType,
                    Username = txtUser.Text,
                    Password = txtPassword.Text,
                    Server = txtServer.Text,
                };

                if (!string.IsNullOrEmpty(txtPort.Text))
                    cfg.Port = int.Parse(txtPort.Text);

                IDbProvider dbProvider = ProviderFabric.GetProvider(ApplicationConfig.Instance.ConnectionCfg.DBType, cfg);
                IEnumerable<string> databaseList = dbProvider.GetDatabaseList();

                database.Items.AddRange(databaseList.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.Enabled = true;
            }
        }

        private void chkdWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            if (chkdWindowsAuth.Checked)
            {
                txtPassword.Enabled = false;
                txtUser.Enabled = false;
            }
            else
            {
                txtPassword.Enabled = true;
                txtUser.Enabled = true;
            }
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
