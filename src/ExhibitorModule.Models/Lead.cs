using System;
namespace ExhibitorModule.Models
{
    public class Lead : Attendee
    {
        public string LeadId { get; set; }
        public string ExhibitorId { get; set; }
        public string Notes { get; set; }
        public DateTime VisitedAt { get; set; }
        public DateTime VisitEndedAt { get; set; }
    }
}
