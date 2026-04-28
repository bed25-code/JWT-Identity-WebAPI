using WebAPIAuth28April.Models;

namespace WebAPIAuth28April.Services;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(ApplicationUser user);
}
