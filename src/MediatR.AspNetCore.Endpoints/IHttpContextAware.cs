using Microsoft.AspNetCore.Http;

namespace MediatR.AspNetCore.Endpoints
{
    public interface IHttpContextAware
    {
        HttpContext HttpContext { get; set; }
    }
}