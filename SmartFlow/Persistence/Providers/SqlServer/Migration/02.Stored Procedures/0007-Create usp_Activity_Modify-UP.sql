CREATE OR ALTER PROCEDURE [FlowCiao].[usp_Activity_Modify]
	@Id UNIQUEIDENTIFIER,
	@Name NVARCHAR(400)= NULL,
	@ActivityTypeCode INT= NULL,
	@Process UNIQUEIDENTIFIER= NULL,
	@Executor NVARCHAR(500)= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[Activity] WHERE Id = @Id)
	BEGIN
		UPDATE FlowCiao.[Activity]
		SET
			[Name] = @Name,
			[Executor] = @Executor
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[Activity]
				   ([Id]
				   ,[Name]
				   ,[ActivityTypeCode]
				   ,[Process]
				   ,[Executor])
		VALUES
           (
			   @Id
			   ,@Name
			   ,@ActivityTypeCode
			   ,@Process
			   ,@Executor
		   )
	END
END