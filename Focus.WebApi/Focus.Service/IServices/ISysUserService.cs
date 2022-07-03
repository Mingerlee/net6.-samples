using Focus.Repository.Models.DbEntitys;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Service.IServices
{
    public interface ISysUserService
    {
        public Task<SysUser> GetSysUser(int id);
        public Task<SysUser> GetSysUsers(int id);
        public SysUser GetSysUserById(int id);
        public Task<ResultModel<SysUser>> GetSysUser(SysUser sysUser);

    }
}
