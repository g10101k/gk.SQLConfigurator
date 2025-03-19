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
    /// <summary>
    /// Провайдер для PostgresSQL
    /// </summary>
    public class PostgreSQLProvider : IDbProvider
    {
        /// <summary>
        /// Запрос для получения данных
        /// </summary>
        private const string GetDatabaseName = "SELECT datname FROM pg_database WHERE datistemplate = false ORDER BY datname";
        private ConnectionConfig _connectionConfig;

        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string DBName => _connectionConfig.Database;
        /// <summary>
        /// Имя сервера
        /// </summary>
        public string Server => _connectionConfig.Server;

        public PostgreSQLProvider(ConnectionConfig cfg) 
        {
            _connectionConfig = cfg;
        }

        /// <summary>
        /// Получить соединие с Postgres базой
        /// </summary>
        /// <returns>Подключение</returns>
        private NpgsqlConnection GetConnection()
        {
            NpgsqlConnectionStringBuilder scsb = new NpgsqlConnectionStringBuilder();

            scsb.Host     = _connectionConfig.Server;
            scsb.Database = _connectionConfig.Database;
            scsb.Username = _connectionConfig.Username;
            scsb.Password = _connectionConfig.Password;
            scsb.Port     = _connectionConfig.Port;

            return new NpgsqlConnection(scsb.ConnectionString);
        }
        /// <summary>
        /// Прошло ли подключение
        /// </summary>
        /// <returns>True - если прошло, False - если нет</returns>
        public bool CheckConnection()
        {
            try
            {
                using (NpgsqlConnection connection = GetConnection())
                {
                    connection.Open();

                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Получить список баз данных на сервере
        /// </summary>
        /// <returns>Список баз данных</returns>
        public IEnumerable<string> GetDatabaseList()
        {
            NpgsqlConnectionStringBuilder scsb = new NpgsqlConnectionStringBuilder();

            scsb.Host     = _connectionConfig.Server;
            // Ставим базу postgres поскольку она всегда
            // Существует
            scsb.Database = "postgres";
            scsb.Username = _connectionConfig.Username;
            scsb.Password = _connectionConfig.Password;
            scsb.Port     = _connectionConfig.Port;

            using (NpgsqlConnection connection = new NpgsqlConnection(scsb.ConnectionString))
            {
                DataTable dt = SqlUtils.Execute(connection, GetDatabaseName);

                if (dt == null || dt.Rows.Count == 0) return null;

                return dt.AsEnumerable().Select(row => row["datname"].ToString());
            }
        }
        /// <summary>
        /// Запустить запрос на изменение данных
        /// </summary>
        /// <param name="cmd">запрос</param>
        /// <returns>Количество измененных строк</returns>
        public int ExecuteNonQuery(string cmd)
        {
            using (NpgsqlConnection connection = GetConnection())
                return SqlUtils.ExecuteNonQuery(connection, cmd);
        }
        /// <summary>
        /// Получить данные из базы данных
        /// </summary>
        /// <param name="cmd">Запрос на получение</param>
        /// <returns>Таблицу с результатами</returns>
        public DataTable Execute(string cmd)
        {
            using (NpgsqlConnection connection = GetConnection())
                return SqlUtils.Execute(connection, cmd);
        }
    }
}
