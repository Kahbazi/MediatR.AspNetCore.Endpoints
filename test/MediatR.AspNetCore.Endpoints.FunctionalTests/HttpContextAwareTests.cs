using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MediatR.AspNetCore.Endpoints.FunctionalTests
{
    public class HttpContextAwareTests
    {
        private TestServer testServer;

        [SetUp]
        public void Setup()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();

                    services.AddMediatR(GetType().Assembly);
                    services.AddMediatREndpoints();
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapMediatR();
                    });
                });

            testServer = new TestServer(builder);
        }

        [Test]
        public async Task HttpContextSetsOnRequest()
        {
            var client = testServer.CreateClient();
            await client.GetAsync("/");
        }

        public class HttpContextAwareRequest : IRequest, IHttpContextAware
        {
            public HttpContext HttpContext { get; set; }
        }

        public class HttpContextAwareRequestHandler : IRequestHandler<HttpContextAwareRequest>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public HttpContextAwareRequestHandler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }


            public Task<Unit> Handle(HttpContextAwareRequest request, CancellationToken cancellationToken)
            {
                Assert.AreSame(_httpContextAccessor.HttpContext, request.HttpContext);
                return Unit.Task;
            }
        }
    }
}
