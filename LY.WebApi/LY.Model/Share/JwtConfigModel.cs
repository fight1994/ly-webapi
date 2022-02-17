namespace LY.Model.Share
{
    public class JwtConfigModel
    {
        public string Issuer { get; set; }

        public string Key { get; set; }

        public double Expires { get; set; }
    }
}
