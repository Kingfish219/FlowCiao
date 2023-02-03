CREATE OR ALTER PROCEDURE [SmartFlow].[usp_State_Insert]
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
	INSERT INTO SmartFlow.[State]
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