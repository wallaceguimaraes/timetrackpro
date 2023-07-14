using api.Models.EntityModel.Projects;
using api.Models.EntityModel.UserProjects;
using api.Validations;
using Newtonsoft.Json;

namespace api.Models.ViewModel.Projects
{
    public class ProjectModel
    {
        [JsonProperty("title"), JsonRequiredValidate, JsonMaxLength(45)]
        public string? Title { get; set; }

        [JsonProperty("user_id"), JsonRequiredValidate]
        public ICollection<int>? UserIds { get; set; }

        [JsonProperty("description"), JsonRequiredValidate, JsonMaxLength(30)]
        public string? Description { get; set; }


        public Project Map()
        {
            var project = new Project
            {
                Title = Title,
                Description = Description,
            };

            var userProjects = new List<UserProject>();

            foreach (var userId in UserIds)
            {
                var userProject = new UserProject();
                userProject.Project = project;
                userProject.UserId = userId;

                userProjects.Add(userProject);
            }

            project.UserProjects = userProjects;

            return project;
        }

        public Project Map(Project project)
        {
            project.Title = Title;
            project.Description = Description;

            var userProjects = new List<UserProject>();

            var unregisteredUsers = UserIds.Except(project.UserProjects.Select(x => x.UserId)).ToList();

            unregisteredUsers.ForEach(userId =>
            {
                var userProject = new UserProject();
                userProject.Project = project;
                userProject.UserId = userId;
                userProjects.Add(userProject);
            });

            project.UserProjects = userProjects;

            return project;
        }
    }
}