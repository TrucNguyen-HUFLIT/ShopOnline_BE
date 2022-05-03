using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShopOnline.Business;
using ShopOnline.Business.Customer;
using ShopOnline.Business.Logic;
using ShopOnline.Business.Logic.Customer;
using ShopOnline.Business.Logic.Staff;
using ShopOnline.Business.Staff;
using ShopOnline.Core.Appsetting;
using ShopOnline.Data.Repositories.Product;
using ShopOnline.Infrastructure.Helper;
using System;
using System.Text;

namespace ShopOnline.Infrastructure.Extensions
{
    public static class ServiceExten
    {
        public static IServiceCollection RegisterDI(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
         
            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IClientBusiness, ClientBusiness>();
            services.AddScoped<IStaffBusiness, StaffBusiness>();
            services.AddScoped<ICartBusiness, CartBusiness>();
            services.AddScoped<IProductBusiness, ProductBusiness>();
            services.AddScoped<IReviewBusiness, ReviewBusiness>();
            services.AddScoped<ICustomerBusiness, CustomerBusiness>();
            services.AddScoped<IOrderBusiness, OrderBusiness>();
            services.AddScoped<IReportBusiness, ReportBusiness>();
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }

        public static IServiceCollection SettingAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            AppConfigs.ConnectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
            AppConfigs.BearerToken = configuration.GetSection(nameof(BearerToken)).Get<BearerToken>();

            return services;
        }

        public static IServiceCollection RegisterCors(this IServiceCollection services)
        {
            //services.AddCors(o => o.AddPolicy(corsPolicy, builder =>
            //                 builder
            //               .AllowAnyMethod()
            //               .AllowAnyHeader()
            //               .WithOrigins("http://localhost:4000/")
            //               .AllowAnyOrigin()));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllCorsPolicy",
                builder =>
                {
                    // Not a permanent solution, but just trying to isolate the problem
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            return services;
        }

        public static IServiceCollection AddBearerAuthen(this IServiceCollection services)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfigs.BearerToken.SecretKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(x =>
                     {
                         x.RequireHttpsMetadata = false;
                         x.SaveToken = true;
                         x.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = secretKey,
                             ValidIssuer = AppConfigs.BearerToken.Issuer,
                             ValidAudience = AppConfigs.BearerToken.Audience,
                             ValidateIssuer = true,
                             ValidateAudience = true
                         };
                     });

            return services;
        }

        public static IServiceCollection AddAutomapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(x => new Mapper(MapperHelper.MapperConfiguration));

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = new Uri("https://twitter.com/spboyer"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
              });
            });

            return services;
        }
    }
}
