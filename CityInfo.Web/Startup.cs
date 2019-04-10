using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CityInfo.Web
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie() 
                .AddOpenIdConnect(x => 
                 {
                    x.Authority = "http://localhost:5001";
                    x.ClientId = "CityInfo.Web";
                    x.SignInScheme = "Cookies";     // Wires up above Cookie auth to this open id
                    x.RequireHttpsMetadata = false;
                    x.ResponseType = "id_token";    // Authentication only not authorisation at this point
                    x.Scope.Add("openid");
                    x.Scope.Add("email");
                    x.Scope.Add("office");
                    x.SaveTokens = true;   // Save the Id token into the claim, 
                                            // middleware ow avoids the consent screen, also need client to have PostRedirectUri set
                                            // Cos signout is authenticated
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            // dotnet core 1.0 approach:
            //app.UseOpenIdConnectAuthentication()
            //app.UseCookieAuthentication()

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
