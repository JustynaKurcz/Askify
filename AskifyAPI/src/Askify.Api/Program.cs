using Askify.Api;
using Askify.Shared;
using Askify.Shared.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.LoadLayers(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddPolicy();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddlewares();
app.MapEndpoints();
app.UseHttpsRedirection();

app.Run();