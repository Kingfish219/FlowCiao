IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[ProcessStepComment]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[ProcessStepComment](
	[Id] [uniqueidentifier] NOT NULL,
	[ProcessStepId] [uniqueidentifier] NULL,
	[EntityId] [uniqueidentifier] NOT NULL,
	[StatusId] [uniqueidentifier] NOT NULL,
	[Comment] [nvarchar](1000) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[UserId] [uniqueidentifier] NULL,
	[IsUser] [bit] NOT NULL,
	[Seen] [bit] NULL
) ON [PRIMARY]

END
