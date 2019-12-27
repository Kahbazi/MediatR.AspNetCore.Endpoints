using System;
using System.Linq;
using System.Text.Json;
using MediatR;
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
        public static void MapMediatR(this IEndpointRouteBuilder endpointsBuilder, string pathString)
        {
            endpointsBuilder.MapMediatR(new PathString(pathString));
        }

        public static void MapMediatR(this IEndpointRouteBuilder endpointsBuilder)
        {
            endpointsBuilder.MapMediatR(PathString.Empty);
        }

        public static void MapMediatR(this IEndpointRouteBuilder endpointsBuilder, PathString pathString)
        {
            var options = endpointsBuilder.ServiceProvider.GetService<IOptions<MediatorEndpointOptions>>();

            var mediatorRequestDelegate = CreateRequestDelegate();

            foreach (var handlerType in options.Value.HandlerTypes)
            {
                if (handlerType.GetGenericTypeDefinition() != typeof(IRequestHandler<,>))
                {
                    throw new InvalidOperationException($"Type ({handlerType.FullName}) is not an IReqeustHandler<,>" +
                        $"All types in {nameof(MediatorEndpointOptions)}.{nameof(MediatorEndpointOptions.HandlerTypes)} must implement IReqeustHandler<,>.");
                }

                var requestType = handlerType.GetGenericArguments()[0];
                var responseType = handlerType.GetGenericArguments()[1];
                var requestMetadata = new RequestMetadata(requestType, responseType);

                var metadata = requestMetadata.RequestType.GetCustomAttributes(false);

                var httpAttributes = metadata.OfType<HttpMethodMetadataAttribute>().ToArray();
                if (httpAttributes.Length == 0)
                {
                    var httpMethodMetadata = new HttpMethodMetadata(new[] { HttpMethods.Post });
                    CreateEndpoint(endpointsBuilder, requestMetadata, metadata, requestMetadata.RequestType.Name, pathString, httpMethodMetadata, mediatorRequestDelegate);
                }
                else
                {
                    for (int i = 0; i < httpAttributes.Length; i++)
                    {
                        var httpAttribute = httpAttributes[i];
                        var httpMethodMetadata = new HttpMethodMetadata(new[] { httpAttribute.HttpMethod });

                        string template;
                        //if (string.IsNullOrEmpty(httpAttribute.Template))
                        //{
                        //    template = "/";
                        //}
                        //else
                        //{
                            template = httpAttribute.Template;
                        //}

                        CreateEndpoint(endpointsBuilder, requestMetadata, metadata, httpAttribute.Template, pathString, httpMethodMetadata, mediatorRequestDelegate);
                    }
                }
            }
        }

        private static void CreateEndpoint(IEndpointRouteBuilder endpointsBuilder,
            RequestMetadata requestMetadata,
            object[] metadata,
            string template,
            PathString pathString,
            HttpMethodMetadata httpMethodMetadata,
            RequestDelegate mediatorRequestDelegate)
        {
            if (pathString.HasValue)
            {
                template = $"{pathString.Value.TrimEnd('/')}/{template}";
            }

            var routePattern = RoutePatternFactory.Parse(template);

            var builder = endpointsBuilder.Map(routePattern, mediatorRequestDelegate);
            builder.WithDisplayName(requestMetadata.RequestType.Name);
            builder.WithMetadata(requestMetadata);
            builder.WithMetadata(httpMethodMetadata);

            for (int i = 0; i < metadata.Length; i++)
            {
                builder.WithMetadata(metadata[i]);
            }


        }


        private static RequestDelegate CreateRequestDelegate()
        {
            return async context =>
            {
                var endpoint = context.GetEndpoint();

                var requestMetadata = endpoint.Metadata.GetMetadata<IRequestMetadata>();

                object model;
                if (context.Request.ContentLength.GetValueOrDefault() != 0)
                {
                    //https://github.com/aspnet/AspNetCore/blob/ec8304ae85d5a94cf3cd5efc5f89b986bc4eafd2/src/Mvc/Mvc.Core/src/Formatters/SystemTextJsonInputFormatter.cs#L72-L98
                    try
                    {
                        model = await JsonSerializer.DeserializeAsync(context.Request.Body, requestMetadata.RequestType, null, context.RequestAborted);
                    }
                    catch (JsonException)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }
                    catch (Exception exception) when (exception is FormatException || exception is OverflowException)
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }
                }
                else
                {
                    model = Activator.CreateInstance(requestMetadata.RequestType);
                }

                if (model is IHttpContextAware httpContextAware)
                {
                    httpContextAware.HttpContext = context;
                }

                IMediator mediator = context.RequestServices.GetService<IMediator>();

                var response = await mediator.Send(model, context.RequestAborted);

                var objectType = response?.GetType() ?? requestMetadata.ResponseType;

                await JsonSerializer.SerializeAsync(context.Response.Body, response, objectType, null, context.RequestAborted);

                await context.Response.Body.FlushAsync(context.RequestAborted);
            };
        }
    }
}