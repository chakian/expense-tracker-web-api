using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using ExpenseTracker.Business.Options;

namespace ExpenseTracker.Web.Api.Bootstrap
{
    public static class JwtExtension
    {
        public static AuthenticationBuilder AddCustomJwtBearer(this AuthenticationBuilder builder, IServiceCollection services, IConfiguration Configuration)
        {
            //get jwt options
            var jwtOptionsSection = Configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(jwtOptionsSection);
            var jwtOptions = jwtOptionsSection.Get<JwtOptions>();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

            return builder.AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "https://expense.cagdaskorkut.com/api",
                    ValidateAudience = false,
                    ValidAudiences = new List<string>() { "api://web", "api://mobile" },
                    //RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
