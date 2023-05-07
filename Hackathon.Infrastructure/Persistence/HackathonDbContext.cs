using Hackathon.Domain.Entities;
using Hackathon.Infrastructure.Persistence.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Infrastructure.Persistence;

public class HackathonDbContext : IdentityDbContext<ApplicationUser>
{
    public HackathonDbContext(DbContextOptions<HackathonDbContext> options): base(options) {}

    public DbSet<CreditApplication> Applications { get; set; }
}
