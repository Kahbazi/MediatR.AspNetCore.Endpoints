using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatREndpoint;
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

        public DateTime Timestemp { get; set; }
    }

    public class DerivedSampleResponse  : SampleResponse
    {
        public int AnotherProperty { get; set; }
    }

    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return Run(request);
        }

        public abstract Task<TResponse> Run(TRequest request);
    }

    public class SampleRequestHandler : BaseRequestHandler<SampleRequest, SampleResponse>
    {
        public override Task<SampleResponse> Run(SampleRequest request)
        {
            return Task.FromResult<SampleResponse>(new DerivedSampleResponse
            {
                Name = "Kahbazi",
                Timestemp = DateTime.Now
            });
        }
    }
}