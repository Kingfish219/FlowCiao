IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[TransitionActivity]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[TransitionActivity](
	[TransitionId] [uniqueidentifier],
	[ActivityId] [uniqueidentifier],
	[Priority] [int] NULL
) ON [PRIMARY]

END
