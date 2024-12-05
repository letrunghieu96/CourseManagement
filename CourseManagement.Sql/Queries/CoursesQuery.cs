using System.Diagnostics.CodeAnalysis;

namespace CourseManagement.Sql.Queries
{
    /// <summary>
    /// Courses query
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CoursesQuery
    {
        public static string GetTotal =>
        @"
            SELECT  COUNT(CourseId)
              FROM  Courses
        ";

        public static string GetTopLatest =>
        @"
            SELECT  TOP (@Top)
                    Courses.*,
                    Users.FullName AS CreatorName
              FROM  Courses
                    INNER JOIN Users ON
                    (
                      Courses.CreatedBy = Users.UserId
                    )
             WHERE  Courses.IsActive = 1
               AND  (Courses.StartDate <= GETDATE())
               AND  (Courses.EndDate >= GETDATE())
          ORDER BY  Courses.CreatedAt
        ";

        public static string Count =>
        @"
            SELECT  COUNT(*)
              FROM  Courses
                    INNER JOIN Users ON
                    (
                      Courses.CreatedBy = Users.UserId
                    )
             WHERE  (
                      Courses.CourseCode LIKE '%' + @SearchWord + '%'
                      OR
                      Courses.CourseName LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (Courses.StartDate >= @StartDateFrom OR @StartDateFrom IS NULL)
               AND  (Courses.StartDate <= @StartDateTo OR @StartDateTo IS NULL)
               AND  (Courses.EndDate >= @EndDateFrom OR @EndDateFrom IS NULL)
               AND  (Courses.EndDate <= @EndDateTo OR @EndDateTo IS NULL)
               AND  (Courses.IsActive = @IsActive OR @IsActive IS NULL)
               AND  (Users.FullName LIKE '%' + @CreatorName + '%' OR @CreatorName = '')
        ";

        public static string Search =>
        @"
            SELECT  Courses.*,
                    Users.FullName AS CreatorName
              FROM  Courses
                    INNER JOIN Users ON
                    (
                      Courses.CreatedBy = Users.UserId
                    )
             WHERE  (
                      Courses.CourseCode LIKE '%' + @SearchWord + '%'
                      OR
                      Courses.CourseName LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (Courses.StartDate >= @StartDateFrom OR @StartDateFrom IS NULL)
               AND  (Courses.StartDate <= @StartDateTo OR @StartDateTo IS NULL)
               AND  (Courses.EndDate >= @EndDateFrom OR @EndDateFrom IS NULL)
               AND  (Courses.EndDate <= @EndDateTo OR @EndDateTo IS NULL)
               AND  (Courses.IsActive = @IsActive OR @IsActive IS NULL)
               AND  (Users.FullName LIKE '%' + @CreatorName + '%' OR @CreatorName = '')
          ORDER BY
                    CASE @OrderBy
                      WHEN  '1' THEN Courses.CourseCode
                      WHEN  '3' THEN Courses.CourseName
                      WHEN  '15' THEN Users.FullName
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '2' THEN Courses.CourseCode
                      WHEN  '4' THEN Courses.CourseName
                      WHEN  '16' THEN Users.FullName
                      ELSE  '0'
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '5' THEN Courses.Duration
                      WHEN  '11' THEN Courses.IsActive
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '6' THEN Courses.Duration
                      WHEN  '12' THEN Courses.IsActive
                      ELSE  '0'
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '7' THEN Courses.StartDate
                      WHEN  '9' THEN Courses.EndDate
                      WHEN  '13' THEN Courses.CreatedAt
                      ELSE  NULL
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '8' THEN Courses.StartDate
                      WHEN  '10' THEN Courses.EndDate
                      WHEN  '14' THEN Courses.CreatedAt
                      ELSE  NULL
                    END
                      DESC,
                    Courses.CourseId DESC
            OFFSET  @BeginRowNum ROWS
            FETCH NEXT  @RowsOfPage ROWS ONLY
        ";

        public static string Get =>
        @"
            SELECT  *
              FROM  Courses
             WHERE  CourseId = @CourseId
        ";

        public static string CheckExistCourseCode =>
        @"
            SELECT  CASE WHEN EXISTS (SELECT 1 FROM Courses WHERE CourseCode = @CourseCode AND CourseId <> @CourseId)
                        THEN 1
                        ELSE 0
                    END AS Result
        ";

        public static string Insert =>
        @"
            INSERT INTO Courses
                (
                    CourseCode
                    ,CourseName
                    ,Description
                    ,Duration
                    ,StartDate
                    ,EndDate
                    ,IsActive
                    ,Price
                    ,CreatedBy
                    ,CreatedAt
                    ,LastChanged
                )
            VALUES
                (
                    @CourseCode
                    ,@CourseName
                    ,@Description
                    ,@Duration
                    ,@StartDate
                    ,@EndDate
                    ,@IsActive
                    ,@Price
                    ,@CreatedBy
                    ,GETDATE()
                    ,@LastChanged
                )

            SELECT SCOPE_IDENTITY();
        ";

        public static string Update =>
        @"
            UPDATE  Courses
               SET  CourseCode = @CourseCode
                    ,CourseName = @CourseName
                    ,Description = @Description
                    ,Duration = @Duration
                    ,StartDate = @StartDate
                    ,EndDate = @EndDate
                    ,Price = @Price
                    ,IsActive = @IsActive
                    ,UpdatedAt = GETDATE()
                    ,LastChanged = @LastChanged
             WHERE  CourseId = @CourseId
        ";

        public static string Delete =>
        @"
            DELETE  Courses
             WHERE  CourseId = @CourseId
        ";
    }
}
