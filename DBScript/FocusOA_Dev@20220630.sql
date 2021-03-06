USE [Focus]
GO
/****** Object:  Table [dbo].[SysUser]    Script Date: 2022/6/30 9:15:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUser](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserCode] [varchar](50) NULL,
	[UserName] [varchar](255) NULL,
	[UserPwd] [varchar](50) NULL,
	[UserPwdTimesTamp] [varchar](20) NULL,
	[UserStatus] [int] NULL,
	[LastLoginTime] [datetime] NULL,
	[JobId] [int] NULL,
	[AddTime] [datetime] NULL,
	[PhoneNumber] [varchar](11) NULL,
	[CompanyName] [varchar](255) NULL,
	[Valid] [tinyint] NOT NULL,
 CONSTRAINT [PK_SysUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SysSqlLog]    Script Date: 2022/6/30 9:15:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysLogSQL](
	[LogCode] [varchar](100) NULL,
	[UserCode] [varchar](50) NULL,
	[LogTime] [datetime] NULL,
	[LogContent] [nvarchar](max) NULL,
	[LogIpAddress] [varchar](255) NULL,
	[ExecTime] DECIMAL(18,4) NOT NULL
) ON [PRIMARY]
GO
