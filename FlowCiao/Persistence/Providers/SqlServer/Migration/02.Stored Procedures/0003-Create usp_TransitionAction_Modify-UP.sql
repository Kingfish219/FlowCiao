CREATE OR ALTER PROCEDURE [FlowCiao].[usp_TransitionAction_Modify]
	@TransitionId UNIQUEIDENTIFIER,
	@ActionId UNIQUEIDENTIFIER,
	@Priority INT = NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[TransitionAction] 
				WHERE [TransitionId] = @TransitionId AND [ActionId] = @ActionId)
	BEGIN
		UPDATE FlowCiao.[TransitionAction]
		SET
			[Priority] = @Priority
		WHERE [TransitionId] = @TransitionId AND [ActionId] = @ActionId
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[TransitionAction]
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