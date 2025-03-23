using iftar_api.Domain.DTOs;
using iftar_api.Domain.DTOs.AuthDTOs;
using iftar_api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace iftar_api.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService, UserManager<ControllerBase> userManager) : ControllerBase
{

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDTO model)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var result = await authService.RegisterUserAsync(model);
    if (!result.Succeeded) return BadRequest(result.Errors);

    return Ok("User registered successfully!");
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginDTO model)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var jwtToken = await authService.LoginUserAsync(model);
    if (jwtToken.IsNullOrEmpty()) return Unauthorized("Invalid credentials");

    return new OkObjectResult(jwtToken)
    {
      StatusCode = 200
    };
  }

  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO model)
  {
    var authenticatedObject = await authService.RefreshToken(model);

    return new OkObjectResult(authenticatedObject)
    {
      StatusCode = 200
    };
  }
}