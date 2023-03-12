using Havit.Blazor.Components.Web;
using Jibini.SharedBase.Util.Services;

namespace Jibini.SharedBase.Util.Extensions;

/// <summary>
/// Standardized program startup and configuration settings for the service
/// collection and application configuration.
/// </summary>
public static class ProgramStartupExtensions
{
    /// <summary>
    /// Extension to register standard services for the application.
    /// </summary>
    public static void AddSharedBaseServer(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor((config) =>
        {
            config.DisconnectedCircuitMaxRetained = 120;
            config.DisconnectedCircuitRetentionPeriod = TimeSpan.FromHours(4);
        });
        services.AddControllers();
        services.AddControllersWithViews();
        services.AddHxServices();
        services.AddHxMessenger();
        services.AddHxMessageBoxHost();

        services.AddSingleton<DatabaseService>();
        services.AddSingleton<ChromiumPdfService>();
        services.AddSingleton<IPdfService>((sp) => sp.GetService<ChromiumPdfService>()!);
        services.AddSingleton<ActiveDirectoryService>();
        services.AddScoped<DownloadService>();
        services.AddScoped<WinnovativePdfService>();
    }

    /// <summary>
    /// Extension to set up standard application configuration settings.
    /// </summary>
    public static void SetupSharedBaseServer(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.MapRazorPages();
    }
}
