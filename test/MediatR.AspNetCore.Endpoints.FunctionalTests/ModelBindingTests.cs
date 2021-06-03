using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatREndpoint.FunctionalTests.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MediatR.AspNetCore.Endpoints.FunctionalTests
{
    public class ModelBindingTests
    {
        private TestServer _testServer;

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

            _testServer = new TestServer(builder);
        }

        [Test]
        public async Task RouteBinding()
        {
            var client = _testServer.CreateClient();
            var response = await client.GetAsync($"route/47/true/kahbazi/c5ce57ee-db2f-4d7a-b627-b8e36b375fc9");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RouteBindingRequest>(json);

            Assert.AreEqual(47, result.Id);
            Assert.IsTrue(result.IsActive);
            Assert.AreEqual("kahbazi", result.Name);
            Assert.AreEqual("c5ce57ee-db2f-4d7a-b627-b8e36b375fc9", result.Identifier.ToString());
        }
    }
}
