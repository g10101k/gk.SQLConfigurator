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
using System.Net;
using System.IO;


namespace gk.SQLConfigurator
{
    public partial class frmDbConnect : Form
    {
        public SqlConnection cnt;
        public SqlConnectionStringBuilder cnsb;
        public frmDbConnect()
        {
            InitializeComponent();
        }

        public frmDbConnect(SqlConnection _cnt, SqlConnectionStringBuilder _cnsb)
        {
            cnt = _cnt;
            cnsb = _cnsb;
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            // = Properties.Settings.Default.ConnectionString;
            // if (cnt == null)

            cnsb.ConnectTimeout = 5;
            txtServer.Text = cnsb.DataSource;
            txtUser.Text = cnsb.UserID;
            txtPassword.Text = cnsb.Password;
            chkdWindowsAuth.Checked = cnsb.IntegratedSecurity;
            string db = cnsb.InitialCatalog;
            //database_DropDown(null, null);
            database.Text = db;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cnsb.ConnectTimeout = 60;
            cnsb.DataSource = txtServer.Text;
            cnsb.UserID = txtUser.Text;
            cnsb.Password = txtPassword.Text;
            cnsb.IntegratedSecurity = chkdWindowsAuth.Checked;
            cnsb.InitialCatalog = database.Text;
            Properties.Settings.Default.ConnectionString = cnsb.ConnectionString;
            Properties.Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void database_DropDown(object sender, EventArgs e)
        {
            cnsb = new SqlConnectionStringBuilder();
            cnsb.ConnectTimeout = 60;
            cnsb.DataSource = txtServer.Text;
            if (chkdWindowsAuth.Checked)
            {
                cnsb.IntegratedSecurity = true;
            }
            else
            {
                cnsb.IntegratedSecurity = false;
                cnsb.UserID = txtUser.Text;
                cnsb.Password = txtPassword.Text;
            }

            cnt = new SqlConnection(cnsb.ConnectionString);
            try
            {
                this.Enabled = false;
                database.Items.Clear();
                cnt.Open();
                if (cnt.State == ConnectionState.Open)
                {
                    DataTable schemaTable = cnt.GetSchema("Databases");
                    foreach (DataRow r in schemaTable.Rows)
                    {
                        database.Items.Add(r[0]);
                    }
                }
                this.Enabled = true;
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
    }
}
