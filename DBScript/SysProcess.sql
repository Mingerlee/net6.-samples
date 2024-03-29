--流程主表
CREATE TABLE dbo.SysProcess(
	ProcessId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,--流程Id，主键
	ProcessName NVARCHAR(100) NOT NULL,--流程名称
	ProcessType INT NOT NULL,--流程类型
	--ProcessStatus INT NOT NULL DEFAULT(1),--流程状态
	ProcessDescription NVARCHAR(2000) NULL,--流程描述
	CreateDate DATETIME NOT NULL DEFAULT(GETDATE()),--创建时间
	Creator VARCHAR(64) NOT NULL DEFAULT('U0000001'),--创建人
	IsEnable BIT NOT NULL DEFAULT(1), --是否启用，默认为启用
	IsAutoAudit BIT NOT NULL DEFAULT(0) --相邻两个审核人为同一个人时，是否自动审核
)
--流程配置表
CREATE TABLE dbo.SysProcessConfig(
	ProcessConfigId BIGINT PRIMARY KEY IDENTITY(1,1) NOT NULL,--流程配置表Id，主键
	ProcessId INT NOT NULL,--流程Id
	ProcessStep INT NOT NULL,--流程步骤
	AuditMethod INT NOT NULL,--审核方式：按指定人员审核，按指定岗位、按指定角色
	AuditType INT NOT NULL,--审核类别：或签（一名审批人同意或者拒绝即可），会签（必须所有人审核通过）
	CreateDate DATETIME NOT NULL DEFAULT(GETDATE()),--创建时间
	Creator VARCHAR(64) NOT NULL DEFAULT('U0000001'),--创建人
	--IsEnable BIT NOT NULL DEFAULT(1) --是否启用，默认为启用
)
--流程审核配置表
CREATE TABLE dbo.SysProcessAudit(
	SysProcessAuditId BIGINT PRIMARY KEY IDENTITY(1,1) NOT NULL,--流程审核表Id,主键
	ProcessConfigId BIGINT NOT NULL,--流程配置表Id，主键
	AuditUser VARCHAR(50) NULL,--审核人（Code或者Id）
	AuditPosition VARCHAR(50) NULL,--审核岗位（Code或者Id）
	AuditRole VARCHAR(50) NULL,--审核角色（Code或者Id）
	CreateDate DATETIME NOT NULL DEFAULT(GETDATE())--创建时间
)
--业务流程关联表
CREATE TABLE dbo.SysWorkProcess(
	ProcessId INT NOT NULL,--流程Id
	WorkType INT NOT NULL,--系统业务类别Id，Tips：注册主表--1
	IsEnable BIT NOT NULL DEFAULT(1), --是否启用，默认为启用
	Creator VARCHAR(64) NOT NULL DEFAULT('U0000001'),--创建人
	CreateDate DATETIME NOT NULL DEFAULT(GETDATE())--创建时间
)
--业务流程审核表
CREATE TABLE dbo.SysWorkAudit(
	SysProcessAuditId INT NOT NULL,--流程Id
	WorkId INT NOT NULL,--系统业务Id，Tips：注册主表的Id
	WorkType INT NOT NULL,--系统业务类别Id，Tips：注册主表--1
	AuditTime DATETIME NULL,--审核时间，默认为当前系统时间
	AuditStatus INT NULL,--审核状态：审核通过，驳回
	RejectionReason NVARCHAR(4000) NULL, --驳回原因
	--IsEnable BIT NOT NULL DEFAULT(1), --是否启用，默认为启用
	Creator VARCHAR(64) NOT NULL DEFAULT('U0000001'),--创建人
	CreateDate DATETIME NOT NULL DEFAULT(GETDATE())--创建时间
)