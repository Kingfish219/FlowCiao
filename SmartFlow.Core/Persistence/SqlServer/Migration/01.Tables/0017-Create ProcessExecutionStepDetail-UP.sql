IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[ProcessExecutionStepDetail]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[ProcessExecutionStepDetail](
	[Id] [UNIQUEIDENTIFIER] NOT NULL,
	[ProcessExecutionStepId] [UNIQUEIDENTIFIER] NOT NULL,
	[EntityId] [UNIQUEIDENTIFIER] NULL,
	[TransitionId] [UNIQUEIDENTIFIER] NULL,
	[ActionId] [UNIQUEIDENTIFIER] NULL,
	[IsCompleted] [BIT] NULL,
	[CreatedOn] [DATETIME] NOT NULL,
	[ModifiedOn] [DATETIME] NULL,
	[UserIdAction] [UNIQUEIDENTIFIER] NULL,
	[EntityType] [INT] NOT NULL,
 CONSTRAINT [PK_ProcessExecutionStepDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY]

END
