using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using MediatR;
using MediatR.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<SampleHttpPost>();
        }
    }

    [RankColumn, MemoryDiagnoser]
    public class SampleHttpPost
    {
        private HttpClient _mediatorServer;
        private HttpClient _mvcServer;

        [GlobalSetup]
        public void Setup()
        {
            _mediatorServer = CreateMediatorServer().CreateClient();
            _mvcServer = CreateMvcServer().CreateClient();
        }

        [Benchmark]
        public async Task<string> Mediator()
        {
            return await SendRequest(_mediatorServer);
        }

        [Benchmark]
        public async Task<string> Mvc()
        {
            return await SendRequest(_mvcServer);
        }

        private static async Task<string> SendRequest(HttpClient httpClient)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/greetings");
            var content = Encoding.UTF8.GetBytes("{\"Id\":47,\"Name\":\"kahbazi\"}");
            request.Content = new ByteArrayContent(content);
            request.Content.Headers.ContentLength = content.Length;
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await httpClient.SendAsync(request);
            var output = await response.Content.ReadAsStringAsync();
            return output;
        }

        private TestServer CreateMediatorServer()
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

            return new TestServer(builder);
        }

        private TestServer CreateMvcServer()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddControllers();
                })
                .Configure(app =>
                {
                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });

            return new TestServer(builder);
        }

    }
}