namespace api.Models.EntityModel.Projects
{
    public static class ProjectQuery
    {
        public static IQueryable<Project> WhereId(this IQueryable<Project> projects, int projectId)
           => projects.Where(project => project.Id == projectId);

        public static IQueryable<Project> WhereTitle(this IQueryable<Project> projects, string title)
          => projects.Where(project => project.Title.ToLower() == title);
    }
}