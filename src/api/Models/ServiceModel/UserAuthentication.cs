// using api.Authorization;
using Microsoft.Extensions.Options;
// using api.Data.Context;
using api.Models.EntityModel.Users;
using Microsoft.EntityFrameworkCore;
using api.Data.Context;
using api.Authorization;
using api.Extensions;
// using api.Extensions;

namespace api.Models.ServiceModel
{
    public class UserAuthentication
    {
        private readonly ApiDbContext _dbContext;
        private readonly AuthOptions _authOptions;

        public UserAuthentication(
            ApiDbContext dbContext,
            IOptions<AuthOptions> authOptionsAccessor
             )
        {
            _dbContext = dbContext;
            _authOptions = authOptionsAccessor.Value;
        }


        public User User { get; private set; }
        public bool HashExpired { get; private set; }
        public bool HashIsInvalid { get; private set; }
        public bool WrongPassword { get; private set; }
        public bool PasswordUsedRecently { get; private set; }


        public async Task<bool> SignIn(string login, string password)
        {
            User = await _dbContext.Users
                .WhereEmail(login)
                .SingleOrDefaultAsync();

            if (User == null) return false;

            return User.Password == password.Encrypt(User.Salt);
        }
    }
}