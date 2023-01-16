namespace JwtLab.Models
{
    public class JwtConfiguration
    {
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public string IssuerSigningKey { get; set; } = string.Empty;
        public int JwtExpireMinutes { get; set; } = 20;
        public int JwtRefreshExpireDays { get; set; } = 7;
        public int ClockSkew { get; set; } = 5;
    }
}
