using System;

namespace MediatR.AspNetCore.Endpoints
{
    public interface IMediatorEndpointMetadata
    {
        Type RequestType { get; }
        Type ResponseType { get; }
    }
}
