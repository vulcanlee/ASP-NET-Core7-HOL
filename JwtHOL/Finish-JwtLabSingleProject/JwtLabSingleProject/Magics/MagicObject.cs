using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtLabSingleProject.Magics
{
    public class MagicObject
    {
        public const string OnAuthenticationFailedExceptionJson = "ExceptionJson";
        public const string SectionNameOfCustomNLogConfiguration = "CustomNLog";
        public const string CookieAuthenticationScheme = "BackendCookieAuthenticationScheme"; // CookieAuthenticationDefaults.AuthenticationScheme
        public const string JwtBearerAuthenticationScheme = "BackendJwtBearerAuthenticationScheme"; // JwtBearerDefaults.AuthenticationScheme
        public const string SectionNameOfJwtConfiguration = "JWT";
        public const string ClaimTypeRoleNameSymbol = "role";
        public const string RoleRefreshToken = "RefreshToken";
        public const string RoleUser = "User";
        public const string RoleAdmin = "Admin";
    }
}
