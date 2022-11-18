using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.Core.Models
{
    public class EditWorkFlowCurrentStatus
    {
        public Guid CurrentStateId { get; set; }
        public string Name { get; set; }
    }
    public class EditWorkFlowNextStatus
    {
        public Guid NextStateId { get; set; }
        public string Name { get; set; }
    }
}
