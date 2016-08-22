using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Warden.Spawn.Security;

namespace Warden.Spawn.Extensions.SqlTde
{
    public class SqlTdeCredentialsManager : ICredentialsManager
    {
        private readonly string _connectionString;
        private readonly IEncrypter _encrypter;
        private readonly string _tableName;

        public SqlTdeCredentialsManager(string connectionString,
            IEncrypter encrypter,
            string tableName = "Credentials")
        {
            _connectionString = connectionString;
            _encrypter = encrypter;
            _tableName = tableName;
        }

        public string Get(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                name = name.ToLowerInvariant();
                var credential = connection
                    .Query<Credential>($"select value,salt from {_tableName} where name=@name",
                        new {name})
                    .FirstOrDefault();

                return credential == null ? string.Empty : _encrypter.Decrypt(credential.Value, credential.Salt);
            }
        }

        public void Save(string name, string value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                name = name.ToLowerInvariant();
                var salt = _encrypter.GetSalt();
                var encryptedValue = _encrypter.Encrypt(value, salt);
                var affectedResults = connection
                    .Execute($"insert into {_tableName} values (@name, @value, @salt)",
                        new {name, value = encryptedValue, salt});
            }
        }

        public void Remove(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                name = name.ToLowerInvariant();
                var affectedResults = connection
                    .Execute($"delete from {_tableName} where name=@name",
                        new {name});
            }
        }

        private class Credential
        {
            public string Value { get; set; }
            public string Salt { get; set; }
        }
    }
}