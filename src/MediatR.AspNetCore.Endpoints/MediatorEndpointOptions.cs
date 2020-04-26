using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatR.AspNetCore.Endpoints
{
    public class MediatorEndpointOptions
    {
        public IEnumerable<MediatorEndpoint> Endpoints { get; set; }
    }
}
