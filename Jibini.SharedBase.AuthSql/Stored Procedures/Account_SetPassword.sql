CREATE PROCEDURE [dbo].[Account_SetPassword]
	@Id INT,
	@PasswordHash NVARCHAR(200) = NULL,
	@PasswordSalt NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Account]
	SET [PasswordHash] = @PasswordHash,
		[PasswordSalt] = @PasswordSalt,
		[PasswordSet] = GETDATE(),
		[Updated] = GETDATE()

	IF @@ROWCOUNT = 0
	BEGIN
		RAISERROR('Account not found to update password', 18, 1);
		RETURN;
	END

	EXEC Account_Get @Id;
END
