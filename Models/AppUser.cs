using Microsoft.AspNetCore.Identity;
using Simulation1.DAL;

namespace Simulation1.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
