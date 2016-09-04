using System.Collections.Generic;
using Warden.Watchers.Web;

namespace Warden.Spawn.Watchers.Web
{
    public class HttpRequestDefinition
    {
        public object Data { get; set; }
        public string Endpoint { get; set; }
        public HttpMethod Method { get; set; }
        public IEnumerable<Header> Headers { get; set; }

        public class Header
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}