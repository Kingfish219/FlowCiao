using Microsoft.EntityFrameworkCore;

namespace SmartFlow.Persistence.Providers.SqlServer
{
    internal class SmartFlowSqlServerDbContext : DbContext
    {
        private readonly string _connectionString;

        public SmartFlowSqlServerDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
