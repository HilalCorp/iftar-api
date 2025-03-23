using System.ComponentModel.DataAnnotations;

namespace iftar_api.Domain.Validation.Annotations;

public class RequiredDependsOn(
  string otherPropertyName,
  string errorMessage = "Le champ {0} est requis lorsque {1} a une valeur spécifiée.")
  : ValidationAttribute
{

  protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
  {
    var otherProperty = validationContext.ObjectType.GetProperty(otherPropertyName);
    if (otherProperty == null) return new ValidationResult($"La propriété {otherPropertyName} n'a pas été trouvée.");

    var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance);

    if (otherPropertyValue is true && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
      return new ValidationResult(string.Format(errorMessage, validationContext.DisplayName, otherPropertyName));

    return ValidationResult.Success;
  }
}