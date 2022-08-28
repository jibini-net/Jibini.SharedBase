using Jibini.SharedBase.Auth;
using Jibini.SharedBase.Data;
using Jibini.SharedBase.Util.Extensions;

using BaseApp = Jibini.SharedBase.Server.App;
BaseApp.StartupType = typeof(App);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSharedBaseServer();
builder.Services.AddSingleton<AccountRepository>();

var app = builder.Build();
app.SetupSharedBaseServer();

app.Run();
