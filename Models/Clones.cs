namespace soladal_core.Data
{
    public class Clone
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Type { get; set; }
        public required int GroupId { get; set; } = -1;
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string TwoFactor { get; set; } = "";
        public string Agent { get; set; } = "";
        public string Proxy { get; set; } = "";
        public string Country { get; set; } = "";
        public string Status { get; set; } = "";
        public bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}