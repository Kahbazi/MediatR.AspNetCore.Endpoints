using System.Threading.Tasks;
using MediatR;
using MediatR.AspNetCore.Endpoints;
using MediatR.AspNetCore.Endpoints.OpenApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace OpenApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {         
            services.AddMediatR(GetType().Assembly);
            services.AddMediatREndpoints();

            services.AddSingleton<IApiDescriptionGroupCollectionProvider, MediatorEndpointApiDescriptionGroupCollectionProvider>();
            services.AddOpenApiDocument(document => document.DocumentName = "v1");
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/", context =>
                {
                    context.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                });

                endpoints.MapMediatR();
            });
        }
    }
}
