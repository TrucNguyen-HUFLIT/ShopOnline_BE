namespace ShopOnline.Appsetting
{
    public class BearerToken
    {
        public string SecretKey { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpireTime { get; set; } //hour
    }
}
