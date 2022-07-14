using Focus.Repository.Models.DbEntitys;
using Focus.IService;
using Infrastructur.AutofacManager;
using Focus.Repository.DBContext;
using Focus.Service.Validations;
using FluentValidation.Results;
using Infrastructure.Models;
using Infrastructure.Enums;
using Infrastructure.CacheManager;

namespace Focus.Service
{
    //
    public class SysUserService : IDependency, ISysUserService
    {
        public SysUserService()
        {
            
        }
        public async Task<SysUser> GetSysUser(int id)
        {
            //SysUser SysUser = default(SysUser);
            using (DbHelper db = DbHelperFactory.Create())
            {
                string sql = "SELECT * FROM dbo.SysUser WHERE UserId=@UserId";
                return await db.QueryModelAsync<SysUser>(sql, new { @UserId = id });
            }
            //return SysUser;
            //throw new NotImplementedException();
        }
        public async Task<SysUser> GetSysUsers(int id)
        {
            //using (DbHelper db = DbHelperFactory.Create())
            //{
                string sql = "SELECT * FROM dbo.SysUser WHERE UserId=@UserId";
                return await DbHelperFactory.Create().QueryModelAsync<SysUser>(sql, new { @UserId = id });
            //}
        }
        public SysUser GetSysUserById(int id)
        {
            using (DbHelper db = DbHelperFactory.Create())
            {
                //string sql = "SELECT * FROM dbo.SysUser WHERE UserId=@UserId";
                return db.QueryById<SysUser>(id);
            }
        }
        public async Task<ResultModel<SysUser>> GetSysUser(SysUser sysUser)
        {
            var result = new ResultModel<SysUser>();
            SysUserValidation validationRules = new SysUserValidation();
            ValidationResult validaResult = await validationRules.ValidateAsync(sysUser);
            if (!validaResult.IsValid)   //校验通过
            {
                return new ResultModel<SysUser>(0, ResponseCode.sys_verify_failed, validaResult.ToString("||"));
            }
            return new ResultModel<SysUser>(1, ResponseCode.sys_success, "");
            //throw new NotImplementedException();
        }
    }
}
