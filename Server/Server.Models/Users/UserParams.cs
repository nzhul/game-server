namespace Server.Models.Users
{
    public class UserParams : QueryParams
    {
        public int UserId { get; set; }

        public string Gender { get; set; }

        public int MinAge { get; set; } = 18;

        public int MaxAge { get; set; } = 99;

        public bool Likees { get; set; } = false;

        public bool Likers { get; set; } = false;
    }
}