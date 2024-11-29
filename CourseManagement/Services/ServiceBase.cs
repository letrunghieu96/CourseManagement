using CourseManagement.Domain;
using System.Security.Cryptography;
using System.Text;

namespace CourseManagement.Services
{
    /// <summary>
    /// Service base
    /// </summary>
    public class ServiceBase
    {
        /// <summary>Configuration interface</summary>
        private readonly IConfiguration _config;
        protected readonly IDomainFacade _domainFacade;

        public ServiceBase(IConfiguration config, IDomainFacade domainFacade)
        {
            _config = config;
            _domainFacade = domainFacade;
        }

        /// <summary>
        /// Hash password
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>Encrypted password</returns>
        public string HashPassword(string password)
        {
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_config["SecurityKey"] ?? string.Empty)))
            {
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var encryptedPassword = Convert.ToBase64String(hash);

                return encryptedPassword;
            }
        }

        /// <summary>
        /// Is exist email
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="email">Email</param>
		/// <returns>True: if exist, otherwise: False</returns>
        public bool IsExistEmail(int userId, string email)
        {
            var isExist = _domainFacade.Users.IsExistEmail(userId, email);
            return isExist;
        }
    }
}
