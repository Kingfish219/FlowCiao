using System.Threading.Tasks;
using SmartFlow.Models;

namespace SmartFlow.Persistence.Interfaces
{
    public interface ILogRepository
    {
        Task<bool> Create(Log log);
    }
}
