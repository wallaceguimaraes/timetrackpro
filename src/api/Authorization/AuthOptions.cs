namespace api.Authorization
{
    public class AuthOptions
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Key { get; set; }
        public string? Secret { get; set; }
        public int ExpireTokenIn { get; set; }
        public string? Salt { get; set; }

    }
}