using System.Diagnostics.CodeAnalysis;

namespace CourseManagement.Sql.Queries
{
    /// <summary>
    /// Courses query
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CoursesQuery
    {
        public static string Count =>
        @"
            SELECT  COUNT(*)
              FROM  Courses
             WHERE  CourseId = @CourseId
               AND  DeletedFlag = 0
        ";

        public static string Search =>
        @"
            SELECT  *
              FROM  Courses
             WHERE  CourseId = @CourseId
               AND  DeletedFlag = 0
        ";

        public static string Get =>
        @"
            SELECT  *
              FROM  Courses
             WHERE  CourseId = @CourseId
               AND  DeletedFlag = 0
        ";

        public static string CheckExistCourseCode =>
        @"
            SELECT  CASE WHEN EXISTS (SELECT 1 FROM Courses WHERE CourseCode = @CourseCode AND CourseId <> @CourseId AND DeletedFlag = 0)
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
                    ,DeletedFlag
                )
            VALUES
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
                    ,GETDATE()
                    ,LastChanged
                    ,0
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
            UPDATE  Courses
               SET  DeletedFlag = 1
                    ,DeletedAt = GETDATE()
                    ,LastChanged = @LastChanged
             WHERE  CourseId = @CourseId
        ";
    }
}
