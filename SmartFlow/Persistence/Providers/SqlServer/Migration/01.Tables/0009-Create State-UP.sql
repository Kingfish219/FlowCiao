IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[State]') AND type in (N'U'))
BEGIN

CREATE TABLE [FlowCiao].[State](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[RequestResponse] [bit] NULL,
	[ResponseController] [nvarchar](100) NULL,
	[ResponseActions] [nvarchar](100) NULL,
	[IsFinalResponse] [bit] NULL,
	[Number] [nvarchar](50) NULL,
	[IsVisible] [bit] NULL,
	[Description] [nvarchar](500) NULL,
	[IsStart] [bit] NULL,
 CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [FlowCiao].[State] ADD  CONSTRAINT [DF_State_Id]  DEFAULT (NEWID()) FOR [Id]

ALTER TABLE [FlowCiao].[State] ADD  DEFAULT ((1)) FOR [IsVisible]

ALTER TABLE [FlowCiao].[State] ADD  DEFAULT ((0)) FOR [IsStart]

END
