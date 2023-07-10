using api.Data.Context;
using api.Models.EntityModel.Times;
using api.Models.ServiceModel.Projects;
using api.Models.ServiceModel.Users;
using api.Models.ViewModel.Times;
using Microsoft.EntityFrameworkCore;

namespace api.Models.ServiceModel.Times
{
    public class TimeService
    {
        private readonly ApiDbContext? _dbContext;
        private readonly ProjectService _projectService;
        private readonly UserService _userService;


        public TimeService(ApiDbContext dbContext, ProjectService projectService, UserService userService)
        {
            _dbContext = dbContext;
            _projectService = projectService;
            _userService = userService;
        }

        public Time Time { get; private set; }
        public List<Time> Times { get; private set; }
        public bool TimeRegisterError { get; private set; }
        public bool TimeNotRegistered { get; private set; }
        public bool TimeNotFound { get; private set; }
        public bool ProjectNotFound { get; private set; }
        public bool UserNotFound { get; private set; }
        public bool TimeUpdateError { get; private set; }

        public async Task<bool> CreateTime(Time time)
        {
            if (time == null)
                return !(TimeRegisterError = true);

            if (!await CheckIdsExisting(time.ProjectId, time.UserId))
                return false;

            try
            {
                _dbContext.Times.Add(time);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return !(TimeRegisterError = true);
            }

            Time = time;

            return true;
        }

        public async Task<bool> GetTimesByProject(int projectId)
        {
            Times = await _dbContext.Times.WhereProjectId(projectId)
                                          .IncludeProject()
                                          .ToListAsync();
            if (!Times.Any())
                return !(TimeNotRegistered = true);

            return true;
        }

        public async Task<bool> UpdateTime(TimeModel model, int timeId)
        {
            Time = await _dbContext.Times.WhereId(timeId)
                                         .SingleOrDefaultAsync();
            if (Time is null)
                return !(TimeNotFound = true);

            if (!await CheckIdsExisting(model.ProjectId, model.UserId))
                return false;

            Time = model.Map(Time);

            try
            {
                _dbContext.Times.Update(Time);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return !(TimeUpdateError = true);
            }

            return true;
        }

        private async Task<bool> CheckIdsExisting(int projectId, int userId)
        {
            var projectExists = await _projectService.FindProject(projectId);

            if (!projectExists)
                return !(ProjectNotFound = true);

            var userExists = await _userService.FindUser(userId);

            if (!userExists)
                return !(UserNotFound = true);

            return true;
        }

    }
}