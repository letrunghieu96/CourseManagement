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
    [CourseImage] [nvarchar] (255) NOT NULL DEFAULT (N''),
    [MainContent] [nvarchar] (MAX) NOT NULL DEFAULT (N''),
    [CourseFile] [nvarchar] (255) NOT NULL DEFAULT (N''),
    [Duration] [int] NOT NULL,
    [StartDate] [date] NOT NULL,
    [EndDate] [date],
    [Price] [decimal] (18, 2) NOT NULL,
    [Status] [tinyint] NOT NULL DEFAULT (1),
    [Lecturer] [nvarchar] (100) NOT NULL DEFAULT (N''),
    [CreatedAt] [datetime] NOT NULL DEFAULT (GETDATE()),
    [UpdatedAt] [datetime],
    [LastChanged] [nvarchar] (100) NOT NULL DEFAULT (N'')
) ON [PRIMARY]
GO

-- Primary key
ALTER TABLE [dbo].[Courses] WITH NOCHECK ADD
    CONSTRAINT [PK_Courses] PRIMARY KEY CLUSTERED
    (
        [CourseId]
    ) ON [PRIMARY]
GO
-- ===========================================================================
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Enrollments')
    BEGIN
        DROP TABLE [dbo].[Enrollments]
    END
GO

CREATE TABLE [dbo].[Enrollments] (
    [CourseId] [int] NOT NULL,
    [UserId] [int] NOT NULL,
    [EnrollmentTime] [datetime] NOT NULL DEFAULT (GETDATE()),
) ON [PRIMARY]
GO

-- Primary key
ALTER TABLE [dbo].[Enrollments] WITH NOCHECK ADD
    CONSTRAINT [PK_Enrollments] PRIMARY KEY CLUSTERED
    (
        [CourseId],
        [UserId]
    ) ON [PRIMARY]
GO
-- Foreign key
ALTER TABLE [dbo].[Enrollments] WITH NOCHECK ADD
    CONSTRAINT [FK_Enrollments_Courses] FOREIGN KEY
    (
        [CourseId]
    ) REFERENCES [dbo].[Courses] ([CourseId])
GO
ALTER TABLE [dbo].[Enrollments] WITH NOCHECK ADD
    CONSTRAINT [FK_Enrollments_Users] FOREIGN KEY
    (
        [UserId]
    ) REFERENCES [dbo].[Users] ([UserId])
GO