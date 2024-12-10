using System.Diagnostics.CodeAnalysis;

namespace CourseManagement.Sql.Queries
{
    /// <summary>
    /// Users query
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class UsersQuery
    {
        public static string GetTotal =>
        @"
            SELECT  COUNT(UserId)
              FROM  Users
        ";

        public static string Count =>
        @"
            SELECT  COUNT(UserId)
              FROM  Users
             WHERE  (
                      UserName LIKE '%' + @SearchWord + '%'
                      OR
                      Email LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (UserRole = @UserRole OR @UserRole = '')
               AND  (Status = @Status OR @Status IS NULL)
        ";

        public static string Search =>
        @"
            SELECT  *
              FROM  Users
             WHERE  (
                      UserName LIKE '%' + @SearchWord + '%'
                      OR
                      Email LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (UserRole = @UserRole OR @UserRole = '')
               AND  (Status = @Status OR @Status IS NULL)
          ORDER BY
                    CASE @OrderBy
                      WHEN  '1' THEN UserName
                      WHEN  '3' THEN Email
                      WHEN  '5' THEN UserRole
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '2' THEN UserName
                      WHEN  '4' THEN Email
                      WHEN  '6' THEN UserRole
                      ELSE  '0'
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '7' THEN Status
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '8' THEN Status
                      ELSE  '0'
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '9' THEN CreatedAt
                      ELSE  NULL
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '10' THEN CreatedAt
                      ELSE  NULL
                    END
                      DESC,
                    UserId DESC
            OFFSET  @BeginRowNum ROWS
            FETCH NEXT  @RowsOfPage ROWS ONLY
        ";

        public static string CheckLogin =>
        @"
            SELECT  *
              FROM  Users
             WHERE  Email = @Email
               AND  PasswordHash = @PasswordHash
               AND  Status = 1
        ";

        public static string Get =>
        @"
            SELECT  *
              FROM  Users
             WHERE  UserId = @UserId
        ";

        public static string CheckExistEmail =>
        @"
            SELECT  CASE WHEN EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND (UserId <> @UserId OR @UserId IS NULL))
                        THEN 1
                        ELSE 0
                    END AS Result
        ";

        public static string Insert =>
        @"
            INSERT INTO Users
                (
                    UserName
                    ,Email
                    ,PasswordHash
                    ,UserRole
                    ,Status
                    ,CreatedAt
                    ,LastChanged
                )
            VALUES
                (
                    @UserName
                    ,@Email
                    ,@PasswordHash
                    ,@UserRole
                    ,@Status
                    ,GETDATE()
                    ,@LastChanged
                )

            SELECT SCOPE_IDENTITY();
        ";

        public static string Update =>
        @"
            UPDATE  Users
               SET  UserName = @UserName
                    ,Email = @Email
                    ,UserRole = @UserRole
                    ,Status = @Status
                    ,UpdatedAt = GETDATE()
                    ,LastChanged = @LastChanged
             WHERE  UserId = @UserId
        ";

        public static string UpdateUser =>
        @"
            UPDATE  Users
               SET  UserName = @UserName
                    ,Email = @Email
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
            DELETE  Users
             WHERE  UserId = @UserId
        ";
    }
}
