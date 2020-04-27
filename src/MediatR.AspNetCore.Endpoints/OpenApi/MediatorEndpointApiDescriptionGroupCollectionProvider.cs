using System.Collections.Generic;
using System.Reflection;
using MediatR.AspNetCore.Endpoints.OpenApi;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace MediatR.AspNetCore.Endpoints.OpenApi
{
    public class MediatorEndpointApiDescriptionGroupCollectionProvider : IApiDescriptionGroupCollectionProvider
    {
        private readonly IOptions<MediatorEndpointOptions> _options;

        public MediatorEndpointApiDescriptionGroupCollectionProvider(IOptions<MediatorEndpointOptions> options)
        {
            _options = options;
        }

        public int Order => 1;

        public ApiDescriptionGroupCollection ApiDescriptionGroups
        {
            get {
                var apis = new List<ApiDescription>();

                foreach (var endpoint in _options.Value.Endpoints)
                {
                    var httpMethodMetadata = endpoint.GetMetadata<HttpMethodMetadata>();

                    var controllerActionDescriptor = new ControllerActionDescriptor()
                    {
                        DisplayName = endpoint.RequestType.Name + "-D",
                        ActionName = endpoint.RequestType.Name + "-A",
                        ControllerName = endpoint.RequestType.Name + "-C",
                        ControllerTypeInfo = endpoint.HandlerType.GetTypeInfo(),
                        MethodInfo = endpoint.HandlerType.GetMethod("Handle")
                    };

                    var apiDescription = new ApiDescription
                    {
                        GroupName = "Mediator",
                        HttpMethod = httpMethodMetadata.HttpMethods[0],
                        RelativePath = "/" + endpoint.RequestType.Name,
                        ActionDescriptor = controllerActionDescriptor
                    };

                    var responseTypes = new ApiResponseTypeProvider().GetApiResponseTypes(controllerActionDescriptor, endpoint);

                    foreach (var responseType in responseTypes)
                    {
                        apiDescription.SupportedResponseTypes.Add(responseType);
                    }

                    apis.Add(apiDescription);
                }

                var group = new ApiDescriptionGroup("Mediator", apis);

                return new ApiDescriptionGroupCollection(new[] { group }, 1);
            }
        }
    }
}
