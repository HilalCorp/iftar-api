using System.Text;
using iftar_api.Domain.Entities;
using iftar_api.Infrastructure;
using iftar_api.Infrastructure.Data;
using iftar_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Ajouter IHttpContextAccessor au conteneur de services
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["PostgreSql:ConnectionString"] ??
                       throw new InvalidOperationException("La connexion PostgreSQL est introuvable.");

builder.Services.AddDbContext<IftarDbContext>(options =>
  options.UseNpgsql(connectionString));

var dbBuilder = new NpgsqlConnectionStringBuilder(connectionString);

builder.Services.AddDbContext<IftarDbContext>(options => options.UseNpgsql(dbBuilder.ConnectionString));

// 🔹 Ajouter ASP.NET Identity
builder.Services.AddIdentity<User, Role>()
  .AddEntityFrameworkStores<IftarDbContext>()
  .AddDefaultTokenProviders();

// 🔹 Configurer JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(options =>
  {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(options =>
  {
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(key),
      ValidateIssuer = false,
      ValidateAudience = false,
      ClockSkew = TimeSpan.Zero
    };
  });

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Créer les rôles au démarrage de l'application
using (var scope = app.Services.CreateScope())
{
  var serviceProvider = scope.ServiceProvider;
  await RoleSeeder.SeedRolesAsync(serviceProvider); // Crée les rôles si nécessaire
}

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