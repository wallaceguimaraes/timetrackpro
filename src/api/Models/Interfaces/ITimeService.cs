using api.Models.EntityModel.Times;
using api.Models.ViewModel.Times;

namespace api.Models.Interfaces
{
    public interface ITimeService
    {
        Task<(Time time, string error)> CreateTime(Time time);
        Task<(List<Time> times, string error)> GetTimesByProject(int projectId, int userId);
        Task<(Time time, string error)> UpdateTime(TimeModel model, int timeId, int userId);
    }
}