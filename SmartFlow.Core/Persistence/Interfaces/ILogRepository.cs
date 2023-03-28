using System.Threading.Tasks;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Persistence.Interfaces
{
    public interface ILogRepository
    {
        Task<bool> Create(Log log);
    }
}
