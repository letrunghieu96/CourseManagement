USE [DB_CourseManagement]
GO
-- ===========================================================================
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
    BEGIN
        DROP TABLE [dbo].[Users]
    END
GO

CREATE TABLE [dbo].[Users] (
    [UserId] [int] IDENTITY (1, 1) NOT NULL,
    [FullName] [nvarchar] (100) NOT NULL DEFAULT (N''),
    [Email] [varchar] (255) NOT NULL UNIQUE,
    [PasswordHash] [varchar] (255) NOT NULL DEFAULT (''),
    [Role] [varchar] (20) NOT NULL DEFAULT (''),
    [IsActive] [bit] NOT NULL DEFAULT (1),
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime],
    [DeletedAt] [datetime],
    [LastChanged] [nvarchar] (100) NOT NULL DEFAULT (N''),
    [DeletedFlag] [tinyint] NOT NULL DEFAULT (0)
) ON [PRIMARY]
GO

-- Primary key
ALTER TABLE [dbo].[Users] WITH NOCHECK ADD
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED
    (
        [UserId]
    ) ON [PRIMARY]
GO
-- ===========================================================================
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Courses')
    BEGIN
        DROP TABLE [dbo].[Courses]
    END
GO

CREATE TABLE [dbo].[Courses] (
    [CourseId] [int] IDENTITY (1, 1) NOT NULL,
    [CourseCode] [varchar] (20) NOT NULL UNIQUE,
    [CourseName] [nvarchar] (255) NOT NULL DEFAULT (N''),
    [Description] [nvarchar] (MAX) NOT NULL DEFAULT (N''),
    [Duration] [int] NOT NULL,
    [StartDate] [date] NOT NULL,
    [EndDate] [date],
    [Price] [decimal] (18, 2) NOT NULL,
    [IsActive] [bit] NOT NULL DEFAULT (1),
    [CreatedBy] [int] NOT NULL,
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime],
    [DeletedAt] [datetime],
    [LastChanged] [nvarchar] (100) NOT NULL DEFAULT (N''),
    [DeletedFlag] [tinyint] NOT NULL DEFAULT (0)
) ON [PRIMARY]
GO

-- Primary key
ALTER TABLE [dbo].[Courses] WITH NOCHECK ADD
    CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED
    (
        [CourseId]
    ) ON [PRIMARY]
GO
-- Foreign key
ALTER TABLE [dbo].[Courses] WITH NOCHECK ADD
    CONSTRAINT [FK_Courses_Users] FOREIGN KEY
    (
        [CreatedBy]
    ) REFERENCES [dbo].[Users] ([UserId])
GO