using System.Diagnostics.CodeAnalysis;

namespace CourseManagement.Sql.Queries
{
    /// <summary>
    /// Enrollments query
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class EnrollmentsQuery
    {
        public static string Count =>
        @"
            SELECT  COUNT(*)
              FROM  Enrollments
                    INNER JOIN Courses ON
                    (
                      Enrollments.CourseId = Courses.CourseId
                    )
                    INNER JOIN Users ON
                    (
                      Enrollments.UserId = Users.UserId
                    )
             WHERE  (
                      Courses.CourseCode LIKE '%' + @SearchWord + '%'
                      OR
                      Courses.CourseName LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (CONVERT(DATE, EnrollmentTime) >= @DateFrom OR @DateFrom IS NULL)
               AND  (CONVERT(DATE, EnrollmentTime) <= @DateTo OR @DateTo IS NULL)
               AND  (Courses.Lecturer LIKE '%' + @Lecturer + '%' OR @Lecturer = '')
               AND  (Courses.Status = @Status OR @Status IS NULL)
               AND  (Users.UserName LIKE '%' + @UserName + '%' OR @UserName = '')
               AND  (Enrollments.UserId = @UserId OR @UserId IS NULL)
        ";

        public static string Search =>
        @"
            SELECT  Enrollments.*,
                    Courses.CourseId,
                    Courses.CourseCode,
                    Courses.CourseName,
                    Courses.Status,
                    Courses.Price,
                    Courses.Lecturer,
                    Users.UserName
              FROM  Enrollments
                    INNER JOIN Courses ON
                    (
                      Enrollments.CourseId = Courses.CourseId
                    )
                    INNER JOIN Users ON
                    (
                      Enrollments.UserId = Users.UserId
                    )
             WHERE  (
                      Courses.CourseCode LIKE '%' + @SearchWord + '%'
                      OR
                      Courses.CourseName LIKE '%' + @SearchWord + '%'
                      OR
                      @SearchWord = ''
                    )
               AND  (CONVERT(DATE, EnrollmentTime) >= @DateFrom OR @DateFrom IS NULL)
               AND  (CONVERT(DATE, EnrollmentTime) <= @DateTo OR @DateTo IS NULL)
               AND  (Courses.Lecturer LIKE '%' + @Lecturer + '%' OR @Lecturer = '')
               AND  (Courses.Status = @Status OR @Status IS NULL)
               AND  (Users.UserName LIKE '%' + @UserName + '%' OR @UserName = '')
               AND  (Enrollments.UserId = @UserId OR @UserId IS NULL)
          ORDER BY
                    CASE @OrderBy
                      WHEN  '1' THEN Enrollments.EnrollmentTime
                      ELSE  NULL
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '2' THEN Enrollments.EnrollmentTime
                      ELSE  NULL
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '7' THEN Courses.CourseCode
                      WHEN  '7' THEN Courses.CourseName
                      WHEN  '7' THEN Courses.Lecturer
                      WHEN  '7' THEN Users.UserName
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '8' THEN Courses.CourseCode
                      WHEN  '8' THEN Courses.CourseName
                      WHEN  '8' THEN Courses.Lecturer
                      WHEN  '8' THEN Users.UserName
                      ELSE  '0'
                    END
                      DESC,
                    CASE @OrderBy
                      WHEN  '7' THEN Courses.Status
                      WHEN  '7' THEN Courses.Price
                      ELSE  '0'
                    END
                      ASC,
                    CASE @OrderBy
                      WHEN  '8' THEN Courses.Status
                      WHEN  '8' THEN Courses.Price
                      ELSE  '0'
                    END
                      DESC,
                    Enrollments.EnrollmentTime DESC
            OFFSET  @BeginRowNum ROWS
            FETCH NEXT  @RowsOfPage ROWS ONLY

        ";

        public static string CheckExistEnrollment =>
        @"
            SELECT  CASE WHEN EXISTS (SELECT 1 FROM Enrollments WHERE CourseId = @CourseId AND UserId = @UserId)
                        THEN 1
                        ELSE 0
                    END AS Result
        ";

        public static string Insert =>
        @"
            INSERT INTO Enrollments
                (
                    CourseId
                    ,UserId
                    ,EnrollmentTime
                )
            VALUES
                (
                    @CourseId
                    ,@UserId
                    ,GETDATE()
                )
        ";

        public static string Delete =>
        @"
            DELETE  Enrollments
             WHERE  CourseId = @CourseId
               AND  UserId = UserId
        ";
    }
}
