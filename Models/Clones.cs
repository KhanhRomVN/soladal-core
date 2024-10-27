namespace soladal_core.Data
{
    public class Clones
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Email { get; set; } = ""; 
        public required string Password { get; set; } = ""; 
        public required string TwoFactor { get; set; } = ""; 
        public string Agent { get; set; } = ""; 
        public string Proxy { get; set; } = ""; 
        public string Country { get; set; } = ""; 
        public string Status { get; set; } = ""; 
        public required bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}