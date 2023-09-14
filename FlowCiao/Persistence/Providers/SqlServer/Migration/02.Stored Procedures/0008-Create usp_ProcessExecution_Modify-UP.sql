CREATE OR ALTER PROCEDURE [FlowCiao].[usp_ProcessExecution_Modify]
	@Id UNIQUEIDENTIFIER,
	@ExecutionState INT= NULL,
	@CreatedOn DATETIME= NULL,
	@State UNIQUEIDENTIFIER= NULL,
	@ProcessId UNIQUEIDENTIFIER= NULL,
	@Progress NVARCHAR(MAX)= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[ProcessExecution] WHERE Id = @Id)
	BEGIN
		UPDATE FlowCiao.[ProcessExecution]
		SET
			[Progress] = @Progress
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[ProcessExecution]
		(
		    [Id],
		    [ProcessId],
		    [ExecutionState],
			[State],
		    [CreatedOn],
		    [Progress]
		)
		VALUES
		(   @Id,      -- Id - uniqueidentifier
		    @ProcessId,      -- ProcessId - uniqueidentifier
		    @ExecutionState,         -- ExecutionState - int
			@State,
		    @CreatedOn, -- CreatedOn - datetime
		    @Progress        -- Progress - nvarchar(max)
		    )
	END
END