using api.Models.ViewModel.Users;

namespace tests.Factories.Models.ViewModels
{
    public static class UserModelFactory
    {
        public static UserModel Build(this UserModel model, string? email = null)
        {
            model.Name = "novo usuario";
            model.Login = "novousuario";
            model.Email = email ?? "novo@mail.com";
            model.Password = "12345";

            return model;
        }
    }
}