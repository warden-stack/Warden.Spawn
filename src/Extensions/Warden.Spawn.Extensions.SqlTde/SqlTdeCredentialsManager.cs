using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
            string tableName = "Configurations")
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
                warden = warden.ToLowerInvariant();
                name = name.ToLowerInvariant();
                watcher = watcher?.ToLowerInvariant();
                integration = integration?.ToLowerInvariant();
                hook = hook?.ToLowerInvariant();
                var hash = Hash(warden, name, watcher, integration, hook);
                var credential = connection
                    .Query<Credential>($"select value,salt,watcher,integration,hook from {_tableName} " +
                                       "where hash=@hash",
                        new {hash})
                    .FirstOrDefault();

                return credential == null ? string.Empty : _encrypter.Decrypt(credential.Value, credential.Salt);
            }
        }

        public void Save(string warden, string name, string value, string watcher = "", string integration = "",
            string hook = "")
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    Remove(warden, name, watcher, integration, hook, connection, transaction);
                    warden = warden.ToLowerInvariant();
                    name = name.ToLowerInvariant();
                    watcher = watcher?.ToLowerInvariant();
                    integration = integration?.ToLowerInvariant();
                    hook = hook?.ToLowerInvariant();
                    var salt = _encrypter.GetSalt();
                    var encryptedValue = _encrypter.Encrypt(value, salt);
                    var hash = Hash(warden, name, watcher, integration, hook);
                    var affectedResults = connection.Execute($"insert into {_tableName} values " +
                                                             "(@hash, @warden, @name, @value, @salt," +
                                                             "@watcher, @integration, @hook, @createdAt)",
                        new
                        {
                            hash,
                            warden,
                            name,
                            value = encryptedValue,
                            salt,
                            watcher,
                            integration,
                            hook,
                            createdAt = DateTime.UtcNow
                        }, transaction);
                    transaction.Commit();
                }
            }
        }

        public void Remove(string warden, string name, string watcher = "", string integration = "", string hook = "")
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Remove(warden, name, watcher, integration, hook, connection, null);
            }
        }

        private void Remove(string warden, string name, string watcher, string integration, string hook,
            DbConnection connection, IDbTransaction transaction)
        {
            warden = warden.ToLowerInvariant();
            name = name.ToLowerInvariant();
            watcher = watcher?.ToLowerInvariant();
            integration = integration?.ToLowerInvariant();
            hook = hook?.ToLowerInvariant();
            var hash = Hash(warden, name, watcher, integration, hook);
            var affectedResults = connection
                .Execute($"delete from {_tableName} where hash=@hash",
                    new {hash}, transaction);
        }

        private string Hash(string warden, string name, string watcher, string integration, string hook)
            => _encrypter.Hash(warden, name, watcher, integration, hook);

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