namespace be_project_swp.Core.Dtos.Mail
{
    public class EmailServiceConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
