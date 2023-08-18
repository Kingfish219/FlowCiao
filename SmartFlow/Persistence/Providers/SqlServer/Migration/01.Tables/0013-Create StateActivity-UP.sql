IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[StateActivity]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[StateActivity](
	[StateId] [uniqueidentifier],
	[ActivityId] [uniqueidentifier],
	[Priority] [int] NULL
) ON [PRIMARY]

END
