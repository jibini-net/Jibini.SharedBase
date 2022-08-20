using Jibini.SharedBase.Data;
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
        services.AddServerSideBlazor();
        services.AddControllers();
        services.AddControllersWithViews();

        services.AddSingleton<DatabaseService>();
        services.AddScoped<AccountRepository>();
        services.AddScoped<GiftCardRepository>();
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
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
        app.MapRazorPages();
    }
}
