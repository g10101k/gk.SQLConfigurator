using gk.SQLConfigurator.Enums;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace gk.SQLConfigurator.Config
{
    public class ApplicationConfig
    {
        /// <summary>
        /// Параметры подключения
        /// </summary>
        public ConnectionConfig ConnectionCfg { get; set; }
        /// <summary>
        /// Индекс выбранного объекта
        /// </summary>
        public int SelectedObjectIndex { get; set; }
        public bool FirstStart { get; set; }
        public int EditorType { get; set; }
        public string PanelName { get; set; }
        public string UpdatePath { get; set; }

        private static ApplicationConfig _instance = null;

        public static ApplicationConfig Instance
        {
            get
            {
                if (_instance == null)
                    CreateInstance();

                return _instance;
            }
        }

        private ApplicationConfig()
        {
            Properties.Settings.Default.SettingChanging += LoadFromSettings;
            ConnectionCfg = new ConnectionConfig();
            LoadFromSettings(this, null);
        }

        private void LoadFromSettings(object sender, SettingChangingEventArgs e)
        {
            // Не пересохраняем настройки, если измнения вызвали этот класс
            if (sender == null && typeof(ApplicationConfig).IsAssignableFrom(sender.GetType()))
                return;

            SelectedObjectIndex = Properties.Settings.Default.SelectedObjectIndex;
            FirstStart          = Properties.Settings.Default.FirstStart;
            EditorType          = Properties.Settings.Default.EditorType;
            PanelName           = Properties.Settings.Default.PanelName;
            UpdatePath          = Properties.Settings.Default.UpdatePath;

            ConnectionCfg.Server   = Properties.Settings.Default.Server;
            ConnectionCfg.Database = Properties.Settings.Default.Database;
            ConnectionCfg.Username = Properties.Settings.Default.Username;
            ConnectionCfg.Password = Properties.Settings.Default.Password;
            ConnectionCfg.Port     = Properties.Settings.Default.Port;
            ConnectionCfg.DBType   = (DatabaseType)Enum.Parse(typeof(DatabaseType), Properties.Settings.Default.DatabaseType, true);
        }

        public static void CreateInstance()
        {
            _instance = new ApplicationConfig();
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }
    }
}
