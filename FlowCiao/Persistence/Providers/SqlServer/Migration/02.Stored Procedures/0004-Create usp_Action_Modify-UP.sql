CREATE OR ALTER PROCEDURE [FlowCiao].[usp_Action_Modify]
	@Id UNIQUEIDENTIFIER,
	@Name NVARCHAR(400)= NULL,
	@ActionTypeCode INT= NULL,
	@ProcessId UNIQUEIDENTIFIER= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[Action] WHERE Id = @Id)
	BEGIN
		UPDATE FlowCiao.[Action]
		SET
			[Name] = @Name,
			[ActionTypeCode] = @ActionTypeCode,
			[ProcessId] = @ProcessId
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[Action]
				   ([Id]
				   ,[Name]
				   ,[ActionTypeCode]
				   ,[ProcessId])
		VALUES
			(@Id
			,@Name
			,@ActionTypeCode
			,@ProcessId)
	END
END