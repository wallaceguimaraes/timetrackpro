namespace api.Infrastructure.Security
{
    public class Salt
    {
        private string _salt;

        public Salt() => _salt = Guid.NewGuid().ToString("N");

        public override string ToString() => _salt;
    }
}