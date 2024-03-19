namespace be_project_swp.Core.Dtos.Zalopays.Response
{
    public class CreateZalopayResponse
    {
        public int returnCode { get; set; }
        public string returnMessage { get; set; } = string.Empty;
        public string orderUrl { get; set; } = string.Empty;
    }
}
