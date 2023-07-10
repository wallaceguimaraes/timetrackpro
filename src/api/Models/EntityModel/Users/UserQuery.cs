using Microsoft.EntityFrameworkCore;

namespace api.Models.EntityModel.Users
{
    public static class UserQuery
    {
        public static IQueryable<User> WhereId(this IQueryable<User> users, int userId)
            => users.Where(user => user.Id == userId);

        public static IQueryable<User> IncludeTimesAndProject(this IQueryable<User> users)
            => users.Include(user => user.Times).ThenInclude(time => time.Project);

        public static IQueryable<User> WhereIds(this IQueryable<User> users, ICollection<int> userIds)
           => users.Where(user => userIds.Contains(user.Id));

        public static IQueryable<User> WhereLogin(this IQueryable<User> users, string login)
           => users.Where(user => user.Login == login);

    }
}