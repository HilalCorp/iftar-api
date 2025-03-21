using iftar_api;
using iftar_api.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Ajouter IHttpContextAccessor au conteneur de services
builder.Services.AddHttpContextAccessor();

// Ajouter les services nécessaires à l'application
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurer le DbContext avec Npgsql pour PostgreSQL
var connectionString = builder.Configuration["PostgreSql:ConnectionString"];
var dbPassword = builder.Configuration["PostgreSql:DbPassword"];
var dbBuilder = new NpgsqlConnectionStringBuilder(connectionString)
{
  Password = dbPassword
};
builder.Services.AddDbContext<IftarDbContext>(options => options.UseNpgsql(dbBuilder.ConnectionString));

// Configurer les services CORS avant Build()
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();