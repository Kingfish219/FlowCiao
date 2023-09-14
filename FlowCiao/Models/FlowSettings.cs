using System.Data;
using Microsoft.Data.SqlClient;

namespace FlowCiao.Models
{
    public class FlowSettings
    {
        public bool PersistFlow { get; private set; }
        public FlowPersistanceSettings PersistanceSettings { get; set; }

        public FlowPersistanceSettings Persist()
        {
            PersistFlow = true;
            PersistanceSettings = new FlowPersistanceSettings();

            return PersistanceSettings;
        }
    }

    public class FlowPersistanceSettings
    {
        public string ConnectionString { get; private set; }
        public void UseSqlServer(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
