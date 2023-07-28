CREATE OR ALTER PROCEDURE [SmartFlow].[usp_TransitionAction_Modify]
	@TransitionId UNIQUEIDENTIFIER,
	@ActionId UNIQUEIDENTIFIER,
	@Priority INT = NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [SmartFlow].[TransitionAction] 
				WHERE [TransitionId] = @TransitionId AND [ActionId] = @ActionId)
	BEGIN
		UPDATE SmartFlow.[TransitionAction]
		SET
			[Priority] = @Priority
		WHERE [TransitionId] = @TransitionId AND [ActionId] = @ActionId
	END
	ELSE
	BEGIN
		INSERT INTO [SmartFlow].[TransitionAction]
			   ([Id]
			   ,[ActionId]
			   ,[TransitionId]
			   ,[Priority])
		VALUES
			   (NEWID()
			   ,@ActionId
			   ,@TransitionId
			   ,@Priority)
	END
END