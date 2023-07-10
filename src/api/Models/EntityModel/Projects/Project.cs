using api.Models.EntityModel.Times;
using api.Models.EntityModel.UserProjects;

namespace api.Models.EntityModel.Projects
{
    public class Project
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public ICollection<Time>? Times { get; set; }
        public ICollection<UserProject>? UserProjects { get; set; }
    }
}