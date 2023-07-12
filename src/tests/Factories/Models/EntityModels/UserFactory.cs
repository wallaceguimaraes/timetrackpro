using api.Models.EntityModel.Users;

namespace tests.Factories.Models.EntityModels
{
    public static class UserFactory
    {
        public static User Build(this User user,
                                 string? login = null,
                                 string? password = null,
                                 string? name = null,
                                 string? email = null)
        {
            user.Login = login ?? "teste";
            user.Password = password ?? "rBG1oDjTq9qBhW4EI7ouNdkBxI9C/IdF/FlU1+hn5yg=";
            user.Name = name ?? "teste";
            user.Email = email ?? "teste@mail.com";
            user.Salt = "d2de614740c24985b7194ba7f095e5a9";
            user.CreatedAt = DateTime.Now;

            return user;
        }
    }
}