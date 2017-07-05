use [master]
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'##DatabaseName##')
	ALTER DATABASE [##DatabaseName##] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [##DatabaseName##]
GO