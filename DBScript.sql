USE [master]
GO
/****** Object:  Database [HotelManagement]    Script Date: 05-10-2026 16:18:34 ******/
CREATE DATABASE [HotelManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HotelManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLSERVER1\MSSQL\DATA\HotelManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HotelManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLSERVER1\MSSQL\DATA\HotelManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [HotelManagement] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HotelManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HotelManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HotelManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HotelManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HotelManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HotelManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [HotelManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HotelManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HotelManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HotelManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HotelManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HotelManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HotelManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HotelManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HotelManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HotelManagement] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HotelManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HotelManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HotelManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HotelManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HotelManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HotelManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HotelManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HotelManagement] SET RECOVERY FULL 
GO
ALTER DATABASE [HotelManagement] SET  MULTI_USER 
GO
ALTER DATABASE [HotelManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HotelManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HotelManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HotelManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HotelManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [HotelManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'HotelManagement', N'ON'
GO
ALTER DATABASE [HotelManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [HotelManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [HotelManagement]
GO
/****** Object:  Table [dbo].[ItemList]    Script Date: 05-10-2026 16:18:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemList](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](max) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_ItemList] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItemList]    Script Date: 05-10-2026 16:18:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItemList](
	[OrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ItemId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[FinalPrice] [decimal](18, 2) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_OrderItemList] PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderList]    Script Date: 05-10-2026 16:18:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderList](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](max) NOT NULL,
	[TableId] [int] NOT NULL,
	[BillAmount] [decimal](18, 2) NOT NULL,
	[BillPayed] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_OrderList] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TableList]    Script Date: 05-10-2026 16:18:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TableList](
	[TableId] [int] IDENTITY(1,1) NOT NULL,
	[TableNumber] [int] NOT NULL,
	[TableName] [nvarchar](max) NOT NULL,
	[DisplayTableName] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
 CONSTRAINT [PK_TableList] PRIMARY KEY CLUSTERED 
(
	[TableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05-10-2026 16:18:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[ImageName] [nvarchar](max) NULL,
	[ResetToken] [nvarchar](max) NULL,
	[ResetTokenExpiry] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ItemList] ON 
GO
INSERT [dbo].[ItemList] ([ItemId], [ItemName], [Price], [IsActive], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy]) VALUES (6, N'Roti', CAST(10.00 AS Decimal(18, 2)), 1, 0, CAST(N'2026-09-19T14:54:46.183' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[ItemList] ([ItemId], [ItemName], [Price], [IsActive], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy]) VALUES (7, N'Panir Tikka', CAST(150.00 AS Decimal(18, 2)), 1, 0, CAST(N'2026-09-19T14:55:11.620' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[ItemList] ([ItemId], [ItemName], [Price], [IsActive], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy]) VALUES (8, N'Chhas', CAST(15.00 AS Decimal(18, 2)), 1, 0, CAST(N'2026-09-19T14:55:36.413' AS DateTime), 2, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ItemList] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderItemList] ON 
GO
INSERT [dbo].[OrderItemList] ([OrderItemId], [OrderId], [ItemId], [Quantity], [Price], [FinalPrice], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy]) VALUES (15, 4, 6, 4, CAST(10.00 AS Decimal(18, 2)), CAST(40.00 AS Decimal(18, 2)), 0, CAST(N'2026-09-19T14:57:04.237' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[OrderItemList] ([OrderItemId], [OrderId], [ItemId], [Quantity], [Price], [FinalPrice], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy]) VALUES (16, 4, 7, 1, CAST(150.00 AS Decimal(18, 2)), CAST(150.00 AS Decimal(18, 2)), 0, CAST(N'2026-09-19T14:57:04.237' AS DateTime), 2, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[OrderItemList] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderList] ON 
GO
INSERT [dbo].[OrderList] ([OrderId], [CustomerName], [TableId], [BillAmount], [BillPayed], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy], [IsDeleted]) VALUES (4, N'Denish', 5, CAST(190.00 AS Decimal(18, 2)), 1, CAST(N'2026-09-19T14:57:04.237' AS DateTime), 2, NULL, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[OrderList] OFF
GO
SET IDENTITY_INSERT [dbo].[TableList] ON 
GO
INSERT [dbo].[TableList] ([TableId], [TableNumber], [TableName], [DisplayTableName], [IsActive], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy]) VALUES (5, 1, N'A', N'1_A', 1, 0, CAST(N'2026-09-19T14:55:55.690' AS DateTime), 2, NULL, NULL)
GO
INSERT [dbo].[TableList] ([TableId], [TableNumber], [TableName], [DisplayTableName], [IsActive], [IsDeleted], [CreatedDate], [CreatedBy], [UpdatedDate], [UpdatedBy]) VALUES (6, 2, N'B', N'2_B', 1, 0, CAST(N'2026-09-19T14:56:06.867' AS DateTime), 2, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[TableList] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserId], [Username], [Email], [Password], [ImageName], [ResetToken], [ResetTokenExpiry], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate], [IsActive], [IsDeleted]) VALUES (2, N'Admin', N'admin@mailinator.com', N'Rtw5TGnwmLpGccxaDMpTgg==', N'shiv_20260919145301776.png', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJhNDg1MDBlNi1kZGQ3LTRmMTktYjFiMC1mNmIwODAzODBmZWMiLCJVc2VySWQiOiIyIiwiVXNlcm5hbWUiOiJBZG1pbiIsIkVtYWlsIjoiYWRtaW5AbWFpbGluYXRvci5jb20iLCJleHAiOjE3ODk4MTM4Mzl9.m0tZucRHm7yibjLOzkrPinIG9m6mmS8_MWugMD_fbps', CAST(N'2026-09-19T10:30:39.920' AS DateTime), NULL, CAST(N'2026-09-19T14:53:01.867' AS DateTime), NULL, NULL, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[ItemList]  WITH CHECK ADD  CONSTRAINT [FK_ItemList_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[ItemList] CHECK CONSTRAINT [FK_ItemList_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[ItemList]  WITH CHECK ADD  CONSTRAINT [FK_ItemList_UpdatedBy_Users_UserId] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[ItemList] CHECK CONSTRAINT [FK_ItemList_UpdatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[OrderItemList]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemList_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[OrderItemList] CHECK CONSTRAINT [FK_OrderItemList_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[OrderItemList]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemList_ItemId_ItemList_ItemId] FOREIGN KEY([ItemId])
REFERENCES [dbo].[ItemList] ([ItemId])
GO
ALTER TABLE [dbo].[OrderItemList] CHECK CONSTRAINT [FK_OrderItemList_ItemId_ItemList_ItemId]
GO
ALTER TABLE [dbo].[OrderItemList]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemList_OrderId_OrderList_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[OrderList] ([OrderId])
GO
ALTER TABLE [dbo].[OrderItemList] CHECK CONSTRAINT [FK_OrderItemList_OrderId_OrderList_OrderId]
GO
ALTER TABLE [dbo].[OrderItemList]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemList_UpdatedBy_Users_UserId] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[OrderItemList] CHECK CONSTRAINT [FK_OrderItemList_UpdatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[OrderList]  WITH CHECK ADD  CONSTRAINT [FK_OrderList_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[OrderList] CHECK CONSTRAINT [FK_OrderList_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[OrderList]  WITH CHECK ADD  CONSTRAINT [FK_OrderList_TableId_TableList_TableId] FOREIGN KEY([TableId])
REFERENCES [dbo].[TableList] ([TableId])
GO
ALTER TABLE [dbo].[OrderList] CHECK CONSTRAINT [FK_OrderList_TableId_TableList_TableId]
GO
ALTER TABLE [dbo].[OrderList]  WITH CHECK ADD  CONSTRAINT [FK_OrderList_UpdatedBy_Users_UserId] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[OrderList] CHECK CONSTRAINT [FK_OrderList_UpdatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[TableList]  WITH CHECK ADD  CONSTRAINT [FK_TableList_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[TableList] CHECK CONSTRAINT [FK_TableList_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[TableList]  WITH CHECK ADD  CONSTRAINT [FK_TableList_UpdatedBy_Users_UserId] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[TableList] CHECK CONSTRAINT [FK_TableList_UpdatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_CreatedBy_Users_UserId] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_CreatedBy_Users_UserId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UpdatedBy_Users_UserId] FOREIGN KEY([UpdatedBy])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UpdatedBy_Users_UserId]
GO
USE [master]
GO
ALTER DATABASE [HotelManagement] SET  READ_WRITE 
GO
