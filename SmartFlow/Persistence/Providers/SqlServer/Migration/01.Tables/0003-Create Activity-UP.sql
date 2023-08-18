IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[Activity]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[Activity](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](400) NULL,
	[ActivityTypeCode] [int] NULL,
	[Process] [uniqueidentifier] NULL,
	[Executor] [nvarchar](500) NULL,
 CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [SmartFlow].[Activity] ADD  CONSTRAINT [DF_Activity_Id]  DEFAULT (NEWID()) FOR [Id]

END
