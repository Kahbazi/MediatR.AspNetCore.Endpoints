using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MediatREndpoint.Tests
{
    public class EndpointRouteBuilderExtensionsTest
    {
        [Test]
        public void ThrowException()
        {
            var services = new ServiceCollection();
            var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
            var builder = new DefaultEndpointRouteBuilder(applicationBuilder);

            Assert.Throws<InvalidOperationException>(() => builder.MapMediatR("/api"));
        }

        [Test]
        public void WithPathString()
        {
            var services = new ServiceCollection();

            services.AddMediatREndpoints(new Type[] { typeof(IRequestHandler<SimpleRequest, Unit>) });

            var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
            IEndpointRouteBuilder builder = new DefaultEndpointRouteBuilder(applicationBuilder);

            builder.MapMediatR("/api");

            var dataSource = builder.DataSources.First();
            var endpoint = dataSource.Endpoints.First();

            AssertEndpoint(endpoint, HttpMethods.Post, "/api/" + typeof(SimpleRequest).Name);

        }

        [Test]
        public void RequestWithNoAttribute()
        {
            var services = new ServiceCollection();

            services.AddMediatREndpoints(new Type[] { typeof(IRequestHandler<SimpleRequest, Unit>) });

            var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
            IEndpointRouteBuilder builder = new DefaultEndpointRouteBuilder(applicationBuilder);

            builder.MapMediatR();

            var dataSource = builder.DataSources.First();
            var endpoint = dataSource.Endpoints.First();

            AssertEndpoint(endpoint, HttpMethods.Post, typeof(SimpleRequest).Name);

        }

        [Test]
        public void MultipleRouteRequestAddsMultipleEndpoints()
        {
            var services = new ServiceCollection();

            services.AddMediatREndpoints(new Type[] { typeof(IRequestHandler<MultipleRouteRequest, Unit>) });

            var applicationBuilder = new ApplicationBuilder(services.BuildServiceProvider());
            IEndpointRouteBuilder builder = new DefaultEndpointRouteBuilder(applicationBuilder);

            builder.MapMediatR();

            var dataSource = builder.DataSources.First();

            Assert.AreEqual(3, dataSource.Endpoints.Count);

            var endpoint1 = dataSource.Endpoints[0];
            AssertEndpoint(endpoint1, HttpMethods.Get, "route1");

            var endpoint2 = dataSource.Endpoints[1];
            AssertEndpoint(endpoint2, HttpMethods.Get, "route2");

            var endpoint3 = dataSource.Endpoints[2];
            AssertEndpoint(endpoint3, HttpMethods.Post, "route3");
        }

        private static void AssertEndpoint(Endpoint endpoint, string expectedHttpmethod, string expectedRoute)
        {
            var httpMethodMetadata = endpoint.Metadata.GetMetadata<HttpMethodMetadata>();
            Assert.AreEqual(1, httpMethodMetadata.HttpMethods.Count);
            Assert.AreEqual(expectedHttpmethod, httpMethodMetadata.HttpMethods[0]);

            Assert.AreEqual(typeof(RouteEndpoint), endpoint.GetType());

            var routeEndpoint = endpoint as RouteEndpoint;

            Assert.AreEqual(expectedRoute, routeEndpoint.RoutePattern.RawText);
        }

        private class SimpleRequest : IRequest
        {

        }

        [Get("route1")]
        [Get("route2")]
        [Post("route3")]
        private class MultipleRouteRequest : IRequest
        {

        }
    }
}
