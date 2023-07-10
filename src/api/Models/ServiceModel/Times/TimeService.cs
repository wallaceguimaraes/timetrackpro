using api.Data.Context;
using api.Models.EntityModel.Times;

namespace api.Models.ServiceModel.Times
{
    public class TimeService
    {
        private readonly ApiDbContext? _dbContext;

        public TimeService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Time Time { get; private set; }
        public bool TimeRegisterError { get; private set; }


        public async Task<bool> CreateTime(Time time)
        {
            if (time == null)
                return !(TimeRegisterError = true);

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
    }
}