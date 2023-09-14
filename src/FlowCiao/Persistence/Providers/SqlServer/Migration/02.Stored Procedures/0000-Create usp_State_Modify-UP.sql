CREATE OR ALTER PROCEDURE [FlowCiao].[usp_State_Modify]
	@Id UNIQUEIDENTIFIER = NULL,
	@Name NVARCHAR(100)= NULL,
	@RequestResponse BIT= NULL,
	@ResponseController NVARCHAR(100)= NULL,
	@ResponseActions NVARCHAR(100)= NULL,
	@IsFinalResponse BIT= NULL,
	@Number NVARCHAR(50)= NULL,
	@IsVisible BIT= NULL,
	@Description NVARCHAR(500)= NULL,
	@IsStart BIT= NULL
AS
BEGIN
	IF EXISTS (SELECT 1 FROM [FlowCiao].[State] WHERE Id = @Id)
	BEGIN
		UPDATE FlowCiao.[State]
		SET
			[Name] = @Name,
			RequestResponse = @RequestResponse,
			ResponseController = @ResponseController,
			ResponseActions = @ResponseActions,
			IsFinalResponse = @IsFinalResponse,
			Number = @Number,
			IsVisible = @IsVisible,
			[Description] = @Description,
			IsStart = @IsStart
		WHERE Id = @Id
	END
	ELSE
	BEGIN
		INSERT INTO FlowCiao.[State]
		(
			Id,
			[Name],
			RequestResponse,
			ResponseController,
			ResponseActions,
			IsFinalResponse,
			Number,
			IsVisible,
			[Description],
			IsStart
		)
		VALUES
		(   @Id, -- Id - uniqueidentifier
			@Name,  -- Name - nvarchar(100)
			@RequestResponse, -- RequestResponse - bit
			@ResponseController,  -- ResponseController - nvarchar(100)
			@ResponseActions,  -- ResponseActions - nvarchar(100)
			@IsFinalResponse, -- IsFinalResponse - bit
			@Number,  -- Number - nvarchar(50)
			@IsVisible, -- IsVisible - bit
			@Description,  -- Description - nvarchar(500)
			@IsStart -- IsStart - bit
		)
	END
END