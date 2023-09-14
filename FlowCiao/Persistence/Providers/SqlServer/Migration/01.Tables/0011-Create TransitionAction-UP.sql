IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[TransitionAction]') AND type in (N'U'))
BEGIN

CREATE TABLE [FlowCiao].[TransitionAction](
	[Id] [uniqueidentifier] NOT NULL,
	[ActionId] [uniqueidentifier] NOT NULL,
	[TransitionId] [uniqueidentifier] NOT NULL,
	[Priority] [int] NULL,
 CONSTRAINT [PK_TransitionAction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [FlowCiao].[TransitionAction] ADD  CONSTRAINT [DF_TransitionAction_Id]  DEFAULT (NEWID()) FOR [Id]

END
