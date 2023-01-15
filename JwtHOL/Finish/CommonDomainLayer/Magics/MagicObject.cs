using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonDomainLayer.Magics
{
    public class MagicObject
    {
        public const string SectionNameOfCustomNLogConfiguration = "CustomNLog";
        public const string CookieAuthenticationScheme = "BackendCookieAuthenticationScheme"; // CookieAuthenticationDefaults.AuthenticationScheme
        public const string JwtBearerAuthenticationScheme = "BackendJwtBearerAuthenticationScheme"; // JwtBearerDefaults.AuthenticationScheme
        public const string SectionNameOfJwtConfiguration = "JWT";
        public static readonly string ValidIssuer = "BackendTokens:ValidIssuer";
        public static readonly string ValidAudience = "BackendTokens:ValidAudience";
    }
}
