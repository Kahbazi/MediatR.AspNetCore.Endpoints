using Microsoft.AspNetCore.Http;

namespace MediatREndpoint
{
    public interface IHttpContextAware
    {
        HttpContext HttpContext { get; set; }
    }
}