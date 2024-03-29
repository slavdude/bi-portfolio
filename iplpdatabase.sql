USE [master]
GO
/****** Object:  Database [IPLP]    Script Date: 10/21/2011 13:51:56 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'IPLP')
BEGIN
CREATE DATABASE [IPLP] ON  PRIMARY 
( NAME = N'IPLP', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\IPLP.mdf' , SIZE = 427136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'IPLP_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\IPLP_log.ldf' , SIZE = 52416KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END
GO
ALTER DATABASE [IPLP] SET COMPATIBILITY_LEVEL = 90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [IPLP].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [IPLP] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [IPLP] SET ANSI_NULLS OFF
GO
ALTER DATABASE [IPLP] SET ANSI_PADDING OFF
GO
ALTER DATABASE [IPLP] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [IPLP] SET ARITHABORT OFF
GO
ALTER DATABASE [IPLP] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [IPLP] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [IPLP] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [IPLP] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [IPLP] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [IPLP] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [IPLP] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [IPLP] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [IPLP] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [IPLP] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [IPLP] SET  DISABLE_BROKER
GO
ALTER DATABASE [IPLP] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [IPLP] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [IPLP] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [IPLP] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [IPLP] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [IPLP] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [IPLP] SET  READ_WRITE
GO
ALTER DATABASE [IPLP] SET RECOVERY FULL
GO
ALTER DATABASE [IPLP] SET  MULTI_USER
GO
ALTER DATABASE [IPLP] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [IPLP] SET DB_CHAINING OFF
GO
USE [IPLP]
GO
/****** Object:  StoredProcedure [dbo].[PacketExists]    Script Date: 10/21/2011 13:51:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PacketExists]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PacketExists]
GO
/****** Object:  StoredProcedure [dbo].[SelectPacketDataId]    Script Date: 10/21/2011 13:51:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectPacketDataId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SelectPacketDataId]
GO
/****** Object:  StoredProcedure [dbo].[SelectPacketDataDateSerialId]    Script Date: 10/21/2011 13:51:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectPacketDataDateSerialId]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SelectPacketDataDateSerialId]
GO
/****** Object:  StoredProcedure [dbo].[InsertPacketData]    Script Date: 10/21/2011 13:51:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertPacketData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertPacketData]
GO
/****** Object:  Table [dbo].[EventPacket]    Script Date: 10/21/2011 13:51:58 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_EventPacket_PacketSize]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[EventPacket] DROP CONSTRAINT [DF_EventPacket_PacketSize]
END
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_EventPacket_Created]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[EventPacket] DROP CONSTRAINT [DF_EventPacket_Created]
END
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventPacket]') AND type in (N'U'))
DROP TABLE [dbo].[EventPacket]
GO
/****** Object:  Table [dbo].[EventPacket]    Script Date: 10/21/2011 13:51:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventPacket]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EventPacket](
	[EventPacketId] [int] IDENTITY(1,1) NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[SerialId] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[PacketSize] [int] NULL CONSTRAINT [DF_EventPacket_PacketSize]  DEFAULT ((0)),
	[Created] [datetime] NOT NULL CONSTRAINT [DF_EventPacket_Created]  DEFAULT (getdate()),
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_EventPacket] PRIMARY KEY CLUSTERED 
(
	[EventPacketId] ASC,
	[EventDate] ASC,
	[SerialId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [dbo].[InsertPacketData]    Script Date: 10/21/2011 13:51:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertPacketData]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		Donald Thumim
-- Create date: 10/13/2011
-- Description:	Inserts packet data into EventPacket table.
-- =============================================
CREATE PROCEDURE [dbo].[InsertPacketData] 
	@date datetime, 
	@serialid int,
	@data nvarchar(max),
	@size int
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[EventPacket]([EventDate], [SerialId], [Data], [PacketSize], [Created]) 
    VALUES(@date, @serialid, @data, @size, GETDATE()); SELECT SCOPE_IDENTITY() AS [Id]
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[SelectPacketDataDateSerialId]    Script Date: 10/21/2011 13:51:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectPacketDataDateSerialId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		Donald Thumim
-- Create date: 10/13/2011
-- Description:	Selects packet data by date and serial ID.
-- =============================================
CREATE PROCEDURE [dbo].[SelectPacketDataDateSerialId] 
	@date datetime, 
	@serialid int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[EventPacket] WITH (NoLock) WHERE [EventDate] = @date AND [SerialId] = @serialId
	ORDER BY [EventPacketId]
END

' 
END
GO
/****** Object:  StoredProcedure [dbo].[SelectPacketDataId]    Script Date: 10/21/2011 13:51:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SelectPacketDataId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Donald Thumim
-- Create date: 10/13/2011
-- Description:	Selects the packet data based on the ID.
-- =============================================
CREATE PROCEDURE [dbo].[SelectPacketDataId] 
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[EventPacket] WITH (NoLock) WHERE EventPacketId = @id
END
' 
END
GO
/****** Object:  StoredProcedure [dbo].[PacketExists]    Script Date: 10/21/2011 13:51:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PacketExists]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		Donald Thumim
-- Create date: 10/13/2011
-- Description:	Checks to see whether a specified packet exists in the database.
-- =============================================
CREATE PROCEDURE [dbo].[PacketExists] 
	@date datetime, 
	@serialid int,
	@data nvarchar(max)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*) FROM [dbo].[EventPacket] WITH (NoLock) WHERE [EventDate] = @date AND [SerialId] = @serialid AND [Data] = @data
END
' 
END
GO
