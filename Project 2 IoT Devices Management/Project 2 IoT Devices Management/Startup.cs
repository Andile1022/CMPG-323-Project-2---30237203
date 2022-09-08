using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Project_2_IoT_Devices_Management.Controllers;
using Project_2_IoT_Devices_Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_2_IoT_Devices_Management
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(swag=>swag.SwaggerDoc(
                "v1", new OpenApiInfo
                {
                    Title="Project 2 IoT Device Management",
                    Version="v1"
                }
            ));
            services.AddDbContext<Project2databaseContext>(options=>options.UseSqlServer(Configuration.GetConnectionString("project2connectionstring")));
            services.AddControllers();
            services.AddScoped<ZonesController,ZonesController>();
            services.AddScoped<CategoriesController, CategoriesController>();
            services.AddScoped<DevicesController, DevicesController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUi=>swaggerUi.SwaggerEndpoint("/swagger/v1/swagger.json", "Project 2 IoT Device Management"));
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
