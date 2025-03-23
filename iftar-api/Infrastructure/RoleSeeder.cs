using iftar_api.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace iftar_api.Infrastructure;

public static class RoleSeeder
{
  public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
  {
    var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

    var roleNames = Enum.GetValues(typeof(UserRole)).Cast<UserRole>()
      .Select(e => e.ToString())
      .ToArray();

    foreach (var roleName in roleNames)
    {
      Console.WriteLine($"Seeding role {roleName}");
      var roleExist = await roleManager.RoleExistsAsync(roleName);
      if (!roleExist) await roleManager.CreateAsync(new Role { Name = roleName });
    }
  }
}