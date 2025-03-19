using gk.SQLConfigurator.Config;
using gk.SQLConfigurator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gk.SQLConfigurator.Providers
{
    delegate IDbProvider FabricMethod(ConnectionConfig confing);

    /// <summary>
    /// Фабрика для получения провайдеров разного типа
    /// </summary>
    public static class ProviderFabric
    {
        private static Dictionary<DatabaseType, FabricMethod> _providers = new Dictionary<DatabaseType, FabricMethod>
        {
            { DatabaseType.SqlServer,  ConnectionConfig => new SqlServerProvider(ConnectionConfig)  },
            { DatabaseType.PostgreSql, ConnectionConfig => new PostgreSQLProvider(ConnectionConfig) },            
        };

        /// <summary>
        /// Получить экземпляр провайдера
        /// </summary>
        /// <param name="type">Тип базы данных</param>
        /// <param name="config">Конфиг для подключения</param>
        /// <returns>Провайдера</returns>
        public static IDbProvider GetProvider(DatabaseType type, ConnectionConfig config)
        {
            FabricMethod fabricMethod;
            _providers.TryGetValue(type, out fabricMethod);

            if (fabricMethod == default(FabricMethod))
                return null;

            return fabricMethod(config);
        }
    }
}
