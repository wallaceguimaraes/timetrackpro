using api.Data.Context;
using api.Extensions;
using api.Infrastructure.Security;
using api.Models.EntityModel.Users;
using api.Models.Interfaces;
using api.Models.ViewModel.Users;
using Microsoft.EntityFrameworkCore;

namespace api.Models.ServiceModel.Users
{
    public class UserService : IUserService
    {
        private readonly ApiDbContext? _dbContext;

        public UserService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User User { get; private set; }
        private const string USER_REGISTER_ERROR = "USER_REGISTER_ERROR";
        private const string EMAIL_ALREADY_EXISTS = "EMAIL_ALREADY_EXISTS";
        private const string USER_UPDATE_ERROR = "USER_UPDATE_ERROR";
        private const string USER_NOT_FOUND = "USER_NOT_FOUND";

        public async Task<(User?, string?)> CreateUser(UserModel model)
        {
            User = model.Map();

            var emailExists = await CheckEmailExisting(User.Email);

            if (emailExists)
                return (null, EMAIL_ALREADY_EXISTS);

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
                return (null, USER_REGISTER_ERROR);
            }

            return (User, null);
        }

        public async Task<(bool, User?, string?)> FindUser(int userId)
        {
            User = await _dbContext.Users.WhereId(userId)
                                         .IncludeTimesAndProject()
                                         .SingleOrDefaultAsync();
            if (User is null)
                return (false, null, USER_NOT_FOUND);

            return (true, User, null);
        }

        public async Task<(List<User>? users, string error)> FindUsers(ICollection<int> userIds)
        {
            if (!userIds.Any())
            {
                return (null, USER_NOT_FOUND);
            }

            var users = await _dbContext.Users.WhereIds(userIds)
                                         .ToListAsync();

            return (users, null);
        }

        public async Task<(User?, string)> UpdateUser(UserModel model, int userId)
        {
            User = await _dbContext.Users.WhereId(userId)
                                         .IncludeTimesAndProject()
                                         .SingleOrDefaultAsync();

            if (User is null)
                return (null, USER_NOT_FOUND);

            User = model.Map(User);

            var emailExists = await CheckEmailExisting(model.Email, userId);

            if (emailExists)
                return (null, EMAIL_ALREADY_EXISTS);

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
                return (null, USER_UPDATE_ERROR);
            }

            return (User, null);
        }

        private async Task<bool> CheckEmailExisting(string email, int id = 0)
        {
            bool emailExists = false;

            if (id == 0)
                emailExists = await _dbContext.Users.WhereEmail(email).AnyAsync();

            if (id > 0)
                emailExists = await _dbContext.Users.WhereEmail(email)
                                                    .WhereNotId(id)
                                                    .AnyAsync();

            return emailExists;
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