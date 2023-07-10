using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Users;

namespace api.Models.EntityModel.Times
{
    public class Time
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public Project? Project { get; set; }
        public User? User { get; set; }
    }
}