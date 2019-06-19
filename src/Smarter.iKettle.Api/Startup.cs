using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Smarter.iKettle.Api.Middleware;
using Smarter.iKettle.Application.Interfaces;
using Smarter.iKettle.Application.Kettle;
using Smarter.iKettle.Infrastructure;
using Smarter.iKettle.Infrastructure.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Smarter.iKettle.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddVersionedApiExplorer(options =>
            {
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<KettleSettings>(options => Configuration.GetSection(nameof(KettleSettings)).Bind(options));

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, Swagger.ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            services.AddScoped<IKettleService, KettleService>();
            services.AddScoped<IKettleClient, KettleClient>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseMiddleware(typeof(GeneralErrorHandlingMiddleware), env);

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";

                foreach(var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }
    }
}