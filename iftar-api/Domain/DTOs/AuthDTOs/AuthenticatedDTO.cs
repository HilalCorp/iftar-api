namespace iftar_api.Domain.DTOs.AuthDTOs;

public class AuthenticatedDTO
{
  public required string Token { get; set; }
  public required string RefreshToken { get; set; }
}