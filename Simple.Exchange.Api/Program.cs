using Simple.Exchange.Api.Endpoints;
using Simple.Exchange.Api.Extensions;
using Simple.Exchange.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.RegisterMediatr()
    .RegisterDependencies()
    .RegisterHttpClients(builder.Configuration)
    .InjectConfigurations(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// Add Api Endpoints
app.MapExchangeEndpoints();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();