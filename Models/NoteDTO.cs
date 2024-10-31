namespace soladal_core.Data
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; } = "";
        public required string Type { get; set; } = "";
        public required int GroupId { get; set; } = -1;
        public string GroupName { get; set; } = "";
        public string Notes { get; set; } = "";
        public bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}