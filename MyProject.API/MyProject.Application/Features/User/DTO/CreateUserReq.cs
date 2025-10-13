namespace MyProject.Application.Features.User.DTO
{
    public class CreateUserReq
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public Guid RoleId { get; set; }
        public string Avatar { get; set; } = null!;
    }
}
