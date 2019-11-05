USE [cursomvcapi]
GO

/****** Object:  Table [dbo].[Contenido]    Script Date: 5/11/2019 17:49:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Contenido](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[titulo] [nvarchar](300) NOT NULL,
	[subtitulo] [nvarchar](300) NULL,
	[detalle] [nvarchar](max) NULL,
	[categoria] [int] NOT NULL,
	[tags] [nvarchar](300) NULL,
	[picture] [nvarchar](max) NULL,
 CONSTRAINT [PK_Contenido] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

