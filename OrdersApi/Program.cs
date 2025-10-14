using Microsoft.EntityFrameworkCore;
using OrdersApi.Entities;
using OrdersApi.Infrastructure.Data;
using OrdersApi.Infrastructure.Repositories;
using OrdersApi.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("SqlLiteDbConnection");
builder.Services.AddDbContext<OrdersApiDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IRepository<Order>, OrdersRepository>();
builder.Services.AddScoped<IRepository<ProductConfiguration>, ProductConfigurationRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
