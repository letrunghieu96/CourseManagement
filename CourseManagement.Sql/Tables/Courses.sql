USE [DB_CourseManagement]
GO

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