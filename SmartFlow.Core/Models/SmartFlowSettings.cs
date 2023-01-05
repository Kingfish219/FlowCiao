
namespace SmartFlow.Core.Models
{
    public class SmartFlowSettings
    {
        public string ConnectionString { get; private set; }

        public void UseSqlServer(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
