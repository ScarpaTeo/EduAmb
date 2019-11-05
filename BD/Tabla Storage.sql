USE [cursomvcapi]
GO

/****** Object:  Table [dbo].[Storage]    Script Date: 5/11/2019 17:48:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Storage](
	[id_galeria] [int] IDENTITY(1,1) NOT NULL,
	[foto_name] [nvarchar](300) NOT NULL,
	[foto] [nvarchar](max) NOT NULL,
	[id_identificador] [int] NOT NULL,
 CONSTRAINT [PK_Storage] PRIMARY KEY CLUSTERED 
(
	[id_galeria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

