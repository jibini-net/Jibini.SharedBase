CREATE PROCEDURE [dbo].[Account_LogIn]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Account]
	SET [LastLogin] = GETDATE()
	WHERE [Id] = @Id;

	IF @@ROWCOUNT = 0
	BEGIN
		RAISERROR('Account not found to log in', 18, 1);
		RETURN;
	END

	EXEC Account_Get @Id;
END
