using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel;

namespace EventsIT.Models
{
    public class User : IdentityUser
    {
        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }

    }
}
