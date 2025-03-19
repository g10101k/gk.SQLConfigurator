using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gk.SQLConfigurator.Utils
{
    public static class SqlUtils
    {
        /// <summary>
        /// Получить данные из базы данных
        /// </summary>
        /// <param name="connection">Подключение</param>
        /// <param name="cmd">запрос</param>
        /// <returns>Таблицу с результатами запроса</returns>
        public static DataTable Execute(DbConnection connection, string cmd)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                DbCommand command = connection.CreateCommand();
                command.CommandText = cmd;

                DbDataReader reader = command.ExecuteReader();

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                return dataTable;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Запустить запрос на изменение данных
        /// </summary>
        /// <param name="connection">Подключени</param>
        /// <param name="cmd">Запрос</param>
        /// <returns>Кол-во измененных строк</returns>
        public static int ExecuteNonQuery(DbConnection connection, string cmd)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                DbCommand command = connection.CreateCommand();
                command.CommandText = cmd;

                int rowChangeCount = command.ExecuteNonQuery();

                return rowChangeCount;
            }
            catch 
            { 
                return 0; 
            }
        }
    }
}
