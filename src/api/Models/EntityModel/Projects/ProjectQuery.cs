using Microsoft.EntityFrameworkCore;

namespace api.Models.EntityModel.Projects
{
    public static class ProjectQuery
    {
        public static IQueryable<Project> WhereId(this IQueryable<Project> projects, int projectId)
           => projects.Where(project => project.Id == projectId);

        public static IQueryable<Project> IncludeTimes(this IQueryable<Project> projects)
           => projects.Include(project => project.Times);
        public static IQueryable<Project> IncludeUserProject(this IQueryable<Project> projects)
          => projects.Include(project => project.UserProjects).ThenInclude(userProject => userProject.User);
        public static IQueryable<Project> WhereUserId(this IQueryable<Project> projects, int userId)
          => projects.Where(project => project.UserProjects.Any(userProject => userProject.UserId == userId));
        public static IQueryable<Project> WhereTitle(this IQueryable<Project> projects, string title)
          => projects.Where(project => project.Title.ToLower() == title.ToLower());

        public static IQueryable<Project> WhereNotId(this IQueryable<Project> projects, int id)
          => projects.Where(project => project.Id != id);
    }
}