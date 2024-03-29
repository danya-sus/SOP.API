﻿
using System.Reflection;
using Microsoft.OpenApi.Models;
using SOP.Data;
using SOP.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using SOP.API.GraphQL.Schemas;
using GraphQL;
using GraphiQl;
using SOP.API.GraphQL.GraphTypes;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SOP.API.Hubs;

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
                options.UseNpgsql(Configuration.GetConnectionString("SOPContext")), ServiceLifetime.Singleton);

            services.AddTransient<IManufacturerRepository, ManufacturerRepository>();
            services.AddTransient<IModelRepository, ModelRepository>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();
            services.AddTransient<IOwnerRepository, OwnerRepository>();

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

            services.AddGraphQL(builder => builder
                .AddNewtonsoftJson()
                .AddAutoSchema<OwnerSchema>()
                .AddSchema<OwnerSchema>()
                .AddGraphTypes(typeof(OwnerGraphType).Assembly)
                );

            var bus = RabbitHutch.CreateBus(Configuration.GetConnectionString("SOPRabbitMQ"));
            services.AddSingleton<IBus>(bus);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseGraphQL<OwnerSchema>();
            app.UseGraphiQl("/graphiql");

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
