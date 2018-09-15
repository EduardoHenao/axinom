# axinom
axinom test.





#Database

from "Package Manager Console" inside visual studio
cd DataManagementSystem
update-database
--------------------------------------------------------------------------------------------------------------------------------------------
use master

create database AxinomDB

USE [AxinomDB]
GO

/****** Object:  Table [dbo].[DbFileNodes]    Script Date: 9/13/2018 9:02:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DbFileNodes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RelativePath] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[FileBytes] [varbinary](max) NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DbFileNodes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
--------------------------------------------------------------------------------------------------------------------------------------------

select* from [AxinomDB].[dbo].DbFileNodes

dotnet publish