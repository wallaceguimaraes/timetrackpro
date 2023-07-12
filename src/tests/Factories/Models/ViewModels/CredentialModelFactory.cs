using api.Models.ViewModel;

namespace tests.Factories.Models.ViewModels
{
    public static class CredentialModelFactory
    {
        public static CredentialModel Build(this CredentialModel model,
                                                   string? login = null,
                                                   string? password = null)
        {
            model.Login = login ?? "teste";
            model.Password = password ?? "12345678";

            return model;
        }
    }
}