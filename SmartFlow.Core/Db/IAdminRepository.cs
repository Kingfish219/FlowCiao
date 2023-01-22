using SmartFlow.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Core.Db
{
    public interface IAdminRepository
    {
        Task<FuncResult<List<EditWorkFlowProcess>>> GetAllProccess();
        Task<FuncResult> CreateProccess(EditWorkFlowProcess workFlowProcessViewModel);
        Task<FuncResult> UpdateProccess(EditWorkFlowProcess workFlowProcessViewModel);
        Task<FuncResult> DeleteProccess(EditWorkFlowProcess workFlowProcessViewModel);
        Task<List<EditWorkFlowTransition>> GetProcessTransitions(Guid ProcessId);
        Task<List<EditWorkFlowAction>> GetAllAction();
        Task<List<EditWorkFlowCurrentStatus>> GetAllCurrentState();
        Task<List<EditWorkFlowNextStatus>> GetAllNextState();
        Task<FuncResult> CreateProcessTransition(EditWorkFlowTransition workFlowTransition);
        Task<FuncResult> UpdateProcessTransition(EditWorkFlowTransition workFlowTransition);
        Task<FuncResult> DeleteProcessTransition(EditWorkFlowTransition workFlowTransition);
        Task<List<EditCompanyProcess>> GetAllCompanyProcess(Guid CompanyId);
        Task<List<EditProcess>> GetAllEditProcess();
        Task<FuncResult> UpdateCompanyProcess(EditCompanyProcess companyProcess);
    }
}
