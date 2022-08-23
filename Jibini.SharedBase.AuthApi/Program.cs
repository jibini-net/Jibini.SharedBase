using Jibini.SharedBase.Data;
using Jibini.SharedBase.Util.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSharedBaseServer();
builder.Services.AddSingleton<AccountRepository>();

var app = builder.Build();
app.SetupSharedBaseServer();

app.Run();
