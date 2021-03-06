CREATE TABLE [dbo].[SysLogEmails]
(
[LogEmailId] [int] NOT NULL IDENTITY(1, 1),
[TemplateEmailId] [int] NOT NULL,
[Key] [int] NULL,
[KeyType] [int] NULL,
[TriggerByUserId] [int] NOT NULL,
[AccessIP] [nvarchar] (45) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL,
[FromAddress] [nvarchar] (150) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL,
[ToAddresses] [nvarchar] (500) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL,
[CcAddresses] [nvarchar] (500) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL,
[BccAddresses] [nvarchar] (500) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL,
[Subject] [nvarchar] (255) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL,
[Body] [nvarchar] (max) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL,
[IsHTML] [bit] NOT NULL,
[AttachmentsCount] [int] NOT NULL,
[TimeStamp] [datetime] NOT NULL CONSTRAINT [DF__LogEmails__TimeS__114A936A] DEFAULT (getdate()),
[IsSuccess] [bit] NOT NULL CONSTRAINT [DF__LogEmails__IsSuc__151B244E] DEFAULT ((0)),
[ErrorMessage] [nvarchar] (1024) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL,
[Body_Text] [nvarchar] (max) COLLATE Chinese_Hong_Kong_Stroke_90_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[SysLogEmails] ADD CONSTRAINT [PK_dbo.SysLogEmails] PRIMARY KEY CLUSTERED ([LogEmailId]) ON [PRIMARY]
GO