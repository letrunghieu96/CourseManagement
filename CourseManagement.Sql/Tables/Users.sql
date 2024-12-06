USE [DB_CourseManagement]
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
    BEGIN
        DROP TABLE [dbo].[Users]
    END
GO

CREATE TABLE [dbo].[Users] (
    [UserId] [int] IDENTITY (1, 1) NOT NULL,
    [UserName] [nvarchar] (100) NOT NULL DEFAULT (N''),
    [Email] [varchar] (255) NOT NULL UNIQUE,
    [PasswordHash] [varchar] (255) NOT NULL DEFAULT (''),
    [UserRole] [varchar] (20) NOT NULL DEFAULT (''),
    [Status] [bit] NOT NULL DEFAULT (1),
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime],
    [LastChanged] [nvarchar] (100) NOT NULL DEFAULT (N'')
) ON [PRIMARY]
GO

-- Primary key
ALTER TABLE [dbo].[Users] WITH NOCHECK ADD
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
    (
        [UserId]
    ) ON [PRIMARY]
GO