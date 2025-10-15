namespace MyProject.Core.Entities
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; }= null!;
        public string Avatar { get; set; }= null!;
        public bool IsValidEmail { get; set; }
        public Guid RoleId { get; set; }
        public string? RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int RetryPassworkCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Status { get; set; }

        // Navigation property
        public Roles Role { get; set; } = null!;
    }
}
