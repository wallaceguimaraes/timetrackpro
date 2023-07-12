using api.Models.EntityModel.Users;
using api.Models.Interfaces;

namespace tests.Factories.Models.ServiceModels
{
    public class UserAuthenticationFake : IUserAuthentication
    {
        public bool UserNotExists { get; set; }

        public Task<(bool, User)> SignIn(string login, string password)
        {
            if (UserNotExists)
                return Task.FromResult((false, new User()));

            return Task.FromResult((true, new User
            {
                Id = 1,
                Login = login,
                Password = password,
                Name = "teste",
                CreatedAt = DateTime.Now,
                Salt = "YuyIYiuyhggjhfr",
                Email = "teste@mail.com",
            }));
        }
    }
}