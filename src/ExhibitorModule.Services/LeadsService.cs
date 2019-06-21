using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;

namespace ExhibitorModule.Services
{
    public class LeadsService : ILeadsService
    {
        private readonly List<Attendee> _attendees;
        private readonly List<Lead> _fullList;

        public LeadsService()
        {
            _attendees = new List<Attendee> {
                new Attendee { Id = "1", FirstName="Jonathan", LastName="P", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://img.icons8.com/ios/100/000000/user-filled.png" },
                new Attendee { Id = "2", FirstName="James", LastName="M", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://img.icons8.com/ios/100/000000/user-filled.png" },
                new Attendee { Id = "3", FirstName="Maddie", LastName="L", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://img.icons8.com/ios/100/000000/user-filled.png" },
                new Attendee { Id = "4", FirstName="Dan", LastName="Siegel", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://img.icons8.com/ios/100/000000/user-filled.png" },
                new Attendee { Id = "5", FirstName="Hussain", LastName="Abbasi", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://img.icons8.com/ios/100/000000/user-filled.png" },
                new Attendee { Id = "6", FirstName="Shane", LastName="Something", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://img.icons8.com/ios/100/000000/user-filled.png" },
            };

            _fullList = new List<Lead> {
                new Lead { Id = "1", FirstName="Dan", LastName="Siegel", AdmissionType=AdmissionType.General, Title="Owner", Company="AvantiPoint LLC", Avatar="https://media.licdn.com/dms/image/C5603AQHJhIia5wa_tw/profile-displayphoto-shrink_800_800/0?e=1566432000&v=beta&t=iBrLuAs8wnhYfVCBQaPIi7W5YZ1kOmoFuDPoxHAER70", ExhibitorId = "1", Notes="Talked about X" },
                new Lead { Id = "2", FirstName="Hussain", LastName="Abbasi", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q", ExhibitorId = "1", Notes="Talked about Y" },
                new Lead { Id = "3", FirstName="Allan", LastName="Ritchie", AdmissionType=AdmissionType.General, Title="Microsoft MVP", Avatar="https://media.licdn.com/dms/image/C4E03AQGwQT1RAN5gRw/profile-displayphoto-shrink_800_800/0?e=1566432000&v=beta&t=835JVTNnuPjfPdELLM80vi0cLTfEfi8sP9HPK7pmFVI", ExhibitorId = "1", Notes="Talked about Z" },
            };
        }

        public async Task AddUpdateLead(Lead lead)
        {
            _fullList.Add(lead);
        }

        public async Task<Lead> GetLeadById(string id)
        {
            return _fullList.FirstOrDefault(_ => _.Id.Equals(id));
        }

        public async Task<List<Lead>> GetLeads()
        {
            return _fullList.OrderByDescending(_=>_.VisitedAt).ToList();
        }

        public async Task<List<Lead>> GetLeads(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetLeads();

            return _fullList.Where(_=>_.FirstName.ToLower().Contains(query.ToLower()) || _.LastName.ToLower().Contains(query)).ToList();
        }

        public async Task<List<Attendee>> LooupAttendees(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Attendee>();

            return _attendees.Where(_ => _.FirstName.ToLower().Contains(query.ToLower()) || _.LastName.ToLower().Contains(query)).ToList();
        }
    }
}
