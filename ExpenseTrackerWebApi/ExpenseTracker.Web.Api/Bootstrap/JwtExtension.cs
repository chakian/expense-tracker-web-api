using Microsoft.AspNetCore.Authentication;
using ExpenseTracker.Web.Api.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

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
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
