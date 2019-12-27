using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Authorization;

namespace MediatREndpoint.FunctionalTests.Requests
{
    [Authorize]
    [Get("AuthorizedRequest")]
    public class AuthorizedRequest : IRequest
    {
    }
    public class AuthorizedRequestHandler : IRequestHandler<AuthorizedRequest>
    {
        public Task<Unit> Handle(AuthorizedRequest request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}