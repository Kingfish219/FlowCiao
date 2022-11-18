using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SmartFlow.Core.Models
{
    public class EditWorkFlowProcess
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Guid Owner { get; set; }
        public int EntityType { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefultProccess { get; set; }
    }
}
