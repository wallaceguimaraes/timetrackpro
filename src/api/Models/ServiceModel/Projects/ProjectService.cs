using api.Data.Context;
using api.Models.EntityModel.Projects;
using api.Models.Interfaces;
using api.Models.ViewModel.Projects;
using Microsoft.EntityFrameworkCore;

namespace api.Models.ServiceModel.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly ApiDbContext? _dbContext;
        private readonly IUserService _userService;


        public ProjectService(ApiDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public Project Project { get; private set; }
        private const string PROJECT_REGISTER_ERROR = "PROJECT_REGISTER_ERROR";
        private const string PROJECT_UPDATE_ERROR = "PROJECT_UPDATE_ERROR";
        private const string PROJECT_NOT_FOUND = "PROJECT_NOT_FOUND";
        private const string PROJECT_ALREADY_EXISTS = "PROJECT_ALREADY_EXISTS";
        private const string USER_NOT_FOUND = "USER_NOT_FOUND";


        public async Task<List<Project>> GetAllProjects(int userId)
            => await _dbContext.Projects.IncludeTimes()
                                        .IncludeUserProject()
                                        .ToListAsync();

        public async Task<(bool success, Project project, string error)> FindProject(int projectId)
        {
            Project = await _dbContext.Projects.WhereId(projectId)
                                               .IncludeTimes()
                                               .IncludeUserProject()
                                               .SingleOrDefaultAsync();

            if (Project is null)
                return (false, null, PROJECT_NOT_FOUND);

            return (true, Project, null);
        }

        public async Task<(Project project, string error)> CreateProject(ProjectModel model)
        {
            if (model is null)
                return (null, PROJECT_REGISTER_ERROR);

            var project = model.Map();
            var projectExists = await _dbContext.Projects.WhereTitle(project.Title.ToLower()).AnyAsync();

            if (projectExists)
                return (null, PROJECT_ALREADY_EXISTS);

            if (!await CheckIdsExisting(model.UserIds))
                return (null, USER_NOT_FOUND);

            try
            {
                _dbContext.Projects.AddRange(project);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (null, PROJECT_REGISTER_ERROR);
            }

            return (project, null);
        }

        public async Task<(Project project, string error)> UpdateProject(ProjectModel model, int projectId)
        {
            if (model is null)
                return (null, PROJECT_UPDATE_ERROR);

            Project = await _dbContext.Projects.WhereId(projectId).SingleOrDefaultAsync();

            if (Project is null)
                return (null, PROJECT_NOT_FOUND);

            if (!await CheckIdsExisting(model.UserIds))
                return (null, USER_NOT_FOUND);

            Project = model.Map(Project);

            try
            {
                _dbContext.Projects.UpdateRange(Project);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (null, PROJECT_UPDATE_ERROR);
            };

            return (Project, null);
        }

        private async Task<bool> CheckIdsExisting(ICollection<int> userIds)
        {
            var response = await _userService.FindUsers(userIds);
            var idsExisting = response.users.Select(user => user.Id).ToList();

            var idsNotExisting = userIds.Except(idsExisting);

            if (idsNotExisting.Any())
                return false;

            return true;
        }
    }
}