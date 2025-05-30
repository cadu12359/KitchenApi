using System.Text.Json.Serialization;
using Kitchen.Application.Interfaces;
using Kitchen.Application.UseCases.Order;
using Kitchen.Domain.Interfaces;
using Kitchen.Infrastructure.Data;
using Kitchen.Infrastructure.Interfaces;
using Kitchen.Infrastructure.Repositories;
using Kitchen.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "Kitchen API", Version = "v1" }); });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });

    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000") // Permite o front-end
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; })
    .AddNewtonsoftJson(options => { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; });
;
builder.Services.AddEndpointsApiExplorer();

// Registrando o repositório
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Registrando o RabbitMQService
builder.Services.AddSingleton<IRabbitMQService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();

    var hostName = configuration["RabbitMQ:HostName"];
    var userName = configuration["RabbitMQ:UserName"];
    var password = configuration["RabbitMQ:Password"];

    return new RabbitMQService(hostName, userName, password);
});

// Registrando o Use Case
builder.Services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
builder.Services.AddScoped<IGetOrderUseCase, GetOrderUseCase>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("AllowAll");
app.UseCors("AllowReactApp");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kitchen API v1"); });
}

// Rodar a migração automaticamente na inicialização
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();