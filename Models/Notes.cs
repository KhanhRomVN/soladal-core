namespace soladal_core.Data
{
    public class Note
    {
        public required int Id { get; set; }
        public required int UserId { get; set; }
        public required string Title { get; set; }
        public required string Type { get; set; }
        public required int GroupId { get; set; } = -1;
        public string Notes { get; set; } = "";
        public required bool IsFavorite { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}