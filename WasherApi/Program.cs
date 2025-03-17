using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WasherApi.Interface;
using WasherApi.Process;
using WasherApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IWasher, WasherRepository>();
builder.Services.AddScoped<WasherProcess, WasherProcess>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<TokenValidator>();



builder.Services.AddDbContext<WashersDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.
    SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
