using EShop.ServiceDefaults;
using FastEndpoints;
using FastEndpoints.Swagger;
using OrderService.Application.Endpoints.CartEndpoints;
using OrderService.Data.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services
   .AddFastEndpoints(x => x.Assemblies = [typeof(GetCartByUserIdEndpoint).Assembly])
   .SwaggerDocument(x =>
   {
       x.NewtonsoftSettings = s =>
       {
           s.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
       };
   });

builder.Services.AddData(builder.Configuration.GetConnectionString("OrderDb")!);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "/openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors("AllowLocalhost3000");

app.UseFastEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.Run();