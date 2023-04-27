IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[ProcessExecution]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[ProcessExecution]
(
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [ProcessId] [UNIQUEIDENTIFIER] NOT NULL,
    [State] [INT] NULL,
    [CreatedOn] [DATETIME] NOT NULL,
    [Progress] [NVARCHAR](MAX) NULL,
    CONSTRAINT [PK_ProcessExecution]
        PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]

END
