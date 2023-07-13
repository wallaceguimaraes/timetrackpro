using api.Models.EntityModel.Projects;
using api.Models.ViewModel.Projects;

namespace api.Models.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllProjects(int userId);
        Task<(bool success, Project project, string error)> FindProject(int projectId);
        Task<(Project project, string error)> CreateProject(ProjectModel model);
        Task<(Project project, string error)> UpdateProject(ProjectModel model, int projectId);
    }
}