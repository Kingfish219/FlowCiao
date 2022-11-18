using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartFlow.Core.Models
{
    public class EditCompanyProcess
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }
        [UIHint("ProcessDropdown")]
        public EditProcess Process { get; set; }
        public string RequestTypeName { get; set; }
    }
    public class EditProcess {
        public Guid ProcessId { get; set; }
        public String ProcessName { get; set; }
    }
}
