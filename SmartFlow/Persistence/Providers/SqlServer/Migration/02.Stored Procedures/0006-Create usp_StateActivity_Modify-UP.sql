CREATE OR ALTER PROCEDURE [FlowCiao].[usp_StateActivity_Modify]
	@StateId UNIQUEIDENTIFIER,
	@ActivityId UNIQUEIDENTIFIER,
	@Priority INT = NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].StateActivity 
				WHERE [StateId] = @StateId AND ActivityId = @ActivityId)
	BEGIN
		UPDATE FlowCiao.StateActivity
		SET
			[Priority] = @Priority
		WHERE [StateId] = @StateId AND ActivityId = @ActivityId
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[StateActivity]
			   ([StateId]
			   ,ActivityId
			   ,[Priority])
		VALUES
			   (@StateId
			   ,@ActivityId
			   ,@Priority)
	END
END