using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using MediatR.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatREndpoints(this IServiceCollection services)
        {
            if (!services.Any(x => x.ServiceType == typeof(IMediator)))
            {
                throw new InvalidOperationException($"typeof(IMediator) is not added to service collection. Make sure to call services.AddMediatR() before service.AddMediatR.AspNetCore.Endpoints()");
            }

            var handlerTypes = services.Where(x => x.ServiceType.IsGenericType && x.ServiceType.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(x => x.ImplementationType);

            return AddMediatREndpoints(services, handlerTypes);
        }

        public static IServiceCollection AddMediatREndpoints(this IServiceCollection services, IEnumerable<Type> handlerTypes)
        {
            var endpoints = new List<MediatorEndpoint>();

            foreach (var handlerType in handlerTypes)
            {
                var requestHandlerType = handlerType.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
                if (requestHandlerType == null)
                {
                    throw new InvalidOperationException($"Type ({handlerType.FullName}) is not an IReqeustHandler<,>");
                }

                var requestArguments = requestHandlerType.GetGenericArguments();
                var requestType = requestArguments[0];
                var responseType = requestArguments[1];

                var requestMetadata = new MediatorEndpointMetadata(requestType, responseType);

                var attributes = handlerType.GetMethod("Handle").GetCustomAttributes(false);

                var httpAttributes = attributes.OfType<HttpMethodAttribute>().ToArray();
                if (httpAttributes.Length == 0)
                {
                    var httpMethodMetadata = new HttpMethodMetadata(new[] { HttpMethods.Post });

                    var metadata = new List<object>(attributes);
                    metadata.Add(httpMethodMetadata);
                    metadata.Add(requestMetadata);

                    endpoints.Add(new MediatorEndpoint
                    {
                        Metadata = metadata,
                        RequestType = requestMetadata.RequestType,
                        ResponseType = requestMetadata.ResponseType,
                        Uri = requestMetadata.RequestType.Name
                    });
                }
                else
                {
                    for (var i = 0; i < httpAttributes.Length; i++)
                    {
                        var httpAttribute = httpAttributes[i];
                        var httpMethodMetadata = new HttpMethodMetadata(httpAttribute.HttpMethods);

                        string template;
                        if (string.IsNullOrEmpty(httpAttribute.Template))
                        {
                            template = "/" + requestType.Name;
                        }
                        else
                        {
                            template = httpAttribute.Template;
                        }

                        var metadata = new List<object>(attributes);
                        metadata.Add(httpMethodMetadata);
                        metadata.Add(requestMetadata);

                        endpoints.Add(new MediatorEndpoint
                        {
                            Metadata = metadata,
                            RequestType = requestMetadata.RequestType,
                            ResponseType = requestMetadata.ResponseType,
                            Uri = template
                        });
                    }
                }
            }

            services.Configure<MediatorEndpointOptions>(options =>
            {
                options.Endpoints = endpoints;
            });

            return services;
        }
    }
}
