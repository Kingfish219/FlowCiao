IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[Trigger]') AND type in (N'U'))
BEGIN
	CREATE TABLE [FlowCiao].[Trigger](
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](400) NULL,
		[TriggerType] [int] NULL,
		[ProcessId] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_Trigger_1] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [FlowCiao].[Trigger] ADD  CONSTRAINT [DF_Trigger_Id]  DEFAULT (NEWID()) FOR [Id]
END