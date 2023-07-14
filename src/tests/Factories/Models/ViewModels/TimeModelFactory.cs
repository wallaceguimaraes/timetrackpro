using api.Models.ViewModel.Times;

namespace tests.Factories.Models.ViewModels
{
    public static class TimeModelFactory
    {
        public static TimeModel Build(this TimeModel model,
                                      int? projectId = null,
                                      int? userId = null,
                                      DateTime? startDate = null,
                                      DateTime? endDate = null)
        {
            model.ProjectId = projectId ?? 1;
            model.UserId = userId ?? 1;
            model.StartedAt = startDate ?? DateTime.Now;
            model.EndedAt = endDate ?? DateTime.Now.AddDays(2);

            return model;
        }
    }
}