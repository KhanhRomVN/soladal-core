namespace soladal_core.Data
{
    public class Group
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; }
        public required string Type { get; set; }
        public string LucideIcon { get; set; } = "<Archive className=\"h-4 w-4 text-icon-primary\" />";
        public required bool CanDelete { get; set; }
        public bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}