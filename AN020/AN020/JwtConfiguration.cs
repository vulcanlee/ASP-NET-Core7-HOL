namespace AN020
{
    public class JwtConfiguration
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int ExpireMinutes { get; set; }
        public int RefreshExpireDays { get; set; }
        public string IssuerSigningKey { get; set; }
    }
}
