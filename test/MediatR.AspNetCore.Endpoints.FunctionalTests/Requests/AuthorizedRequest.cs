using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediatREndpoint.FunctionalTests.Requests
{
    public class AuthorizedRequest : IRequest
    {
    }

    public class AuthorizedRequestHandler : IRequestHandler<AuthorizedRequest>
    {

        [Authorize]
        [HttpGet("AuthorizedRequest")]
        public Task<Unit> Handle(AuthorizedRequest request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}