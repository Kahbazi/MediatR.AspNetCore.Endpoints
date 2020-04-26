using System;
using System.Collections.Generic;

namespace MediatR.AspNetCore.Endpoints
{
    public class MediatorEndpoint
    {
        public Type RequestType { get; set; }
        public Type ResponseType { get; set; }
        public IReadOnlyList<object> Metadata { get; set; }
        public string Uri { get; set; }
    }
}
