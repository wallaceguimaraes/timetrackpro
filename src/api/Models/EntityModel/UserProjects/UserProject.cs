using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Users;

namespace api.Models.EntityModel.UserProjects
{
    public class UserProject
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}