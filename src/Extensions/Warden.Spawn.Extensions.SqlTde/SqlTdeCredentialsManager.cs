using System.Collections.Generic;

namespace Warden.Spawn.Extensions.SqlTde
{
    public class SqlTdeCredentialsManager : ICredentialsManager
    {
        public IEnumerable<Credential> Decrypt(IEnumerable<Credential> credentials)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Credential> Encrypt(IEnumerable<Credential> credentials)
        {
            throw new System.NotImplementedException();
        }
    }
}