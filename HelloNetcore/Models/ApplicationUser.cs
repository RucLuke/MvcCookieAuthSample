using Microsoft.AspNetCore.Identity;

namespace HelloNetcore.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Avator { get; set; }
    }
}
