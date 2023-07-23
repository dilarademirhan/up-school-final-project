using FinalProject.WebApi.Hubs;
using Microsoft.EntityFrameworkCore;
using FinalProject.Persistence.EntityFramework.Contexts;
using FinalProject.WebApi.Hubs;
using Domain.Settings;
//using FinalProject.Domain.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

//builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<GoogleSettings>(builder.Configuration.GetSection("GoogleSettings"));


var mariaDbConnectionString = builder.Configuration.GetConnectionString("MariaDB")!;

builder.Services.AddDbContext<FinalProjectDbContext>(opt => opt.UseMySql(mariaDbConnectionString, ServerVersion.AutoDetect(mariaDbConnectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed((host) => true)
            .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();



app.MapHub<OrdersHub>("/Hubs/OrdersHub");
app.MapHub<SeleniumLogHub>("/Hubs/SeleniumLogHub");

app.Run();