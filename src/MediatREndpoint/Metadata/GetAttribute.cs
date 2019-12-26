using Microsoft.AspNetCore.Http;

namespace MediatREndpoint
{
    public class GetAttribute : HttpMethodMetadataAttribute
    {
        public GetAttribute()
            : this(string.Empty)
        {
        }

        public GetAttribute(string template)
            : base(HttpMethods.Get, template)
        {
        }
    }
}