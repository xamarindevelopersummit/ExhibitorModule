using System;
namespace ExhibitorModule.Models
{
    public class Attendee
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Company { get; set; }
        public string Title { get; set; }
        public string Avatar { get; set; }
        public AdmissionType AdmissionType { get; set; }
    }
}
