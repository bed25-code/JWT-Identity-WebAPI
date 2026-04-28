using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIAuth28April.Controllers;

/// <summary>
/// Demo endpoints showing anonymous, authenticated, and role-based access.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    /// <summary>
    /// Returns products for any signed-in user.
    /// </summary>
    [HttpGet]
    public IActionResult GetProductList()
    {
        var products = new[]
        {
            new { Id = 1, Name = "Product A", Price = 19.99m },
            new { Id = 2, Name = "Product B", Price = 29.99m }
        };

        return Ok(products);
    }

    /// <summary>
    /// Returns admin-only data. Requires the Admin role.
    /// </summary>
    [HttpGet("admin-only")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAdminOnlyData()
    {
        return Ok(new
        {
            Message = "Admin secret data",
            AccessLevel = "Admin"
        });
    }

    /// <summary>
    /// Public endpoint - no token required.
    /// </summary>
    [HttpGet("public-info")]
    [AllowAnonymous]
    public IActionResult GetPublicInfo()
    {
        return Ok(new
        {
            Message = "Anyone can see this endpoint.",
            RequiresAuthentication = false
        });
    }

    /// <summary>
    /// Returns identity information from the current JWT token.
    /// </summary>
    [HttpGet("my-profile")]
    public IActionResult GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var fullName = User.FindFirstValue("fullName");
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();

        return Ok(new
        {
            UserId = userId,
            Email = email,
            FullName = fullName,
            Roles = roles
        });
    }
}
