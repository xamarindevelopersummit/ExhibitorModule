using System;
using Prism.Mvvm;

namespace ExhibitorModule.Models
{
    public class LeadContactInfo : BindableBase
    {
        private string _notes;
        private Guid _id;
        private Guid _attendeeId;
        private DateTimeOffset _lastUpdate;
        private string _email;
        private string _ticketNumber;
        private string _ticketType;
        private string _firstName;
        private string _lastName;
        private string _company;
        private string _gravatarUri;

        public Guid Id { get => _id; set { SetProperty(ref _id, value); } }
        public Guid AttendeeId { get => _attendeeId; set { SetProperty(ref _attendeeId, value); } }
        public string Notes { get => _notes; set { SetProperty(ref _notes, value); } }
        public DateTimeOffset LastUpdate { get => _lastUpdate; set { SetProperty(ref _lastUpdate, value); } }
        public string Email { get => _email; set { SetProperty(ref _email, value); } }
        public string TicketNumber { get => _ticketNumber; set { SetProperty(ref _ticketNumber, value); } }
        public string TicketType { get => _ticketType; set { SetProperty(ref _ticketType, value); } }
        public string FirstName { get => _firstName; set { SetProperty(ref _firstName, value); } }
        public string LastName { get => _lastName; set { SetProperty(ref _lastName, value); } }
        public string Company { get => _company; set { SetProperty(ref _company, value); } }
        public string GravatarUri { get => _gravatarUri; set { SetProperty(ref _gravatarUri, value); } }
    }
}
