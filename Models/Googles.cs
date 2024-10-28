namespace soladal_core.Data
{
    public class Google
    {
        // Primary keys and relationships
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Type { get; set; }
        public required int GroupId { get; set; } = -1;

        // Account credentials
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string RecoveryEmail { get; set; } = "";
        public string TwoFactor { get; set; } = "";

        // Personal information
        public string Phone { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public DateTime DateOfBirth { get; set; }

        // Regional settings
        public string Country { get; set; } = "";
        public string Language { get; set; } = "";

        // Technical settings
        public string Agent { get; set; } = "";
        public string Proxy { get; set; } = "";

        // Account status and metadata
        public string Status { get; set; } = "";
        public string Notes { get; set; } = "";
        public bool IsFavorite { get; set; } = false;

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}