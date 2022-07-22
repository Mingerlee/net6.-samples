using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public class ReqSysProcess
    {
        public string ProcessName { get; set; }
        public SysProcessType ProcessType { get; set; }
        public string ProcessDescription { get; set; }
        public bool IsEnable { get; set; }
        public bool IsAutoAudit { get; set; }
        public List<ReqSysProcessConfig> reqSysProcessConfigs { get; set; }
    }
}
