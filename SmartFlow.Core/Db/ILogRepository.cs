using SmartFlow.Core.Models;
using System.Threading.Tasks;

namespace SmartFlow.Core.Interfaces.Db
{
    public interface ILogRepository
    {
        Task<bool> Create(Log log);
    }
}
