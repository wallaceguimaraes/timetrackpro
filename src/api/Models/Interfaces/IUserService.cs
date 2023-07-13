using api.Models.EntityModel.Users;
using api.Models.ViewModel.Users;

namespace api.Models.Interfaces
{
    public interface IUserService
    {
        Task<(User? user, string? error)> CreateUser(UserModel model);
        Task<(bool success, User? user, string? error)> FindUser(int userId);
        Task<(User? user, string error)> UpdateUser(UserModel model, int userId);
        Task<(List<User>? users, string error)> FindUsers(ICollection<int> userIds);
        Task<User?> FindUserAuthenticated(string userId);
    }
}