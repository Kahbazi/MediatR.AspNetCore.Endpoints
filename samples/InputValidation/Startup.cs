using MediatR.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using InputValidation.Middlewares;

namespace InputValidation
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(GetType().Assembly);
            services.AddMediatREndpoints();
            services.AddValidatorsFromAssemblies(new[] { GetType().Assembly });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ValidationMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMediatR();
            });
        }
    }
}
