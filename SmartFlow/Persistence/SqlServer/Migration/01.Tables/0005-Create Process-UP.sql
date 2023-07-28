IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[Process]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[Process](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](400) NULL,
	[FlowKey] [nvarchar](200) NULL,
	[Owner] [uniqueidentifier] NULL,
	[EntityType] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[IsDefultProccess] [bit] NOT NULL
 CONSTRAINT [PK_Process] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [SmartFlow].[Process] ADD  CONSTRAINT [DF_Process_Id]  DEFAULT (NEWID()) FOR [Id]

ALTER TABLE [SmartFlow].[Process] ADD  CONSTRAINT [DF_Process_IsActive]  DEFAULT ((1)) FOR [IsActive]

ALTER TABLE [SmartFlow].[Process] ADD  CONSTRAINT [DF_Process_IsDefultProccess]  DEFAULT ((0)) FOR [IsDefultProccess]

END
