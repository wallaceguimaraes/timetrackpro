using Microsoft.EntityFrameworkCore;

namespace api.Models.EntityModel.Times
{
    public static class UserQuery
    {
        public static IQueryable<Time> WhereProjectId(this IQueryable<Time> times, int projectId)
            => times.Where(time => time.ProjectId == projectId);

        public static IQueryable<Time> IncludeProject(this IQueryable<Time> times)
            => times.Include(time => time.Project);

        public static IQueryable<Time> WhereId(this IQueryable<Time> times, int timeId)
            => times.Where(time => time.Id == timeId);
        public static IQueryable<Time> WhereUserId(this IQueryable<Time> times, int userId)
            => times.Where(time => time.UserId == userId);

    }
}