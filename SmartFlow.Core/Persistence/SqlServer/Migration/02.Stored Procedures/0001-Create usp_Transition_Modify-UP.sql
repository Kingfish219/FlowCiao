CREATE OR ALTER PROCEDURE [SmartFlow].[usp_Transition_Modify]
	@Id UNIQUEIDENTIFIER = NULL,
	@ProcessId UNIQUEIDENTIFIER= NULL,
	@CurrentStateId UNIQUEIDENTIFIER= NULL,
	@NextStateId UNIQUEIDENTIFIER= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [SmartFlow].Transition WHERE Id = @Id)
	BEGIN
		UPDATE SmartFlow.Transition
		SET
			ProcessId = @ProcessId,
			CurrentStateId = @CurrentStateId,
			NextStateId = @NextStateId
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO SmartFlow.Transition
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