using Autofac.Extras.DynamicProxy;
using Focus.DynamicProxys.Interceptors;
using Focus.Repository.Models;
using Infrastructur.AutofacManager;
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
        Task<ResultModel<string>> RegisterSysUsers(RegisterSysUser registerSysUser);
        Task<SysUser> Login(ReqSysLogin login);

    }
}
