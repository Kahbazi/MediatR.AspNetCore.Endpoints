using System.Collections.Generic;

namespace MediatR.AspNetCore.Endpoints
{
    public class MediatorEndpointCollections
    {
        public MediatorEndpointCollections(IEnumerable<MediatorEndpoint> endpoints)
        {
            Endpoints = endpoints;
        }

        public IEnumerable<MediatorEndpoint> Endpoints { get; }
    }
}
