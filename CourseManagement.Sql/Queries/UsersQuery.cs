using System.Diagnostics.CodeAnalysis;

namespace CourseManagement.Sql.Queries
{
    /// <summary>
    /// Users query
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class UsersQuery
    {
        public static string CheckLogin =>
        @"
            SELECT  *
              FROM  Users
             WHERE  (
                      UserName = @UserName
                      OR
                      Email = @UserName
                    )
               AND  PasswordHash = @PasswordHash
               AND  IsActive = 1
               AND  DeletedFlag = 0
        ";

        public static string Get =>
        @"
            SELECT  *
              FROM  Users
             WHERE  UserId = @UserId
               AND  DeletedFlag = 0
        ";

        public static string CheckExistUserName =>
        @"
            SELECT  CASE WHEN EXISTS (SELECT 1 FROM Users WHERE UserName = @UserName AND UserId <> @UserId AND DeletedFlag = 0)
                        THEN 1
                        ELSE 0
                    END AS Result
        ";

        public static string CheckExistEmail =>
        @"
            SELECT  CASE WHEN EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND UserId <> @UserId AND DeletedFlag = 0)
                        THEN 1
                        ELSE 0
                    END AS Result
        ";

        public static string Insert =>
        @"
            INSERT INTO Users
                (
                    UserName
                    ,PasswordHash
                    ,FullName
                    ,Email
                    ,Role
                    ,IsActive
                    ,CreatedAt
                    ,LastChanged
                    ,DeletedFlag
                )
            VALUES
                (
                    @FullName
                    ,@PhoneNumber
                    ,@Email
                    ,@PasswordHash
                    ,@UserRole
                    ,@IsActive
                    ,GETDATE()
                    ,@LastChanged
                    ,0
                )

            SELECT SCOPE_IDENTITY();
        ";

        public static string Update =>
        @"
            UPDATE  Users
               SET  UserName = @UserName
                    ,FullName = @FullName
                    ,Email = @Email
                    ,Role = @Role
                    ,IsActive = @IsActive
                    ,UpdatedAt = GETDATE()
                    ,LastChanged = @LastChanged
             WHERE  UserId = @UserId
        ";

        public static string UpdatePassword =>
        @"
            UPDATE  Users
               SET  PasswordHash = @PasswordHash
                    ,UpdatedAt = GETDATE()
                    ,LastChanged = @LastChanged
             WHERE  UserId = @UserId
        ";

        public static string Delete =>
        @"
            UPDATE  Users
               SET  DeletedFlag = 1
                    ,DeletedAt = GETDATE()
                    ,LastChanged = @LastChanged
             WHERE  UserId = @UserId
        ";
    }
}
