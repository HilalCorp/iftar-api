using Microsoft.AspNetCore.Mvc;

namespace iftar_api.Controllers;

[ApiController] // Attribut pour indiquer qu'il s'agit d'un contrôleur d'API
[Route("[controller]")] // Attribuer une route de base pour ce contrôleur
public class AppController(ILogger<AppController> logger) : ControllerBase // Héritage de ControllerBase
{

  // Constructeur avec injection de dépendance pour le logger

  // Méthode pour le endpoint "hello"
  [HttpGet("hello")] // Définition de la route pour l'action HelloWorld
  public IActionResult HelloWorld()
  {
    logger.LogInformation("Hello World"); // Log d'information
    return Content("Hello World"); // Test avec un texte brut
  }
}