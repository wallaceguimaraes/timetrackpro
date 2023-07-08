using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Users;

namespace api.Models.EntityModel.UserProjects
{
    public class UserProject
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public Project? Project { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
    }
}