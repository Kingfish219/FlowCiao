IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[ProcessStepHistory]') AND type in (N'U'))
BEGIN

CREATE TABLE [FlowCiao].[ProcessStepHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[ProcessId] [uniqueidentifier] NULL,
	[EntityId] [uniqueidentifier] NULL,
	[TransitionId] [uniqueidentifier] NULL,
	[ActionId] [uniqueidentifier] NULL,
	[IsCompleted] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[FinishedOn] [datetime] NULL,
	[UserIdAction] [uniqueidentifier] NULL,
	[EntityType] [int] NULL,
	[OperationTypeCode] [int] NOT NULL
) ON [PRIMARY]

END
