using System;

namespace MediatR.AspNetCore.Endpoints
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HttpMethodMetadataAttribute : Attribute
    {
        public HttpMethodMetadataAttribute(string httpMethod, string template)
        {
            HttpMethod = httpMethod;
            Template = template;
        }

        public string HttpMethod { get; }

        public string Template { get; }
    }
}