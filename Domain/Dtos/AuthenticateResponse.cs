namespace Domain.Dtos
{
    public class AuthenticateResponse
    {
        public long UserId { get; set; }
        public long ClubId { get; set; }
        public string ClubName { get; set; } = string.Empty;
        public string ClubLogo { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
    }
}
