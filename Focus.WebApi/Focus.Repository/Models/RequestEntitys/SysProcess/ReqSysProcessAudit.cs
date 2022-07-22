using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class ReqSysProcessAudit
    {
        public string? AuditUser { get; set; }
        public string? AuditPosition { get; set; }
        public string? AuditRole { get; set; }
    }
}
