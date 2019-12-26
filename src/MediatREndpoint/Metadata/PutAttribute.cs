using Microsoft.AspNetCore.Http;

namespace MediatREndpoint
{
    public class PutAttribute : HttpMethodMetadataAttribute
    {
        public PutAttribute()
            : this(string.Empty)
        {
        }

        public PutAttribute(string template)
            : base(HttpMethods.Put, template)
        {
        }
    }
}