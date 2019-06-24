using System;
namespace ExhibitorModule.Models
{
    public class Lead
    {
        public Guid Id { get; set; }
        public Guid AttendeeId { get; set; }
        public Guid ExhibitorId { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset LastUpdate { get; set; }
    }
}
