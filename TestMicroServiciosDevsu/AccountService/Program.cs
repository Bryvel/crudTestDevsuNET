using AccountService.Data;
using AccountService.Repositories;
using AccountService.Sevices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitofWork, UnitofWork>();

var clientServiceUrl = builder.Configuration["Services:ClientService"] ?? "http://localhost:5062";
builder.Services.AddHttpClient<IClientServiceClient, ClientSeviceClient>(client =>
{
    client.BaseAddress = new Uri(clientServiceUrl);
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // IsRelational() evita que esto explote cuando las pruebas de integración
    // sustituyen Postgres por el proveedor InMemory (que no soporta migraciones).
    if (db.Database.IsRelational())
        db.Database.Migrate();
}
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
