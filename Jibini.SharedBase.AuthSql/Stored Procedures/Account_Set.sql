CREATE PROCEDURE [dbo].[Account_Set]
	@Id INT,
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50) = NULL,
	@Email NVARCHAR(50),
	@CellNumber NVARCHAR(50) = NULL,
	@HomeNumber NVARCHAR(50) = NULL,
	@PasswordDuration INT = NULL,
	@Enabled BIT
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @Id = 0
	BEGIN

		IF EXISTS (SELECT TOP (1) a.* FROM [Account] a WHERE a.[Email] = @Email)
		BEGIN
			RAISERROR('An account already exists with that email', 18, 1);
			RETURN;
		END

		INSERT INTO [Account] ([FirstName],
			[LastName],
			[Email],
			[CellNumber],
			[HomeNumber],
			[PasswordDuration],
			[Enabled])
		VALUES (@FirstName,
			@LastName,
			@Email,
			@CellNumber,
			@HomeNumber,
			@PasswordDuration,
			@Enabled);

		SET @Id = SCOPE_IDENTITY();

	END
	ELSE
	BEGIN

		UPDATE [Account]
		SET [FirstName] = @FirstName,
			[LastName] = @LastName,
			[Email] = @Email,
			[CellNumber] = @CellNumber,
			[HomeNumber] = @HomeNumber,
			[PasswordDuration] = @PasswordDuration,
			[Enabled] = @Enabled,
			[EnabledToggled] = (CASE WHEN [Enabled] = @Enabled
				THEN [EnabledToggled]
				ELSE GETDATE() END),
			[Updated] = GETDATE()
		WHERE [Id] = @Id

		IF @@ROWCOUNT = 0
		BEGIN
			RAISERROR('Specified record could not be found', 18, 1);
			RETURN;
		END

	END

	EXEC Account_Get @Id;
END
