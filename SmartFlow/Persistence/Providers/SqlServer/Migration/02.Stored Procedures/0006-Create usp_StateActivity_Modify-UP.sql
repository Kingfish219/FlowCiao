CREATE OR ALTER PROCEDURE [SmartFlow].[usp_StateActivity_Modify]
	@StateId UNIQUEIDENTIFIER,
	@ActivityId UNIQUEIDENTIFIER,
	@Priority INT = NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [SmartFlow].StateActivity 
				WHERE [StateId] = @StateId AND ActivityId = @ActivityId)
	BEGIN
		UPDATE SmartFlow.StateActivity
		SET
			[Priority] = @Priority
		WHERE [StateId] = @StateId AND ActivityId = @ActivityId
	END
	ELSE
	BEGIN
		INSERT INTO [SmartFlow].[StateActivity]
			   ([StateId]
			   ,ActivityId
			   ,[Priority])
		VALUES
			   (@StateId
			   ,@ActivityId
			   ,@Priority)
	END
END