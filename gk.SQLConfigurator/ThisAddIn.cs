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
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.IO;



namespace gk.SQLConfigurator
{
    public partial class ThisAddIn
    {
        private SQLConfiguratorRibbon ribbon;
        private object lc = new object();
        private SqlConnection cnt;
        private SqlConnectionStringBuilder cnsb;
        public static ItemChangerList ICList = new ItemChangerList();
        public static string ConfigDir {
            get {
                return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\gk.SQLConfigurator\";
            }
        }

        public static string ConfigName
        {
            get
            {
                return "SQLItems.xml";
            }
        }
        public static string ConfigPath
        {
            get
            {
                return ConfigDir + ConfigName;
            }
        }
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            cnt = new SqlConnection();
            ConnectToSql();
        }

        public void ReadICL()
        {
            try
            {
                if (!System.IO.Directory.Exists(ConfigDir))
                    System.IO.Directory.CreateDirectory(ConfigDir);
                ICList = LoadItemChangerList(ConfigPath);
            }
            catch (Exception ex)
            {
                Logger.Error("(ReadICL)", ex);
            }
        }

        public static ItemChangerList LoadItemChangerList(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, global::gk.SQLConfigurator.Properties.Resources.SQLItems);
                }

                XmlSerializer formatter = new XmlSerializer(typeof(ItemChangerList));

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    return (ItemChangerList)formatter.Deserialize(fs);
                }

            }
            catch (Exception ex)
            {
                Logger.Error("(LoadItemChangerList)", ex);
            }
            return null;
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
            catch (Exception ex)
            {
                Logger.Error("(ThisAddIn_Shutdown)", ex);
            }
        }

        public void SaveICL()
        {
            try
            {
                // TODO: Если не изменилось - не сохранять
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\gk.SQLConfigurator\SQLItems.xml";

                XmlSerializer formatter = new XmlSerializer(typeof(ItemChangerList));
                using (FileStream fs = new FileStream(path, FileMode.Truncate))
                {
                    formatter.Serialize(fs, ICList);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("(SaveICL)", ex);
            }
        }

        private void SetBtnState(bool b)
        {
            ribbon.btnAction.Enabled = b;
        }

        bool ConnectInProcess = false;

        private void ConnectToSql()
        {
            System.Action action = () =>
            {
                lock (lc)
                {
                    ConnectInProcess = true;
                    cnsb = new SqlConnectionStringBuilder(Properties.Settings.Default.ConnectionString);
                    cnsb.AsynchronousProcessing = true;
                    if (cnt.State == ConnectionState.Open)
                        cnt.Close();
                    cnt.ConnectionString = cnsb.ConnectionString;
                    try
                    {
                        cnt.Open();

                        if (cnt.State == ConnectionState.Open)
                        {
                            try
                            {
                                ribbon.lConnectState.Label = "Состояние: Подключено";
                                ribbon.lServer.Label = string.Format("Сервер: {0}", cnsb.DataSource);
                                ribbon.lDb.Label = string.Format("База данных: {0}", cnsb.InitialCatalog);
                                SetBtnState(true);
                            }
                            catch (Exception ex)
                            {
                                ribbon.lConnectState.Label = "Состояние: Отключено";
                                ribbon.lServer.Label = string.Format("Сервер: {0}", "");
                                ribbon.lDb.Label = string.Format("База данных: {0}", "");
                                Logger.Error("(connectToSql.action)", ex);
                                SetBtnState(false);
                            }
                        }
                        else
                        {
                            ribbon.lConnectState.Label = "Состояние: Отключено";
                            ribbon.lServer.Label = string.Format("Сервер: {0}", "");
                            ribbon.lDb.Label = string.Format("База данных: {0}", "");
                            SetBtnState(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        ribbon.lConnectState.Label = "Состояние: Отключено";
                        ribbon.lServer.Label = string.Format("Сервер: {0}", "");
                        ribbon.lDb.Label = string.Format("База данных: {0}", "");
                        Logger.Error("(connectToSql.action)", ex);
                        SetBtnState(false);
                    }
                }
                ConnectInProcess = false;

            };
            System.Action animation = () =>
            {
                int sleep = 300;
                while (ConnectInProcess)
                {

                    ribbon.btnConnect.Image = global::gk.SQLConfigurator.Properties.Resources.database_lightning;
                    Thread.Sleep(sleep);
                    ribbon.btnConnect.Image = global::gk.SQLConfigurator.Properties.Resources.database;
                    Thread.Sleep(sleep);
                }
                ribbon.btnConnect.Image = global::gk.SQLConfigurator.Properties.Resources.database;

            };
            new Task(action).Start();
            new Task(animation).Start();
            return;
        }



        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {

            // Читаем настройки
            ReadICL();
            // Проверяем обнову
            CheckUpdate_Click();

            ribbon = new SQLConfiguratorRibbon();
            ribbon.Addin = this;

            ribbon.BtnConnectSetings += ribbon_connectSetingsButtonClick;
            ribbon.BtnExecuteToDBClicked += ribbon_btnExcecuteToDB;
            ribbon.BtSqlEditClicked += ribbon_btSqlEditClick;
            ribbon.BtnSQLSaveCliked += ribbon_btnSQLSaveCliked;
            ribbon.BtnSettingCliked += ribbon_btnSettingCliked;

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
                ConnectToSql();
            frmDbConnect f = new frmDbConnect(cnt, cnsb);
            DialogService.ShowDialog(f);

            if (f.DialogResult == DialogResult.OK)
            {
                cnt = f.cnt;
                cnsb = f.cnsb;
                ConnectToSql();
            }
            UpdateICConteiner();
            f.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SaveAsSql"></param>
        /// <param name="Check"></param>
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

        private void ribbon_btnSettingCliked()
        {
            DialogService.ShowDialog(new frmSelectTech(ICList));
            UpdateICConteiner();
        }

        public void UpdateICConteiner()
        {
            ribbon.UpdateICConteiner();
        }

        public static void CheckUpdate_Click()
        {
            try
            {
                string path = Properties.Settings.Default.UpdatePath;
                string xml = "";
                if (path.ToLower().StartsWith(@"\\"))
                {
                    StreamReader sr = new StreamReader(path);
                    xml = sr.ReadToEnd();
                }
                else if (path.ToLower().StartsWith("ftp") || path.ToLower().StartsWith("http"))
                {
                    // Объект запроса
                    HttpWebRequest rew = (HttpWebRequest)WebRequest.Create(path);
                    // Отправить запрос и получить ответ
                    HttpWebResponse resp = (HttpWebResponse)rew.GetResponse();
                    // Получить поток
                    Stream str = resp.GetResponseStream();
                    // Выводим в TextBox
                    int ch;
                    string message = "";
                    for (int i = 1; ; i++)
                    {
                        ch = str.ReadByte();
                        if (ch == -1) break;
                        message += (char)ch;
                    }
                    xml = message;

                    // Закрыть поток
                    str.Close();
                }
                // Получить файл
                // Проверить версию
                // Оповестить
                string nverstring = gk.SQLConfigurator.ThisAddIn.LoadItemChangerList(path).CurrentVersion;
                if (!string.IsNullOrEmpty(nverstring))
                {
                    Version n = Version.Parse(nverstring);
                    Version c = Version.Parse(ThisAddIn.ICList.CurrentVersion);
                    if (n > c)
                    {
                        if (DialogResult.OK == MessageBox.Show(Properties.Resources.AvaibleNewConfigVersion, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                        {
                            try
                            {
                                string dest = ThisAddIn.ConfigDir + DateTime.Now.ToString().Replace(":", string.Empty) + ThisAddIn.ConfigName + ".bak";
                                File.Copy(ThisAddIn.ConfigPath, dest);
                                ThisAddIn.ICList = gk.SQLConfigurator.ThisAddIn.LoadItemChangerList(path);
                            }
                            catch (IOException copyError)
                            {
                                Logger.Error("(checkUpdate_Click_Copy_and_Update)", copyError);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("(checkUpdate_Click)", ex);
            }
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

    class sqlparam
    {
        public string WhereString { get; set; }
    }

    public static class DialogService
    {
        public static DialogResult ShowDialog(Form dialog)
        {
            dialog.StartPosition = FormStartPosition.CenterParent;
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
            DialogResult dialogResult = MessageBox.Show(mainWindow, message, Properties.Settings.Default.PanelName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
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
            DialogResult dialogResult = MessageBox.Show(mainWindow, message, Properties.Settings.Default.PanelName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            mainWindow.ReleaseHandle();
            return dialogResult;
        }
    }
}
