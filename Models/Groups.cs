namespace soladal_core.Data
{
    public class Group
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; }
        public string LucideIcon { get; set; } = "archive";
        public required bool CanDelete { get; set; } = true;
        public bool IsFavorite { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

    public class GroupCreate
    {
        public int UserId { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = "";
    }
}