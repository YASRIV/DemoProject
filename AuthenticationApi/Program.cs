using AuthenticationApi.Process;
using Car_Wash_Library.Models;
using Microsoft.EntityFrameworkCore;
using static AuthenticationApi.Repository.UserRespository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//using Microsoft.EntityFrameworkCore;
//using AuthenticationService.EntityModel;


var connStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services
    .AddOpenApi()
    .AddDbContext<AuthDbContext>(
        options => options.UseSqlServer(connStr)
    )
    .Configure<AppSettings>(
        builder.Configuration.GetSection("AppSettings")
    )
    .AddHttpContextAccessor()
    .AddScoped<IUserRepository, UsersRepository>()
    .AddScoped<AuthProcess>()
    .AddScoped<TokenManager>()
    .AddScoped<HttpClient>()
    .AddCors(policyConfig =>
    {
        policyConfig.AddPolicy("allowAllPolicy",
            policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
    })
    

    .AddSwaggerGen()
    .AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler();
}
app.UseCors("allowAllPolicy");

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();