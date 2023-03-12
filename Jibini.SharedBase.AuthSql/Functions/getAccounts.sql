CREATE FUNCTION [dbo].[getAccounts]
(	
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
	@Enabled BIT = NULL,
	@Deleted BIT = NULL,
	@Exact BIT = 1
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT a.[Id],
		TRIM(a.[FirstName] + ' ' + a.[LastName]) AS [Name],
		a.[FirstName],
		a.[LastName],
		a.[Email],
		a.[CellNumber],
		a.[HomeNumber],
		a.[PasswordHash],
		a.[PasswordSalt],
		a.[PasswordSet],
		a.[PasswordDuration],
		CASE WHEN (a.[PasswordHash] IS NOT NULL AND (a.[PasswordDuration] IS NULL OR DATEADD(DAY, a.[PasswordDuration], a.[PasswordSet]) > GETDATE()))
			THEN 1 ELSE 0 END AS [PasswordValid],
		CASE WHEN (a.[PasswordDuration] IS NOT NULL AND DATEADD(DAY, a.[PasswordDuration], a.[PasswordSet]) <= GETDATE())
			THEN 1 ELSE 0 END AS [PasswordExpired],
		a.[LastLogin],
		a.[Enabled],
		a.[EnabledToggled],
		a.[Deleted],
		a.[Created],
		a.[Updated]
	FROM [Account] a
	WHERE (@Id IS NULL OR @Id = a.[Id])
		AND dbo.nullableContains(@Name, TRIM(a.[FirstName] + ' ' + a.[LastName]), @Exact) = 1
		AND dbo.nullableContains(@FirstName, a.[FirstName], @Exact) = 1
		AND dbo.nullableContains(@LastName, a.[LastName], @Exact) = 1
		AND dbo.nullableContains(@Email, a.[Email], @Exact) = 1
		AND dbo.nullableContains(@CellNumber, a.[CellNumber], @Exact) = 1
		AND dbo.nullableContains(@HomeNumber, a.[HomeNumber], @Exact) = 1
        AND dbo.nullableCompareBit(@HasPassword,
            CASE WHEN a.[PasswordHash] IS NOT NULL
            THEN 1 ELSE 0 END) = 1
		AND dbo.nullableCompareBit(@PasswordValid,
            CASE WHEN (a.[PasswordHash] IS NOT NULL AND DATEADD(DAY, a.[PasswordDuration], a.[PasswordSet]) > GETDATE())
			THEN 1 ELSE 0 END) = 1
		AND dbo.nullableCompareBit(@PasswordExpired,
			CASE WHEN (a.[PasswordDuration] IS NOT NULL AND DATEADD(DAY, a.[PasswordDuration], a.[PasswordSet]) <= GETDATE())
			THEN 1 ELSE 0 END) = 1
		AND dbo.nullableCompareBit(@HasLoggedIn,
			CASE WHEN a.[LastLogin] IS NOT NULL
			THEN 1 ELSE 0 END) = 1
		AND dbo.nullableCompareBit(@Enabled, a.[Enabled]) = 1
		AND dbo.nullableCompareBit(@Deleted,
			CASE WHEN a.[Deleted] IS NOT NULL
			THEN 1 ELSE 0 END) = 1
)
