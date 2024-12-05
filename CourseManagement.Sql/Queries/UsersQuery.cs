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

        public static string GetTopLatest =>
        @"
            SELECT  TOP (@Top) *
              FROM  Users
          ORDER BY  CreatedAt DESC
        ";

        public static string Count =>
        @"
            SELECT  COUNT(UserId)
              FROM  Users
             WHERE  (
                      FullName LIKE '%' + @SearchWord + '%'
                      OR
                      Email LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (Role = @Role OR @Role = '')
               AND  (IsActive = @IsActive OR @IsActive IS NULL)
        ";

        public static string Search =>
        @"
            SELECT  *
              FROM  Users
             WHERE  (
                      FullName LIKE '%' + @SearchWord + '%'
                      OR
                      Email LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (Role = @Role OR @Role = '')
               AND  (IsActive = @IsActive OR @IsActive IS NULL)
          ORDER BY
                    CASE @OrderBy
                      WHEN  '1' THEN FullName
                      WHEN  '3' THEN Email
                      WHEN  '5' THEN Role
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '2' THEN FullName
                      WHEN  '4' THEN Email
                      WHEN  '6' THEN Role
                      ELSE  '0'
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '7' THEN IsActive
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '8' THEN IsActive
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
               AND  IsActive = 1
        ";

        public static string Get =>
        @"
            SELECT  *
              FROM  Users
             WHERE  UserId = @UserId
        ";

        public static string CheckExistEmail =>
        @"
            SELECT  CASE WHEN EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND UserId <> @UserId)
                        THEN 1
                        ELSE 0
                    END AS Result
        ";

        public static string Insert =>
        @"
            INSERT INTO Users
                (
                    FullName
                    ,Email
                    ,PasswordHash
                    ,Role
                    ,IsActive
                    ,CreatedAt
                    ,LastChanged
                )
            VALUES
                (
                    @FullName
                    ,@Email
                    ,@PasswordHash
                    ,@Role
                    ,@IsActive
                    ,GETDATE()
                    ,@LastChanged
                )

            SELECT SCOPE_IDENTITY();
        ";

        public static string Update =>
        @"
            UPDATE  Users
               SET  FullName = @FullName
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
            DELETE  Users
             WHERE  UserId = @UserId
        ";
    }
}
