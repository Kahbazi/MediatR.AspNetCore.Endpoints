using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MediatREndpoint.FunctionalTests.Requests
{
    public class RouteBindingRequest : IRequest<RouteBindingRequest>
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public Guid Identifier { get; set; }
    }

    public class RouteBindingRequestHandler : IRequestHandler<RouteBindingRequest, RouteBindingRequest>
    {
        [HttpGet("route/{id}/{isActive}/{name}/{identifier}")]
        public Task<RouteBindingRequest> Handle(RouteBindingRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }
    }
}
