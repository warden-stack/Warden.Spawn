using System.Collections.Generic;

namespace Warden.Spawn
{
    public interface ICredentialsManager
    {
        IEnumerable<Credential> Decrypt(IEnumerable<Credential> credentials);
        IEnumerable<Credential> Encrypt(IEnumerable<Credential> credentials);
    }
}