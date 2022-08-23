using Jibini.SharedBase.Util.Extensions;

var builder = WebApplication.CreateBuilder();
builder.Services.AddSharedBaseServer();

var app = builder.Build();
app.SetupSharedBaseServer();

app.Run();