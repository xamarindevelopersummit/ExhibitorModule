using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExhibitorModule.Models;

namespace ExhibitorModule.Services.Abstractions
{
    public interface ILeadsService
    {
        Task<List<Lead>> GetLeads();
        Task<List<Lead>> GetLeads(string query);
        Task<Lead> GetLeadById(string id);
        Task AddUpdateLead(Lead lead);
        Task<List<Attendee>> LooupAttendees(string query);
    }
}
