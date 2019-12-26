using System;

namespace MediatREndpoint
{
    internal class RequestMetadata : IRequestMetadata
    {
        public RequestMetadata(Type requestType, Type responseType)
        {
            RequestType = requestType;
            ResponseType = responseType;
        }

        public Type RequestType { get; }
        public Type ResponseType { get; }
    }
}