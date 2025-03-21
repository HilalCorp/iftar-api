namespace iftar_api.Data;

public class Iftar: BaseEntity
{
  public int Id { get; set; }

  // Target audience (Men, Women, Mixed)
  public PublicTarget PublicTarget { get; set; } // Enum : Men, Women, Mixed

  // Location (City, District)
  public string Location { get; set; }

  // Is the organizer public or private
  public Visibility EventVisibility { get; set; }

  // Number of expected participants
  public int NumberOfPeople { get; set; }

  // Type of meal (Vegetarian, Vegan)
  public MealType MealType { get; set; } 

  // How the meal is organized (Ready meal, Financial contribution, Shop and cook together)
  public string MealOrganization { get; set; }

  // Date and time of the Iftar
  public DateTime DateTime { get; set; }

  // Deadline for registration (optional)
  public DateTime? RegistrationDeadline { get; set; }

  // Detailed description of the meal
  public string MenuDescription { get; set; }

  // Organizer's contact info
  public string OrganizerEmail { get; set; }
  public string OrganizerPhone { get; set; }
  public string OrganizerWebsite { get; set; }

  // Accessibility (if applicable)
  public bool IsAccessible { get; set; }

  // Event status
  public IftarStatus Status { get; set; } // Enum : "Open", "Closed", "Cancelled"

  // Link to related event
  public string EventLink { get; set; }

  // Relationship with the user (organizer)
  public int UserId { get; set; }
  public User Organizer { get; set; }
}

public enum Visibility
{
  Public, 
  Private,
}

// Enum for the target audience (Men, Women, Mixed)
public enum PublicTarget
{
  Men,
  Women
}

// Enum for the meal type (Vegetarian, Vegan)
public enum MealType
{
  Vegetarian,
  Vegan
}

// Enum for the Iftar status (Open, Closed, Cancelled)
public enum IftarStatus
{
  Open,
  Closed,
  Cancelled,
  Reported
}
