namespace iftar_api.Domain.Entities;

public class Iftar : BaseEntity
{
  public int Id { get; init; }

  public PublicTarget PublicTarget { get; init; }

  public required string Location { get; init; }

  public Visibility EventVisibility { get; init; }

  public int NumberOfPeople { get; init; }

  public MealType MealType { get; init; }

  public string MealOrganization { get; init; }

  public DateTime DateTime { get; init; }

  public DateTime? RegistrationDeadline { get; init; }

  public string MenuDescription { get; init; }

  public bool IsAccessible { get; init; }

  public IftarStatus Status { get; init; }

  public string? EventLink { get; init; }

  public int UserId { get; init; }
  public required User Organizer { get; init; }
}

public enum Visibility
{
  Public,
  Private
}

public enum PublicTarget
{
  Men = Gender.Men,
  Women = Gender.Women,
  Mixed
}

public enum Gender
{
  Men,
  Women
}

public enum MealType
{
  Vegetarian,
  Vegan
}

public enum IftarStatus
{
  Open,
  Closed,
  Cancelled,
  Reported
}