using api.Data.Context;
using api.Models.EntityModel.Times;
using api.Models.Interfaces;
using api.Models.ViewModel.Times;
using Microsoft.EntityFrameworkCore;

namespace api.Models.ServiceModel.Times
{
    public class TimeService : ITimeService
    {
        private readonly ApiDbContext? _dbContext;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;


        public TimeService(ApiDbContext dbContext, IProjectService projectService, IUserService userService)
        {
            _dbContext = dbContext;
            _projectService = projectService;
            _userService = userService;
        }

        public Time Time { get; private set; }
        public List<Time> Times { get; private set; }
        private const string TIME_RECORDING_ERROR = "TIME_RECORDING_ERROR";
        private const string TIME_NOT_REGISTERED = "TIME_NOT_REGISTERED";
        private const string TIME_NOT_FOUND = "TIME_NOT_FOUND";
        private const string PROJECT_NOT_FOUND = "PROJECT_NOT_FOUND";
        private const string USER_NOT_FOUND = "USER_NOT_FOUND";
        private const string TIME_UPDATE_ERROR = "TIME_UPDATE_ERROR";

        public async Task<(Time time, string error)> CreateTime(Time time)
        {
            var response = await CheckIdsExisting(time.ProjectId, time.UserId);

            if (!response.success)
                return (null, response.error);

            try
            {
                _dbContext.Times.Add(time);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (null, TIME_RECORDING_ERROR);
            }

            return (time, null);
        }

        public async Task<(List<Time> times, string error)> GetTimesByProject(int projectId, int userId)
        {
            Times = await _dbContext.Times.WhereProjectId(projectId)
                                          .IncludeProject()
                                          .WhereUserId(userId)
                                          .ToListAsync();
            if (!Times.Any())
                return (null, TIME_NOT_REGISTERED);

            return (Times, null);
        }

        public async Task<(Time time, string error)> UpdateTime(TimeModel model, int timeId, int userId)
        {
            Time = await _dbContext.Times.WhereId(timeId)
                                         .WhereUserId(userId)
                                         .SingleOrDefaultAsync();
            if (Time is null)
                return (null, TIME_NOT_FOUND);

            var response = await CheckIdsExisting(model.ProjectId, model.UserId);

            if (!response.success)
                return (null, response.error);

            Time = model.Map(Time);

            try
            {
                _dbContext.Times.Update(Time);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return (null, TIME_UPDATE_ERROR);
            }

            return (Time, null);
        }

        private async Task<(bool success, string error)> CheckIdsExisting(int projectId, int userId)
        {
            var response = await _projectService.FindProject(projectId);

            if (!response.success)
                return (false, PROJECT_NOT_FOUND);

            var (userExists, user, error) = await _userService.FindUser(userId);

            if (!userExists)
                return (false, USER_NOT_FOUND);

            return (true, null);
        }
    }
}