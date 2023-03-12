CREATE FUNCTION [dbo].[nullableCompareBit]
(
	@search BIT = NULL,
	@value BIT
)
RETURNS BIT
AS
BEGIN

	RETURN CASE WHEN @search IS NULL OR @search = @value
			THEN 1
			ELSE 0 END;
END
