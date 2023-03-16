using Jibini.SharedBase.Auth;
using Jibini.SharedBase.Data;
using Jibini.SharedBase.Data.Models;
using Jibini.SharedBase.Util.Extensions;

using BaseApp = Jibini.SharedBase.App;
BaseApp.StartupType = typeof(App);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSharedBaseServer();
builder.Services.AddSingleton<AccountRepository>();

builder.Services.Configure<SiteNavConfiguration>((it) =>
{
    it.Branding = new()
    {
        BrandName = "Auth API",
        BrandImage = "oi oi-lock-locked"
    };
    it.Pages = new()
    {
        new()
        {
            NavTitle = "Dashboard",
            NavPath = "",
            NavIcon = "oi oi-home"
        },
        new()
        {
            NavTitle = "Accounts",
            NavPath = "accounts",
            NavIcon = "oi oi-people"
        }
    };
});

var app = builder.Build();
app.SetupSharedBaseServer();

app.Run();
