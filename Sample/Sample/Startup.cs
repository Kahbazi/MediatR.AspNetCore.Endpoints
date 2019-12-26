using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Text.Json;
using System.IO;
using System.Text;

namespace Sample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatREndpoint(GetType().Assembly);


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.Use((a, z) => 
            {
                a.Request.Method = HttpMethods.Post;
                a.Request.Body= new 
                return z();
            });
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMediatR();
            });
        }
    }

    public interface IRequestMetadata
    {
        public Type RequestType { get; }
        public Type ResponseType { get; }
    }

    public class RequestMetadata : IRequestMetadata
    {
        public RequestMetadata(Type requestType, Type responseType)
        {
            RequestType = requestType;
            ResponseType = responseType;
        }

        public Type RequestType { get; }
        public Type ResponseType { get; }
    }

    public static class EndpointRouteBuilderExtensions
    {
        public static void MapMediatR(this IEndpointRouteBuilder endpointsBuilder)
        {
            var s = endpointsBuilder.ServiceProvider.GetService<IOptions<MediatREndpointOptions>>();

            var handlerTypes = s.Value.HandlerTypes;

            var mediatRRequestDelegate = CreateRequestDelegate();

            Endpoint[] endpoints = new Endpoint[handlerTypes.Count()];

            int i = 0;
            foreach (var handlerType in handlerTypes)
            {
                
                var requestType = handlerType.GetInterfaces()[0].GetGenericArguments()[0];
                var responseType = handlerType.GetInterfaces()[0].GetGenericArguments()[1];

                var builder = new RouteEndpointBuilder(mediatRRequestDelegate,
                   RoutePatternFactory.Parse(requestType.Name),
                   0);

                builder.DisplayName = requestType.Name;
                builder.Metadata.Add(new RequestMetadata(requestType, responseType));
                builder.Metadata.Add(new HttpMethodMetadata(new[] { HttpMethods.Post }));


                endpoints[i] = builder.Build();

                i++;
            }

            endpointsBuilder.DataSources.Add(new DefaultEndpointDataSource(endpoints));
        }


        private static RequestDelegate CreateRequestDelegate()
        {
            return async context =>
            {
                var endpoint = context.GetEndpoint();

                var requestMetadata = endpoint.Metadata.GetMetadata<IRequestMetadata>();

                var model = await JsonSerializer.DeserializeAsync(context.Request.Body, requestMetadata.RequestType);

                IMediator mediator = context.RequestServices.GetService<IMediator>();


                var response = mediator.Send(model, context.RequestAborted);

                await JsonSerializer.SerializeAsync(context.Response.Body, response, requestMetadata.ResponseType);

                await context.Response.Body.FlushAsync();


            };


        }


    }
}
