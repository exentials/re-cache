using Exentials.ReCache.Server.Hubs;
using Exentials.ReCache.Server.Interceptors;
using Exentials.ReCache.Server.Models;
using Exentials.ReCache.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace Exentials.ReCache.Server;

public class Program
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AccountService>();

        services.AddReCacheProvider();

        services.AddGrpc(options =>
        {
#if DEBUG
            options.EnableDetailedErrors = true;
#endif
            options.Interceptors.Add<ServerLoggingInterceptor>();
        }).AddJsonTranscoding();
        services.AddGrpcSwagger();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Re-Cache gRPC transcoding", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });

        // services.AddSingleton<ServerLoggingInterceptor>();
        services.AddHostedService<CacheCollectorService>();

        AuthOptions authOptions = new();
        configuration.Bind(AuthOptions.Auth, authOptions);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecretKey));

        services
            .AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        //services.AddAuthorization();

        services
            .AddAuthorizationBuilder()
            .AddDefaultPolicy(JwtBearerDefaults.AuthenticationScheme, policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireClaim(ClaimTypes.Name);
            });


        services.AddRazorPages();
        services.AddSignalR();
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder.Services, builder.Configuration);


        var app = builder.Build();

        if (!builder.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        var pathBase = builder.Configuration.GetValue<string>("UsePathBase")?.TrimEnd('/');
        if (!string.IsNullOrEmpty(pathBase))
        {
            app.UsePathBase(pathBase);
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapRazorPages();
        app.MapHub<MonitorHub>("/monitorHub");

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Re-Cache API V1");
        });
        app.MapGrpcService<GrpcCacheService>().RequireAuthorization();

        app.Run();
    }
}