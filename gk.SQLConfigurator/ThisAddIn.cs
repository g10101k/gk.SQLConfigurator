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
#define DEBUG 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using Microsoft.Office.Tools.Ribbon;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;

using gk.Log;

using System.Threading;


namespace gk.SQLConfigurator
{
   public partial class ThisAddIn
    {
        private SQLConfiguratorRibbon ribbon;
        private SqlConnection cnt;
        private SqlConnectionStringBuilder cnsb;
        public ItemChangerList ICList = new ItemChangerList();

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
#if (DEBUG)
            //ribbon_AttachConsole();
#endif
            cnt = new SqlConnection();
            connectToSql();
            //Thread t = new Thread(connectToSql);
            //t.Start();
        }

        public void ReadICL()
        {
            try
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\gk.SQLConfigurator\";
                string path = dir + "SQLItems.xml";
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                if (!File.Exists(path))
                {
                    File.WriteAllText(path, global::gk.SQLConfigurator.Properties.Resources.SQLItems);
                }

                XmlSerializer formatter = new XmlSerializer(typeof(ItemChangerList));

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    ICList = (ItemChangerList)formatter.Deserialize(fs);
                }
            }
            catch
            { 
                
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            try
            {
                if (cnt?.State == ConnectionState.Open)
                {
                    cnt.Close();
                }
                SaveICL();
            }
            catch 
            {

            }
        }

        public void SaveICL()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\gk.SQLConfigurator\SQLItems.xml";

            XmlSerializer formatter = new XmlSerializer(typeof(ItemChangerList));
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                formatter.Serialize(fs, ICList);
            }
        }

        private void setBtnState(bool b)
        {
            ribbon.btnAction.Enabled = b;
        }

        private void connectToSql()
        {
            cnsb = new SqlConnectionStringBuilder(Properties.Settings.Default.ConnectionString);
            cnsb.ConnectTimeout = 60;
            cnsb.AsynchronousProcessing = true;
            if (cnt.State == ConnectionState.Open)
                cnt.Close();
            cnt.ConnectionString = cnsb.ConnectionString;
            try
            {
                cnt.Open();

                if (cnt.State == ConnectionState.Open)
                {
                    //SqlDataReader reader;
                    try
                    {
                        //SqlCommand cmd = new SqlCommand(@"select * from SysDatabaseVersion", cnt);
                        //reader = cmd.ExecuteReader();
                        //reader.Read();
                        //Version curVersionDB = new Version(Convert.ToInt32(reader["DatabaseVersionId"]), Convert.ToInt32(reader["MajorVersion"]), Convert.ToInt32(reader["MinorVersion"]), Convert.ToInt32(reader["Revision"]));
                        ribbon.lConnectState.Label = "Состояние: Подключено";
                        ribbon.lServer.Label = string.Format("Сервер: {0}", cnsb.DataSource);
                        ribbon.lDb.Label = string.Format("База данных: {0}", cnsb.InitialCatalog);
                        setBtnState(true);
                        //reader.Close();
                    }
                    catch (Exception ex)
                    {
                        ribbon.lConnectState.Label = "Состояние: Отключено";
                        ribbon.lServer.Label = string.Format("Сервер: {0}", "");
                        ribbon.lDb.Label = string.Format("База данных: {0}", "");
                        gLogger.WriteDebug(ex.ToString());
                        setBtnState(false);
                    }
                }
                else
                {
                    ribbon.lConnectState.Label = "Состояние: Отключено";
                    ribbon.lServer.Label = string.Format("Сервер: {0}", "");
                    ribbon.lDb.Label = string.Format("База данных: {0}", "");
                    setBtnState(false);
                }
            }
            catch (Exception ex)
            {
                ribbon.lConnectState.Label = "Состояние: Отключено";
                ribbon.lServer.Label = string.Format("Сервер: {0}", "");
                ribbon.lDb.Label = string.Format("База данных: {0}", "");
                gLogger.WriteDebug(ex.ToString());
                setBtnState(false);
            }
            return;
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            ReadICL();
            ribbon = new SQLConfiguratorRibbon();
            ribbon.addin = this;

            ribbon.ButtonClicked += ribbon_connectSetingsButtonClick;
            ribbon.btnExecuteToDBClicked += ribbon_btnExcecuteToDB;
            ribbon.AttachConsole += ribbon_AttachConsole;
            ribbon.btSqlEditClicked += ribbon_btSqlEditClick;
            ribbon.btnSQLSaveCliked += ribbon_btnSQLSaveCliked;
            ribbon.btnSettingCliked += ribbon_btnSettingCliked;

            return Globals.Factory.GetRibbonFactory().CreateRibbonManager(new IRibbonExtension[] { ribbon });
        }

        private void ribbon_btSqlEditClick()
        {
            ItemChanger it = ICList.Items[ribbon.cmbItemChanger.SelectedItemIndex];
            frmSqlEdit f = new frmSqlEdit(this);

            switch (ribbon.editorTypeSelect.SelectedItemIndex)
            {
                case 0: // Получить
                    f.sql = it.GetSQL;
                    if (DialogService.ShowDialog(f) == DialogResult.OK)
                    {
                        it.GetSQL = f.sql;
                        SaveICL();
                    }
                    break;
                case 1: // Редактировать
                    f.sql = it.EditSql;
                    if (DialogService.ShowDialog(f) == DialogResult.OK)
                    {
                        it.EditSql = f.sql;
                        SaveICL();
                    }
                    break;
                case 2: // Добавить
                    f.sql = it.CreateSql; 
                    if (DialogService.ShowDialog(f) == DialogResult.OK)
                    {
                        it.CreateSql = f.sql;
                        SaveICL();
                    }
                    break;
                case 3: // Редактировать или добавить 
                    f.sql = it.CreateoreditSql;
                    if (DialogService.ShowDialog(f) == DialogResult.OK)
                    {
                        it.CreateoreditSql = f.sql;
                        SaveICL();
                    }
                    break;
                case 4: // Удалить
                    f.sql = it.DeleteSql;
                    if (DialogService.ShowDialog(f) == DialogResult.OK)
                    {
                        it.DeleteSql = f.sql;
                        SaveICL();
                    }
                    break;
                default: break;
            }
        }

        private void ribbon_connectSetingsButtonClick()
        {
            if (cnt == null)
                connectToSql();
            frmSettings f = new frmSettings(cnt, cnsb);
            DialogService.ShowDialog(f);

            if (f.DialogResult == DialogResult.OK)
            {
                cnt = f.cnt;
                cnsb = f.cnsb;
                connectToSql();
            }
            f.Dispose();
        }

        public void ribbon_ActionCLick(bool SaveAsSql, bool Check)
        {
            if (cnt != null)
            {
                if (cnt.State == ConnectionState.Open)
                {
                    switch (ribbon.editorTypeSelect.SelectedItemIndex)
                    {
                        case 0: // Получить
                            frmExec.GetObjectUniversal(Application.ActiveWorkbook.ActiveSheet, cnt, ribbon.cmbItemChanger.SelectedItem.Tag, SaveAsSql, Check);
                            break;
                        case 1: // Редактировать
                            frmExec.EditObjectUniversal(Application.ActiveWorkbook.ActiveSheet, cnt, ((ItemChanger)ribbon.cmbItemChanger.SelectedItem.Tag).EditSql, SaveAsSql, Check);
                            break;
                        case 2: // Добавить
                            frmExec.EditObjectUniversal(Application.ActiveWorkbook.ActiveSheet, cnt, ((ItemChanger)ribbon.cmbItemChanger.SelectedItem.Tag).CreateSql, SaveAsSql, Check);
                            break;
                        case 3: // Добавить или Редактировать
                            frmExec.EditObjectUniversal(Application.ActiveWorkbook.ActiveSheet, cnt, ((ItemChanger)ribbon.cmbItemChanger.SelectedItem.Tag).CreateoreditSql, SaveAsSql, Check);
                            break;
                        case 4: //Удалить
                            DialogResult dr = DialogService.ShowWarning("Вы уверены что хотите удалить выделенные записи?");
                            if (dr == DialogResult.OK)
                                frmExec.EditObjectUniversal(Application.ActiveWorkbook.ActiveSheet, cnt, ((ItemChanger)ribbon.cmbItemChanger.SelectedItem.Tag).DeleteSql, SaveAsSql, Check);
                            break;
                        default: break;

                    }
                }
                else
                    DialogService.ShowMessage("Отсутствует подключение к серверу");

            }
        }

        private void ribbon_btnExcecuteToDB()
        {
            ribbon_ActionCLick(false, false);
        }
        
        private void ribbon_btnSQLSaveCliked()
        {
            ribbon_ActionCLick(true, false);
        }

        private void ribbon_AttachConsole()
        {
            try
            {
                if (!gLogger.Showed)
                {
                    //gLogger.ShowForm(xlMain);
                    DialogService.ShowDialog(gLogger.Instance);

                }
                else
                {
                    gLogger.Close();
                }
            }
            catch (Exception ex)
            {
                gLogger.WriteDebug(ex.Message);
            }
        }

        private void ribbon_btnSettingCliked()
        {
            DialogService.ShowDialog(new frmSelectTech(ICList));
            ribbon.UpdateICConteiner();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

#endregion
    }

    class sqlparam {
        public string WhereString { get; set; }
    }

    public static class DialogService
    {
        public static DialogResult ShowDialog(Form dialog)
        {
            NativeWindow mainWindow = new NativeWindow();
            mainWindow.AssignHandle(Process.GetCurrentProcess().MainWindowHandle);
            DialogResult dialogResult = dialog.ShowDialog(mainWindow);
            mainWindow.ReleaseHandle();
            return dialogResult;
        }
        public static DialogResult ShowWarning(string message)
        {
            NativeWindow mainWindow = new NativeWindow();
            mainWindow.AssignHandle(Process.GetCurrentProcess().MainWindowHandle);
            DialogResult dialogResult = MessageBox.Show(mainWindow, message, "gk.SQLConfigurator", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            mainWindow.ReleaseHandle();
            return dialogResult;
        }
        public static DialogResult ShowDialog(SaveFileDialog dialog)
        {
            NativeWindow mainWindow = new NativeWindow();
            mainWindow.AssignHandle(Process.GetCurrentProcess().MainWindowHandle);
            DialogResult dialogResult = dialog.ShowDialog(mainWindow);
            mainWindow.ReleaseHandle();
            return dialogResult;
        }

        public static DialogResult ShowMessage(string message)
        {
            NativeWindow mainWindow = new NativeWindow();
            mainWindow.AssignHandle(Process.GetCurrentProcess().MainWindowHandle);
            DialogResult dialogResult = MessageBox.Show(mainWindow, message);
            mainWindow.ReleaseHandle();
            return dialogResult;
        }
    }


}
