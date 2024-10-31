namespace soladal_core.Data
{
    public class CardDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public required string Type { get; set; }
        public required int GroupId { get; set; } = -1;
        public string GroupName { get; set; } = "";
        public string FullName { get; set; } = ""; 
        public string CardNumber { get; set; } = ""; 
        public string ExpirationDate { get; set; } = ""; 
        public string Pin { get; set; } = ""; 
        public string Notes { get; set; } = ""; 
        public bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}