use [master]
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'##DatabaseName##')
	DROP DATABASE [##DatabaseName##]
GO
CREATE DATABASE [##DatabaseName##]
GO
exec sp_dboption N'##DatabaseName##', N'autoclose', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'bulkcopy', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'trunc. log', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'torn page detection', N'true'
GO
exec sp_dboption N'##DatabaseName##', N'read only', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'dbo use', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'single', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'autoshrink', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'ANSI null default', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'recursive triggers', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'ANSI nulls', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'concat null yields null', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'cursor close on commit', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'default to local cursor', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'quoted identifier', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'ANSI warnings', N'false'
GO
exec sp_dboption N'##DatabaseName##', N'auto create statistics', N'true'
GO
exec sp_dboption N'##DatabaseName##', N'auto update statistics', N'true'
GO
use [##DatabaseName##]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EventPacket](
	[EventPacketId] [int] IDENTITY(1,1) NOT NULL,
	[ReceivedDate] [datetime] NOT NULL,
	[SerialId] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[PacketSize] [int] NULL,
	[Created] [datetime] NOT NULL,
	[Timestamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_EventPacket] PRIMARY KEY CLUSTERED 
(
	[EventPacketId] ASC,
	[ReceivedDate] ASC,
	[SerialId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EventPacket] ADD  CONSTRAINT [DF_EventPacket_Created]  DEFAULT (getdate()) FOR [Created]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
