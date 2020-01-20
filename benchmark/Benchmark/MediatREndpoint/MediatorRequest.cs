using MediatR;
using MediatR.AspNetCore.Endpoints;

namespace Benchmark.Mvc
{
    [Post("Greetings")]
    public class MediatorRequest : IRequest<MediatorResponse>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}