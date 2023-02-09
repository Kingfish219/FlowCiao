CREATE OR ALTER PROCEDURE [SmartFlow].[usp_TransitionAction_Insert]
	@TransitionId UNIQUEIDENTIFIER,
	@ActionId UNIQUEIDENTIFIER,
	@Priority INT
AS
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