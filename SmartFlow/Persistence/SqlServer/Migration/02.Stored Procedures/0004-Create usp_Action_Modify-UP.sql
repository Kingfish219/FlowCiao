CREATE OR ALTER PROCEDURE [SmartFlow].[usp_Action_Modify]
	@Id UNIQUEIDENTIFIER,
	@Name NVARCHAR(400)= NULL,
	@ActionTypeCode INT= NULL,
	@ProcessId UNIQUEIDENTIFIER= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [SmartFlow].[Action] WHERE Id = @Id)
	BEGIN
		UPDATE SmartFlow.[Action]
		SET
			[Name] = @Name,
			[ActionTypeCode] = @ActionTypeCode,
			[ProcessId] = @ProcessId
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [SmartFlow].[Action]
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