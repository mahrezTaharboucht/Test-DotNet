using OrdersApi.Extensions;
using OrdersApi.Filters;
using OrdersApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure auto validation filter and behaviour
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

builder.Services.ConfigureAutoValidationBehaviour();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.ConfigureMappers();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureValidators();
builder.Services.ConfigureBusinessServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
