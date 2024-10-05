using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Notifications.Servers.API.Autentication;

namespace System.Notifications.Servers.API.Configuration;

public static class SwaggerConfiguration
{
    public static IHostApplicationBuilder AddSwaggerGenDefault(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("Parameters", new OpenApiInfo 
            { 
                Title = "Parameters",
                Version = "v1"
            });

            options.SwaggerDoc("Authorization", new OpenApiInfo
            {
                Title = "Authentication",
                Version = "v1",
            });

            options.SwaggerDoc("Events", new OpenApiInfo
            {
                Title = "Events",
                Version = "v1",

            });

            #region configuration security
            options.AddSecurityDefinition(BasicAuthenticationHandler.Schema, new OpenApiSecurityScheme
            {
                Description = "Basic Authorization header. Example: \"Authorization: Basic {credentials}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = BasicAuthenticationHandler.Schema
            });
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
             {
                 Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                 Name = "Authorization",
                 In = ParameterLocation.Header,
                 Type = SecuritySchemeType.Http,
                 Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    new string[] { "Events", "Parameters" }
                }
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = BasicAuthenticationHandler.Schema
                        }
                    },
                    new string[] { "Authorization" }
                }
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            #endregion

        });
        return builder;
    }

    public static IApplicationBuilder UseSwaggerDefault(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseSwagger();

        applicationBuilder.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/Parameters/swagger.json", "Parameters v1");
            c.SwaggerEndpoint("/swagger/Authorization/swagger.json", "Authorization v1");
            c.SwaggerEndpoint("/swagger/Events/swagger.json", "Events v1");
        });

        return applicationBuilder;
    }
}


internal class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requiredScopes = context.MethodInfo.DeclaringType!
                        .GetCustomAttributes(true)
                        .OfType<AuthorizeAttribute>()
                        .Select(attr => attr.AuthenticationSchemes)
                        .Distinct();

        var requiredScopes2 = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.AuthenticationSchemes)
                .Distinct();

        bool requireAuth = false;
        string id = "";

        if (requiredScopes.Contains(JwtBearerDefaults.AuthenticationScheme) || requiredScopes2.Contains(JwtBearerDefaults.AuthenticationScheme))
        {
            requireAuth = true;
            id = JwtBearerDefaults.AuthenticationScheme;
        }
        else if (requiredScopes.Contains(BasicAuthenticationHandler.Schema) || requiredScopes2.Contains(BasicAuthenticationHandler.Schema))
        {
            requireAuth = true;
            id = BasicAuthenticationHandler.Schema;
        }

        if (requireAuth && !string.IsNullOrEmpty(id))
        {
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });

            if (id == BasicAuthenticationHandler.Schema)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id }
                            },
                            new[] { "Authorization"}
                        }
                    }
                };
            }

            if (id == JwtBearerDefaults.AuthenticationScheme)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = id }
                            },
                            new[] { "Events", "Parameters" }
                        }
                    }
                };
            }
            
        }
    }
}