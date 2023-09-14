IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[Action]') AND type in (N'U'))
BEGIN
	CREATE TABLE [FlowCiao].[Action](
		[Id] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](400) NULL,
		[ActionTypeCode] [int] NULL,
		[ProcessId] [uniqueidentifier] NULL,
	 CONSTRAINT [PK_Action_1] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [FlowCiao].[Action] ADD  CONSTRAINT [DF_Action_Id]  DEFAULT (NEWID()) FOR [Id]
END