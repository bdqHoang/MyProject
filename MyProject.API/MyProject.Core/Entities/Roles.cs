namespace MyProject.Core.Entities
{
    public class Roles
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }

        // Navigation property
        public ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
