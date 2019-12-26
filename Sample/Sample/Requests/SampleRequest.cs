using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sample.Requests
{
    public class SampleRequest : IRequest<SampleResponse>
    {
        public int Id { get; set; }
    }

    public class SampleResponse
    {
        public string Name { get; set; }
        public DateTime Timestemp { get; set; }
    }

    public class SampleRequestHandler : IRequestHandler<SampleRequest, SampleResponse>
    {
        public Task<SampleResponse> Handle(SampleRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new SampleResponse
            {
                Name = "Kahbazi",
                Timestemp = DateTime.Now
            });
        }
    }
}