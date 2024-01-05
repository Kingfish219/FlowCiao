CREATE OR ALTER   PROCEDURE [FlowCiao].[usp_Process_Modify]
	@Id UNIQUEIDENTIFIER,
	@IsActive BIT= NULL,
	@IsDefultProccess BIT= NULL,
	@Name NVARCHAR(400)= NULL,
	@Key NVARCHAR(200)= NULL,
	@Owner UNIQUEIDENTIFIER= NULL,
	@EntityType INT= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].Process WHERE Id = @Id)
	BEGIN
		UPDATE FlowCiao.Process
		SET
			IsActive = @IsActive,
			IsDefultProccess= @IsDefultProccess,
			[Name]= @IsActive,
			[Key]= @Key,
			[Owner]= @Owner,
			EntityType= @EntityType
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO FlowCiao.Process
		(
			Id,
			IsActive,
			IsDefultProccess,
			[Name],
			[Key],
			[Owner],
			EntityType
		)
		VALUES
		(   
			@Id,
			ISNULL(@IsActive, 1),
			ISNULL(@IsDefultProccess, 0),
			@Name,
			@Key,
			@Owner,
			@EntityType
		)
	END
END