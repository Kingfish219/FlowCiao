CREATE OR ALTER PROCEDURE [SmartFlow].[usp_Transition_Insert]
	@Id UNIQUEIDENTIFIER = NULL,
	@ProcessId UNIQUEIDENTIFIER= NULL,
	@CurrentStateId UNIQUEIDENTIFIER= NULL,
	@NextStateId UNIQUEIDENTIFIER= NULL
AS
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