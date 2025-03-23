using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using iftar_api.Domain.DTOs;
using iftar_api.Domain.DTOs.AuthDTOs;
using iftar_api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace iftar_api.Services;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
{

  public async Task<IdentityResult> RegisterUserAsync(RegisterDTO model)
  {
    var user = new User
    {
      UserName = model.UserName,
      Email = model.Email,
      Gender = model.Gender,
      UserRole = (UserRole)model.Role
    };

    var createdUser = await userManager.CreateAsync(user, model.Password);
    if (!createdUser.Succeeded) return createdUser;

    var assignedRole = await userManager.AddToRoleAsync(user, user.UserRole.ToString());
    if (assignedRole.Errors != null && assignedRole.Errors.Any())
      throw new Exception(string.Join(',', assignedRole.Errors));

    return createdUser;
  }

  public async Task<string?> LoginUserAsync(LoginDTO model)
  {
    var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
    if (!result.Succeeded) return null;
    var user = await userManager.FindByNameAsync(model.Username);
    return GenerateJwtToken(user!);
  }

  public async Task<AuthenticatedDTO> RefreshToken(RefreshTokenDTO model)
  {
    var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == model.RefreshToken);

    if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
      throw new UnauthorizedAccessException("Invalid refresh token");

    var newJwtToken = GenerateJwtToken(user);
    var newRefreshToken = GenerateRefreshToken();

    user.RefreshToken = newRefreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

    await userManager.UpdateAsync(user);

    return new AuthenticatedDTO
    {
      Token = newJwtToken,
      RefreshToken = newRefreshToken
    };
  }

  private string GenerateJwtToken(User user)
  {
    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
    };

    var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new Exception("No JWT secret found")));
    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
      configuration["Jwt:Issuer"],
      configuration["Jwt:Audience"],
      claims,
      expires: DateTime.Now.AddHours(1),
      signingCredentials: signingCredentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  private string GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    return Convert.ToBase64String(randomNumber);
  }
}