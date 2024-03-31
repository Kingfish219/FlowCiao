CREATE OR ALTER PROCEDURE [FlowCiao].[usp_TransitionTrigger_Modify]
	@TransitionId UNIQUEIDENTIFIER,
	@TriggerId UNIQUEIDENTIFIER,
	@Priority INT = NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[TransitionTrigger] 
				WHERE [TransitionId] = @TransitionId AND [TriggerId] = @TriggerId)
	BEGIN
		UPDATE FlowCiao.[TransitionTrigger]
		SET
			[Priority] = @Priority
		WHERE [TransitionId] = @TransitionId AND [TriggerId] = @TriggerId
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[TransitionTrigger]
			   ([Id]
			   ,[TriggerId]
			   ,[TransitionId]
			   ,[Priority])
		VALUES
			   (NEWID()
			   ,@TriggerId
			   ,@TransitionId
			   ,@Priority)
	END
END