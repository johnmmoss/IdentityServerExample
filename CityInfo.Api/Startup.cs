using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.Api.Entities;
using CityInfo.Api.Model;
using CityInfo.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace CityInfo.Api
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
            services.AddMvc();
            //    .AddMvcOptions(o => o.OutputFormatters.Add(
            //        new XmlDataContractSerializerOutputFormatter()
            //    ));
            // To convert from lowercase to uppercase:
            //.AddJsonOptions(o =>
            //{
            //    if (o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
            //        castedResolver.NamingStrategy = null;
            //    }
            // });
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Note that the Configuration property includes any variables from environment vars - see project debug properties in VS
            var connectionString = Configuration["connectionStrings:CityInfoContextConnectionString"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));
            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(LogLevel.Information);
            loggerFactory.AddProvider(new Log4NetProvider());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseMvc();

            AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<City, CityNoPointsOfInterest>();
                    cfg.CreateMap<City, CityDto>();
                    cfg.CreateMap<PointOfInterest, PointOfInterestDto>();
                    cfg.CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
                    cfg.CreateMap<PointOfInterestForUpdateDto, PointOfInterest>();
                    cfg.CreateMap<PointOfInterest, PointOfInterestForUpdateDto>();
                }
            );
        }
    }
}
