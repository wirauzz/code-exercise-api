using Data;
using Data.Models;
using Logic.Managers;
using Logic.Managers.Interfaces;
using Logic.Models;
using Logic.Models.Mapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Presentation.Middleware;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Presentation
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            IConfigurationSection LoggerPath = Configuration.GetSection("Logger").GetSection("LogPath");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel
                .Information()
                .WriteTo.File(LoggerPath.Value, LogEventLevel.Information)
                .CreateLogger();
            Log.Information($"Json file setup has been read: appsettings.{env.EnvironmentName}.json");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .WithExposedHeaders("Authorization")
                );
            });

            services.AddDbContext<EnrollmentDBContext>();
            services.AddTransient<IStudentsManager, StudentsManager>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMapper<StudentDTO, Student>, StudentMapper>();

            services.AddControllers();

            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Configuration.GetSection("EnrollmentsInfo")["Name"],
                    Version = Configuration.GetSection("EnrollmentsInfo")["Version"],
                    Description = Configuration.GetSection("EnrollmentsInfo")["Description"],
                });
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.EnableAnnotations();

                var file = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, file);
                options.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseCors("AllowAnyOrigin");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration.GetSection("ProductsInfo")["Name"]);
            });
        }
    }
}
