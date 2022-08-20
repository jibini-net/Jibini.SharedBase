CREATE FUNCTION [dbo].[nullableContains]
(
	@find NVARCHAR(100),
	@within NVARCHAR(200),
	@exact BIT
)
RETURNS BIT
AS
BEGIN
	DECLARE @append VARCHAR = (
		CASE WHEN @exact = 1 
			THEN ''
			ELSE '%' END);

	RETURN CASE WHEN (ISNULL(@find, '') = ''
		OR @append + ISNULL(@find, '') + @append LIKE ISNULL(@within, ''))
			THEN 1
			ELSE 0 END;
END
