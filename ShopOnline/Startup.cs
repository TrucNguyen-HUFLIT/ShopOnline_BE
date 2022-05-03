using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostalCode.Infrastructure.Middleware;
using ShopOnline.Core.Appsetting;
using ShopOnline.Core;
using ShopOnline.Core.Filters;
using ShopOnline.Infrastructure.Extensions;
using System.Text.Json.Serialization;

namespace ShopOnline
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
            services.RegisterCors();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.SettingAppConfig(Configuration);

            services.RegisterDI();

            services.AddAutomapper();

            services
                .AddScoped<ValidateActionFilterAttribute>()
                .AddScoped<ValidationModelFilterAttribute>()
                .AddScoped<WrapperResultActionFilterAttribute>();

            services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(AppConfigs.ConnectionStrings.DefaultConnection)
            .UseLoggerFactory(LoggerFactory.Create(x => x.AddConsole()))
            );

            services.AddSwagger();
            services.AddBearerAuthen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app
               .UseMiddleware(typeof(GlobalExceptionHandlerMiddleware));

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAllCorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
