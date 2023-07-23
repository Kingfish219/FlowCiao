using Microsoft.EntityFrameworkCore;

namespace SmartFlow.Persistence
{
    internal class SmartFlowDbContext : DbContext
    {
        private readonly string _connectionString;

        public SmartFlowDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
