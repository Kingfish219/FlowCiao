CREATE OR ALTER   PROCEDURE [SmartFlow].[usp_Process_Modify]
	@Id UNIQUEIDENTIFIER,
	@IsActive BIT= NULL,
	@IsDefultProccess BIT= NULL,
	@Name NVARCHAR(400)= NULL,
	@FlowKey NVARCHAR(200)= NULL,
	@Owner UNIQUEIDENTIFIER= NULL,
	@EntityType INT= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [SmartFlow].Process WHERE Id = @Id)
	BEGIN
		UPDATE SmartFlow.Process
		SET
			IsActive = @IsActive,
			IsDefultProccess= @IsDefultProccess,
			[Name]= @IsActive,
			FlowKey= @FlowKey,
			[Owner]= @Owner,
			EntityType= @EntityType
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO SmartFlow.Process
		(
			Id,
			IsActive,
			IsDefultProccess,
			[Name],
			FlowKey,
			[Owner],
			EntityType
		)
		VALUES
		(   
			@Id,
			ISNULL(@IsActive, 1),
			ISNULL(@IsDefultProccess, 0),
			@Name,
			@FlowKey,
			@Owner,
			@EntityType
		)
	END
END