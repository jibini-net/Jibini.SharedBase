CREATE PROCEDURE [dbo].[Account_Get]
	@Id INT = NULL,
	@Email NVARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (@Id IS NULL AND @Email IS NULL)
	BEGIN
		RAISERROR('Specify either a record number or unique email', 18, 1);
		RETURN;
	END
	
	SELECT a.* INTO #results
	FROM dbo.getAccounts(@Id,
		DEFAULT,
		DEFAULT,
		DEFAULT,
		@Email,
		DEFAULT,
		DEFAULT,
		DEFAULT,
		DEFAULT,
		DEFAULT,
		DEFAULT,
		DEFAULT,
		DEFAULT,
		1) a;

	IF (SELECT COUNT(*) FROM #results) > 1
	BEGIN
		RAISERROR('Multiple records found matching the provided key', 18, 1);
		RETURN;
	END

	SELECT TOP (1) r.* FROM #results r
	FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;
END
