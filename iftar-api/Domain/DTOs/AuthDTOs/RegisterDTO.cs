using System.ComponentModel.DataAnnotations;
using iftar_api.Domain.Entities;
using iftar_api.Domain.Validation.Annotations;

namespace iftar_api.Domain.DTOs;

public class RegisterDTO
{
  [Required]
  [StringLength(64)]
  public required string UserName { get; set; }

  [RequiredDependsOn("IsRegularUser")]
  public string? FirstName { get; set; }
  [RequiredDependsOn("IsRegularUser")]
  public string? LastName { get; set; }

  [Required]
  [EmailAddress]
  [StringLength(256)]
  public required string Email { get; set; }

  [Required]
  [DataType(DataType.Password)]
  [StringLength(100, MinimumLength = 6)] // La longueur du mot de passe
  public required string Password { get; set; }

  [Required]
  [DataType(DataType.Password)]
  [Compare("Password", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas.")]
  public required string ConfirmPassword { get; set; } // Vérification du mot de passe

  [Required]
  public Gender Gender { get; set; } // Sexe de l'utilisateur

  public string? OrganizerWebsite { get; set; } // Optionnel : site Web de l'organisateur

  [Required]
  public RegisterRole Role { get; set; } // Type d'utilisateur : Admin, Organisation, RegularUser

  // Si un utilisateur est de type "Organization", tu peux ajouter une règle pour vérifier la présence de FirstName/LastName.
  public bool IsRegularUser => Role == RegisterRole.RegularUser;
}

public enum RegisterRole
{
  Organization = UserRole.Organization,
  RegularUser = UserRole.RegularUser
}