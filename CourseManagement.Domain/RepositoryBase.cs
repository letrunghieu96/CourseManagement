using Microsoft.Data.SqlClient;
using System.Data;

namespace CourseManagement.Domain
{
    /// <summary>
    /// Repository base
    /// </summary>
    public class RepositoryBase
    {
        #region +Fields
        /// <summary>Sql connection</summary>
        protected readonly IDbConnection _dbConnection;
        /// <summary>Transaction</summary>
        protected readonly IDbTransaction _dbTransaction;
        #endregion

        #region +Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlConnection">Sql connection</param>
        /// <param name="dbTransaction">Transaction</param>
        public RepositoryBase(SqlConnection sqlConnection, IDbTransaction dbTransaction)
        {
            _dbConnection = sqlConnection;
            _dbTransaction = dbTransaction;
        }
        #endregion
    }
}
