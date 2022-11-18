using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Core.Interfaces
{
    public interface ILogRepository
    {
        Task<bool> Create(Log log);
    }
}
