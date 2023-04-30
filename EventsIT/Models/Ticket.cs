using System.ComponentModel.DataAnnotations;

namespace EventsIT.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public Event Event { get; set; }
        public User User { get; set; }
    }
}
