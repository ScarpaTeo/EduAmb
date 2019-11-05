USE [cursomvcapi]
GO

/****** Object:  Table [dbo].[User]    Script Date: 5/11/2019 17:48:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](250) NULL,
	[apellido] [varchar](250) NOT NULL,
	[ubicacion] [varchar](300) NULL,
	[email] [varchar](300) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[token] [varchar](100) NULL,
	[foto] [nvarchar](max) NULL,
	[rol] [int] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

