IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SmartFlow].[Transition]') AND type in (N'U'))
BEGIN

CREATE TABLE [SmartFlow].[Transition](
	[Id] [uniqueidentifier] NULL,
	[ProcessId] [uniqueidentifier] NULL,
	[CurrentStateId] [uniqueidentifier] NULL,
	[NextStateId] [uniqueidentifier] NULL
) ON [PRIMARY]

ALTER TABLE [SmartFlow].[Transition] ADD  CONSTRAINT [DF_Transition_Id]  DEFAULT (NEWID()) FOR [Id]

END
