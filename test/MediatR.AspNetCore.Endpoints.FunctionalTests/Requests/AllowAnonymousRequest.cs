using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediatREndpoint.FunctionalTests.Requests
{
    public class AllowAnonymousRequest : IRequest
    {
    }

    public class AllowAnonymousRequestHandler : IRequestHandler<AllowAnonymousRequest>
    {

        [AllowAnonymous]
        [HttpGet("AllowAnonymousRequest")]
        public Task<Unit> Handle(AllowAnonymousRequest request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}