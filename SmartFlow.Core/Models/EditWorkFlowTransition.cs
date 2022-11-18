
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.Core.Models
{
    public class EditWorkFlowTransition
    {
       public Guid Id { get; set; }
       public Guid ProcessId { get; set; }
       [UIHint("CurrentStateDropdown")]
       public EditWorkFlowCurrentStatus CurrentState { get; set;}
       [UIHint("NextStateDropdown")]
       public EditWorkFlowNextStatus NextState { get; set;}
       public Guid TransitionActionId { get; set;}
       [UIHint("ActionDropdown")]
       public EditWorkFlowAction Action { get; set;}
    }
}
