CREATE OR ALTER PROCEDURE [FlowCiao].[usp_TransitionActivity_Modfiy]
	@TransitionId UNIQUEIDENTIFIER,
	@ActivityId UNIQUEIDENTIFIER,
	@Priority INT = NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[TransitionActivity] 
				WHERE [TransitionId] = @TransitionId AND ActivityId = @ActivityId)
	BEGIN
		UPDATE FlowCiao.[TransitionActivity]
		SET
			[Priority] = @Priority
		WHERE [TransitionId] = @TransitionId AND ActivityId = @ActivityId
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[TransitionActivity]
			   ([TransitionId]
			   ,ActivityId
			   ,[Priority])
		VALUES
			   (@TransitionId
			   ,@ActivityId
			   ,@Priority)
	END
END