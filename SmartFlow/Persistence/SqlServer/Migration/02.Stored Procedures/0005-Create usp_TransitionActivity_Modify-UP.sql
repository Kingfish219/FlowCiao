CREATE OR ALTER PROCEDURE [SmartFlow].[usp_TransitionActivity_Modfiy]
	@TransitionId UNIQUEIDENTIFIER,
	@ActivityId UNIQUEIDENTIFIER,
	@Priority INT = NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [SmartFlow].[TransitionActivity] 
				WHERE [TransitionId] = @TransitionId AND ActivityId = @ActivityId)
	BEGIN
		UPDATE SmartFlow.[TransitionActivity]
		SET
			[Priority] = @Priority
		WHERE [TransitionId] = @TransitionId AND ActivityId = @ActivityId
	END
	ELSE
	BEGIN
		INSERT INTO [SmartFlow].[TransitionActivity]
			   ([TransitionId]
			   ,ActivityId
			   ,[Priority])
		VALUES
			   (@TransitionId
			   ,@ActivityId
			   ,@Priority)
	END
END