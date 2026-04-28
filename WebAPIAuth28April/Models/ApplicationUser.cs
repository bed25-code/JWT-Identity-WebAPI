using Microsoft.AspNetCore.Identity;

namespace WebAPIAuth28April.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
