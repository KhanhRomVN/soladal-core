namespace soladal_core.Data
{
    public class Cards
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Type { get; set; }
        public required int GroupId { get; set; } = -1;
        public required string FullName { get; set; } = ""; 
        public required string CardNumber { get; set; } = ""; 
        public required string ExpirationDate { get; set; } = ""; 
        public required string Pin { get; set; } = ""; 
        public required string Notes { get; set; } = ""; 
        public required bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}