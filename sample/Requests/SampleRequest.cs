using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Http;

namespace Sample.Requests
{
    [Post]
    public class SampleRequest : IRequest<SampleResponse>, IHttpContextAware
    {
        public int Id { get; set; }

        public HttpContext HttpContext { get; set; }
    }

    public class SampleResponse
    {
        public string Name { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class SampleRequestHandler : IRequestHandler<SampleRequest, SampleResponse>
    {
        public Task<SampleResponse> Handle(SampleRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult<SampleResponse>(new SampleResponse
            {
                Name = "Kahbazi",
                Timestamp = DateTime.Now
            });
        }
    }
}