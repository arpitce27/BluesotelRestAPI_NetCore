using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BluesotelRestAPI_NetCore.Filter;
using BluesotelRestAPI_NetCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using NSwag.AspNetCore;
using BluesotelRestAPI_NetCore.Services;
using AutoMapper;
using BluesotelRestAPI_NetCore.Infrastructure;
using Newtonsoft.Json;

namespace BluesotelRestAPI_NetCore
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<HotelInfo>(Configuration.GetSection("Info"));
            services.Configure<HotelOptions>(Configuration);

            services.AddScoped<IRoomService, DefaultRoomService>();
            services.AddScoped<IOpeningService, DefaultOpeningService>();
            services.AddScoped<IBookingService, DefaultBookingService>();
            services.AddScoped<IDateLogicService, DefaultDateLogicService>();


            // Using InMemory Database
            services.AddDbContext<HotelApiDbContext>(options =>
            {
                options.UseInMemoryDatabase("BlusotelDev");
            });

            services
                .AddMvc(options =>
                {
                    options.Filters.Add<JsonExceptionFilter>();

                    // For rewritting the links which is returned from the controller class
                    options.Filters.Add<LinkRewritingFilter>();

                    // For not redirecting the request from HTTP -> HTTPS
                    options.Filters.Add<RequiredHttpOrCloseAttribute>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    // These should be the defaults, but we can be explicit:
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;

                });

            services.AddRouting(option => option.LowercaseUrls = true);

            //Setting up the version for the APIs
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });

            // Adding the automapper 
            services.AddAutoMapper(options => options.AddProfile<MappingProfile>());

            // For allowing Cross origin request 
            services.AddCors(options =>
            {
                if (_env.IsDevelopment())
                {
                    // For allowing any origin to make the requests
                    options.AddPolicy("AppSubDomain", policy => policy.AllowAnyOrigin());
                }
                else
                {
                    // For Allowing specific origin to make the requests
                    options.AddPolicy("AppSubDomain", policy =>
                       policy.WithOrigins("https://api.other.com"));
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Swagger UI Configuration
                app.UseSwaggerUi3WithApiExplorer(options =>
                {
                    options.GeneratorSettings.DefaultPropertyNameHandling 
                        = NJsonSchema.PropertyNameHandling.CamelCase;
                });
            }
            else
            {
                app.UseHsts();
            }

            // This will allow to rediret the request from HTTP -> HTTPS
            //app.UseHttpsRedirection();

            // For allowing the cross domain requests set here 
            app.UseCors("AppSubDomain");
            app.UseMvc();
        }
    }
}
