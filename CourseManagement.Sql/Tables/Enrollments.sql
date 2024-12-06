USE [DB_CourseManagement]
GO

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