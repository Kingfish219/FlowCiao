IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[ProcessExecution]') AND type in (N'U'))
BEGIN

CREATE TABLE [FlowCiao].[ProcessExecution]
(
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [ProcessId] [UNIQUEIDENTIFIER] NOT NULL,
    [ExecutionState] [INT] NULL,
    [State] [UNIQUEIDENTIFIER] NULL,
    [CreatedOn] [DATETIME] NOT NULL,
    [Progress] [NVARCHAR](MAX) NULL,
    CONSTRAINT [PK_ProcessExecution]
        PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]

END
