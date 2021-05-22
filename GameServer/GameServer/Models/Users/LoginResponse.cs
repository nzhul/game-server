namespace GameServer.Models.Users
{
    public class LoginResponse
    {
        public string TokenString { get; set; }

        public User User { get; set; }
    }
}
