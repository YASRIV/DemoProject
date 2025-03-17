using Car_Wash_Library.Models;
using CustomersAPI.Interface;
using CustomersAPI.Process;
using CustomersAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<CustomerProcess, CustomerProcess>();
builder.Services.AddScoped<ICustomer, CustomerRepository>();
builder.Services.AddScoped<ICar, CarRepository>();
builder.Services.AddScoped<TokenValidator>();


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
//MiddleWares are the components that aRE ASSEMBLED intto an
//Applictiopn pipline to handle request and response 
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    //Run the application and then type the below URL in the browser 
    //http:/locvalhost:XXXX/swagger
}

app.UseAuthorization();

app.MapControllers();

app.Run();
