IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[FlowCiao].[Activity]') AND type in (N'U'))
BEGIN
    ALTER TABLE [FlowCiao].[Activity] DROP COLUMN [Process]
    EXEC sp_RENAME '[FlowCiao].[Activity].[Executor]' , 'ActorName', 'COLUMN'
    ALTER TABLE [FlowCiao].[Activity] ADD [ActorContent] VARBINARY(MAX)
END
