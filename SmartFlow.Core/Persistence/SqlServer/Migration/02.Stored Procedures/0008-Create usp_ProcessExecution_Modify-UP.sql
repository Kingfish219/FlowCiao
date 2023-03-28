CREATE OR ALTER PROCEDURE [SmartFlow].[usp_ProcessExecution_Modify]
	@Id UNIQUEIDENTIFIER,
	@State INT= NULL,
	@CreatedOn DATETIME= NULL,
	@ProcessId UNIQUEIDENTIFIER= NULL,
	@Progress NVARCHAR(MAX)= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [SmartFlow].[ProcessExecution] WHERE Id = @Id)
	BEGIN
		UPDATE SmartFlow.[ProcessExecution]
		SET
			[Progress] = @Progress
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [SmartFlow].[ProcessExecution]
		(
		    [Id],
		    [ProcessId],
		    [State],
		    [CreatedOn],
		    [Progress]
		)
		VALUES
		(   @Id,      -- Id - uniqueidentifier
		    @ProcessId,      -- ProcessId - uniqueidentifier
		    @State,         -- State - int
		    @CreatedOn, -- CreatedOn - datetime
		    @Progress        -- Progress - nvarchar(max)
		    )
	END
END