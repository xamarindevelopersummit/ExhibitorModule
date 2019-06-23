using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExhibitorModule.Common;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;
using ExhibitorModule.Services.Helpers;

namespace ExhibitorModule.Services
{
    public class LeadsService : ILeadsService
    {
        private readonly IApiService _apiService;
        private readonly ICacheService _cacheService;

        public LeadsService(IApiService apiService, ICacheService cacheService)
        {
            _apiService = apiService;
            _cacheService = cacheService;
        }

        public async Task AddUpdateLead(Lead lead)
        {
            var response = await _apiService.Post<HttpResponseMessage>(ApiKeys.AddLeadApi, lead.ToContent());
            var result = await response.ReadAsAsync<string>();
            var x = result;
        }

        public async Task<List<Attendee>> GetAttendees()
        {
            var response = await _apiService.Get<HttpResponseMessage>(new Uri(ApiKeys.AttendeesApi));
            var result = await response.ReadAsAsync<List<Attendee>>();
            _cacheService?.Device?.AddOrUpdateValue(CacheKeys.AttendeesKey, result);
            return result;
        }

        public async Task<LeadItem> GetLeadById(Guid id)
        {
            var tcs = new TaskCompletionSource<LeadItem>();
            await Task.Run(() =>
            {
                try
                {
                    var leads = _cacheService.Device.GetObject<List<LeadItem>>(CacheKeys.LeadsKey);
                    tcs.SetResult(leads.FirstOrDefault(_ => _.Id == id));
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task.Result;
        }

        public async Task<List<LeadItem>> GetLeads()
        {
            var response = await _apiService.Get<HttpResponseMessage>(new Uri(ApiKeys.LeadsApi));
            var leads = await response.ReadAsAsync<List<Lead>>();
            var result = new List<LeadItem>();
            foreach (var lead in leads)
            {
                var attendee = await GetAttendeeById(lead.AttendeeId);
                result.Add(new LeadItem {
                    Id = lead.Id,
                    ExhibitorId = lead.ExhibitorId,
                    Notes = lead.Notes,
                    AttendeeId = lead.AttendeeId,
                    Attendee = attendee
                });
            }

            _cacheService?.Device?.AddOrUpdateValue(CacheKeys.LeadsKey, result);
            return result;
        }

        public async Task<List<LeadItem>> LookupLead(string query)
        {
            var tcs = new TaskCompletionSource<List<LeadItem>>();
            await Task.Run(() =>
            {
                try
                {
                    var leads = _cacheService.Device.GetObject<List<LeadItem>>(CacheKeys.LeadsKey);
                    tcs.SetResult(leads.Where(_ => _.Attendee.FirstName.Contains(query) || _.Attendee.LastName.Contains(query)).ToList());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task.Result;
        }

        public async Task<Attendee> GetAttendeeById(Guid id)
        {
            var tcs = new TaskCompletionSource<Attendee>();
            await Task.Run(() =>
            {
                try
                {
                    var attendees = _cacheService.Device.GetObject<List<Attendee>>(CacheKeys.AttendeesKey);
                    tcs.SetResult(attendees.FirstOrDefault(_=>_.Id == id));
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task.Result;
        }

        public async Task<List<Attendee>> LookupAttendees(string query)
        {
            var tcs = new TaskCompletionSource<List<Attendee>>();
            await Task.Run(()=>
            {
                try
                {
                    var attendees = _cacheService.Device.GetObject<List<Attendee>>(CacheKeys.AttendeesKey);
                    tcs.SetResult(attendees.Where(_ => _.FirstName.Contains(query) || _.LastName.Contains(query)).ToList());
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task.Result;
        }
    }
}
