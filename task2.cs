/* * PROJECT: Performance Monitoring Dashboard
 * TASK 2: Monitoring .NET App Metrics with Application Insights
 * TECHNOLOGY: ASP.NET Core, Azure Application Insights SDK
 */

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

// 1. SETUP: Startup configuration for Monitoring
public class Startup {
    public void ConfigureServices(IServiceCollection services) {
        // Adding Application Insights Telemetry
        // This will automatically track CPU, Memory, and Request rates
        services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
    }

    public void Configure(IApplicationBuilder app) {
        // Custom Middleware to track "Business Metrics" (e.g., Total Sales in real-time)
        app.Use(async (context, next) => {
            var telemetryClient = context.RequestServices.GetRequiredService<TelemetryClient>();
            
            // Track how long each request takes (Latency)
            var timer = System.Diagnostics.Stopwatch.StartNew();
            await next();
            timer.Stop();

            // Send custom metric to Dashboard
            telemetryClient.TrackMetric("RequestProcessingTime", timer.Elapsed.TotalMilliseconds);
            
            if (context.Response.StatusCode >= 400) {
                telemetryClient.TrackEvent("ErrorOccurred", new Dictionary<string, string> {
                    { "Path", context.Request.Path },
                    { "Status", context.Response.StatusCode.ToString() }
                });
            }
        });
    }
}
