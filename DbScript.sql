USE [master]
GO
/****** Object:  Database [ProfesTestDb]    Script Date: 06/12/2024 16:45:23 ******/
CREATE DATABASE [ProfesTestDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ProfesTestDb', FILENAME = N'C:\Server\MSSQLData\ProfesTestDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ProfesTestDb_log', FILENAME = N'C:\Server\MSSQLData\ProfesTestDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ProfesTestDb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProfesTestDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ProfesTestDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ProfesTestDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ProfesTestDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ProfesTestDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ProfesTestDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [ProfesTestDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ProfesTestDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ProfesTestDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ProfesTestDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ProfesTestDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ProfesTestDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ProfesTestDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ProfesTestDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ProfesTestDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ProfesTestDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ProfesTestDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ProfesTestDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ProfesTestDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ProfesTestDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ProfesTestDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ProfesTestDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ProfesTestDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ProfesTestDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ProfesTestDb] SET  MULTI_USER 
GO
ALTER DATABASE [ProfesTestDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ProfesTestDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ProfesTestDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ProfesTestDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ProfesTestDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ProfesTestDb] SET QUERY_STORE = OFF
GO
USE [ProfesTestDb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [ProfesTestDb]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 06/12/2024 16:45:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesOrder]    Script Date: 06/12/2024 16:45:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesOrder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [varchar](50) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[CustomerId] [int] NOT NULL,
 CONSTRAINT [PK_SalesOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SalesOrder]  WITH CHECK ADD  CONSTRAINT [FK_SalesOrder_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[SalesOrder] CHECK CONSTRAINT [FK_SalesOrder_Customer]
GO
USE [master]
GO
ALTER DATABASE [ProfesTestDb] SET  READ_WRITE 
GO

USE [ProfesTestDb]
GO


INSERT INTO dbo.Customer (Name) VALUES ('PROFES')
INSERT INTO dbo.Customer (Name) VALUES ('TITAN')
INSERT INTO dbo.Customer (Name) VALUES ('DIPS')
INSERT INTO dbo.Customer (Name) VALUES ('CUST01')


INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_001', '2024-12-06', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_002', '2024-12-07', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_003', '2024-12-08', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_004', '2024-12-09', 4)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_005', '2024-12-10', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_006', '2024-12-11', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_007', '2024-12-12', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_008', '2024-12-13', 4)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_009', '2024-12-14', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_010', '2024-12-15', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_011', '2024-12-16', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_012', '2024-12-17', 4)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_013', '2024-12-18', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_014', '2024-12-19', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_015', '2024-12-20', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_016', '2024-12-21', 4)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_017', '2024-12-22', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_018', '2024-12-23', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_019', '2024-12-24', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_020', '2024-12-25', 4)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_021', '2024-12-26', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_022', '2024-12-27', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_023', '2024-12-28', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_024', '2024-12-29', 4)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_025', '2024-12-30', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_026', '2024-12-31', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_027', '2025-01-01', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_028', '2025-01-02', 4)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_029', '2025-01-03', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_030', '2025-01-04', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_031', '2025-01-05', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_032', '2025-01-06', 4)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_033', '2025-01-07', 1)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_034', '2025-01-08', 2)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_035', '2025-01-09', 3)
INSERT INTO dbo.SalesOrder (OrderNumber,OrderDate,CustomerId) VALUES ('50_036', '2025-01-10', 4)
GO

