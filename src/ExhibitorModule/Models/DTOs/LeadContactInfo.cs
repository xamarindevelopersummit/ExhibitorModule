using System;
namespace ExhibitorModule.Models
{
    public class LeadContactInfo
    {
        public Guid Id { get; set; }
        public Guid AttendeeId { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
        public string Email { get; set; }
        public string TicketNumber { get; set; }
        public string TicketType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string GravatarUri { get; set; }
    }
}
