using Focus.Repository.Models;
using Focus.IService;
using Infrastructur.AutofacManager;
using Focus.Repository.DBContext;
using Focus.Service.Validations;
using FluentValidation.Results;
using Infrastructure.Models;
using Infrastructure.Enums;
using Infrastructure.CacheManager;
using Microsoft.Extensions.Logging;
using Infrastructure.DEncrypt;
using Infrastructure.UserManager;

namespace Focus.Service
{
    public class SysUserService : IDependency, ISysUserService
    {
        private readonly ILogger<SysUserService> _logger;
        public SysUserService(ILogger<SysUserService> logger)
        {
            _logger = logger;
        }

        public Task<SysUser> Login(ReqSysLogin login)
        {
            using (DbHelper db = DbHelperFactory.Create())
            {
                string sqlStr = "";
            }
            throw new NotImplementedException();
        }

        public async Task<ResultModel> RegisterSysUsers(RegisterSysUser registerSysUser)
        {
            using (DbHelper db = DbHelperFactory.Create())
            {
                DateTime currentDatetime = DateTime.Now;
                db.BeginTransaction();
                try
                {
                    SysUser sysUser = new SysUser
                    {
                        UserName = registerSysUser.UserName,
                        PhoneNumber = registerSysUser.PhoneNumber,
                        UserAddress = registerSysUser.Address,
                        Email = registerSysUser.Email,
                        IsInternal = false,
                        RegDate = currentDatetime
                    };

                    long sysUserId = await db.InsertAsync(sysUser);
                    string encryptSecret = Guid.NewGuid().GetEncryptSecret();
                    SysLogin sysLogin = new SysLogin
                    {
                        UserId = Convert.ToInt32(sysUserId),
                        LoginName = registerSysUser.LoginName,
                        LoginPwd = Encrypts.EncryptMd5(registerSysUser.LoginPwd + encryptSecret),
                        EncryptSecret = encryptSecret,
                        PasswordExpire = currentDatetime.AddYears(1),
                        RetryNo = 0,
                        MaxRetryNo = 5,
                        IsLocked = false,
                        IsActive = false
                    };

                    await db.InsertAsync(sysLogin);

                    bool result = await CreateWorkProcess(SysProcessWorkType.register, Convert.ToInt32(sysUserId), db);

                    if (result)
                    {
                        db.CommitTransaction();
                        return new ResultModel("注册成功");
                    }
                    else
                    {
                        _logger.LogInformation($"注册流程创建失败！");
                        db.RollbackTransaction();
                        return new ResultModel(0, ResponseCode.Register, "注册失败");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    db.RollbackTransaction();
                    return new ResultModel(0, ResponseCode.Register, "注册失败");
                }
            }
        }

        #region 私有辅助方法
        private async Task<bool> CreateWorkProcess(SysProcessWorkType workType, int workId)
        {
            bool results = false;
            using (DbHelper db = DbHelperFactory.Create())
            {
                db.BeginTransaction();
                try
                {
                    string userCode = UserContext.Current.UserInfo.UserCode;
                    userCode = string.IsNullOrEmpty(userCode) ? "U0000001" : userCode;

                    string sql = @"SELECT spa.SysProcessAuditId
                                        FROM dbo.SysProcess sp 
                                        INNER JOIN dbo.SysProcessConfig spc ON spc.ProcessId = sp.ProcessId
                                        INNER JOIN dbo.SysProcessAudit spa ON spa.ProcessConfigId = spc.ProcessConfigId
                                        INNER JOIN dbo.SysWorkProcess swp ON swp.ProcessId = sp.ProcessId
                                        WHERE swp.WorkType=@WorkType AND sp.IsEnable=1 AND swp.IsEnable=1 ";
                    List<int> SysProcessAuditIds = (await db.QueryListAsync<int>(sql, new { @WorkType = workType })).ToList();
                    foreach (int SysProcessAuditId in SysProcessAuditIds)
                    {
                        SysWorkAudit workAudit = new SysWorkAudit
                        {
                            SysProcessAuditId = SysProcessAuditId,
                            WorkId = workId,
                            WorkType = workType,
                            Creator = userCode,
                            CreateDate = DateTime.Now
                        };
                        await db.InsertAsync(workAudit);
                    }

                    db.CommitTransaction();
                    results = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    db.RollbackTransaction();
                    results = false;
                }
            }
            return results;
        }
        private async Task<bool> CreateWorkProcess(SysProcessWorkType workType, int workId, DbHelper db)
        {
            bool results = false;
            try
            {
                string userCode = UserContext.Current.UserInfo.UserCode;
                userCode = string.IsNullOrEmpty(userCode) ? "U0000001" : userCode;

                string sql = @"SELECT spa.SysProcessAuditId
                                        FROM dbo.SysProcess sp 
                                        INNER JOIN dbo.SysProcessConfig spc ON spc.ProcessId = sp.ProcessId
                                        INNER JOIN dbo.SysProcessAudit spa ON spa.ProcessConfigId = spc.ProcessConfigId
                                        INNER JOIN dbo.SysWorkProcess swp ON swp.ProcessId = sp.ProcessId
                                        WHERE swp.WorkType=@WorkType AND sp.IsEnable=1 AND swp.IsEnable=1 ";
                List<int> SysProcessAuditIds = (await db.QueryListAsync<int>(sql, new { @WorkType = workType })).ToList();
                foreach (int SysProcessAuditId in SysProcessAuditIds)
                {
                    SysWorkAudit workAudit = new SysWorkAudit
                    {
                        SysProcessAuditId = SysProcessAuditId,
                        WorkId = workId,
                        WorkType = workType,
                        Creator = userCode,
                        CreateDate = DateTime.Now
                    };
                    await db.InsertAsync(workAudit);
                }

                results = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                results = false;
            }
            return results;
        }
        #endregion
    }
}
