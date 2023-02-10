using SmartFlow.Core.Models;
using System.Threading.Tasks;

namespace SmartFlow.Core.Repositories
{
    public interface ILogRepository
    {
        Task<bool> Create(Log log);
    }
}
