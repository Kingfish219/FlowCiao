using System.Threading.Tasks;
using FlowCiao.Models;

namespace FlowCiao.Persistence.Interfaces
{
    public interface ILogRepository
    {
        Task<bool> Create(Log log);
    }
}
