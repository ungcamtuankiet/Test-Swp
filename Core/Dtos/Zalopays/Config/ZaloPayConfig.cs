namespace be_project_swp.Core.Dtos.Zalopays.Config
{
    public class ZaloPayConfig
    {
        public static string ConfigName => "ZaloPay";
        public string AppUser { get; set; } = string.Empty;
        public string PaymentUrl { get; set; } = string.Empty;
        public string RedirecUrl { get; set; } = string.Empty;
        public string IpnUrl { get; set; } = string.Empty;
        public string AppId { get; set; }
        public string Key1 { get; set; } = string.Empty;
        public string Key2 { get; set; } = string.Empty;
    }
}
