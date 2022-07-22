using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Focus.Repository.Models
{
    public enum SysProcessType
    {
        [Description("注册")]
        register=0,

        [Description("其他")]
        other =99
    }

    public enum SysAuditMethod
    {
        [Description("指定人员")]
        assign_personnel =1,
        [Description("指定岗位")]
        assign_post = 2,
        [Description("指定角色")]
        assign_roles = 3,
    }

    public enum SysAuditType
    {
        /// <summary>
        /// 或签（一名审批人同意或者拒绝即可）
        /// </summary>
        [Description("或签")]
        or_sign = 1,

        /// <summary>
        /// 会签（必须所有人审核通过）
        /// </summary>
        [Description("会签")]
        and_sign = 2,
    }
}
