using gk.SQLConfigurator.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using gk.SQLConfigurator.Utils;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace gk.SQLConfigurator.Providers
{
    public class SqlServerProvider : IDbProvider
    {
        /// <summary>
        /// Запрос для получения данных
        /// </summary>
        private const string GetDatabaseName = "SELECT name from sys.databases";
        private ConnectionConfig _connectionConfig;
        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string DBName => _connectionConfig.Database;
        /// <summary>
        /// Имя сервера
        /// </summary>
        public string Server => _connectionConfig.Server;

        public SqlServerProvider(ConnectionConfig cfg) 
        {
            _connectionConfig = cfg;    
        }

        /// <summary>
        /// Получить соединие с MS SQL базой
        /// </summary>
        /// <returns>Подключение</returns>
        private SqlConnection GetConnection()
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();

            scsb.DataSource = _connectionConfig.Server;
            scsb.InitialCatalog = _connectionConfig.Database;

            scsb.IntegratedSecurity = _connectionConfig.WindowsAuth;

            if (!_connectionConfig.WindowsAuth)
            {
                scsb.UserID = _connectionConfig.Username;
                scsb.Password = _connectionConfig.Password;
            }

            return new SqlConnection(scsb.ConnectionString);
        }
        /// <summary>
        /// Прошло ли подключение
        /// </summary>
        /// <returns>True - если прошло, False - если нет</returns>
        public bool CheckConnection()
        {
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();

                return connection.State == System.Data.ConnectionState.Open;
            }
        }
        /// <summary>
        /// Получить список баз данных на сервере
        /// </summary>
        /// <returns>Список баз данных</returns>
        public IEnumerable<string> GetDatabaseList()
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();

            scsb.DataSource = _connectionConfig.Server;
            scsb.IntegratedSecurity = _connectionConfig.WindowsAuth;

            if (!_connectionConfig.WindowsAuth)
            {
                scsb.UserID = _connectionConfig.Username;
                scsb.Password = _connectionConfig.Password;
            }
            
            using (SqlConnection connection = new SqlConnection(scsb.ConnectionString))
            {
                DataTable dt = SqlUtils.Execute(connection, GetDatabaseName);

                if (dt == null || dt.Rows.Count == 0) return null;

                return dt.AsEnumerable().Select(row => row["name"].ToString());
            }
        }
        /// <summary>
        /// Запустить запрос на изменение данных
        /// </summary>
        /// <param name="cmd">запрос</param>
        /// <returns>Количество измененных строк</returns>
        public int ExecuteNonQuery(string cmd)
        {
            using (SqlConnection connection = GetConnection())
            {
                return SqlUtils.ExecuteNonQuery(connection, cmd);
            }
        }
        /// <summary>
        /// Получить данные из базы данных
        /// </summary>
        /// <param name="cmd">Запрос на получение</param>
        /// <returns>Таблицу с результатами</returns>
        public DataTable Execute(string cmd)
        {
            using (SqlConnection connection = GetConnection())
            {
                return SqlUtils.Execute(connection, cmd);
            }
        }
    }
}
