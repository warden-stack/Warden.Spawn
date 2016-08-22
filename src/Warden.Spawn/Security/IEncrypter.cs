namespace Warden.Spawn.Security
{
    public interface IEncrypter
    {
        string GetSalt();
        string Decrypt(string value, string salt);
        string Encrypt(string value, string salt);
        string Hash(params string[] values);
    }
}