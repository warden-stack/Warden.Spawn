using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Warden.Spawn.Security;

namespace Warden.Spawn.Extensions.Security
{
    public class Encrypter : IEncrypter
    {
        private readonly string _key;
        private readonly int _keySize;
        private const int MinSaltSize = 30;
        private const int MaxSaltSize = 60;
        private static readonly Random Random = new Random();

        public Encrypter(string key, int keySize = 256)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Encrypter key can not be empty.", nameof(key));

            _key = key;
            _keySize = keySize;
        }

        public string GetSalt()
        {
            var saltSize = Random.Next(MinSaltSize, MaxSaltSize);
            var saltBytes = new byte[saltSize];
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        public string Encrypt(string value, string salt)
            => Encrypt<AesManaged>(value, salt);

        public string Decrypt(string value, string salt)
            => Decrypt<AesManaged>(value, salt);


        public string Hash(params string[] values)
        {
            var input = values.Aggregate((a, b) => $"{a?.ToLowerInvariant()};{b?.ToLowerInvariant()}");
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var stringBuilder = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    stringBuilder.Append(hashByte.ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }

        private string Encrypt<T>(string value, string salt) where T : SymmetricAlgorithm, new()
        {
            var initVectorBytes = Encoding.UTF8.GetBytes(_key);
            var plainTextBytes = Encoding.UTF8.GetBytes(value);
            var password = new PasswordDeriveBytes(salt, null);
            var keyBytes = password.GetBytes(_keySize / 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();

            return Convert.ToBase64String(cipherTextBytes);
        }

        private string Decrypt<T>(string value, string salt) where T : SymmetricAlgorithm, new()
        {
            var initVectorBytes = Encoding.ASCII.GetBytes(_key);
            var cipherTextBytes = Convert.FromBase64String(value);
            var password = new PasswordDeriveBytes(salt, null);
            var keyBytes = password.GetBytes(_keySize/ 8);
            var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC};
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();

            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}