using System;
namespace ExhibitorModule.Models
{
    public class Attendee
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string TicketNumber { get; set; }

        public string TicketType { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public DateTimeOffset PaymentDate { get; set; }

        public string DiscountCode { get; set; }

        public decimal TicketPrice { get; set; }

        public string GravatarUri { get; set; }
    }
}
