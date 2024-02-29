using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace FlowCiao.Models
{
    public class FlowSettings
    {
        public bool PersistFlow { get; private set; }
        public FlowPersistenceSettings PersistenceSettings { get; set; }

        public FlowSettings Persist(Action<FlowPersistenceSettings> settings)
        {
            PersistFlow = true;
            PersistenceSettings = new FlowPersistenceSettings();
            settings(PersistenceSettings);
            
            return this;
        }
        
        // public FlowPersistenceSettings Persist()
        // {
        //     PersistFlow = true;
        //     PersistenceSettings = new FlowPersistenceSettings();
        //
        //     return PersistenceSettings;
        // }
    }

    public class FlowPersistenceSettings
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
