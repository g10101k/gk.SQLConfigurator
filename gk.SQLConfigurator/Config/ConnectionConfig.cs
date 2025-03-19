using gk.SQLConfigurator.Enums;
using gk.SQLConfigurator.Properties;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace gk.SQLConfigurator.Config
{
    public class ConnectionConfig
    {
        /// <summary>
        /// Тип базы данных
        /// </summary>
        public DatabaseType DBType { get; set; }
        /// <summary>
        /// Адресс сервера
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// База данных
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Авторизация Windows
        /// </summary>
        public bool WindowsAuth { get; set; }
        /// <summary>
        /// Порт
        /// </summary>
        public int Port { get; set; }
    }
}
