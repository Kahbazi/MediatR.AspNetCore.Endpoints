using System.Collections.Generic;

namespace MediatR.AspNetCore.Endpoints
{
    public class MediatorEndpointOptions
    {
        public IEnumerable<MediatorEndpoint> Endpoints { get; set; }
    }
}
