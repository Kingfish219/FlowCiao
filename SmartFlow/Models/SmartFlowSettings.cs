
namespace SmartFlow.Models
{
    public class SmartFlowSettings
    {
        public string ConnectionString { get; private set; }
        public bool PersistFlow { get; private set; }

        public void Persist()
        {
            PersistFlow = true;
        }

        public void UseSqlServer(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
