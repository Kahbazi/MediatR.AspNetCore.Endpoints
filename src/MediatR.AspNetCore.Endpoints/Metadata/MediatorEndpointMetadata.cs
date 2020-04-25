using System;

namespace MediatR.AspNetCore.Endpoints
{
    internal class MediatorEndpointMetadata : IMediatorEndpointMetadata
    {
        public MediatorEndpointMetadata(Type requestType, Type responseType)
        {
            RequestType = requestType;
            ResponseType = responseType;
        }

        public Type RequestType { get; }
        public Type ResponseType { get; }
    }
}