using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;

namespace MediatR.AspNetCore.Endpoints.OpenApi
{
    public class MediatorEndpointApiDescriptionGroupCollectionProvider : IApiDescriptionGroupCollectionProvider
    {
        private readonly MediatorEndpointCollections _mediatorEndpointCollections;

        public MediatorEndpointApiDescriptionGroupCollectionProvider(MediatorEndpointCollections mediatorEndpointCollections)
        {
            _mediatorEndpointCollections = mediatorEndpointCollections;
        }

        public int Order => 1;

        public ApiDescriptionGroupCollection ApiDescriptionGroups
        {
            get {
                var apis = new List<ApiDescription>();

                foreach (var endpoint in _mediatorEndpointCollections.Endpoints)
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
