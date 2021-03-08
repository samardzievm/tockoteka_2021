using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace tockoteka.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

    }
}
