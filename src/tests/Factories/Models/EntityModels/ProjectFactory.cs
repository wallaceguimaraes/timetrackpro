using api.Models.EntityModel.Projects;

namespace tests.Factories.Models.EntityModels
{
    public static class ProjectFactory
    {
        public static Project Build(this Project user,
                                string? title = null,
                                string? description = null
                               )
        {
            user.Title = title ?? "teste";
            user.Description = description ?? "projeto de teste";

            return user;
        }
    }
}