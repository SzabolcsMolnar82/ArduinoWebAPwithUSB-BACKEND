using ArduinoWebAPwithUSB.Context;
using ArduinoWebAPwithUSB.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuring the DbContext with SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add the CardReaderService as a background service
builder.Services.AddHostedService<CardReaderService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS enged�lyez�se minden origin �s minden v�gpont sz�m�ra
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()      // Enged�lyez minden origin (domain)
               .AllowAnyMethod()      // Enged�lyez minden HTTP met�dust (GET, POST, DELETE, stb.)
               .AllowAnyHeader();     // Enged�lyez minden HTTP fejl�ces inform�ci�t
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
