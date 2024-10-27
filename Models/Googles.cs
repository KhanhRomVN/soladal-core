namespace soladal_core.Data
{
    public class Googles
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Email { get; set; } = ""; 
        public string Phone { get; set; } = ""; 
        public required string Password { get; set; } = ""; 
        public required string TwoFactor { get; set; } = ""; 
        public required bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}