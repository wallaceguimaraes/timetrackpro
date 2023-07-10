using api.Data.Context;
using api.Models.EntityModel.Projects;
using api.Models.EntityModel.UserProjects;
using api.Models.ViewModel.Projects;
using Microsoft.EntityFrameworkCore;

namespace api.Models.ServiceModel.Projects
{
    public class ProjectService
    {
        private readonly ApiDbContext? _dbContext;

        public ProjectService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Project Project { get; private set; }
        public bool ProjectRegisterError { get; private set; }
        public bool ProjectUpdateError { get; private set; }
        public bool ProjectNotFound { get; private set; }
        public bool ProjectExisting { get; private set; }


        public async Task<List<Project>> GetAllProjects()
            => await _dbContext.Projects.ToListAsync();

        public async Task<bool> FindProject(string projectId)
        {
            Project = await _dbContext.Projects.WhereId(Convert.ToInt32(projectId))
                                            .SingleOrDefaultAsync();

            if (Project is null)
                return !(ProjectNotFound = true);

            return true;
        }

        public async Task<bool> CreateProject(ProjectModel model)
        {
            if (model is null)
                return !(ProjectRegisterError = true);

            var project = model.Map();
            var hasProject = await _dbContext.Projects.WhereTitle(project.Title.ToLower()).AnyAsync();

            if (hasProject)
                return !(ProjectExisting = true);

            try
            {
                _dbContext.Projects.AddRange(project);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return !(ProjectRegisterError = true);
            }

            Project = project;

            return true;
        }

        public async Task<bool> UpdateProject(ProjectModel model, int projectId)
        {
            if (model is null)
                return !(ProjectUpdateError = true);

            Project = await _dbContext.Projects.WhereId(projectId).SingleOrDefaultAsync();

            if (Project is null)
                return !(ProjectNotFound);

            Project.Title = model.Title;
            Project.Description = model.Description;

            CompleteUpdatedProject(model);

            try
            {
                _dbContext.Projects.UpdateRange(Project);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return !(ProjectUpdateError = true);
            };

            return true;
        }

        private void CompleteUpdatedProject(ProjectModel model)
        {
            Project.Title = model.Title;
            Project.Description = model.Description;

            var userProjects = new List<UserProject>();

            var ids = model.UserIds.Select(x => x).ToList();

            ids.ForEach(userId =>
            {
                var userProject = new UserProject();
                userProject.Project = Project;
                userProject.UserId = userId;
                userProjects.Add(userProject);
            });

            Project.UserProjects = userProjects;
        }

    }
}