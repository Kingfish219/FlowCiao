IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[StateActivity]') AND type in (N'U'))
BEGIN

CREATE TABLE [FlowCiao].[StateActivity](
	[StateId] [uniqueidentifier],
	[ActivityId] [uniqueidentifier],
	[Priority] [int] NULL
) ON [PRIMARY]

END
