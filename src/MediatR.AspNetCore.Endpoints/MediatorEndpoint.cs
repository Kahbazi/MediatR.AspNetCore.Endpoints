using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.AspNetCore.Endpoints
{
    public class MediatorEndpoint
    {
        public Type RequestType { get; set; }

        public Type ResponseType { get; set; }

        public Type HandlerType { get; set; }

        public IReadOnlyList<object> Metadata { get; set; }

        public string Uri { get; set; }

        public T GetMetadata<T>()
        {
            return Metadata.OfType<T>().FirstOrDefault();
        }
    }
}
