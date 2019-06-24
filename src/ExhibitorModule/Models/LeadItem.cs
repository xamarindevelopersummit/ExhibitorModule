using System;
namespace ExhibitorModule.Models
{
    public class LeadItem : Lead
    {
        public Attendee Attendee { get; set; }
    }
}
