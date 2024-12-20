using CourseManagement.Domain;
using System.Globalization;
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
        protected readonly IConfiguration _config;
        protected readonly IDomainFacade _domainFacade;

        public ServiceBase(IConfiguration config, IDomainFacade domainFacade)
        {
            _config = config;
            _domainFacade = domainFacade;
        }

        public string HashPassword(string password)
        {
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_config["SecurityKey"] ?? string.Empty)))
            {
                var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var encryptedPassword = Convert.ToBase64String(hash);

                return encryptedPassword;
            }
        }

        public static DateTime? ConvertDate(string? dateString)
        {
            if (DateTime.TryParseExact(dateString, WebConstants.DATE_FORMAT_VN, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result)) return result;

            return null;
        }

        public bool IsExistEmail(string email, int? userId = null)
        {
            var isExist = _domainFacade.Users.IsExistEmail(email, userId);
            return isExist;
        }
    }
}
