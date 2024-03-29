USE [IPLP]
GO
/****** Object:  Table [dbo].[EventPacket]    Script Date: 11/03/2011 13:44:47 ******/
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
/****** Object:  StoredProcedure [dbo].[InsertPacketData]    Script Date: 11/03/2011 13:45:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

    INSERT INTO [dbo].[EventPacket]([ReceivedDate], [SerialId], [Data], [PacketSize], [Created]) 
    VALUES(@date, @serialid, @data, @size, GETDATE()); SELECT SCOPE_IDENTITY() AS [Id]
END
GO
/****** Object:  StoredProcedure [dbo].[SelectPacketDataDateSerialId]    Script Date: 11/03/2011 13:45:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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

	SELECT * FROM [dbo].[EventPacket] WITH (NoLock) WHERE [ReceivedDate] = @date AND [SerialId] = @serialId
	ORDER BY [EventPacketId]
END
GO
/****** Object:  StoredProcedure [dbo].[SelectPacketDataId]    Script Date: 11/03/2011 13:45:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
GO
/****** Object:  StoredProcedure [dbo].[PacketExists]    Script Date: 11/03/2011 13:45:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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

	SELECT COUNT(*) FROM [dbo].[EventPacket] WITH (NoLock) WHERE [ReceivedDate] = @date AND [SerialId] = @serialid AND [Data] = @data
END
GO
/****** Object:  StoredProcedure [dbo].[GetSerialIds]    Script Date: 11/03/2011 13:45:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donald Thumim
-- Create date: 10/24/2011
-- Description:	Gets a list of the serial numbers in the database.
-- =============================================
CREATE PROCEDURE [dbo].[GetSerialIds] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [SerialId] FROM [dbo].[EventPacket]
	ORDER BY [SerialId]
END
GO
/****** Object:  StoredProcedure [dbo].[GetDateRange]    Script Date: 11/03/2011 13:45:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donald Thumim
-- Create date: 10/24/2011
-- Description:	Gets the range of event dates available for the provided serial number.
-- =============================================
CREATE PROCEDURE [dbo].[GetDateRange] 
	@serialid int 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT MIN(ReceivedDate) AS StartDate, MAX(ReceivedDate) AS EndDate FROM [dbo].[EventPacket] WHERE serialid = @serialid
END
GO
/****** Object:  StoredProcedure [dbo].[GetPacketDataBySerialId]    Script Date: 11/03/2011 13:45:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donald Thumim
-- Create date: 10/24/2011
-- Description:	Gets packet data for the selected device.
-- =============================================
CREATE PROCEDURE [dbo].[GetPacketDataBySerialId] 
	@serialid int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM [dbo].[EventPacket] WHERE SerialId = @serialid
	ORDER BY ReceivedDate
END
GO
/****** Object:  StoredProcedure [dbo].[GetPacketDataByDate]    Script Date: 11/03/2011 13:45:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Donald Thumim
-- Create date: 10/24/2011
-- Description:	Gets the event packets for the provided device between the provided timestamps.
-- =============================================
CREATE PROCEDURE [dbo].[GetPacketDataByDate] 
	@serialid int, 
	@start datetime,
	@end datetime
AS
BEGIN
	SET NOCOUNT ON;

SELECT * FROM [dbo].[EventPacket] WHERE [SerialId] = @serialid AND [ReceivedDate] BETWEEN @start AND @end
ORDER BY [ReceivedDate]
END
GO
/****** Object:  Default [DF_EventPacket_PacketSize]    Script Date: 11/03/2011 13:44:47 ******/
ALTER TABLE [dbo].[EventPacket] ADD  CONSTRAINT [DF_EventPacket_PacketSize]  DEFAULT ((0)) FOR [PacketSize]
GO
/****** Object:  Default [DF_EventPacket_Created]    Script Date: 11/03/2011 13:44:47 ******/
ALTER TABLE [dbo].[EventPacket] ADD  CONSTRAINT [DF_EventPacket_Created]  DEFAULT (getdate()) FOR [Created]
GO
