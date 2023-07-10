namespace api.Infrastructure.Security
{
    public class Uuid
    {
        private byte[] time;
        private byte[] key;

        public Uuid()
        {
            time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            key = Guid.NewGuid().ToByteArray();
        }

        public override string ToString()
        {
            return BitConverter.ToString(key.Concat(time).ToArray()).Replace("-", string.Empty);
        }
    }
}