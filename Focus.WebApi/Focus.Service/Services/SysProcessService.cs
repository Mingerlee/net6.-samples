using Focus.IService.IServices;
using Focus.Repository.DBContext;
using Focus.Repository.Models;
using Infrastructur.AutofacManager;
using Infrastructure.Models;
using Infrastructure.UserManager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Service.Services
{
    public class SysProcessService: IDependency, ISysProcessService
    {
        private readonly ILogger _logger;
        public SysProcessService(ILogger<SysProcessService> logger)
        {
            _logger = logger;
        }
        public async Task<ResultModel> GetSysProcessList(SysProcessSc sc)
        {
            using (DbHelper db=DbHelperFactory.Create())
            {
                string sql = "SELECT * FROM SysProcess ";
                ListPage<SysProcess> listPage =await db.QueryListAsync<SysProcess>(sql, sc);
                return new ResultModel(listPage);
            }
        }
        public async Task<ResultModel> AddSysProcess(ReqSysProcess req)
        {
            using (DbHelper db = DbHelperFactory.Create())
            {
                //string sql = "SELECT * FROM SysProcess ";
                //ListPage<SysProcess> listPage = await db.QueryListAsync<SysProcess>(sql, sc);
                db.BeginTransaction();
                try 
                {
                    string userCode=UserContext.Current.UserInfo.UserCode;
                    userCode = string.IsNullOrEmpty(userCode) ? "U0000001" : userCode;
                    DateTime currentDateTime = DateTime.Now;

                    //主表 sysProcess
                    SysProcess sysProcess = new SysProcess();
                    sysProcess.ProcessName = req.ProcessName;
                    sysProcess.ProcessType = req.ProcessType;
                    sysProcess.ProcessDescription=req.ProcessDescription;
                    sysProcess.IsAutoAudit = req.IsAutoAudit;
                    sysProcess.IsEnable=req.IsEnable;
                    sysProcess.Creator = userCode;
                    sysProcess.CreateDate = currentDateTime;

                    long sysProcessId =await db.InsertAsync(sysProcess);

                    foreach (var reqSysProcessConfig in req.reqSysProcessConfigs)
                    {
                        SysProcessConfig sysProcessConfig = new SysProcessConfig();
                        sysProcessConfig.ProcessId =Convert.ToInt32(sysProcessId);
                        sysProcessConfig.ProcessStep = reqSysProcessConfig.ProcessStep;
                        sysProcessConfig.AuditMethod = reqSysProcessConfig.AuditMethod;
                        sysProcessConfig.AuditType=reqSysProcessConfig.AuditType;
                        sysProcessConfig.Creator = userCode;
                        sysProcessConfig.CreateDate = currentDateTime;
                        long sysProcessConfigId= await db.InsertAsync(sysProcessConfig);

                        foreach (var reqSysProcessAudit in reqSysProcessConfig.reqSysProcessAudits)
                        {
                            SysProcessAudit sysProcessAudit = new SysProcessAudit {
                                ProcessConfigId = Convert.ToInt32(sysProcessConfigId),
                                AuditUser=reqSysProcessAudit.AuditUser,
                                AuditRole=reqSysProcessAudit.AuditRole,
                                AuditPosition=reqSysProcessAudit.AuditPosition,
                                CreateDate = currentDateTime
                            };
                            await db.InsertAsync(sysProcessAudit);
                        }
                    }

                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    _logger.LogError(ex, ex.Message);
                    return new ResultModel("添加失败！");
                }

                return new ResultModel();
            }
        }
    }
}
