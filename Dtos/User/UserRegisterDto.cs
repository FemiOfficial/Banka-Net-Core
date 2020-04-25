namespace banka_net_core.Dtos.User
{
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Type { get; set; }
        public string Lastname { get; set; }
        public float AccountOpeningBalance { get; set; }

    }
}