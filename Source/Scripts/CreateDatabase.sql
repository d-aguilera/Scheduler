USE [master]
GO

/****** Object:  Database [$(varDbName)]    Script Date: 01/09/2016 18:27:05 ******/
CREATE DATABASE [$(varDbName)] ON  PRIMARY 
( NAME = N'$(varDbName)', FILENAME = N'$(varDataPath)\$(varDbName).mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'$(varDbName)_log', FILENAME = N'$(varDataPath)\$(varDbName)_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [$(varDbName)] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [$(varDbName)].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [$(varDbName)] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [$(varDbName)] SET ANSI_NULLS OFF
GO
ALTER DATABASE [$(varDbName)] SET ANSI_PADDING OFF
GO
ALTER DATABASE [$(varDbName)] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [$(varDbName)] SET ARITHABORT OFF
GO
ALTER DATABASE [$(varDbName)] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [$(varDbName)] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [$(varDbName)] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [$(varDbName)] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [$(varDbName)] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [$(varDbName)] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [$(varDbName)] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [$(varDbName)] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [$(varDbName)] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [$(varDbName)] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [$(varDbName)] SET  DISABLE_BROKER
GO
ALTER DATABASE [$(varDbName)] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [$(varDbName)] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [$(varDbName)] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [$(varDbName)] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [$(varDbName)] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [$(varDbName)] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [$(varDbName)] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [$(varDbName)] SET  READ_WRITE
GO
ALTER DATABASE [$(varDbName)] SET RECOVERY FULL
GO
ALTER DATABASE [$(varDbName)] SET  MULTI_USER
GO
ALTER DATABASE [$(varDbName)] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [$(varDbName)] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'Scheduler', N'ON'
GO

USE [$(varDbName)]
GO

/****** Object:  Table [dbo].[Clients]    Script Date: 01/09/2016 18:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NetworkName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[AgentPort] [int] NULL,
	[AgentVirtualDirectory] [varchar](100) NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[LastUpdated] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_Clients_NetworkName] ON [dbo].[Clients] 
(
	[NetworkName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ScheduleEntries]    Script Date: 01/09/2016 18:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ScheduleEntries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[CronExpression] [varchar](50) NOT NULL,
	[ShellCommand] [nvarchar](max) NOT NULL,
	[WorkingDirectory] [nvarchar](500) NULL,
	[Enabled] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[LastUpdated] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_ScheduleEntries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_ScheduleEntries_ClientId] ON [dbo].[ScheduleEntries] 
(
	[ClientId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[LogEntries]    Script Date: 01/09/2016 18:27:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogEntries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShellCommand] [nvarchar](max) NULL,
	[WorkingDirectory] [nvarchar](500) NULL,
	[ScheduleEntryId] [int] NULL,
	[ClientId] [int] NOT NULL,
	[Started] [datetime] NULL,
	[Finished] [datetime] NULL,
	[ExitCode] [int] NULL,
	[ProcessId] [int] NULL,
	[ConsoleOut] [nvarchar](max) NULL,
	[ErrorOut] [nvarchar](max) NULL,
	[Forced] [bit] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[LastUpdated] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_LogEntries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_LogEntries] ON [dbo].[LogEntries] 
(
	[ClientId] ASC,
	[Started] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Default [DF_ScheduleEntries_Enabled]    Script Date: 01/09/2016 18:27:06 ******/
ALTER TABLE [dbo].[ScheduleEntries] ADD  CONSTRAINT [DF_ScheduleEntries_Enabled]  DEFAULT ((0)) FOR [Enabled]
GO

/****** Object:  ForeignKey [FK_ScheduleEntries_Clients]    Script Date: 01/09/2016 18:27:06 ******/
ALTER TABLE [dbo].[ScheduleEntries]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleEntries_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
GO
ALTER TABLE [dbo].[ScheduleEntries] CHECK CONSTRAINT [FK_ScheduleEntries_Clients]
GO

/****** Object:  ForeignKey [FK_LogEntries_ScheduleEntries]    Script Date: 01/09/2016 18:27:06 ******/
ALTER TABLE [dbo].[LogEntries]  WITH CHECK ADD  CONSTRAINT [FK_LogEntries_ScheduleEntries] FOREIGN KEY([ScheduleEntryId])
REFERENCES [dbo].[ScheduleEntries] ([Id])
GO
ALTER TABLE [dbo].[LogEntries] CHECK CONSTRAINT [FK_LogEntries_ScheduleEntries]
GO
