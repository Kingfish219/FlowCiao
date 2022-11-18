using System.Threading.Tasks;

namespace SmartFlow.Core.Interfaces
{
    public interface IEntityCreateHistory
    {
        Task<bool> Create(SmartFlow.Core.Models.ProcessStep processStep);
    }
}
