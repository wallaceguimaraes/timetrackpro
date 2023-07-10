using api.Data.Context;
using api.Extensions;
using api.Infrastructure.Security;
using api.Models.EntityModel.Users;
using api.Models.ViewModel.Users;
using Microsoft.EntityFrameworkCore;

namespace api.Models.ServiceModel.Users
{
    public class UserService
    {
        private readonly ApiDbContext? _dbContext;

        public UserService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User User { get; private set; }
        public bool UserRegisterError { get; private set; }
        public bool UserUpdateError { get; private set; }
        public bool UserNotFound { get; private set; }

        public async Task<bool> CreateUser(UserModel model)
        {
            if (model == null)
                return !(UserRegisterError = true);

            User = model.Map();

            var salt = new Salt().ToString();
            User.Salt = salt;
            User.Password = User.Password.Encrypt(salt);

            try
            {
                _dbContext.Users.Add(User);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return !(UserRegisterError = true);
            }

            return true;
        }

        public async Task<bool> FindUser(int userId)
        {
            User = await _dbContext.Users.WhereId(userId)
                                         .IncludeTimesAndProject()
                                         .SingleOrDefaultAsync();
            if (User is null)
                return !(UserNotFound = true);

            return true;
        }

        public async Task<List<User>?> FindUsers(ICollection<int> userIds)
        {
            if (!userIds.Any())
            {
                UserNotFound = true;
                return null;
            }

            return await _dbContext.Users.WhereIds(userIds)
                                         .ToListAsync();
        }

        public async Task<bool> UpdateUser(UserModel model, int userId)
        {
            if (model == null)
                return !(UserUpdateError = true);

            User = await _dbContext.Users.WhereId(userId)
                                         .IncludeTimesAndProject()
                                         .SingleOrDefaultAsync();

            if (User is null)
                return !(UserNotFound);

            User = model.Map(User);

            var salt = new Salt().ToString();

            User.Salt = salt;
            User.Password = User.Password.Encrypt(salt);

            try
            {
                _dbContext.Users.Update(User);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return !(UserUpdateError = true);
            }

            return true;
        }

        public async Task<User?> FindUserAuthenticated(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                return null;

            int id;

            if (!int.TryParse(userId, out id))
                return null;

            var user = await _dbContext.Users.WhereId(id)
                                             .SingleOrDefaultAsync();
            return user;
        }

    }
}