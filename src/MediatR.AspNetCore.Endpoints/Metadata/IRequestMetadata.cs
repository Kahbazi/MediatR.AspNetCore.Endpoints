using System;

namespace MediatR.AspNetCore.Endpoints
{
    public interface IRequestMetadata
    {
        Type RequestType { get; }
        Type ResponseType { get; }
    }
}