using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Benchmark.Mvc
{
    [Route("api")]
    public class TestController : Controller
    {
        [HttpPost("Greetings")]
        public Task<MvcResponse> Greetings([FromBody]MvcRequest request)
        {
            return Task.FromResult(new MvcResponse
            {
                Message = $"Hello {request.Name}"
            });
        }
    }
}