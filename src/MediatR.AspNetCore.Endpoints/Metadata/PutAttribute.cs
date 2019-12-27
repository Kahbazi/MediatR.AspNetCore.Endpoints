using Microsoft.AspNetCore.Http;

namespace MediatR.AspNetCore.Endpoints
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