using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Users;
using api.Models.EntityModel.UserProjects;

namespace tests.Factories.Models.EntityModels
{
    public static class ProjectFactory
    {
        public static Project Build(this Project project,
                                string? title = null,
                                string? description = null
                               )
        {
            project.Title = title ?? "teste";
            project.Description = description ?? "projeto de teste";

            return project;
        }

        public static Project WithUser(this Project project, User user)
        {
            project.UserProjects = new List<UserProject>
            {
                new UserProject{
                  Project = project,
                  User = user
                },
            };

            return project;
        }

    }
}