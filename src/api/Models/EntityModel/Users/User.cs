using api.Models.EntityModel.Times;

namespace api.Models.EntityModel.Users
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Salt { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdateAt { get; set; }
        public ICollection<Time>? Times { get; set; }
    }
}