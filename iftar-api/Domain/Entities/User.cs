using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace iftar_api.Domain.Entities;

public class User : IdentityUser<Guid>
{
  public required Gender Gender { get; set; }
  [StringLength(255)]
  public string? OrganizerWebsite { get; init; }
  public UserRole UserRole { get; set; }

  public string? RefreshToken { get; set; }
  public DateTime RefreshTokenExpiryTime { get; set; }
}

public enum UserRole
{
  Admin,
  Organization,
  RegularUser
}