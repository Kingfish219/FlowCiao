IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[ActionType]') AND type in (N'U'))
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
	[OperationTypeCode] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [FlowCiao].[ProcessStepHistory] ADD  DEFAULT (NEWID()) FOR [Id]

ALTER TABLE [FlowCiao].[ProcessStepHistory] ADD  CONSTRAINT [DF_ProcessStepHistory]  DEFAULT (GETDATE()) FOR [FinishedOn]

ALTER TABLE [FlowCiao].[ProcessStepHistory] ADD  DEFAULT ((0)) FOR [OperationTypeCode]

END
