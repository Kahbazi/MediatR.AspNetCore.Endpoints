using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MediatREndpoint.FunctionalTests.Requests
{
    [AllowAnonymous]
    [Get("AllowAnonymousRequest")]
    public class AllowAnonymousRequest : IRequest
    {
    }
    public class AllowAnonymousRequestHandler : IRequestHandler<AllowAnonymousRequest>
    {
        public Task<Unit> Handle(AllowAnonymousRequest request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }
    }
}