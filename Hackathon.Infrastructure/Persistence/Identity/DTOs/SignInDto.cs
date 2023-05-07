using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Infrastructure.Persistence.Identity.DTOs
{
    public class SignInDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
