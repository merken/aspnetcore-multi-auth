/****** Object:  Table [dbo].[Authentication]    Script Date: 24/09/2018 21:05:58 ******/
SET ANSI_NULLS ON
GO


SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Authentication](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Username] [varchar](255) NOT NULL,
  [Password] [varchar](255) NOT NULL,
  CONSTRAINT [PK_Authentication] PRIMARY KEY CLUSTERED
  (
    [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
              
GO
              
INSERT INTO [dbo].[Authentication]
([Username], [Password])
VALUES ('maarten', 'peanuts')
              
GO