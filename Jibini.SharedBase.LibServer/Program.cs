using BlazorTemplater;
using Jibini.SharedBase.Auth;
using Jibini.SharedBase.Server.Shared;
using Jibini.SharedBase.Util;
using Jibini.SharedBase.Util.Extensions;

Console.WriteLine(new TestTemplate("Something set!").ToHtml());

/*
Console.WriteLine(new ComponentRenderer<TestTemplate>()
    .Set((it) => it.Param, "Something set")
    .Render());
*/

var builder = WebApplication.CreateBuilder();
builder.Services.AddSharedBaseServer();

var app = builder.Build();
app.SetupSharedBaseServer();

var api = new AuthApi("https://auth.jibini.net");

/*
var account = await api.Account
    .Get(1)
    .InvokeAsync();
await api.Account
    .SetPassword
    .InvokeAsync(account);

await api.Tenant
    .Get(1)
    .InvokeAsync();
*/

//app.Run();