using System;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MediatR.AspNetCore.Endpoints
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void MapMediatR(this IEndpointRouteBuilder endpointsBuilder)
        {
            var mediator = endpointsBuilder.ServiceProvider.GetService<IMediator>();
            if (mediator == null)
            {
                throw new InvalidOperationException($"IMediator has not added to IServiceCollection. You can add it with services.AddMediatR(...);");
            }

            var mediatorEndpointCollections = endpointsBuilder.ServiceProvider.GetService<MediatorEndpointCollections>();

            foreach (var endpoint in mediatorEndpointCollections.Endpoints)
            {
                var routePattern = RoutePatternFactory.Parse(endpoint.Uri);

                var builder = endpointsBuilder.Map(routePattern, MediatorRequestDelegate);
                builder.WithDisplayName(endpoint.RequestType.Name);

                for (var i = 0; i < endpoint.Metadata.Count; i++)
                {
                    builder.WithMetadata(endpoint.Metadata[i]);
                }
            }
        }

        private static async Task MediatorRequestDelegate(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            var requestMetadata = endpoint.Metadata.GetMetadata<IMediatorEndpointMetadata>();

            var optionsAccessor = context.RequestServices.GetService<IOptions<MediatorEndpointOptions>>();
            var options = optionsAccessor.Value;

            object model;
            if (context.Request.ContentLength.GetValueOrDefault() != 0)
            {
                //https://github.com/aspnet/AspNetCore/blob/ec8304ae85d5a94cf3cd5efc5f89b986bc4eafd2/src/Mvc/Mvc.Core/src/Formatters/SystemTextJsonInputFormatter.cs#L72-L98
                try
                {
                    model = await JsonSerializer.DeserializeAsync(context.Request.Body, requestMetadata.RequestType, options.JsonSerializerOptions, context.RequestAborted);
                    MapRouteData(requestMetadata, context.GetRouteData(), model);
                }
                catch (JsonException exception)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await options.OnDeserializeError(context, exception);
                    return;
                }
                catch (Exception exception) when (exception is FormatException || exception is OverflowException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await options.OnDeserializeError(context, exception);
                    return;
                }
            }
            else
            {
                model = Activator.CreateInstance(requestMetadata.RequestType);
                MapRouteData(requestMetadata, context.GetRouteData(), model);
            }

            if (model is IHttpContextAware httpContextAware)
            {
                httpContextAware.HttpContext = context;
            }

            var mediator = context.RequestServices.GetService<IMediator>();

            var response = await mediator.Send(model, context.RequestAborted);

            context.Response.Headers.Add("content-type", "application/json");

            var objectType = response?.GetType() ?? requestMetadata.ResponseType;
            await JsonSerializer.SerializeAsync(context.Response.Body, response, objectType, options.JsonSerializerOptions, context.RequestAborted);

            await context.Response.Body.FlushAsync(context.RequestAborted);
        }

        private static void MapRouteData(IMediatorEndpointMetadata requestMetadata, RouteData routeData, object model)
        {
            if (model == null || routeData == null || routeData.Values.Count == 0)
            {
                return;
            }

            var properties = requestMetadata.RequestType.GetProperties();
            foreach (var item in routeData.Values)
            {
                for (var i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    if (property.Name.Equals(item.Key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var value = TypeDescriptor.GetConverter(property.PropertyType).ConvertFromString(item.Value.ToString());
                        property.SetValue(model, value);
                        break;
                    }
                }
            }
        }
    }
}
