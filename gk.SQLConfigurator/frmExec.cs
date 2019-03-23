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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using gk.Log;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Linq;

namespace gk.SQLConfigurator
{
    public partial class frmExec : Form
    {

        private static volatile frmExec instance;
        private static object syncRoot = new Object();
        private static frmExec Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new frmExec();
                    }
                }

                return instance;
            }
        }

        public delegate void EndExecuteEventHandler();
        public event EndExecuteEventHandler EndExecuteEvent;

        public delegate void UserEndExecuteEventHandler();
        public event UserEndExecuteEventHandler UserEndExecuteEvent;

        public delegate void UpdateProgressbarEventHandler(int start, int end, int cur);
        public event UpdateProgressbarEventHandler UpdateProgressbar;

        public delegate void ErrorEventHandler(Exception ex, string s);
        public event ErrorEventHandler ErrorInThread;

        public delegate void DebugEventHandler(string s);
        public event DebugEventHandler DebugInThread;

        public delegate void SaveFileEventHandler(string s);
        public event SaveFileEventHandler SaveFile;

        public delegate void SqlInfoEventHandler(string s);
        public event SqlInfoEventHandler SqlInfoEvent;
        private bool SqlInfoEventHandled = false;
        public bool StopExecute = false;

        Excel.Worksheet wSheet;
        SqlConnection cnt;
        string query;
        bool SaveAsSql;
        bool Check;
        Thread t;
        ItemChanger ic;
        public frmExec()
        {
            InitializeComponent();
            EndExecuteEvent += FrmExec_EndExecuteEvent;
            UserEndExecuteEvent += FrmExec_UserEndExecuteEvent;
            UpdateProgressbar += FrmExec_UpdateProgressbar;
            ErrorInThread += FrmExec_ErrorInThread;
            DebugInThread += FrmExec_DebugInThread;
            SaveFile += FrmExec_SaveFile;
            SqlInfoEvent += FrmExec_SqlInfoEvent;
        }

        private void FrmExec_SqlInfoEvent(string s)
        {
            gLogger.WriteDebug(s);
        }

        private void FrmExec_SaveFile(string s)
        {
            try
            {
                frmSqlEdit f = new frmSqlEdit(null);
                f.sql = s;
                DialogService.ShowDialog(f);                
            }
            catch (Exception ex)
            {
                gLogger.WriteError(ex, s);
            }
        }

        private void FrmExec_DebugInThread(string s)
        {
            gLogger.WriteDebug(s);
        }

        private void FrmExec_ErrorInThread(Exception ex, string s)
        {
            //MessageBox.Show(xlMain, ex.Message + "\r\n" + ex.StackTrace);
            gLogger.WriteError(ex, s);
        }

        private void FrmExec_UpdateProgressbar(int start, int end, int cur)
        {
            bar.Minimum = start;
            bar.Maximum = end;
            bar.Value = cur;
        }

        private void FrmExec_UserEndExecuteEvent()
        {
            this.Hide();
            DialogService.ShowMessage("Операция прервана");
            this.Close();
        }

        private void FrmExec_EndExecuteEvent()
        {
            this.Hide();
            DialogService.ShowMessage("Операция завершена");
            this.Close();
        }

        public void EditObjectUniversal()
        {
            try
            {
                string sqltext = "";

                int lLastCol = wSheet.Cells[1, wSheet.Columns.Count].End(Excel.XlDirection.xlToLeft).Column; // находим последнюю колонку
                int lLastRow = wSheet.Cells[wSheet.Rows.Count, 1].End(Excel.XlDirection.xlUp).Row; // Последнюю строку

                Dictionary<String, int> HeaderList = new Dictionary<String, int>();
                if (wSheet.Cells[1, 1].Value.ToString().ToLower() != "select(x)")
                    return;

                // Ищем колонки
                for (int i = 1; i <= lLastCol; i++)
                {
                    if (wSheet.Cells[1, i].Text != "")
                    {
                        HeaderList.Add(wSheet.Cells[1, i].Text, i);
                    }
                }

                for (int i = 2; i <= lLastRow; i++)
                {
                    this.BeginInvoke(UpdateProgressbar, new object[] { 2, lLastRow, i });
                    if (this.StopExecute)
                    {
                        this.BeginInvoke(UserEndExecuteEvent);
                        return;
                    };
                    if (wSheet.Cells[i, 1].Text.ToLower() == "x")
                    {
                        string sql = query;
                        string val;

                        var sorted = from s in HeaderList
                                     orderby s.Key.Length descending
                                     select s;
                        foreach (KeyValuePair<string, int> pair in sorted)
                        {
                            int j = pair.Value;
                            if (wSheet.Cells[i, j].Text != "")
                            {
                                val = (wSheet.Cells[i, j].Value2 != null) ? wSheet.Cells[i, j].Value2.ToString() : wSheet.Cells[i, j].Value.ToString();
                                val = ConvertValueToSqlString(val);
                                
                                if (val == "null")
                                    sql = sql.Replace("N'$(" + pair.Key + ")'", val).Replace("'$(" + pair.Key + ")'", val).Replace("$(" + pair.Key + ")", val);
                                else
                                    sql = sql.Replace("$(" + pair.Key + ")", val);

                            }
                            else
                                sql = sql.Replace("$(" + pair.Key + ")", "");
                        }

                        if (!SaveAsSql)
                        {
                            SqlCommand cmd = new SqlCommand(sql, cnt);

                            try
                            {
                                wSheet.Rows[i].Select();
                                wSheet.Rows[i].Font.Color = System.Drawing.Color.Black;
                                cmd.CommandTimeout = 360;

                                int c = cmd.ExecuteNonQuery();

                                /*if (c > 0) 
                                    wSheet.Rows[i].Font.Color = System.Drawing.Color.Black;
                                else
                                    wSheet.Rows[i].Font.Color = System.Drawing.Color.Red;*/
                            }
                            catch (Exception ex)
                            {
                                wSheet.Rows[i].Font.Color = System.Drawing.Color.Red;
                                this.BeginInvoke(ErrorInThread, new object[] { ex, "EditObjectUniversal" });
                            }
                            this.BeginInvoke(DebugInThread, new object[] { sql });
                            if (Check)
                            {
                                this.BeginInvoke(EndExecuteEvent);
                                return;
                            }
                        }
                        else
                        {
                            sqltext += sql + "\r\ngo\r\n\r\n";
                        }
                    }
                }
                if (SaveAsSql)
                {
                    this.BeginInvoke(SaveFile, new object[] { sqltext });
                }

                this.BeginInvoke(EndExecuteEvent);
            }
            catch (Exception ex) {
                this.Close();
                this.BeginInvoke(ErrorInThread, new object[] { ex, "EditObjectUniversal" });
            }
        }

        public static void EditObjectUniversal(Excel.Worksheet _wSheet, SqlConnection _cnt, string _ic, bool _SaveAsSql, bool _Check)
        {
            if (!Instance.SqlInfoEventHandled)
            {
                Instance.SqlInfoEventHandled = true;
                _cnt.InfoMessage += _cnt_InfoMessage;
            }
            Instance.StopExecute = false;
            Instance.cnt = _cnt;
            Instance.wSheet = _wSheet;
            Instance.query = _ic;
            Instance.SaveAsSql = _SaveAsSql;
            Instance.Check = _Check;
            Instance.bar.Style = ProgressBarStyle.Continuous;
            Instance.t = new Thread(Instance.EditObjectUniversal);
            Instance.t.Name = "SQL Thread";
            Instance.t.Start();
            DialogService.ShowDialog(Instance);         
        }

        private static void _cnt_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Instance.BeginInvoke(Instance.SqlInfoEvent, new object[] { e.Message });
        }

        private void GetObjectUniversal()
        {
            SqlDataReader reader = null;

            try
            {
                if (SaveAsSql)
                {
                    Stream myStream;
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    saveFileDialog1.Filter = "txt files (*.sql)|*.sql|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;

                    if (DialogService.ShowDialog(saveFileDialog1) == DialogResult.OK)
                    {
                        if ((myStream = saveFileDialog1.OpenFile()) != null)
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes(query);
                            myStream.Write(bytes, 0, bytes.Length);
                            myStream.Close();
                        }
                    }
                }
                else
                {
                    SqlCommand command = new SqlCommand(query, cnt);
                    reader = command.ExecuteReader();

                    // Делаем шапку
                    //var wSheet = Application.ActiveWorkbook.ActiveSheet;

                    int lLastCol = wSheet.Cells[1, wSheet.Columns.Count].End(Excel.XlDirection.xlToLeft).Column; // находим последнюю колонку
                    int lLastRow = wSheet.Cells[wSheet.Rows.Count, 1].End(Excel.XlDirection.xlUp).Row; // Последнюю строку
                    Excel.Worksheet s = wSheet;
                    s.Range[s.Cells[1, 1], s.Cells[lLastRow, lLastCol]].Clear();
                    object[] arr = new object[reader.FieldCount];
                    //reader.g
                    for (int j = 0, c = reader.FieldCount; j < c; j++)
                    {
                        arr[j] = reader.GetName(j);
                    }

                    wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[1, reader.FieldCount]].Value = arr;

                    
                    int i = 2;
                    while (reader.Read())
                    {
                        //this.BeginInvoke(UpdateProgressbar, new object[] { 2, lLastRow, i });
                        //Thread.Sleep(100);
                        if (this.StopExecute)
                        {
                            reader?.Close();
                            this.BeginInvoke(UserEndExecuteEvent);
                            return;
                        };
                        try
                        {
                            for (int j = 0, c = reader.FieldCount; j < c; j++)
                            {
                                try
                                {
                                    //var res = reader[reader.GetName(j)];
                                    var res = reader[reader.GetName(j)];
                                    if (res.GetType() == typeof(byte[]))
                                    {
                                        arr[j] = "'0x" + BitConverter.ToString((byte[])res).Replace("-", "");
                                    }
                                    else if (res.GetType() == typeof(System.DBNull))
                                    {
                                        arr[j] = "'null";
                                    }
                                    else
                                    {
                                        arr[j] = "'" + res.ToString();
                                    }
                                }
                                catch { }
                            }
                            wSheet.Range[wSheet.Cells[i, 1], wSheet.Cells[i, reader.FieldCount]].Value = arr;
                        }
                        catch (Exception ex)
                        {
                            this.BeginInvoke(ErrorInThread, new object[] { ex, "GetObjectUniversal" });
                        }
                        i++;
                    }
                    reader.Close();
                    //wSheet.Columns.AutoFit();
                    wSheet.Columns.WrapText = false;
                    //wSheet.Columns.Font.Name = "Lucida Console";
                    //l.WriteDebug("Что то прочитали");
                }

            }
            catch (Exception exc)
            {
                this.BeginInvoke(ErrorInThread, new object[] { exc, "GetObjectUniversal" });
                this.BeginInvoke(DebugInThread, new object[] { query });
            }
            reader?.Close();
            this.BeginInvoke(EndExecuteEvent);

        }

        public static void GetObjectUniversal(Excel.Worksheet _wSheet, SqlConnection _cnt, ItemChanger _ic, bool _SaveAsSql, bool _Check)
        {
            if (!Instance.SqlInfoEventHandled)
            {
                Instance.SqlInfoEventHandled = true;
                _cnt.InfoMessage += _cnt_InfoMessage;
            }
            Instance.StopExecute = false;

            Instance.wSheet = _wSheet;
            Instance.cnt = _cnt;
            Instance.ic = _ic;
            Instance.SaveAsSql = _SaveAsSql;
            Instance.Check = _Check;
            //Instance.xlMain = _xlMain;
            Instance.bar.Style = ProgressBarStyle.Marquee;
            sqlparam mask = new sqlparam();
            mask.WhereString = Instance.ic.GetSQLWhereString; // перенести в xml
            frmSelectTest fSelectTests = new frmSelectTest(mask);
            DialogService.ShowDialog(fSelectTests);
            if (fSelectTests.DialogResult == DialogResult.OK)
            {
                Instance.ic.GetSQLWhereString = ((sqlparam)fSelectTests.obj).WhereString;
                Instance.query = string.Format(Instance.ic.GetSQL, Instance.ic.GetSQLWhereString.Replace('*', '%'));
                Instance.t = new Thread(Instance.GetObjectUniversal);
                Instance.t.Priority = ThreadPriority.Highest;
                Instance.t.Start();
                DialogService.ShowDialog(Instance);

            }
        }

        private void frmExec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                StopExecute = true;
            }
        }

        public static string ConvertValueToSqlString(string val)
        {
            val = (val.ToLower() == "true") ? "1" : val;
            val = (val.ToLower() == "false") ? "0" : val;
            return val;
        }

        private void frmExec_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopExecute = true;
            while (t.ThreadState == System.Threading.ThreadState.Running)
            {
                t.Suspend();
                Thread.Sleep(100);
            }
        }
    }
}
