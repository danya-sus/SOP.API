
using System.Reflection;
using Microsoft.OpenApi.Models;
using SOP.Data;
using SOP.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using GraphQL.Types;
using SOP.API.GraphQL.Schemas;
using GraphQL.Server;

namespace SOP.API
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers().AddNewtonsoftJson();
            services.AddEndpointsApiExplorer();

            services.AddDbContext<SOPContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("SOPContext")));

            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();

            services.AddScoped<ISchema, OwnerSchema>();
            services.AddGraphQL(options => options.EnableMetrics = true).AddSystemTextJson();

            services.AddSwaggerGen(
                config => {
                    config.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Title = "SOP API"
                    });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    config.IncludeXmlComments(xmlPath);
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseGraphQLAltair();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseGraphQL<ISchema>();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
