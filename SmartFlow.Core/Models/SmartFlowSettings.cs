
namespace SmartFlow.Core.Models
{
    public class SmartFlowSettings
    {
        public string ConnectionString { get; private set; }
        public bool Persist { get; private set; }

        public void PersistFlow()
        {
            Persist = true;
        }

        public void UseSqlServer(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
