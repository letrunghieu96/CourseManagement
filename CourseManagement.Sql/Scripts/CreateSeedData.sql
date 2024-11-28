----------------------------------- CREATE SEED DATA -----------------------------------
USE [DB_CourseManagement]
GO
-- =============================================

IF NOT EXISTS (SELECT * FROM Users)
    BEGIN

        -- Insert
        INSERT Users (
            FullName,
            PhoneNumber,
            Email,
            PasswordHash,
            UserRole,
            Status,
            Description,
            CreatedBy,
            CreatedDate,
            DeletedFlag)
        VALUES
            (N'Lê Trung Hiếu', '0966800514', 'letrunghieu@gmail.com', 'C0KNYEfdd6UplaRuDPCT03N8Xs/alb8s+tdm57jgChk=', 'Admin', 1, '', 'System', GETDATE(), 0)
            ,(N'Admin', '0999999999', 'Admin@gmail.com', 'C0KNYEfdd6UplaRuDPCT03N8Xs/alb8s+tdm57jgChk=', 'Admin', 1, '', 'System', GETDATE(), 0)
            ,(N'Thủ kho', '0888888888', 'ThuKho@gmail.com', 'C0KNYEfdd6UplaRuDPCT03N8Xs/alb8s+tdm57jgChk=', 'Stocker', 1, '', 'System', GETDATE(), 0);

    END
GO