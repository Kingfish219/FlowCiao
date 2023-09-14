CREATE OR ALTER PROCEDURE [FlowCiao].[usp_Transition_Modify]
	@Id UNIQUEIDENTIFIER = NULL,
	@ProcessId UNIQUEIDENTIFIER= NULL,
	@CurrentStateId UNIQUEIDENTIFIER= NULL,
	@NextStateId UNIQUEIDENTIFIER= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].Transition WHERE Id = @Id)
	BEGIN
		UPDATE FlowCiao.Transition
		SET
			ProcessId = @ProcessId,
			CurrentStateId = @CurrentStateId,
			NextStateId = @NextStateId
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO FlowCiao.Transition
		(
			Id,
			ProcessId,
			CurrentStateId,
			NextStateId
		)
		VALUES
		(   @Id, -- Id - uniqueidentifier
			@ProcessId, -- ProcessId - uniqueidentifier
			@CurrentStateId, -- CurrentStateId - uniqueidentifier
			@NextStateId  -- NextStateId - uniqueidentifier
		)
	END
END