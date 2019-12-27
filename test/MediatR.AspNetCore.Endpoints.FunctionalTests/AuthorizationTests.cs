using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using MediatR.AspNetCore.Endpoints;
using MediatREndpoint.FunctionalTests.Authentication;
using MediatREndpoint.FunctionalTests.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MediatREndpoint.FunctionalTests
{
    public class AuthorizationTests
    {
        private TestServer testServer;

        [SetUp]
        public void Setup()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddAuthorization();
                    services.AddAuthentication("mock")
                    .AddScheme<MockAuthenticationHandlerOptions, MockAuthenticationHandler>("mock", _ => { });

                    services.AddMediatR(GetType().Assembly);
                    services.AddMediatREndpoints();
                })
                .Configure(app =>
                {
                    app.UseRouting();

                    app.UseAuthentication();
                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapMediatR();
                    });
                });

            testServer = new TestServer(builder);
        }

        [Test]
        public async Task UnauthorizedStatusCodeOnAuthorizedRequest()
        {
            var client = testServer.CreateClient();
            HttpResponseMessage response = await client.GetAsync("/" + nameof(AuthorizedRequest));

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Test]
        public async Task OkStatusCodeOnAllowAnonymousRequest()
        {
            var client = testServer.CreateClient();
            HttpResponseMessage response = await client.GetAsync("/" + nameof(AllowAnonymousRequest));

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}