using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace ExpenseTracker.Web.Api.Bootstrap
{
    public static class SwaggerExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ExpenseTracker API",
                    Description = "ExpenseTracker Web API",
                    //TermsOfService = new System.Uri("https://www.talkingdotnet.com"),
                    Contact = new OpenApiContact() { Name = "Cagdas Korkut", Email = "cagdas@cagdaskorkut.com" }
                });
                
                var scheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                var security = new OpenApiSecurityRequirement()
                {
                    { scheme, new string[0] }
                };
                c.AddSecurityDefinition("Bearer", scheme);
                c.AddSecurityRequirement(security);

                //c.SwaggerDoc("v2", new OpenApiInfo
                //{
                //    Version = "v2",
                //    Title = "New API V2",
                //    Description = "Sample Web API",
                //    TermsOfService = new System.Uri("https://www.talkingdotnet.com"),
                //    Contact = new OpenApiContact() { Name = "Talking Dotnet", Email = "contact@talkingdotnet.com" }
                //});

                //c.DescribeAllEnumsAsStrings();
                //c.DescribeStringEnumsInCamelCase();
            });
        }

        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExpenseTracker API V1");
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
            });
        }
    }
}
