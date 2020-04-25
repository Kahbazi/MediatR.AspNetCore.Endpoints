using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using MediatR.AspNetCore.Endpoints;

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
            foreach (var handlerType in handlerTypes)
            {
                if (!(handlerType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))))
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