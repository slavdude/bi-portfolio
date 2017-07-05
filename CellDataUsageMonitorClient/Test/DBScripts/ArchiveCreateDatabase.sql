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

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[address](
 [address_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
 [entity_id] [int] NOT NULL,
 [entity_type_id] [int] NOT NULL,
 [address1] [varchar](50) NULL,
 [address2] [varchar](50) NULL,
 [city] [varchar](50) NULL,
 [state] [varchar](4) NULL,
 [country_code] [int] NULL,
 [time_zone_id] [int] NULL,
 [create_date] [datetime] NOT NULL,
 [change_date] [datetime] NOT NULL,
 [change_user_id] [int] NOT NULL,
 [postal_code] [nvarchar](10) NULL,
 [AddressTypeID] [int] NOT NULL,
 [SequenceID] [int] NULL,
 CONSTRAINT [PK_address] PRIMARY KEY CLUSTERED 
(
 [entity_id] ASC,
 [entity_type_id] ASC,
 [AddressTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[agency](
 [agency_id] int NOT NULL,
 [entity_type_id] int NOT NULL,
 [is_active] bit NOT NULL,
 [agency_name] varchar(50) NOT NULL,
 [customer_number] varchar(50) NULL,
 [agency_type] varchar(10) NULL,
 [selfpay_setting] varchar(1) NULL,
 [contact_last_name] varchar(50) NULL,
 [contact_first_name] varchar(50) NULL,
 [create_date] datetime NOT NULL,
 [change_date] datetime NOT NULL,
 [change_user_id] int NOT NULL,
 [can_fax_alerts] bit NOT NULL,
 [customer_id] int NULL,
 [Agency_Password] uniqueidentifier NULL,
 [service_level] int NOT NULL,
 [appUser_password_expiration_days] int NOT NULL,
 [default_language_id] int NOT NULL,
 [agency_group_id] int NULL,
 [inclusion_grace_period_default] int NULL,
 [rf_grace_period_default] int NULL,
 [terms_id] varchar(50) NULL,
 [CustomerGroupId] int NULL
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Client](
[client_id] int NOT NULL,
[entity_type_id] int NOT NULL,
[is_active] bit NOT NULL,
[last_name] varchar(50) NOT NULL,
[first_name] varchar(50) NOT NULL,
[middle_initial] varchar(1) NULL,
[ssn] varchar(11) NULL,
[birth_date] datetime NULL,
[gender] varchar(1) NULL,
[alias] varchar(50) NULL,
[comment] nvarchar(1000) NULL,
[inactivate_reason] varchar(50) NULL,
[create_date] datetime NOT NULL,
[change_date] datetime NOT NULL,
[change_user_id] int NOT NULL,
[case_number] varchar(20) NULL,
[start_date] datetime NULL,
[end_date] datetime NULL,
[risk_level] int NULL,
[default_language_id] int NOT NULL,
[height] int NULL,
[weight] int NULL,
[client_type_id ]int NULL,
[country_code_of_origin] int NULL,
[spoken_language_id] int NULL,
[EducationLevelId] int NULL,
[EnglishSpokenProficiency] int NOT NULL,
[EnglishWrittenProficiency] int NOT NULL,
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GPSData](
 [GPSData_id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
 [lSerial_ID] [int] NULL,
 [fLatitude] [float] NULL,
 [fLongitude] [float] NULL,
 [nSpeed] [numeric](4, 1) NULL,
 [nBearing] [numeric](4, 1) NULL,
 [tiNum_Sats] [tinyint] NULL,
 [tiFix_Type] [tinyint] NULL,
 [rHDOP] [real] NULL,
 [rPDOP] [real] NULL,
 [client_id] [int] NULL,
 [biPacketID] [bigint] NULL,
 [tiCell_Strength] [tinyint] NULL,
 [event_date_gmt] [datetime] NULL,
 [receive_date_gmt] [datetime] NOT NULL,
 [event_date_local] [datetime] NULL,
 [receive_date_local] [datetime] NULL,
 [point_color] [tinyint] NULL,
 [fix_confidence_level] [int] NULL,
 [fix_confidence_threshold] [int] NULL,
 [was_processed] [bit] NULL,
 [tiCurrent_Zone_State] [tinyint] NULL,
 CONSTRAINT [PK_GPSData_id] PRIMARY KEY CLUSTERED 
(
 [GPSData_id] DESC,
 [receive_date_gmt] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)

GO

SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbldevice](
[ldevice_id] int NOT NULL,
[seid] int NOT NULL,
[sdevice_description] varchar(255) NULL,
[bdevice_active] bit NOT NULL,
[client_id] int NULL,
[ldevice_type_id] int NOT NULL,
[agency_id] int NULL,
[dassigned] datetime NULL,
[status] char(2) NULL,
[dmodified] datetime NULL,
[change_user_id] int NULL,
[dtCallBack] datetime NULL,
[dStatus_date] datetime NULL,
[bAgencyOwned] bit NULL,
[case_id] int NULL,
[is_voice_enabled] bit NOT NULL,
[service_plan_id] int NULL,
[rlc_timestamp] datetime NULL,
[calibration_due_date] datetime NULL,
) ON [PRIMARY]
GO