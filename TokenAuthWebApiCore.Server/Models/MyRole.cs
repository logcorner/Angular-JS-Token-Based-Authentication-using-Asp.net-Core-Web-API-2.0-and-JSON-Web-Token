using Microsoft.AspNetCore.Identity;

namespace TokenAuthWebApiCore.Server.Models
{
    public class MyRole : IdentityRole
    {
        public string Description { get; set; }
    }
}