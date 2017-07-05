USE [##DatabaseName##]
GO
SET NOCOUNT ON
GO

BULK INSERT ##TableName## FROM '##FileName##' WITH(FIELDTERMINATOR = '|', KEEPNULLS, ROWTERMINATOR = '\n', ERRORFILE = '##FileName##.err.log')
GO

SET NOCOUNT OFF
GO