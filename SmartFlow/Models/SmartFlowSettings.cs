
namespace SmartFlow.Models
{
    public class SmartFlowSettings
    {
        public bool PersistFlow { get; private set; }
        public SmartFlowPersistanceSettings PersistanceSettings { get; set; }

        public SmartFlowPersistanceSettings Persist()
        {
            PersistFlow = true;
            PersistanceSettings = new SmartFlowPersistanceSettings();

            return PersistanceSettings;
        }
    }

    public class SmartFlowPersistanceSettings
    {
        public string ConnectionString { get; private set; }
        public void UseSqlServer(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
