using api.Data.Context;
using api.Models.EntityModel.Projects;
using api.Models.ServiceModel.Users;
using api.Models.ViewModel.Projects;
using Microsoft.EntityFrameworkCore;

namespace api.Models.ServiceModel.Projects
{
    public class ProjectService
    {
        private readonly ApiDbContext? _dbContext;
        private readonly UserService _userService;


        public ProjectService(ApiDbContext dbContext, UserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public Project Project { get; private set; }
        public bool ProjectRegisterError { get; private set; }
        public bool ProjectUpdateError { get; private set; }
        public bool ProjectNotFound { get; private set; }
        public bool ProjectExisting { get; private set; }
        public bool UserNotFound { get; private set; }


        public async Task<List<Project>> GetAllProjects()
            => await _dbContext.Projects.IncludeTimes()
                                        .IncludeUserProject()
                                        .ToListAsync();

        public async Task<bool> FindProject(int projectId)
        {
            Project = await _dbContext.Projects.WhereId(projectId)
                                               .IncludeTimes()
                                               .IncludeUserProject()
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

            if (!await CheckIdsExisting(model.UserIds))
                return false;

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
                return !(ProjectNotFound = true);

            if (!await CheckIdsExisting(model.UserIds))
                return !(UserNotFound = true);

            Project = model.Map(Project);

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

        private async Task<bool> CheckIdsExisting(ICollection<int> userIds)
        {
            var userExisting = await _userService.FindUsers(userIds);
            var idsExisting = userExisting.Select(user => user.Id).ToList();

            var idsNotExisting = userIds.Except(idsExisting);

            if (idsNotExisting.Any())
                return !(UserNotFound = true);

            return true;
        }
    }
}