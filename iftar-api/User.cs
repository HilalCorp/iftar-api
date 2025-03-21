namespace iftar_api;

public class User
{
  public int Id { get; set; }  // Clé primaire
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Email { get; set; }
  public string Password { get; set; }  // Si nécessaire pour la gestion de l'authentification
  public string Phone { get; set; }
  public string Gender { get; set; }

  // D'autres propriétés que tu souhaites ajouter pour un utilisateur
}
