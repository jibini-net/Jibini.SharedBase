CREATE PROCEDURE [dbo].[Account_Delete]
	@Id INT,
	@Undo BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [Account]
	SET [Deleted] = (
		CASE WHEN @Undo = 1
			THEN NULL
			ELSE GETDATE() END)
	WHERE [Id] = @Id;

	IF @@ROWCOUNT = 0
	BEGIN
		RAISERROR('Account not found to delete', 18, 1);
		RETURN;
	END

	EXEC Account_Get @Id;
END
