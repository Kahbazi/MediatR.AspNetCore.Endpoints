using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Benchmark.Mvc
{
    public class MediatorRequestHandler : IRequestHandler<MediatorRequest, MediatorResponse>
    {
        public Task<MediatorResponse> Handle(MediatorRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new MediatorResponse
            {
                Message = $"Hello {request.Name}"
            });
        }
    }
}