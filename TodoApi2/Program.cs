using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi2.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "D:\\mini_projet\\database.db" };
var connectionString = connectionStringBuilder.ToString();
builder.Services.AddDbContext<VehiculeContext>(opt =>
    opt.UseSqlite(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
