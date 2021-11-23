using System.Collections.Generic;
using Products.Services.Interfaces;

namespace Products.API.Services
{
    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }

    public class UserService : IUserService
    {
        // TODO: Replace temporary test data
        private readonly IDictionary<string, string> _users = 
            new Dictionary<string, string>
            {
                { "test1", "password1" },
                { "test2", "password2" },
                { "admin", "securePassword" }
            };

        // TODO: Inject your database here for user validation
        public UserService()
        {
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return _users.TryGetValue(userName, out var p) && p == password;
        }

        public bool IsAnExistingUser(string userName)
        {
            return _users.ContainsKey(userName);
        }

        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userName == "admin")
            {
                return UserRoles.Admin;
            }

            return UserRoles.BasicUser;
        }
    }
}
