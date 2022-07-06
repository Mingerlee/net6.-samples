using Focus.Repository.Models.DbEntitys;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.IService
{
    public interface ISysUserService
    {
        Task<SysUser> GetSysUser(int id);
        Task<SysUser> GetSysUsers(int id);
        SysUser GetSysUserById(int id);
        Task<ResultModel<SysUser>> GetSysUser(SysUser sysUser);

    }
}
