using System.Threading.RateLimiting;
using API.Extensions;
using API.Options;
using API.Middleware;
using API.OptionsSetup;
using Application;
using Infrastructure;
using Serilog;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext());
            

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGenWithAuth();
            builder.Services.AddCorsPolicy(builder.Configuration);

            builder.Services.ConfigureOptions<JwtOptionsSetup>();

            builder.Services
                .AddInfrastructureLayer(builder.Configuration)
                .AddApplicationLayer();
            

            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 10,
                            QueueLimit = 2,
                            Window = TimeSpan.FromMinutes(1)
                        }
                    )
                );
            });

            var app = builder.Build();

            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpsContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpsContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpsContext.Request.Scheme);

                    var userId = httpsContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrWhiteSpace(userId))
                    {
                        diagnosticContext.Set("UserId", userId);
                    }
                };
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(CorsOptions.DefaultPolicyName);

            app.UseRateLimiter();

            app.UseMiddleware<ExceptionMiddleware>();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
