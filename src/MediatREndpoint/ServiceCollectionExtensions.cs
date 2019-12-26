using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MediatREndpoint
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatREndpoint(this IServiceCollection services)
        {
            if (!services.Any(x => x.ServiceType == typeof(IMediator)))
            {
                throw new InvalidOperationException($"typeof(IMediator) is not added to service collection. Make sure to call services.AddMediatR() before service.AddMediatREndpoint()");
            }

            var handlerTypes = services.Where(x => x.ServiceType.IsGenericType && x.ServiceType.GetGenericTypeDefinition() == typeof(MediatR.IRequestHandler<,>))
                .Select(x => x.ServiceType);

            return AddMediatREndpoint(services, handlerTypes);
        }

        public static IServiceCollection AddMediatREndpoint(this IServiceCollection services, IEnumerable<Type> handlerTypes)
        {
            foreach (var handlerType in handlerTypes)
            {
                if (handlerType.GetGenericTypeDefinition() != typeof(IRequestHandler<,>))
                {
                    throw new InvalidOperationException($"Type ({handlerType.FullName}) is not an IReqeustHandler<,>" +
                        $"All types in {nameof(MediatorEndpointOptions)}.{nameof(MediatorEndpointOptions.HandlerTypes)} must implement IReqeustHandler<,>.");
                }
            }

            services.Configure<MediatorEndpointOptions>(options =>
            {
                options.HandlerTypes = handlerTypes;
            });

            return services;
        }
    }
}