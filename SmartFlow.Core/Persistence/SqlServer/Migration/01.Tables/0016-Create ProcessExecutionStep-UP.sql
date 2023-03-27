IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[ProcessExecutionStep]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[ProcessExecutionStep](
	[Id] [UNIQUEIDENTIFIER] NOT NULL,
	[IsCompleted] [INT] NOT NULL,
	[CreatedOn] [DATETIME] NOT NULL,
	[ProcessExecutionId] [UNIQUEIDENTIFIER] NOT NULL,
 CONSTRAINT [PK_ProcessExecutionStep] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
) ON [PRIMARY]

END
