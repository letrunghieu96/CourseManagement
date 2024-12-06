----------------------------------- CREATE SEED DATA -----------------------------------
USE [DB_CourseManagement]
GO
-- =============================================

IF NOT EXISTS (SELECT * FROM Users)
    BEGIN

        -- Insert
        INSERT Users (
            UserName,
            Email,
            PasswordHash,
            UserRole,
            Status,
            CreatedAt,
            LastChanged)
        VALUES
            (N'Lê Trung Hiếu', 'letrunghieu@gmail.com', 'mRfMAOd2duSPa6+laInjbIWFI5kVLrg/lODXCXKDrac=', 'Admin', 1, GETDATE(), 'System')

    END
GO