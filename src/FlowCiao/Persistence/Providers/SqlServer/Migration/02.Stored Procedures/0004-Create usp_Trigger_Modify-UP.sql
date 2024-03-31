CREATE OR ALTER PROCEDURE [FlowCiao].[usp_Trigger_Modify]
	@Id UNIQUEIDENTIFIER,
	@Name NVARCHAR(400)= NULL,
	@TriggerType INT= NULL,
	@ProcessId UNIQUEIDENTIFIER= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[Trigger] WHERE Id = @Id)
	BEGIN
		UPDATE [FlowCiao].[Trigger]
		SET
			[Name] = @Name,
			[TriggerType] = @TriggerType,
			[ProcessId] = @ProcessId
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO [FlowCiao].[Trigger]
				   ([Id]
				   ,[Name]
				   ,[TriggerType]
				   ,[ProcessId])
		VALUES
			(@Id
			,@Name
			,@TriggerType
			,@ProcessId)
	END
END