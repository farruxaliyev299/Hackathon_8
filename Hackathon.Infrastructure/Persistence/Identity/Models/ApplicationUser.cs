using Microsoft.AspNetCore.Identity;

namespace Hackathon.Infrastructure.Persistence.Identity.Models;

public class ApplicationUser : IdentityUser 
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
