using Microsoft.AspNetCore.Http;

namespace MediatREndpoint
{
    public class PostAttribute : HttpMethodMetadataAttribute
    {
        public PostAttribute()
            : this(string.Empty)
        {
        }

        public PostAttribute(string template)
            : base(HttpMethods.Post, template)
        {
        }
    }
}