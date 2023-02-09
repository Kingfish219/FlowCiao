IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[ProcessStepHistoryActivity]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[ProcessStepHistoryActivity](
	[Id] [uniqueidentifier] NOT NULL,
	[ActivityId] [uniqueidentifier] NULL,
	[ActivityName] [nvarchar](400) NULL,
	[StepType] [int] NULL,
	[ProcessStepHistoryId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [SmartFlow].[ProcessStepHistoryActivity] ADD  DEFAULT (NEWID()) FOR [Id]

END
