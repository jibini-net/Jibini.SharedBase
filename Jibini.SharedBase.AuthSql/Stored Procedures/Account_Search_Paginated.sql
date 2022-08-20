CREATE PROCEDURE Account_Search_Paginated
	@Id INT = NULL,
	@Name NVARCHAR(100) = NULL,
	@FirstName NVARCHAR(50) = NULL,
	@LastName NVARCHAR(50) = NULL,
	@Email NVARCHAR(50) = NULL,
	@CellNumber NVARCHAR(50) = NULL,
	@HomeNumber NVARCHAR(50) = NULL,
	@HasPassword BIT = NULL,
	@PasswordValid BIT = NULL,
	@PasswordExpired BIT = NULL,
	@HasLoggedIn BIT = NULL,
	@Enabled BIT = 1,
	@Deleted BIT = 0,
	@Exact BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT a.* FROM
	dbo.getAccounts(@Id,
		@Name,
		@FirstName,
		@LastName,
		@Email,
		@CellNumber,
		@HomeNumber,
		@HasPassword,
		@PasswordValid,
		@PasswordExpired,
		@HasLoggedIn,
		@Enabled,
		@Deleted,
		0) a
	ORDER BY a.[LastName],
		a.[FirstName],
		a.[Id]
	FOR JSON PATH;
END
