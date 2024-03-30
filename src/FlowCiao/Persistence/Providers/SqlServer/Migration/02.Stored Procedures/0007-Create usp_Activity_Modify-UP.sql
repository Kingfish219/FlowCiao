CREATE OR ALTER PROCEDURE [FlowCiao].[usp_Activity_Modify]
	@Id UNIQUEIDENTIFIER,
	@Name NVARCHAR(400)= NULL,
	@ActivityTypeCode INT= NULL,
	@Process UNIQUEIDENTIFIER= NULL,
	@ActorName NVARCHAR(500)= NULL,
	@ActorContent VARBINARY(MAX)= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[Activity] WHERE Id = @Id OR [ActorName] = @ActorName)
	BEGIN
		UPDATE FlowCiao.[Activity]
		SET
			[Name] = @Name,
		    [ActorName] = ISNULL(@ActorName, [ActorName]),
			[ActorContent] = ISNULL(@ActorContent, [ActorContent])
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[Activity]
				   ([Id]
				   ,[Name]
				   ,[ActivityTypeCode]
				   ,[ActorName]
				   ,[ActorContent])
		VALUES
           (
			   @Id
			   ,@Name
			   ,@ActivityTypeCode
			   ,@ActorName
			   ,@ActorContent
		   )
	END
END
