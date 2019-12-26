using Microsoft.AspNetCore.Http;

namespace MediatREndpoint
{
    public class DeleteAttribute : HttpMethodMetadataAttribute
    {
        public DeleteAttribute()
            : this(string.Empty)
        {
        }

        public DeleteAttribute(string template)
            : base(HttpMethods.Delete, template)
        {
        }
    }
}