IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[TransitionTrigger]') AND type in (N'U'))
BEGIN

CREATE TABLE [FlowCiao].[TransitionTrigger](
	[Id] [uniqueidentifier] NOT NULL,
	[TriggerId] [uniqueidentifier] NOT NULL,
	[TransitionId] [uniqueidentifier] NOT NULL,
	[Priority] [int] NULL,
 CONSTRAINT [PK_TransitionTrigger] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [FlowCiao].[TransitionTrigger] ADD  CONSTRAINT [DF_TransitionTrigger_Id]  DEFAULT (NEWID()) FOR [Id]

END
