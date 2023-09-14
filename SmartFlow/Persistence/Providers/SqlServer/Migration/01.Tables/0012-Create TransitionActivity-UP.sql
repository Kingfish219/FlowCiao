IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[TransitionActivity]') AND type in (N'U'))
BEGIN

CREATE TABLE [FlowCiao].[TransitionActivity](
	[TransitionId] [uniqueidentifier],
	[ActivityId] [uniqueidentifier],
	[Priority] [int] NULL
) ON [PRIMARY]

END
