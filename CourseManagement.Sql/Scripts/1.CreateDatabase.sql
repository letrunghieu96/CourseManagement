----------------------------------- CREATE DATA BASE -----------------------------------
USE [MASTER];

IF EXISTS (SELECT NAME FROM MASTER.DBO.SYSDATABASES WHERE NAME = 'DB_CourseManagement')
    BEGIN
        DROP DATABASE [DB_CourseManagement];
    END
GO

CREATE DATABASE [DB_CourseManagement];
GO

USE [DB_CourseManagement];
GO