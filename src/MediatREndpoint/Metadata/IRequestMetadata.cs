using System;

namespace MediatREndpoint
{
    public interface IRequestMetadata
    {
        Type RequestType { get; }
        Type ResponseType { get; }
    }
}