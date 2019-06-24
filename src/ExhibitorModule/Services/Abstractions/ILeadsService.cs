using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExhibitorModule.Models;

namespace ExhibitorModule.Services.Abstractions
{
    public interface ILeadsService
    {
        Task<List<LeadItem>> GetLeads();
        Task<LeadItem> GetLeadById(Guid id);
        Task<List<LeadItem>> LookupLead(string query);
        Task AddUpdateLead(Lead lead);
        Task<List<Attendee>> GetAttendees();
        Task<Attendee> GetAttendeeById(Guid id);
        Task<List<Attendee>> LookupAttendees(string query);
    }
}
