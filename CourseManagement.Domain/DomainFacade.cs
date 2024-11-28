﻿using CourseManagement.Domain.Courses;
using CourseManagement.Domain.Users;
using System.Data;

namespace CourseManagement.Domain
{
    /// <summary>
    /// Domain facade
    /// </summary>
    public class DomainFacade : IDomainFacade
    {
        #region +Fields
        IDbTransaction Transaction;
        public IUsersRepository Users { get; }
        public ICoursesRepository Courses { get; }
        #endregion

        public DomainFacade(
            IDbTransaction transaction,
            IUsersRepository users,
            ICoursesRepository courses)
        {
            Transaction = transaction;

            // Set repositories
            Users = users;
            Courses = courses;
        }

        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
            }
        }

        public void Rollback() => Transaction.Rollback();

        public void Dispose()
        {
            // Close the SQL connection and dispose the objects
            Transaction.Connection?.Close();
            Transaction.Connection?.Dispose();
            Transaction.Dispose();
        }
    }
}