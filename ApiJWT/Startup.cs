using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiJWT.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiJWT
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

            // Middleware Inyección de Dependencias
            IoC.AddDependency(services);

            // Autorización y políticas
            services.AddAuthorization(options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                                                                        .RequireAuthenticatedUser()
                                                                        .Build();
            });

            // Autenticación
            var issuer = Configuration["AuthenticationSettings:Issuer"];
            var audience = Configuration["AuthenticationSettings:Audience"];
            var singinKey = Configuration["AuthenticationSettings:SigninKey"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                                                .AddJwtBearer(options =>
                                                                {
                                                                    options.Audience = audience;
                                                                    options.TokenValidationParameters = new TokenValidationParameters()
                                                                    {
                                                                        ValidateIssuer = true,
                                                                        ValidIssuer = issuer,
                                                                        ValidateIssuerSigningKey = true,
                                                                        ValidateLifetime = true,
                                                                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(singinKey))
                                                                    };
                                                                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
