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
                    Courses.*
              FROM  Courses
             WHERE  Status = 0
          ORDER BY  CreatedAt DESC
        ";

        public static string GetTopRegistrations =>
        @"
            WITH RegistrationCount AS (
                SELECT  COUNT(UserId) AS TotalUsers,
                        CourseId
                  FROM  Enrollments
              GROUP BY  CourseId
            )

            SELECT  TOP (@Top)
                    RegistrationCount.CourseId,
                    Courses.CourseCode,
                    Courses.CourseName,
                    Courses.CreatedAt,
                    RegistrationCount.TotalUsers
              FROM  RegistrationCount
                    INNER JOIN Courses ON
                    (
                      RegistrationCount.CourseId = Courses.CourseId
                    )
          ORDER BY  RegistrationCount.TotalUsers DESC;
        ";

        public static string Count =>
        @"
            SELECT  COUNT(*)
              FROM  Courses
             WHERE  (
                      CourseCode LIKE '%' + @SearchWord + '%'
                      OR
                      CourseName LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (Lecturer LIKE '%' + @Lecturer + '%' OR @Lecturer = '')
               AND  (StartDate >= @StartDateFrom OR @StartDateFrom IS NULL)
               AND  (StartDate <= @StartDateTo OR @StartDateTo IS NULL)
               AND  (EndDate >= @EndDateFrom OR @EndDateFrom IS NULL)
               AND  (EndDate <= @EndDateTo OR @EndDateTo IS NULL)
               AND  (Status = @Status OR @Status IS NULL)
        ";

        public static string Search =>
        @"
            SELECT  Courses.*
              FROM  Courses
             WHERE  (
                      CourseCode LIKE '%' + @SearchWord + '%'
                      OR
                      CourseName LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (Lecturer LIKE '%' + @Lecturer + '%' OR @Lecturer = '')
               AND  (StartDate >= @StartDateFrom OR @StartDateFrom IS NULL)
               AND  (StartDate <= @StartDateTo OR @StartDateTo IS NULL)
               AND  (EndDate >= @EndDateFrom OR @EndDateFrom IS NULL)
               AND  (EndDate <= @EndDateTo OR @EndDateTo IS NULL)
               AND  (Status = @Status OR @Status IS NULL)
          ORDER BY
                    CASE @OrderBy
                      WHEN  '1' THEN CourseCode
                      WHEN  '3' THEN CourseName
                      WHEN  '13' THEN Lecturer
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '2' THEN CourseCode
                      WHEN  '4' THEN CourseName
                      WHEN  '14' THEN Lecturer
                      ELSE  '0'
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '5' THEN Duration
                      WHEN  '11' THEN Status
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '6' THEN Duration
                      WHEN  '12' THEN Status
                      ELSE  '0'
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '7' THEN StartDate
                      WHEN  '9' THEN EndDate
                      WHEN  '15' THEN CreatedAt
                      ELSE  NULL
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '8' THEN StartDate
                      WHEN  '10' THEN EndDate
                      WHEN  '16' THEN CreatedAt
                      ELSE  NULL
                    END
                      DESC,
                    CourseId DESC
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
            SELECT  CASE WHEN EXISTS (SELECT 1 FROM Courses WHERE CourseCode = @CourseCode AND (CourseId <> @CourseId OR @CourseId IS NULL))
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
                    ,CourseImage
                    ,MainContent
                    ,CourseFile
                    ,Duration
                    ,StartDate
                    ,EndDate
                    ,Status
                    ,Price
                    ,Lecturer
                    ,CreatedAt
                    ,LastChanged
                )
            VALUES
                (
                    @CourseCode
                    ,@CourseName
                    ,@CourseImage
                    ,@MainContent
                    ,@CourseFile
                    ,@Duration
                    ,@StartDate
                    ,@EndDate
                    ,@Status
                    ,@Price
                    ,@Lecturer
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
                    ,CourseImage = @CourseImage
                    ,MainContent = @MainContent
                    ,CourseFile = @CourseFile
                    ,Duration = @Duration
                    ,StartDate = @StartDate
                    ,EndDate = @EndDate
                    ,Price = @Price
                    ,Status = @Status
                    ,Lecturer = @Lecturer
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
