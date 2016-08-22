using System;
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

        public string Get(string warden, string name, string watcher = "", string integration = "", string hook = "")
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                name = name.ToLowerInvariant();
                var credential = connection
                    .Query<Credential>($"select value,salt,watcher,integration,hook from {_tableName} " +
                                       "where warden=@warden and name=@name and watcher=@watcher " +
                                       "and integration=@integration and hook=@hook",
                        new {warden, name, watcher, integration, hook})
                    .FirstOrDefault();

                return credential == null ? string.Empty : _encrypter.Decrypt(credential.Value, credential.Salt);
            }
        }

        public void Save(string warden, string name, string value, string watcher = "", string integration = "", string hook = "")
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                name = name.ToLowerInvariant();
                var salt = _encrypter.GetSalt();
                var encryptedValue = _encrypter.Encrypt(value, salt);
                var affectedResults = connection
                    .Execute($"insert into {_tableName} values (@warden, @name, @value, @salt, @watcher, @integration, @hook, @createdAt)",
                        new { warden, name, value = encryptedValue, salt, watcher, integration, hook, createdAt = DateTime.UtcNow });
            }
        }

        public void Remove(string warden, string name, string watcher = "", string integration = "", string hook = "")
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                name = name.ToLowerInvariant();
                var affectedResults = connection
                    .Execute($"delete from {_tableName} where warden=@warden and name=@name " +
                             "and watcher=@watcher and integration=@integration and hook=@hook",
                        new {warden, name, watcher, integration, hook});
            }
        }

        private class Credential
        {
            public string Value { get; set; }
            public string Salt { get; set; }
            public string Watcher { get; set; }
            public string Integration { get; set; }
            public string Hook { get; set; }
        }
    }
}