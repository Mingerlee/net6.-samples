using Focus.Repository.Models;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.IService.IServices
{
    public interface ISysProcessService
    {
        Task<ResultModel> GetSysProcessList(SysProcessSc sc);
        Task<ResultModel> AddSysProcess(ReqSysProcess req);
    }
}
