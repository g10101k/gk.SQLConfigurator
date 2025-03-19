using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gk.SQLConfigurator.Providers
{
    public interface IDbProvider
    {
        /// <summary>
        /// Имя базы данных
        /// </summary>
        string DBName { get; }
        /// <summary>
        /// Имя сервера
        /// </summary>
        string Server { get; }

        /// <summary>
        /// Получить список баз данных на сервере
        /// </summary>
        /// <returns>Список баз данных</returns>
        IEnumerable<string> GetDatabaseList();
        /// <summary>
        /// Прошло ли подключение
        /// </summary>
        /// <returns>True - если прошло, False - если нет</returns>
        bool CheckConnection();
        /// <summary>
        /// Запустить запрос на изменение данных
        /// </summary>
        /// <param name="cmd">запрос</param>
        /// <returns>Количество измененных строк</returns>
        int ExecuteNonQuery(string cmd);
        /// <summary>
        /// Получить данные из базы данных
        /// </summary>
        /// <param name="cmd">Запрос на получение</param>
        /// <returns>Таблицу с результатами</returns>
        DataTable Execute(string cmd);
    }
}
