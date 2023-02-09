IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[ActivityType]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[ActivityType](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](400) NULL,
	[TypeCode] [int] NULL,
 CONSTRAINT [PK_ActivityType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [SmartFlow].[ActivityType] ADD  CONSTRAINT [DF_ActivityType_Id]  DEFAULT (NEWID()) FOR [Id]

END
