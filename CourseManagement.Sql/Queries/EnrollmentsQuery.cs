using System.Diagnostics.CodeAnalysis;

namespace CourseManagement.Sql.Queries
{
    /// <summary>
    /// Enrollments query
    /// </summary>
    [ExcludeFromCodeCoverage]
    public  static class EnrollmentsQuery
    {
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
