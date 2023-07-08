using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Users;

namespace api.Models.EntityModel.Times
{
    public class Time
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public Project? Project { get; set; }
        public User? User { get; set; }
    }
}