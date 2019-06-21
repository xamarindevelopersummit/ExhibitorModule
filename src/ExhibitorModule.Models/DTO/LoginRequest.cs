namespace ExhibitorModule.Models
{
    public class LoginRequest
    {
        public string User { get; set; }

        public string Password { get; set; }

        public string InstallId { get; set; }

        public string Os { get; set; }
    }
}
