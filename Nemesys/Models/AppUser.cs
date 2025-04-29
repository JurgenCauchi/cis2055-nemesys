using Microsoft.AspNetCore.Identity;

namespace Nemesys.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        public string AuthorAlias { get; set; }

    }
}
