using Jibini.SharedBase.Auth;
using Jibini.SharedBase.Util;
using Jibini.SharedBase.Util.Extensions;

var builder = WebApplication.CreateBuilder();
builder.Services.AddSharedBaseServer();

var app = builder.Build();
app.SetupSharedBaseServer();

await new AuthApi("https://auth.jibini.net")
    .Tenant
    .Get(1)
    .InvokeAsync<Tenant>();

app.Run();