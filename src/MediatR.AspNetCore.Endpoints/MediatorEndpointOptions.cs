using System.Text.Json;

namespace MediatR.AspNetCore.Endpoints
{
    public class MediatorEndpointOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; set; }
    }
}
