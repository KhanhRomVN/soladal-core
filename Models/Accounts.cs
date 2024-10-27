namespace soladal_core.Data
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public required string Type { get; set; }
        public required int GroupId { get; set; } = -1;
        public string Website_URL { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Password { get; set; } = "";
        public string TwoFactor { get; set; } = "";
        public string Notes { get; set; } = "";
        public bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}