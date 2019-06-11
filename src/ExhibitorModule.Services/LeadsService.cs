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
                new Attendee { Id = "1", FirstName="Jonathan", LastName="P", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q" },
                new Attendee { Id = "2", FirstName="James", LastName="M", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q" },
                new Attendee { Id = "3", FirstName="Maddie", LastName="L", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q" },
                new Attendee { Id = "4", FirstName="Dan", LastName="Siegel", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q" },
                new Attendee { Id = "5", FirstName="Hussain", LastName="Abbasi", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q" },
                new Attendee { Id = "6", FirstName="Shane", LastName="Something", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q" },
            };

            _fullList = new List<Lead> {
                new Lead { Id = "1", FirstName="Dan", LastName="Siegel", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q", ExhibitorId = "1", Notes="Talked about X" },
                new Lead { Id = "2", FirstName="Hussain", LastName="Abbasi", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q", ExhibitorId = "1", Notes="Talked about Y" },
                new Lead { Id = "3", FirstName="Shane", LastName="Something", AdmissionType=AdmissionType.General, Title="Manager", Company="Sogeti", Avatar="https://media.licdn.com/dms/image/C4D03AQGPB4zkv5Ow2Q/profile-displayphoto-shrink_200_200/0?e=1565222400&v=beta&t=jUhXeuOGEXTlxefFNFtRyXLNakUreab5jdxvd4iW98Q", ExhibitorId = "1", Notes="Talked about Z" },
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
