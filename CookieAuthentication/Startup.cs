using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CookieAuthentication
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
            services.AddControllers();

            //add authentication

            services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = AuthenticationSchema.DefaultAuthenticateScheme;
                    option.DefaultScheme = AuthenticationSchema.DefaultAuthenticateScheme;
                    option.DefaultSignInScheme = AuthenticationSchema.DefaultAuthenticateScheme;
                    option.DefaultSignOutScheme = AuthenticationSchema.DefaultAuthenticateScheme;
                    option.DefaultChallengeScheme = AuthenticationSchema.DefaultAuthenticateScheme;
                    option.DefaultForbidScheme = AuthenticationSchema.DefaultAuthenticateScheme;
                })
                .AddCookie(AuthenticationSchema.DefaultAuthenticateScheme, option =>
                {
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(120);
                    option.Cookie.Name = AuthenticationSchema.DefaultAuthenticateScheme;
                    option.Cookie.HttpOnly = true;
                    option.SlidingExpiration = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

    public class AuthenticationSchema
    {
        public const string DefaultAuthenticateScheme = "Authentication.Cookie";

    }
}
