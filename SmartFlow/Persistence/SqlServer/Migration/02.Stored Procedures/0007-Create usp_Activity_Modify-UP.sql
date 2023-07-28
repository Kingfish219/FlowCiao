CREATE OR ALTER PROCEDURE [SmartFlow].[usp_Activity_Modify]
	@Id UNIQUEIDENTIFIER,
	@Name NVARCHAR(400)= NULL,
	@ActivityTypeCode INT= NULL,
	@Process UNIQUEIDENTIFIER= NULL,
	@Executor NVARCHAR(500)= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [SmartFlow].[Activity] WHERE Id = @Id)
	BEGIN
		UPDATE SmartFlow.[Activity]
		SET
			[Name] = @Name,
			[Executor] = @Executor
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [SmartFlow].[Activity]
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