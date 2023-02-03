IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[ActiveProcessStep]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[ActiveProcessStep](
	[Id] [uniqueidentifier] NOT NULL,
	[ProcessId] [uniqueidentifier] NULL,
	[EntityId] [uniqueidentifier] NULL,
	[TransitionId] [uniqueidentifier] NULL,
	[ActionId] [uniqueidentifier] NULL,
	[IsCompleted] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[UserIdAction] [uniqueidentifier] NULL,
	[EntityType] [int] NOT NULL,
 CONSTRAINT [PK_ProcessStep] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [SmartFlow].[ActiveProcessStep] ADD  CONSTRAINT [DF_ProcessStep_Id]  DEFAULT (NEWID()) FOR [Id]

ALTER TABLE [SmartFlow].[ActiveProcessStep] ADD  CONSTRAINT [DF_ProcessStep_IsCompleted]  DEFAULT ((0)) FOR [IsCompleted]

ALTER TABLE [SmartFlow].[ActiveProcessStep] ADD  CONSTRAINT [DF_ProcessStep_CreatedOn]  DEFAULT (GETDATE()) FOR [CreatedOn]

ALTER TABLE [SmartFlow].[ActiveProcessStep] ADD  CONSTRAINT [DF_ProcessStep_ModifiedOn]  DEFAULT (GETDATE()) FOR [ModifiedOn]

ALTER TABLE [SmartFlow].[ActiveProcessStep] ADD  DEFAULT ((1)) FOR [EntityType]

END
