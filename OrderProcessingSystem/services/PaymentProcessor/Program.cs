using Microsoft.EntityFrameworkCore;
using OrderAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var mySqlServerVersion = new MySqlServerVersion(new Version(8, 4, 5));

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(mySqlConnection, mySqlServerVersion);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();