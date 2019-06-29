using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExhibitorModule.Models;

namespace ExhibitorModule.Services.Abstractions
{
    public interface ILeadsService
    {
        Task<List<LeadContactInfo>> GetLeads();
        Task<LeadContactInfo> GetLeadById(Guid id);
        Task<List<LeadContactInfo>> LookupLead(string query);
        Task AddUpdateLead(LeadContactInfo lead);
        Task<List<Attendee>> GetAttendees();
        Task<Attendee> GetAttendeeById(Guid id);
        Task<List<Attendee>> LookupAttendees(string query);
    }
}
